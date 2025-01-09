using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Core;
using WordAssistedTools.Models;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using WordAssistedTools.Utils;
using Word = Microsoft.Office.Interop.Word;
using WordAssistedTools.Properties;
using WordAssistedTools.Views;
using static WordAssistedTools.Utils.Methods;
using static WordAssistedTools.Models.Shared;

namespace WordAssistedTools.ViewModels;

public class PreExportToPptViewModel : ObservableObject {
  private readonly Word.Document _document;

  private string _exportPptPath;
  public string ExportPptPath {
    get => _exportPptPath;
    set {
      if (SetProperty(ref _exportPptPath, value)) {
        if (IsAutoLoadPptAfterBrowseChecked) {
          LoadPptCommand_Execute();
        }
      }
    }
  }

  private string _compareViewWordText;
  public string CompareViewWordText {
    get => _compareViewWordText;
    set => SetProperty(ref _compareViewWordText, value);
  }

  private string _compareViewPptText;
  public string CompareViewPptText {
    get => _compareViewPptText;
    set => SetProperty(ref _compareViewPptText, value);
  }

  private bool _isAutoLoadPptAfterBrowseChecked;
  public bool IsAutoLoadPptAfterBrowseChecked {
    get => _isAutoLoadPptAfterBrowseChecked;
    set => SetProperty(ref _isAutoLoadPptAfterBrowseChecked, value);
  }

  private bool? _isSelectAllItemsChecked;
  public bool? IsSelectAllItemsChecked {
    get => _isSelectAllItemsChecked;
    set {
      if (SetProperty(ref _isSelectAllItemsChecked, value)) {
        bool? isSelectAll = IsSelectAllItemsChecked;
        if (isSelectAll.HasValue) {
          foreach (WordPptCompareViewModel item in WordPptCompareTable) {
            item.IsChecked = isSelectAll.Value;
          }
        }
      }
    }
  }

  private bool _isShowDifferenceChecked;
  public bool IsShowDifferenceChecked {
    get => _isShowDifferenceChecked;
    set => SetProperty(ref _isShowDifferenceChecked, value);
  }

  private IList _selectedCompares = new List<WordPptCompareViewModel>();
  public IList SelectedCompares {
    get => _selectedCompares;
    set => SetProperty(ref _selectedCompares, value);
  }

  private DifferenceType _differenceType = DifferenceType.OnlyText;
  public DifferenceType DifferenceType {
    get => _differenceType;
    set => SetProperty(ref _differenceType, value);
  }


  public ObservableCollection<WordPptCompareViewModel> WordPptCompareTable { get; } = new();
  public RelayCommand LoadWindowCommand { get; }
  public RelayCommand BrowsePptPathCommand { get; }
  public RelayCommand LoadPptCommand { get; }
  public RelayCommand SelectionChangedCommand { get; }
  public RelayCommand ShowDifferenceCommand { get; }
  public RelayCommand<Window> UpdateWordDocumentCommand { get; }
  public RelayCommand<Window> OverwritePptCommand { get; }
  public RelayCommand<Window> CancelCommand { get; }
  public RelayCommand WindowClosingCommand { get; }
  public RelayCommand TableMenuSetCheckedCommand { get; }
  public RelayCommand TableMenuSetUncheckedCommand { get; }

  [Obsolete]
  public PreExportToPptViewModel() { }

  public PreExportToPptViewModel(Word.Document document) {
    _document = document;
    LoadWindowCommand = new RelayCommand(LoadWindowCommand_Execute);
    BrowsePptPathCommand = new RelayCommand(BrowsePptPathCommand_Execute);
    LoadPptCommand = new RelayCommand(LoadPptCommand_Execute);
    SelectionChangedCommand = new RelayCommand(SelectionChangedCommand_Execute);
    ShowDifferenceCommand = new RelayCommand(ShowDifferenceCommand_Execute);
    UpdateWordDocumentCommand = new RelayCommand<Window>(UpdateWordDocumentCommand_Execute);
    OverwritePptCommand = new RelayCommand<Window>(OverwritePptCommand_Execute);
    CancelCommand = new RelayCommand<Window>(CancelCommand_Execute);
    WindowClosingCommand = new RelayCommand(WindowClosingCommand_Execute);
    TableMenuSetCheckedCommand = new RelayCommand(TableMenuSetCheckedCommand_Execute);
    TableMenuSetUncheckedCommand = new RelayCommand(TableMenuSetUncheckedCommand_Execute);

    LoadUserSettings();
  }



  private List<WordPptCompareViewModel> GetCheckedCompares() {
    return WordPptCompareTable.Where(c => c.IsChecked).ToList();
  }

  private List<Dictionary<ProcessType, (string, string)>> _wordToPptNameRulesList;

  private void LoadUserSettings() {
    IsAutoLoadPptAfterBrowseChecked = Settings.Default.IsAutoLoadAfterBrowse;
    IsShowDifferenceChecked = Settings.Default.IsAutoShowDifferAfterLoad;
    DifferenceType = Settings.Default.DifferenceType.ToDifferenceType();
    bool flag = WordToPptRulesUtils.TryParseWordToPptRules(Settings.Default.WordToPptRules, out _wordToPptNameRulesList);
    if (!flag) {
      ShowMsgBox.Error("由于未知的异常，命名规则未能成功解析！");
    }
  }

  private void LoadWindowCommand_Execute() {
    WaitWindow waitWindow = new();
    waitWindow.Show();
    Word.Paragraphs paragraphs = _document.Paragraphs;

    for (int i = 1; i <= paragraphs.Count; i++) {
      Word.Paragraph paragraph = paragraphs[i];
      string text = paragraph.Range.Text;
      if (text.Length < 2) {
        continue;
      }

      if (text.StartsWith("(")) {
        int rightBraceIndex = text.IndexOf(")");
        if (rightBraceIndex > 0) {
          string originTime = text.Substring(1, rightBraceIndex - 1);
          string originTimeWithBraces = text.Substring(0, rightBraceIndex + 1);
          if (TryConvertTimeStrToDouble(originTime, out double _)) {
            WordPptCompareViewModel wordPptCompareItem = new() {
              WordId = i,
              IsChecked = true,
              WordTime = originTimeWithBraces,
              WordText = text.Replace(originTimeWithBraces, string.Empty),
            };
            wordPptCompareItem.UpdateCheckedItemsEvent += WordPptCompareItem_UpdateCheckedItemsEvent;
            WordPptCompareTable.Add(wordPptCompareItem);
          }
        }
      }
    }

    IsSelectAllItemsChecked = true;

    string docPath = _document.FullName;
    string dirPath = Path.GetDirectoryName(docPath);
    if (dirPath != null) {
      string fileName = Path.GetFileNameWithoutExtension(docPath);
      foreach (Dictionary<ProcessType, (string, string)> rule in _wordToPptNameRulesList.Where(rule => rule.Count != 0)) {
        foreach (KeyValuePair<ProcessType, (string, string)> pair in rule) {
          switch (pair.Key) {
            case ProcessType.LeftAdd:
              fileName = pair.Value.Item1 + fileName;
              break;
            case ProcessType.RightAdd:
              fileName += pair.Value.Item1;
              break;
            case ProcessType.Remove:
              fileName = fileName.Replace(pair.Value.Item1, string.Empty);
              break;
            case ProcessType.Replace:
              fileName = fileName.Replace(pair.Value.Item1, pair.Value.Item2);
              break;
          }
        }

        string tryGetPptPath = Path.Combine(dirPath!, $"{fileName}.pptx");
        if (File.Exists(tryGetPptPath)) {
          ExportPptPath = tryGetPptPath;
          break;
        }
      }
    }

    waitWindow.Close();
  }

  private void WordPptCompareItem_UpdateCheckedItemsEvent(object sender, EventArgs e) {
    List<WordPptCompareViewModel> checkedCompares = GetCheckedCompares();
    if (checkedCompares.Count == 0) {
      IsSelectAllItemsChecked = false;
    } else if (checkedCompares.Count == WordPptCompareTable.Count) {
      IsSelectAllItemsChecked = true;
    } else {
      IsSelectAllItemsChecked = null;
    }
  }

  private void TableMenuSetCheckedCommand_Execute() {
    SetSelectedRowsChecked(true);
  }

  private void TableMenuSetUncheckedCommand_Execute() {
    SetSelectedRowsChecked(false);
  }

  private void SetSelectedRowsChecked(bool state) {
    foreach (object selectedItem in SelectedCompares) {
      if (selectedItem is WordPptCompareViewModel item) {
        item.IsChecked = state;
      }
    }
  }

  private void BrowsePptPathCommand_Execute() {
    OpenFileDialog dialog = new() {
      Title = "选择要导出到的PPT文件",
      Filter = "PowerPoint 演示文稿 (*.pptx)|*.pptx",
    };

    if (dialog.ShowDialog() != DialogResult.OK) {
      return;
    }

    ExportPptPath = dialog.FileName;
  }

  private PowerPoint.Application _powerpoint;
  private PowerPoint.Presentation _presentation;
  private readonly List<PowerPoint.TextRange> _notesTextRanges = new();

  private void LoadPptCommand_Execute() {
    if (!File.Exists(ExportPptPath)) {
      ShowMsgBox.Error("导出PPT文件不存在！");
      return;
    }

    ClosePptApplication();
    _powerpoint = new PowerPoint.Application();
    _presentation = _powerpoint.Presentations.Open(ExportPptPath, WithWindow: MsoTriState.msoFalse);

    if (WordPptCompareTable.Count != _presentation.Slides.Count) {
      ShowMsgBox.Error("当前段落总数与PPT幻灯片个数不一致，请返回修改文档或检查PPT。");
      ClosePptApplication();
      return;
    }

    for (int i = 0; i < WordPptCompareTable.Count; i++) {
      WordPptCompareViewModel wordPptCompareItem = WordPptCompareTable[i];
      PowerPoint.Slide slide = _presentation.Slides[i + 1];
      if (slide.HasNotesPage != MsoTriState.msoTrue) {
        continue;
      }

      PowerPoint.SlideRange notesPages = slide.NotesPage;
      foreach (PowerPoint.Shape shape in notesPages.Shapes.Cast<PowerPoint.Shape>().Where(shape => shape.Type == MsoShapeType.msoPlaceholder && shape.PlaceholderFormat.Type == PowerPoint.PpPlaceholderType.ppPlaceholderBody)) {
        _notesTextRanges.Add(shape.TextFrame.TextRange);
        string note = shape.TextFrame.TextRange.Text;

        bool hasFindTime = false;
        if (note.StartsWith("(")) {
          int rightBraceIndex = note.IndexOf(")");
          if (rightBraceIndex > 0) {
            string originTime = note.Substring(1, rightBraceIndex - 1);
            string originTimeWithBraces = note.Substring(0, rightBraceIndex + 1);
            if (TryConvertTimeStrToDouble(originTime, out double _)) {
              wordPptCompareItem.PptTime = originTimeWithBraces;
              wordPptCompareItem.PptText = note.Replace(originTimeWithBraces, string.Empty);
              hasFindTime = true;
            }
          }
        }

        if (!hasFindTime) {
          wordPptCompareItem.PptText = note;
        }

        break;
      }
    }

    if (IsShowDifferenceChecked) {
      UpdateCompareDiffers();
    }
  }

  private void SelectionChangedCommand_Execute() {
    if (SelectedCompares.Count != 1) {
      CompareViewWordText = "选择一行以开始细节比较。";
      CompareViewPptText = "选择一行以开始细节比较。";
      return;
    }

    WordPptCompareViewModel item = (WordPptCompareViewModel)SelectedCompares[0];
    if (!string.IsNullOrWhiteSpace(item.WordText) && !(string.IsNullOrWhiteSpace(item.PptText))) {
      CompareViewWordText = item.WordText.TrimEnd();
      CompareViewPptText = item.PptText.TrimEnd();
    } else {
      CompareViewWordText = "当前行数据不完整。";
      CompareViewPptText = "当前行数据不完整。";
    }
  }

  private void UpdateCompareDiffers() {
    if (_presentation == null) {
      return;
    }

    foreach (WordPptCompareViewModel item in WordPptCompareTable) {
      if (!IsShowDifferenceChecked) {
        item.IsShowDiffer = false;
      } else {
        if (DifferenceType == DifferenceType.OnlyText) {
          item.IsShowDiffer = !item.IsSameText;
        } else if (DifferenceType == DifferenceType.TimeAndText) {
          item.IsShowDiffer = !item.IsSameTime || !item.IsSameText;
        }
      }
    }
  }

  private void ShowDifferenceCommand_Execute() {
    UpdateCompareDiffers();
  }

  private void UpdateWordDocumentCommand_Execute(Window window) {
    if (_presentation == null) {
      ShowMsgBox.Error("请先加载PPT文件！");
      return;
    }

    DialogResult result = ShowMsgBox.WarningOkCancel(
      "确定将勾选项的PPT备注替换现有Word文档吗（空备注或文本内容相同将被跳过，否则内容将替换为PPT中的备注，后续务必重新规划）？注意：此操作不可逆。\r\n点击“确定”进行替换；\r\n点击“取消”放弃操作。");
    if (result == DialogResult.Cancel) {
      return;
    }

    Word.Paragraphs paragraphs = _document.Paragraphs;

    List<WordPptCompareViewModel> checkedCompares = GetCheckedCompares();
    foreach (WordPptCompareViewModel compare in checkedCompares.Where(compare =>
               !string.IsNullOrWhiteSpace(compare.PptText) || !compare.IsSameText)) {
      Word.Range range = paragraphs[compare.WordId].Range;
      range.Text = compare.WordTime + compare.PptText.TrimEnd() + "\r";

      //这里如果不执行下面这段，分的字母会变成宋体，改好像又改不过来，但是把中间部分整个换回存储的时间字符串可行
      if (range.Text.StartsWith("(")) {
        int rightBraceIndex = range.Text.IndexOf(")");
        if (rightBraceIndex > 0) {
          string originTime = range.Text.Substring(1, rightBraceIndex - 1);
          range.Find.Execute(originTime, MatchWholeWord: false);
          if (range.Text == originTime) {
            range.Text = originTime;
          }
        }
      }
    }

    ShowMsgBox.Info("替换成功！");
    ClosePptApplication();
    window.Close();
  }

  private void OverwritePptCommand_Execute(Window window) {
    if (_presentation == null) {
      ShowMsgBox.Error("PPT暂未加载或加载异常!");
      return;
    }

    DialogResult overwriteResult = ShowMsgBox.WarningOkCancel("确定将勾选项的Word文档覆盖PPT中的备注吗（时间与内容均将覆盖）？注意：此操作不可逆。\r\n点击“确定”进行覆盖；\r\n点击“取消”放弃操作。");
    if (overwriteResult == DialogResult.Cancel) {
      return;
    }

    List<WordPptCompareViewModel> checkedCompares = GetCheckedCompares();
    for (int i = 0; i < checkedCompares.Count; i++) {
      if (checkedCompares[i].IsSameTime && checkedCompares[i].IsSameText) {
        continue;
      }

      _notesTextRanges[i].Text = checkedCompares[i].WordTime + checkedCompares[i].WordText.TrimEnd();
    }

    try {
      _presentation.Save();
      ClosePptApplication();

      DialogResult openResult = ShowMsgBox.QuestionOkCancel("备注修改并保存成功！是否立即打开PPT文件？\r\n点击“确定”打开；\r\n点击“取消”不执行任何操作。");
      if (openResult == DialogResult.OK) {
        if (File.Exists(ExportPptPath)) {
          Process.Start(ExportPptPath);
        }
      }
    } catch (Exception ex) {
      FileLog.Error(ex);
      ShowMsgBox.Error("文件被占用，未能成功保存！");
    }

    window.Close();
  }

  private void CancelCommand_Execute(Window window) {
    ClosePptApplication();
    window.Close();
  }

  private void WindowClosingCommand_Execute() {
    ClosePptApplication();
  }

  private void ClosePptApplication() {
    //关闭时没退出_presentation经常出问题
    _presentation?.Close();
    _powerpoint?.Quit();
    _presentation = null;
    _powerpoint = null;
    GC.Collect();
  }

}