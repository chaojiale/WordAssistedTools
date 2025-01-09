using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WordAssistedTools.Models;
using WordAssistedTools.Utils;
using WordAssistedTools.Views;
using Word = Microsoft.Office.Interop.Word;
using WordAssistedTools.Properties;

namespace WordAssistedTools.ViewModels;

internal class EditRequestInfoViewModel : ObservableObject {
  private Word.Application MainWordApp => Globals.ThisAddIn.Application;
  private Word.Document ActiveDoc => MainWordApp.ActiveDocument;
  private Settings Sets => Settings.Default;

  private ReferenceType _referenceTarget;
  public ReferenceType ReferenceTarget {
    get => _referenceTarget;
    set {
      if (SetProperty(ref _referenceTarget, value)) {
        OnPropertyChanged(nameof(CombineResultShowing));
      }
    }
  }

  private string _otherInfo;
  public string OtherInfo {
    get => _otherInfo;
    set {
      if (SetProperty(ref _otherInfo, value)) {
        OnPropertyChanged(nameof(CombineResultShowing));
      }
    }
  }


  private bool _isExpandCombineResult;
  public bool IsExpandCombineResult {
    get => _isExpandCombineResult;
    set {
      if (SetProperty(ref _isExpandCombineResult, value)) {
        OnPropertyChanged(nameof(CombineResultShowing));
      }
    }
  }

  private string _foldedCombineResult = string.Empty;
  public string FoldedCombineResult {
    get => _foldedCombineResult;
    set {
      if (SetProperty(ref _foldedCombineResult, value)) {
        OnPropertyChanged(nameof(CombineResultShowing));
      }
    }
  }


  private string ExpandedCombineResult {
    get {
      string referenceText;
      switch (ReferenceTarget) {
        case ReferenceType.None:
          referenceText = "(none)";
          break;
        case ReferenceType.Selected:
          referenceText = MainWordApp.Selection.Text.Trim();
          break;
        case ReferenceType.CurrentParagraph:
          //获取选择区域的开始位置所在段至结束位置所在段的文本
          StringBuilder result = new();
          foreach (Word.Paragraph para in ActiveDoc.Application.Selection.Range.Paragraphs) {
            result.Append(para.Range.Text);
          }

          referenceText = result.ToString().Trim();
          break;
        case ReferenceType.FullText:
          referenceText = ActiveDoc.Content.Text.Trim();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      return FoldedCombineResult.Replace("$$$", referenceText).Replace("###", OtherInfo).Replace("\r\n\r\n\r\n", "\r\n\r\n").Trim();
    }
  }

  public string CombineResultShowing {
    get {
      if (!IsExpandCombineResult) {
        return FoldedCombineResult?.Trim();
      }

      return ExpandedCombineResult;
    }
  }


  public ObservableCollection<NameContentItemViewModel> QuestionTemplateItems { get; } = [];


  public RelayCommand ControlLoadedCommand { get; }
  public RelayCommand LoadTemplatesCommand { get; }
  public RelayCommand EditTemplatesCommand { get; }
  public RelayCommand CopyToClipboardCommand { get; }
  public RelayCommand StartApiRequestCommand { get; }
  public RelayCommand CopyToWebPageCommand { get; }

  public EditRequestInfoViewModel() {
    ControlLoadedCommand = new RelayCommand(LoadTemplatesCommand_Execute);
    LoadTemplatesCommand = new RelayCommand(LoadTemplatesCommand_Execute);
    EditTemplatesCommand = new RelayCommand(EditTemplatesCommand_Execute);
    CopyToClipboardCommand = new RelayCommand(CopyToClipboardCommand_Execute);
    StartApiRequestCommand = new RelayCommand(StartApiRequestCommand_Execute);
    CopyToWebPageCommand = new RelayCommand(CopyToWebPageCommand_Execute);
  }

  private void Template_OnIsCheckedChanged(object sender, string e) {
    FoldedCombineResult = e;
  }

  private void LoadTemplatesCommand_Execute() {
    QuestionTemplateItems.Clear();
    List<NameContentItemViewModel> templates = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.GptTemplateConfigJsonPath);
    foreach (NameContentItemViewModel template in templates) {
      template.OnIsCheckedChanged += Template_OnIsCheckedChanged;
      if (template.Name == Sets.GptApiTemplateName) {
        template.IsChecked = true;
        FoldedCombineResult = template.Content;
      }
      QuestionTemplateItems.Add(template);
    }
  }

  private void EditTemplatesCommand_Execute() {
    NameContentConfigViewModel viewModel = new(ThemeTypeEnum.Template, "模板名", "内容");
    GptTemplateConfig config = new() {
      DataContext = viewModel
    };
    viewModel.OnSaveEvent += ViewModel_OnSaveEvent;
    config.ShowDialog();
  }

  private void ViewModel_OnSaveEvent(object sender, EventArgs e) {
    LoadTemplatesCommand.Execute(null);
  }

  private void CopyToClipboardCommand_Execute() {
    Clipboard.SetText(ExpandedCombineResult);
  }

  private void StartApiRequestCommand_Execute() {
    string request = ExpandedCombineResult;
    if (string.IsNullOrWhiteSpace(request)) {
      ShowMsgBox.Error("请先完善请求信息！");
      return;
    }

    OnApiRequested?.Invoke(this, request);
  }

  private void CopyToWebPageCommand_Execute() {
    string request = ExpandedCombineResult;
    if (string.IsNullOrWhiteSpace(request)) {
      ShowMsgBox.Error("请先完善请求信息！");
      return;
    }

    OnCopyWebPage?.Invoke(this, request);
  }

  public event EventHandler<string> OnApiRequested;
  public event EventHandler<string> OnCopyWebPage;
}
