using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using WordAssistedTools.Models;
using Word = Microsoft.Office.Interop.Word;
using WordAssistedTools.Properties;
using WordAssistedTools.Utils;
using System.Windows.Forms;
using System.Collections;
using WordAssistedTools.Views;
using static WordAssistedTools.Utils.Methods;

namespace WordAssistedTools.ViewModels;

public class PreAutoPlanViewModel : ObservableObject {

  private readonly Word.Document _document;

  private int _totalWordsCount;
  public int TotalWordsCount {
    get => _totalWordsCount;
    set => SetProperty(ref _totalWordsCount, value);
  }
    
  private int _allParagraphCount;
  public int AllParagraphCount {
    get => _allParagraphCount;
    set => SetProperty(ref _allParagraphCount, value);
  }

  private double _estimatedSpeechSpeed;
  public double EstimatedSpeechSpeed {
    get => _estimatedSpeechSpeed;
    set {
      if (SetProperty(ref _estimatedSpeechSpeed, value)) {
        SpeechSpeedComment = value switch {
          <= 0 => "异常数值",
          < 125 => "很慢",
          < 175 => "较慢",
          < 225 => "适中",
          < 275 => "较快",
          < 325 => "极快",
          _ => "起飞"
        };
      }
    }
  }

  private string _speechSpeedComment;
  public string SpeechSpeedComment {
    get => _speechSpeedComment;
    set => SetProperty(ref _speechSpeedComment, value);
  }

  private double _realTotalTime;
  public double RealTotalTime {
    get => _realTotalTime;
    set => SetProperty(ref _realTotalTime, value);
  }

  private double _selectedUpperLimitTime = Shared.UpperLimitTimes[1];
  public double SelectedUpperLimitTime {
    get => _selectedUpperLimitTime;
    set {
      if (SetProperty(ref _selectedUpperLimitTime, value)) {
        UpdateFinalTimeAndSpeed();
      }
    }
  }

  private double _selectedFinalReservedTime = Shared.FinalReservedTimes[1];
  public double SelectedFinalReservedTime {
    get => _selectedFinalReservedTime;
    set {
      if (SetProperty(ref _selectedFinalReservedTime, value)) {
        UpdateFinalTimeAndSpeed();
      }
    }
  }

  private double _selectedChangeSlideTime = Shared.ChangeSlideTimes[1];
  public double SelectedChangeSlideTime {
    get => _selectedChangeSlideTime;
    set {
      if (SetProperty(ref _selectedChangeSlideTime, value)) {
        UpdateFinalTimeAndSpeed();
      }
    }
  }

  private void UpdateFinalTimeAndSpeed() {
    RealTotalTime = GetAllSeconds();
    EstimatedSpeechSpeed = GetWordsPerSecond() * 60;
  }

  private double GetAllSeconds() {
    return SelectedUpperLimitTime * 60 - SelectedFinalReservedTime;
  }

  private double GetWordsPerSecond() {
    List<ParagraphInfoViewModel> checkedParagraphs = GetCheckedParagraphs();
    double speechTotalSeconds = GetAllSeconds() - SelectedChangeSlideTime * (checkedParagraphs.Count - 1);
    return TotalWordsCount / speechTotalSeconds;
  }

  private IList _selectedParagraphs = new List<ParagraphInfoViewModel>();
  public IList SelectedParagraphs {
    get => _selectedParagraphs;
    set => SetProperty(ref _selectedParagraphs, value);
  }

  public ObservableCollection<ParagraphInfoViewModel> ParagraphInfoTable { get; } = new();
  public RelayCommand LoadWindowCommand { get; }
  public RelayCommand SaveAsDefaultSettingsCommand { get; }
  public RelayCommand RestoreEstimateParaWordCountCommand { get; }
  public RelayCommand RefreshPlanningResultsCommand { get; }
  public RelayCommand<Window> UpdateWordDocumentCommand { get; }
  public RelayCommand TableMenuSetCheckedCommand { get; }
  public RelayCommand TableMenuSetUncheckedCommand { get; }

  [Obsolete]
  public PreAutoPlanViewModel() { }

  public PreAutoPlanViewModel(Word.Document document) {
    _document = document;
    LoadWindowCommand = new RelayCommand(LoadWindowCommand_Execute);
    SaveAsDefaultSettingsCommand = new RelayCommand(SaveAsDefaultSettingsCommand_Execute);
    RestoreEstimateParaWordCountCommand = new RelayCommand(RestoreEstimateParaWordCountCommand_Execute);
    RefreshPlanningResultsCommand = new RelayCommand(RefreshPlanningResultsCommand_Execute);
    UpdateWordDocumentCommand = new RelayCommand<Window>(UpdateWordDocumentCommand_Execute);
    TableMenuSetCheckedCommand = new RelayCommand(TableMenuSetCheckedCommand_Execute);
    TableMenuSetUncheckedCommand = new RelayCommand(TableMenuSetUncheckedCommand_Execute);

    LoadUserSettings();
  }
    
  private void TableMenuSetCheckedCommand_Execute() {
    SetSelectedRowsChecked(true);
  }

  private void TableMenuSetUncheckedCommand_Execute() {
    SetSelectedRowsChecked(false);
  }
  private void SetSelectedRowsChecked(bool state) {
    foreach (object selectedItem in SelectedParagraphs) {
      if (selectedItem is ParagraphInfoViewModel item) {
        item.IsChecked = state;
      }
    }
  }

  private void LoadUserSettings() {
    SelectedUpperLimitTime = Settings.Default.UpperLimitTime;
    SelectedFinalReservedTime = Settings.Default.FinalReservedTime;
    SelectedChangeSlideTime = Settings.Default.ChangeSlideTime;
  }

  private void LoadWindowCommand_Execute() {
    WaitWindow waitWindow = new();
    waitWindow.Show();

    Word.Paragraphs paragraphs = _document.Paragraphs;
    int effectiveParasCount = 0;
    for (int i = 1; i <= paragraphs.Count; i++) {
      Word.Paragraph paragraph = paragraphs[i];
      string text = paragraph.Range.Text;
      if (text.Length < 2) {
        continue;
      }

      effectiveParasCount++;

      ParagraphInfoViewModel paragraphInfo = new() {
        OriginWordParaId = i,
        Text = text,
        IsChecked = true,
        RealParaWordCount = paragraph.Range.ComputeStatistics(Word.WdStatistic.wdStatisticWords),
      };

      paragraphInfo.EstimateParaWordCount = paragraphInfo.RealParaWordCount;

      if (paragraphInfo.Text.StartsWith("(")) {
        int rightBraceIndex = paragraphInfo.Text.IndexOf(")");
        if (rightBraceIndex > 0) {
          string presentStartToParaStartTimeStr = paragraphInfo.Text.Substring(1, rightBraceIndex - 1);
          if (TryConvertTimeStrToDouble(presentStartToParaStartTimeStr, out double presentStartToParaStartTime)) {
            //避免被converter过滤
            paragraphInfo.OldStartToParaStartSeconds = presentStartToParaStartTime == 0 ? 0.1 : presentStartToParaStartTime;

            //第一行不填，后一行填前一行的信息
            if (effectiveParasCount > 1) {
              ParagraphInfoViewModel lastParagraphInfo = ParagraphInfoTable[effectiveParasCount - 2];
              lastParagraphInfo.OldStartToParaEndSeconds = presentStartToParaStartTime - SelectedChangeSlideTime;
              if (lastParagraphInfo.OldStartToParaStartSeconds > 0) {
                double lastParaAllSeconds = presentStartToParaStartTime - lastParagraphInfo.OldStartToParaStartSeconds;
                lastParagraphInfo.OldOnlyParaSeconds = lastParaAllSeconds - SelectedChangeSlideTime;
              } else {
                paragraphInfo.OldOnlyParaSeconds = -10000;
              }
            }
          } else {
            paragraphInfo.OldStartToParaStartSeconds = -10000;
          }
        }
      }

      ParagraphInfoTable.Add(paragraphInfo);
      paragraphInfo.UpdateParaInfoEvent += ParagraphInfo_UpdateParaInfoEvent;
    }

    if (ParagraphInfoTable.Count > 0) {
      ParagraphInfoViewModel finalPara = ParagraphInfoTable[ParagraphInfoTable.Count - 1];
      if (finalPara.Text.TrimEnd().EndsWith(")")) {
        int leftBraceIndex = finalPara.Text.LastIndexOf("(");
        if (leftBraceIndex > 0) {
          string paraText = finalPara.Text.TrimEnd();
          string finalParaEndTimeStr = paraText.Substring(leftBraceIndex + 1, paraText.Length - 2 - leftBraceIndex);
          if (TryConvertTimeStrToDouble(finalParaEndTimeStr, out double finalParaEndTime)) {
            finalPara.OldStartToParaEndSeconds = finalParaEndTime;
            if (finalPara.OldStartToParaStartSeconds > 0) {
              double onlyParaTime = finalParaEndTime - finalPara.OldStartToParaStartSeconds;
              finalPara.OldOnlyParaSeconds = onlyParaTime;
            } else {
              finalPara.OldOnlyParaSeconds = -10000;
            }

          }
        }
      }
    }

    UpdateStatisticInfo();

    waitWindow.Close();
  }

  private void ParagraphInfo_UpdateParaInfoEvent(object sender, EventArgs e) {
    UpdateStatisticInfo();
  }

  private void UpdateStatisticInfo() {
    TotalWordsCount = ParagraphInfoTable.Where(item => item.IsChecked).Sum(item => item.EstimateParaWordCount);
    AllParagraphCount = ParagraphInfoTable.Count(item => item.IsChecked);
    UpdateFinalTimeAndSpeed();
  }

  private void SaveAsDefaultSettingsCommand_Execute() {
    Settings.Default.UpperLimitTime = SelectedUpperLimitTime;
    Settings.Default.FinalReservedTime = SelectedFinalReservedTime;
    Settings.Default.ChangeSlideTime = SelectedChangeSlideTime;
    Settings.Default.Save();
    ShowMsgBox.Info("默认配置信息已成功更新！");
  }

  private void RestoreEstimateParaWordCountCommand_Execute() {
    List<ParagraphInfoViewModel> checkedParagraphs = GetCheckedParagraphs();
    foreach (ParagraphInfoViewModel paragraphInfo in checkedParagraphs) {
      paragraphInfo.EstimateParaWordCount = paragraphInfo.RealParaWordCount;
    }
  }


  private List<ParagraphInfoViewModel> GetCheckedParagraphs() {
    return ParagraphInfoTable.Where(p => p.IsChecked).ToList();
  }

  private bool _hasPlanned = false;

  private void RefreshPlanningResultsCommand_Execute() {
    if (EstimatedSpeechSpeed <= 0) {
      ShowMsgBox.Error("异常数值！");
      return;
    }

    if (EstimatedSpeechSpeed >= 325) {
      DialogResult result = ShowMsgBox.QuestionOkCancel("当前速度非常快，确认继续规划？\r\n点击“确定”继续；\r\n点击“取消”放弃。");
      if (result == DialogResult.Cancel) {
        return;
      }
    }

    List<ParagraphInfoViewModel> checkedParagraphs = GetCheckedParagraphs();
    double speedPerSecond = GetWordsPerSecond();
    int totalWordsToLastEnd = 0;
    for (int i = 0; i < checkedParagraphs.Count; i++) {
      ParagraphInfoViewModel paragraphInfo = checkedParagraphs[i];
      double onlyParaSeconds = paragraphInfo.EstimateParaWordCount / speedPerSecond;
      paragraphInfo.NewOnlyParaSeconds = onlyParaSeconds;

      double startToLastParaSeconds = totalWordsToLastEnd / speedPerSecond + SelectedChangeSlideTime * i;
      paragraphInfo.NewStartToParaStartSeconds = startToLastParaSeconds == 0 ? 0.01 : startToLastParaSeconds;
      paragraphInfo.NewStartToParaEndSeconds = onlyParaSeconds + startToLastParaSeconds;

      totalWordsToLastEnd += paragraphInfo.EstimateParaWordCount;
    }

    _hasPlanned = true;
  }


  private void UpdateWordDocumentCommand_Execute(Window window) {
    if (!_hasPlanned) {
      ShowMsgBox.Warning("请先进行时间规划！");
      return;
    }

    Word.Paragraphs paragraphs = _document.Paragraphs;
    for (int i = 0; i < ParagraphInfoTable.Count; i++) {
      ParagraphInfoViewModel paragraphInfo = ParagraphInfoTable[i];
      if (!paragraphInfo.IsChecked) {
        continue;
      }

      Word.Paragraph paragraph = paragraphs[paragraphInfo.OriginWordParaId];
      string text = paragraphInfo.Text;
      bool hasOldTime = false;
      if (text.StartsWith("(")) {
        int rightBraceIndex = text.IndexOf(")");
        if (rightBraceIndex > 0) {
          string originTime = text.Substring(1, rightBraceIndex - 1);
          if (TryConvertTimeStrToDouble(originTime, out double _)) {
            Word.Range range = paragraph.Range;
            range.Find.Execute(originTime, MatchWholeWord: false);
            if (range.Text == originTime) {
              range.Text = ConvertSecondsToTimeStr(paragraphInfo.NewStartToParaStartSeconds);
              hasOldTime = true;
            }
          }
        }
      }
      if (!hasOldTime) {
        paragraph.Range.InsertBefore($"({ConvertSecondsToTimeStr(paragraphInfo.NewStartToParaStartSeconds)})");
      }

      bool hasOldEndTime = false;
      if (i == ParagraphInfoTable.Count - 1) {
        if (text.TrimEnd().EndsWith(")")) {
          int leftBraceIndex = text.LastIndexOf("(");
          if (leftBraceIndex > 0) {
            string originEndTime = text.Substring(leftBraceIndex + 1, text.TrimEnd().Length - 2 - leftBraceIndex);
            if (TryConvertTimeStrToDouble(originEndTime, out double _)) {
              Word.Range range = paragraph.Range;
              range.Find.Execute(originEndTime, MatchWholeWord: false);
              if (range.Text == originEndTime) {
                range.Text = ConvertSecondsToTimeStr(paragraphInfo.NewStartToParaEndSeconds);
                hasOldEndTime = true;
              }
            }
          }
        }

        if (!hasOldEndTime) {
          Word.Range range = paragraph.Range;
          range.End--;
          range.InsertAfter($"({ConvertSecondsToTimeStr(paragraphInfo.NewStartToParaEndSeconds)})");
        }
      }

      window.Close();
    }
  }
}