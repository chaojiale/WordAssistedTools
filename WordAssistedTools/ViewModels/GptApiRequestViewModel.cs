using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WordAssistedTools.Models;
using WordAssistedTools.Utils;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;
using System.Text.RegularExpressions;
using OpenAI.Chat;
using WordAssistedTools.Properties;

namespace WordAssistedTools.ViewModels;

internal class GptApiRequestViewModel : ObservableObject {
  public const string WaitResult = "等待响应";
  public const string Streaming = "接收信息";
  public const string FinishResult = "完毕";

  private Word.Application MainWordApp => Globals.ThisAddIn.Application;
  private Word.Document ActiveDoc => MainWordApp.ActiveDocument;
  private Word.Selection Selection => ActiveDoc.Application.Selection;
  private static Settings Sets => Settings.Default;

  private string _question;
  public string Question {
    get => _question;
    set => SetProperty(ref _question, value);
  }

  private string _answer;
  public string Answer {
    get => _answer;
    set => SetProperty(ref _answer, value);
  }

  private string _currentState;
  public string CurrentState {
    get => _currentState;
    set => SetProperty(ref _currentState, value);
  }

  public string ProcessedAnswer => Sets.GptApiIsRemoveEmptyLine ? Regex.Replace(Answer, @"(\r\n|\r|\n){2,}", "\r\n").Trim() : Answer.Trim();

  public AsyncRelayCommand StartApiRequestCommand { get; }
  public RelayCommand CopyToBelowCommand { get; }
  public RelayCommand ReplaceParagraphsCommand { get; }
  public RelayCommand ReplaceSelectionCommand { get; }
  public RelayCommand CopyToClipboardCommand { get; }


  public GptApiRequestViewModel() {
    StartApiRequestCommand = new AsyncRelayCommand(StartApiRequestCommand_Execute);
    CopyToBelowCommand = new RelayCommand(CopyToBelowCommand_Execute, () => !string.IsNullOrWhiteSpace(Answer));
    ReplaceParagraphsCommand = new RelayCommand(ReplaceParagraphsCommand_Execute, () => !string.IsNullOrWhiteSpace(Answer));
    ReplaceSelectionCommand = new RelayCommand(ReplaceSelectionCommand_Execute, () => !string.IsNullOrWhiteSpace(Answer));
    CopyToClipboardCommand = new RelayCommand(CopyToClipboardCommand_Execute, () => !string.IsNullOrWhiteSpace(Answer));
    Sets.PropertyChanged += Sets_PropertyChanged;
  }

  private void Sets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
    if (e.PropertyName == nameof(Sets.GptApiIsRemoveEmptyLine)) {
      Sets.Save();
    }
  }

  public async Task StartRequest(string message) {
    Question = message;
    await StartApiRequestCommand.ExecuteAsync(null);
  }

  private async Task StartApiRequestCommand_Execute() {
    if (string.IsNullOrWhiteSpace(Question)) {
      ShowMsgBox.Warning("请输入问题");
      return;
    }

    try {
      GeneralGpt generalGpt = new();
      Answer = string.Empty;
      UpdateCanExecute();

      if (!Sets.GptApiStreamingMode) {
        CurrentState = $"当前进度：{WaitResult}";
        await generalGpt.GetResponseAsync(Question);
      } else {
        CurrentState = $"当前进度：{Streaming}";

        int timeout = Sets.GptApiTimeoutSecond;
        using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
        generalGpt.Messages.Add(new UserChatMessage(Question));
        AsyncCollectionResult<StreamingChatCompletionUpdate> updates = generalGpt.Client.CompleteChatStreamingAsync(generalGpt.UsedMessages, generalGpt.CompletionOptions, cts.Token);
        await foreach (StreamingChatCompletionUpdate update in updates) {
          if (update.ContentUpdate.Count > 0) {
            Answer += update.ContentUpdate[0].Text;
          }
        }
        generalGpt.Messages.Add(new AssistantChatMessage(Answer));
      }

      UpdateCanExecute();
      CurrentState = $"当前进度：{FinishResult}";
      _ = Task.Run(() => {
        Thread.Sleep(2000);
        CurrentState = string.Empty;
      });

    } catch (Exception ex) {
      Shared.FileLog.Error(ex);
    }
  }

  private void UpdateCanExecute() {
    CopyToBelowCommand.NotifyCanExecuteChanged();
    ReplaceParagraphsCommand.NotifyCanExecuteChanged();
    ReplaceSelectionCommand.NotifyCanExecuteChanged();
    CopyToClipboardCommand.NotifyCanExecuteChanged();
  }

  private void CopyToBelowCommand_Execute() {
    Word.Paragraph endPara = Selection.Range.Paragraphs.Last;
    bool isLastParagraph = endPara.Range.End == ActiveDoc.Content.End;
    if (string.IsNullOrWhiteSpace(endPara.Range.Text)) {
      string answer = isLastParagraph ? ProcessedAnswer : ProcessedAnswer + "\r";
      endPara.Range.Text = answer;
    } else {
      // 是最后一段，当前段落末尾的\r会被吞掉
      string answer = isLastParagraph ? "\r" + ProcessedAnswer : ProcessedAnswer + "\r";
      endPara.Range.InsertAfter(answer);
    }
  }


  private void ReplaceParagraphsCommand_Execute() {
    Word.Paragraphs paras = Selection.Range.Paragraphs;
    Word.Paragraph startPara = paras.First;
    Word.Paragraph endPara = paras.Last;
    Word.Range range = ActiveDoc.Range(startPara.Range.Start, endPara.Range.End);
    bool isLastParagraph = endPara.Range.End == ActiveDoc.Content.End;
    string answer = isLastParagraph ? ProcessedAnswer : ProcessedAnswer + "\r";
    range.Text = answer;
  }


  private void ReplaceSelectionCommand_Execute() {
    Word.Range range = Selection.Range;
    range.Text = ProcessedAnswer;
  }

  private void CopyToClipboardCommand_Execute() {
    System.Windows.Clipboard.SetText(ProcessedAnswer);
  }
}