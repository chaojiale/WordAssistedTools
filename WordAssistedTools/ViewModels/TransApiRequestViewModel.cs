using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Word = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.SDK;
using WordAssistedTools.Utils;
using static System.Net.Mime.MediaTypeNames;
using WordAssistedTools.Properties;
using System.Threading;

namespace WordAssistedTools.ViewModels;

internal class TransApiRequestViewModel : ObservableObject {
  private static Settings Sets => Settings.Default;
  private Word.Application MainWordApp => Globals.ThisAddIn.Application;
  private Word.Document ActiveDoc => MainWordApp.ActiveDocument;
  private Word.Selection Selection => MainWordApp.Selection;

  private bool IsNoSelection => Selection.Text.Length == 1 && Selection.Start == Selection.End;

  public List<string> SourceLanList { get; } = Enum.GetValues(typeof(Language)).Cast<Language>().Select(x => x.ToString()).ToList();
  public List<string> TargetLanList { get; } = Enum.GetValues(typeof(Language)).Cast<Language>().Where(x => x != Language.Auto).Select(x => x.ToString()).ToList();

  private string _originalText;
  public string OriginalText {
    get => _originalText;
    set {
      if (SetProperty(ref _originalText, value)) {
        if (Sets.TransApiIsAutoTranslate && !TranslateCommand.IsRunning) {
          SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
          TranslateCommand.ExecuteAsync(null);
        }
      }
    }
  }

  private string _translatedText;
  public string TranslatedText {
    get => _translatedText;
    set => SetProperty(ref _translatedText, value);
  }

  public Language SourceLan => (Language)Enum.Parse(typeof(Language), Sets.TransApiSourceLan);
  public Language TargetLan => (Language)Enum.Parse(typeof(Language), Sets.TransApiTargetLan);

  public RelayCommand ReverseLanguageCommand { get; }
  public AsyncRelayCommand TranslateCommand { get; }
  public RelayCommand CopyToClipboardCommand { get; }
  public RelayCommand InsertResultCommand { get; }
  public RelayCommand CopyToBelowCommand { get; }

  public TransApiRequestViewModel() {
    ReverseLanguageCommand = new RelayCommand(ReverseLanguageCommand_Execute);
    TranslateCommand = new AsyncRelayCommand(TranslateCommand_Execute);
    CopyToClipboardCommand = new RelayCommand(CopyToClipboardCommand_Execute, () => !string.IsNullOrWhiteSpace(TranslatedText));
    InsertResultCommand = new RelayCommand(InsertResultCommand_Execute, () => !string.IsNullOrWhiteSpace(TranslatedText));
    CopyToBelowCommand = new RelayCommand(CopyToBelowCommand_Execute, () => !string.IsNullOrWhiteSpace(TranslatedText));
    Sets.PropertyChanged += Sets_PropertyChanged;
  }

  private void Sets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
    switch (e.PropertyName) {
      case nameof(Sets.TransApiSourceLan) or nameof(Sets.TransApiTargetLan):
        Sets.Save();
        return;
      case nameof(Sets.TransApiIsAutoTranslate):
        if (Sets.TransApiIsAutoTranslate && !TranslateCommand.IsRunning) {
          TranslateCommand.ExecuteAsync(null);
        }
        Sets.Save();
        return;
    }
  }

  public void SetEventStateWithVisibility(bool visible) {
    if (visible) {
      if (!IsNoSelection && OriginalText != Selection.Text) {
        OriginalText = Selection.Text;
      }
      MainWordApp.WindowSelectionChange += MainWordApp_WindowSelectionChange;
    } else {
      MainWordApp.WindowSelectionChange -= MainWordApp_WindowSelectionChange;
    }
  }

  private void MainWordApp_WindowSelectionChange(Word.Selection selection) {
    if (string.IsNullOrWhiteSpace(selection.Text) || IsNoSelection || OriginalText == selection.Text) {
      return;
    }

    OriginalText = selection.Text;
  }

  private void ReverseLanguageCommand_Execute() {
    if (SourceLan == Language.Auto) {
      ShowMsgBox.Warning("目标语言不能设置为自动检测！");
      return;
    }

    (Sets.TransApiSourceLan, Sets.TransApiTargetLan) = (Sets.TransApiTargetLan, Sets.TransApiSourceLan);
    OnPropertyChanged(nameof(SourceLan));
    OnPropertyChanged(nameof(TargetLan));
  }

  private async Task TranslateCommand_Execute() {
    if (string.IsNullOrWhiteSpace(OriginalText)) {
      ShowMsgBox.Warning("请输入要翻译的文本！");
      return;
    }

    if (TransApiManager.CurrentTranslator == null) {
      ShowMsgBox.Warning("未选择翻译API！");
      return;
    }

    TranslatedText = string.Empty;
    UpdateCanExecute();
    TranslatedText = await TransApiManager.CurrentTranslator.TranslateAsync(OriginalText, SourceLan, TargetLan);
    UpdateCanExecute();
  }

  private void UpdateCanExecute() {
    CopyToBelowCommand.NotifyCanExecuteChanged();
    InsertResultCommand.NotifyCanExecuteChanged();
    CopyToClipboardCommand.NotifyCanExecuteChanged();
  }

  private void CopyToClipboardCommand_Execute() {
    if (string.IsNullOrWhiteSpace(TranslatedText)) {
      ShowMsgBox.Warning("当前结果为空！");
      return;
    }

    System.Windows.Clipboard.SetText(TranslatedText);
  }

  private void InsertResultCommand_Execute() {
    Selection?.TypeText(TranslatedText);
  }

  private void CopyToBelowCommand_Execute() {
    Word.Paragraph endPara = Selection.Range.Paragraphs.Last;
    bool isLastParagraph = endPara.Range.End == ActiveDoc.Content.End;
    if (string.IsNullOrWhiteSpace(endPara.Range.Text)) {
      string answer = isLastParagraph ? TranslatedText : TranslatedText + "\r";
      endPara.Range.Text = answer;
    } else {
      // 是最后一段，当前段落末尾的\r会被吞掉
      string answer = isLastParagraph ? "\r" + TranslatedText : TranslatedText + "\r";
      endPara.Range.InsertAfter(answer);
    }
  }

}