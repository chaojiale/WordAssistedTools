// #define ForYearYear
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Compression;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Web.WebView2.Core;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using WordAssistedTools.Utils;
using WordAssistedTools.ViewModels;
using WordAssistedTools.Views;
using WordAssistedTools.Properties;
using static WordAssistedTools.Utils.Methods;
using static WordAssistedTools.Models.Shared;
using System.Diagnostics;
using WordAssistedTools.Models;
using System.Timers;
using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Net;


namespace WordAssistedTools;

internal partial class MainRibbon {
  private Word.Application MainWordApp => Globals.ThisAddIn.Application;
  private Word.Window ActiveWindow => MainWordApp.ActiveWindow;
  private Word.Document ActiveDoc => MainWordApp.ActiveDocument;
  private string DocPath => ActiveDoc.Path;
  private string DocName => ActiveDoc.Name;
  private string DocNameNoExt => Path.GetFileNameWithoutExtension(DocName);
  private string DocFullPath => Path.Combine(DocPath, DocName);

  private Settings Sets => Settings.Default;

  private MainSideBar RightSideBar { get; set; }
  private CustomTaskPane CustomRightTaskPane { get; set; }

  private void RibbonTools_Load(object sender, RibbonUIEventArgs e) {
    InitializeDropdownItems();

#if !DEBUG
      groupDev.Visible = false;
#endif
#if !ForYearYear
    sbtEncourageMyself.Visible = false;
#endif

    chkUseLocalData.Checked = Sets.UseLocalData;

    LoggingConfiguration config = new();
    FileTarget logfile = new("logfile") {
      FileName = Paths.LogFilePath,
    };

    config.AddTarget(logfile);
    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
    LogManager.Configuration = config;
    FileLog = LogManager.GetCurrentClassLogger();
    FileLog.Info("日志记录启动");

    MessageTimer.Elapsed += MessageTimer_Tick;
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
  }

  private void MessageTimer_Tick(object sender, ElapsedEventArgs e) {
    try {
      MainWordApp.StatusBar = string.Empty;
    } catch (Exception ex) {
      FileLog.Error(ex);
    }
    MessageTimer.Stop();
  }

  private System.Timers.Timer MessageTimer { get; } = new() {
    Interval = 5000,
  };

  /// <summary>
  /// 状态栏显示延时信息
  /// </summary>
  /// <param name="message"></param>
  private void ShowStatusMessage(string message) {
    try {
      MessageTimer.Stop();
      MainWordApp.StatusBar = message;
      MessageTimer.Start();
    } catch (Exception ex) {
      FileLog.Error(ex);
    }
  }


  public List<NameContentItemViewModel> TransWebItems { get; private set; }

  private void UpdateTransWebConfig() {
    if (!File.Exists(Paths.TransWebConfigJsonPath)) {
      return;
    }
    TransWebItems = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.TransWebConfigJsonPath);
    dpdTransWebType.Items.Clear();
    foreach (NameContentItemViewModel config in TransWebItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = config.Name;
      dpdTransWebType.Items.Add(downItem);
      if (config.Name == Sets.TransWebName) {
        dpdTransWebType.SelectedItem = downItem;
      }
    }
  }

  private void UpdateTransApiConfig() {
    dpdTransApiType.Items.Clear();
    foreach (TransApiItemViewModel item in TransApiManager.TransApiItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = item.Name;
      dpdTransApiType.Items.Add(downItem);
      if (item.Name == Sets.TransApiName) {
        dpdTransApiType.SelectedItem = downItem;
      }
    }
  }

  public List<NameContentItemViewModel> GptWebItems { get; private set; }

  private void UpdateGptWebConfig() {
    if (!File.Exists(Paths.GptWebConfigJsonPath)) {
      return;
    }
    GptWebItems = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.GptWebConfigJsonPath);
    dpdGptWebType.Items.Clear();
    foreach (NameContentItemViewModel config in GptWebItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = config.Name;
      dpdGptWebType.Items.Add(downItem);
      if (config.Name == Sets.GptWebName) {
        dpdGptWebType.SelectedItem = downItem;
      }
    }
  }

  public List<GptApiItemViewModel> GptApiItems { get; private set; }
  private void UpdateGptApiConfig() {
    if (!File.Exists(Paths.GptApiConfigJsonPath)) {
      return;
    }
    GptApiItems = JsonTool.LoadJsonFile<GptApiItemViewModel>(Paths.GptApiConfigJsonPath);
    dpdGptApiType.Items.Clear();
    foreach (GptApiItemViewModel config in GptApiItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = config.Name;
      dpdGptApiType.Items.Add(downItem);
      if (config.Name == Sets.GptApiName) {
        dpdGptApiType.SelectedItem = downItem;
      }
    }

    UpdateGptApiModelAndTem();
  }

  private void UpdateGptApiModelAndTem() {
    GptApiItemViewModel currentSelected = GptApiItems.FirstOrDefault(x => x.Name == Sets.GptApiName);
    if (currentSelected == null) {
      return;
    }

    dpdGptApiModel.Items.Clear();
    foreach (string model in currentSelected.Models) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = model;
      dpdGptApiModel.Items.Add(downItem);
      if (model == currentSelected.DefaultModel) {
        dpdGptApiModel.SelectedItem = downItem;
      }
    }

    dpdGptApiTemperature.Items.Clear();
    for (int i = currentSelected.LowerTem; i <= currentSelected.UpperTem; i++) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = i.ToString();
      dpdGptApiTemperature.Items.Add(downItem);
      if (i == currentSelected.DefaultTem) {
        dpdGptApiTemperature.SelectedItem = downItem;
      }
    }

    UpdateCurrentApiConfig();
  }

  private void UpdateCurrentApiConfig() {
    GptApiItemViewModel currentGptApi = GptApiItems.FirstOrDefault(x => x.Name == Sets.GptApiName);
    if (currentGptApi == null) {
      return;
    }

    GeneralGpt.GptApiUrl = currentGptApi.Url;
    GeneralGpt.GptApiKey = currentGptApi.Key.StartsWith("$") ? Environment.GetEnvironmentVariable(currentGptApi.Key.Substring(1)) : currentGptApi.Key;
    GeneralGpt.GptApiModel = dpdGptApiModel.SelectedItem.Label;
    GeneralGpt.GptApiTem = int.Parse(dpdGptApiTemperature.SelectedItem.Label) * 0.1f;
    GeneralGpt.GptApiSystem = currentGptApi.System;
  }

  private void InitializeDropdownItems() {
    foreach (int scale in WebScaleItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = scale.ToString();
      dpdTransWebScale.Items.Add(downItem);
    }
    dpdTransWebScale.SelectedItem = dpdTransWebScale.Items.FirstOrDefault(x => x.Label == Sets.TransWebScale.ToString());

    foreach (int scale in WebScaleItems) {
      RibbonDropDownItem downItem = Factory.CreateRibbonDropDownItem();
      downItem.Label = scale.ToString();
      dpdGptWebScale.Items.Add(downItem);
    }
    dpdGptWebScale.SelectedItem = dpdGptWebScale.Items.FirstOrDefault(x => x.Label == Sets.GptWebScale.ToString());

    UpdateTransWebConfig();
    UpdateTransApiConfig();
    UpdateGptWebConfig();
    UpdateGptApiConfig();
  }

  #region 插件管理
  private void btnSetsSettings_Click(object sender, RibbonControlEventArgs e) {
    UserSettingsViewModel viewModel = new();
    UserSettings userSettings = new() {
      DataContext = viewModel
    };
    viewModel.OnConfirm += UserSettingsViewModel_OnConfirm;
    userSettings.ShowDialog();
  }


  private void UserSettingsViewModel_OnConfirm(object sender, SettingsUpdateEventArgs e) {
    if (e.IsUpdateTransWeb) {
      UpdateTransWebConfig();
    }

    if (e.IsUpdateTransApi) {
      UpdateTransApiConfig();
    }

    if (e.IsUpdateGptWeb) {
      UpdateGptWebConfig();
    }

    if (e.IsUpdateGptApi) {
      UpdateGptApiConfig();
    }

    //dpdTransWebType.SelectedItem = dpdTransWebType.Items.FirstOrDefault(x => x.Label == Sets.TransWebTypeName);
    //dpdTransWebScale.SelectedItem = dpdTransWebScale.Items.FirstOrDefault(x => x.Label == Sets.TransWebScale.ToString());
    //dpdTransApiType.SelectedItem = dpdTransApiType.Items.FirstOrDefault(x => x.Label == Sets.TransApiTypeName);
    //chkTransApiAutoUpdate.Checked = Sets.TransApiAutoUpdate;

    //dpdAiWebType.SelectedItem = dpdAiWebType.Items.FirstOrDefault(x => x.Label == Sets.AiWebTypeName);
    //dpdAiWebScale.SelectedItem = dpdAiWebScale.Items.FirstOrDefault(x => x.Label == Sets.AiWebScale.ToString());
    //dpdAiApiType.SelectedItem = dpdAiApiType.Items.FirstOrDefault(x => x.Label == Sets.AiApiTypeName);
    //dpdAiApiModel.SelectedItem = dpdAiApiModel.Items.FirstOrDefault(x => x.Label == Sets.AiApiModel);
    //dpdAiApiTemperature.SelectedItem = dpdAiApiTemperature.Items.FirstOrDefault(x => x.Label == Sets.AiApiTemperature);
  }

  private void btnSetsBrowseProject_Click(object sender, RibbonControlEventArgs e) {
    if (Directory.Exists(Paths.DataPath)) {
      Process.Start(Paths.DataPath);
    } else {
      ShowMsgBox.Error("本地文件夹丢失或文件夹名发生了改动！");
    }
  }

  private async void btnSetsCheckUpdate_Click(object sender, RibbonControlEventArgs e) {
    try {
      SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
      Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
      UpdateInfo updateInfo = await UpdateUtil.GetUpdateInfoAsync();
      Version latestVersion = Version.Parse(updateInfo.LatestVersion.Substring(1, updateInfo.LatestVersion.Length - 1));

      if (currentVersion >= latestVersion) {
        ShowMsgBox.Info("当前已是最新版本。");
        return;
      }

      DialogResult res1 = ShowMsgBox.QuestionOkCancel(
        $"当前版本号为{currentVersion.ToString(2)}，最新版本号为{latestVersion.ToString(2)}；\r\n是否需要立即从{updateInfo.DownloadUrl}下载最新安装包？\r\n点击“确定”开始下载；\r\n点击“取消”放弃操作。");
      if (res1 == DialogResult.Cancel) {
        return;
      }

      string filePath = await UpdateUtil.DownloadNewestVersionAsync(updateInfo.DownloadUrl);
      if (filePath == null || !File.Exists(filePath)) {
        DialogResult res2 = ShowMsgBox.QuestionOkCancel($"下载失败，请检查网络连接。\r\n可手动至{updateInfo.DownloadUrl}进行下载。\r\n点击“确定”打开链接；\r\n点击“取消”放弃操作。");
        if (res2 != DialogResult.OK) {
          return;
        }

        Process.Start(updateInfo.DownloadUrl);
        return;
      }

      DialogResult res3 = ShowMsgBox.QuestionOkCancel($"下载成功，是否立即打开安装包路径？如需更新，您需要关闭所有正在运行的Word。\r\n点击“确定”查看安装包；\r\n点击“取消”放弃操作。");
      if (res3 != DialogResult.OK) {
        return;
      }
      Process.Start("explorer", $"/e,/select,{filePath}");
    } catch (OperationCanceledException ex) {
      ShowMsgBox.Warning("由于连接超时或其他原因，未能获取更新。");
      FileLog.Error(ex);
    } catch (Exception ex) {
      ShowMsgBox.Error(ex);
      FileLog.Error(ex);
    }

  }

  private void btnSetsAbout_Click(object sender, RibbonControlEventArgs e) {
    DialogResult result = MessageBox.Show($"推荐平台：Office365\r\n.NET Framework 4.8\r\n插件版本号：V0.4\r\n当前插件的启动路径为：{AppDomain.CurrentDomain.BaseDirectory}。\r\n\r\n点击“确定”查看指南、更新日志与许可信息\r\n点击“取消”关闭窗口\r\n——le~", "关于", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

    if (result == DialogResult.OK) {
      AboutInfo aboutInfo = new();
      aboutInfo.Show();
    }
  }
  #endregion

  #region Pre工具
  private void btnPreAutoPlan_Click(object sender, RibbonControlEventArgs e) {
    PreAutoPlan preAutoPlan = new();
    PreAutoPlanViewModel preAutoPlanViewModel = new(ActiveDoc);
    preAutoPlan.DataContext = preAutoPlanViewModel;
    preAutoPlan.ShowDialog();
  }

  private void btnPreDeletePlan_Click(object sender, RibbonControlEventArgs e) {
    DialogResult result = ShowMsgBox.QuestionOkCancel("此操作将清空所有的规划信息，确定继续吗？\r\n点击“确定”删除；\r\n点击“取消”放弃操作。");
    if (result == DialogResult.Cancel) {
      return;
    }

    Word.Paragraphs paragraphs = ActiveDoc.Paragraphs;
    foreach (Word.Paragraph paragraph in paragraphs) {
      if (paragraph.Range.ComputeStatistics(Word.WdStatistic.wdStatisticWords) < 2) {
        continue;
      }

      string text = paragraph.Range.Text;
      if (text.StartsWith("(")) {
        int rightBraceIndex = text.IndexOf(")");
        if (rightBraceIndex > 0) {
          string originTimeWithBraces = text.Substring(0, rightBraceIndex + 1);
          string originTime = text.Substring(1, rightBraceIndex - 1);
          if (TryConvertTimeStrToDouble(originTime, out double _)) {
            Word.Range range = paragraph.Range;
            range.Find.Execute(originTimeWithBraces, MatchWholeWord: false);
            if (range.Text == originTimeWithBraces) {
              range.Text = string.Empty;
            }
          }
        }
      }

      if (text.TrimEnd().EndsWith(")")) {
        int leftBraceIndex = text.LastIndexOf("(", StringComparison.Ordinal);
        if (leftBraceIndex > 0) {
          string originEndTimeWithBraces = text.Substring(leftBraceIndex, text.TrimEnd().Length - leftBraceIndex);
          string originEndTime = text.Substring(leftBraceIndex + 1, text.TrimEnd().Length - 2 - leftBraceIndex);
          if (TryConvertTimeStrToDouble(originEndTime, out double _)) {
            Word.Range range = paragraph.Range;
            range.Find.Execute(originEndTimeWithBraces, MatchWholeWord: false);
            if (range.Text == originEndTimeWithBraces) {
              range.Text = string.Empty;
            }
          }
        }
      }

    }
  }

  private void btnPreExportPpt_Click(object sender, RibbonControlEventArgs e) {
    PreExportToPpt preExportToPpt = new();
    PreExportToPptViewModel preExportToPptViewModel = new(ActiveDoc);
    preExportToPpt.DataContext = preExportToPptViewModel;
    preExportToPpt.ShowDialog();
  }

  #endregion Pre工具

  #region 文本处理
  private void btnTextExtractTitle_Click(object sender, RibbonControlEventArgs e) {
    bool startedRecordingTitle = false;
    List<string> titleContents = [];
    foreach (Word.Paragraph paragraph in ActiveDoc.Paragraphs) {
      string paragraphText = paragraph.Range.Text.Trim();
      if (string.IsNullOrWhiteSpace(paragraphText)) {
        if (!startedRecordingTitle) {
          continue;
        }

        break;
      }

      startedRecordingTitle = true;
      titleContents.Add(paragraphText);
    }

    string title = string.Join(" ", titleContents);
    Clipboard.SetText(title);
    ShowStatusMessage($"已复制到剪贴板：{title}");
  }

  private void btnTextExtractContent_Click(object sender, RibbonControlEventArgs e) {
    bool startedRecordingTitle = false;
    bool isTitleEnded = false;
    bool isEnd = false;
    int imageCounter = 1;
    List<string> content = [];

    foreach (Word.Paragraph paragraph in ActiveDoc.Paragraphs) {
      string paragraphText = paragraph.Range.Text.Trim();

      if (!isTitleEnded) {
        if (string.IsNullOrWhiteSpace(paragraphText)) {
          if (!startedRecordingTitle) {
            continue;
          }

          isTitleEnded = true;
        }

        startedRecordingTitle = true;
        continue;
      }

      if (string.IsNullOrWhiteSpace(paragraphText)) {
        continue;
      }

      if (paragraph.Range.InlineShapes.Count > 0) {
        if (content.Count > 0) {
          if (string.IsNullOrEmpty(content.Last())) {
            if (content.Count > 1 && content[content.Count - 2].StartsWith("image")) {
              content.RemoveAt(content.Count - 1);
            }
          } else {
            content.Add(string.Empty);
          }
        }

        for (int i = 0; i < paragraph.Range.InlineShapes.Count; i++) {
          string imageTag = $"image{imageCounter:000}";
          imageCounter++;
          content.Add(imageTag);
        }

        content.Add(string.Empty);
        continue;
      }

      if (!isEnd && paragraph.Format.FirstLineIndent == 0) {
        isEnd = true;
        if (content.Last() != string.Empty) {
          content.Add(string.Empty);
        }
      }

      content.Add(paragraphText);
    }

    string finalContent = string.Join("\r\n", content);
    Clipboard.SetText(finalContent);
    ShowStatusMessage("已复制正文到剪贴板");
  }

  private void btnTextMarkRevisions_Click(object sender, RibbonControlEventArgs e) {
    if (ActiveDoc.TrackRevisions) {
      DialogResult result = ShowMsgBox.QuestionOkCancel("文档当前处于修订模式，是否要继续标记修订部分？");
      if (result == DialogResult.Cancel) {
        return;
      }
    }

    int revisionCount = ActiveDoc.Revisions.Count;
    if (revisionCount > 500) {
      DialogResult result = ShowMsgBox.QuestionOkCancel($"文档中有 {revisionCount} 处修订，继续操作可能会耗费较多时间，是否继续？");
      if (result == DialogResult.Cancel) {
        return;
      }
    }

    foreach (Word.Revision rev in ActiveDoc.Revisions) {
      Word.Range range = rev.Range;
      range.HighlightColorIndex = Word.WdColorIndex.wdGray25;
    }
  }


  private void btnTextExtractImagesHtml_Click(object sender, RibbonControlEventArgs e) {
    if (ActiveDoc.Saved == false) {
      ShowMsgBox.Warning("请先保存当前文档。");
      return;
    }

    string htmlFilePath = Path.Combine(DocPath, DocNameNoExt + ".htm");
    string filesFolderPath = Path.Combine(DocPath, DocNameNoExt + ".files");
    string imagesFolderPath = Path.Combine(DocPath, "images");

    if (File.Exists(htmlFilePath) || Directory.Exists(imagesFolderPath)) {
      ShowMsgBox.Warning("同名HTML文件或者图片文件夹已存在，请确保当前文档所在路径下没有同名.htm文件以及images文件夹。");
      return;
    }

    Word.WdViewType originViewType = ActiveWindow.View.Type;
    string originDocFullPath = Path.Combine(DocPath, DocName);

    ActiveDoc.SaveAs2(htmlFilePath, Word.WdSaveFormat.wdFormatHTML);
    ActiveDoc.Close();
    MainWordApp.Documents.Open(originDocFullPath);
    ActiveWindow.View.Type = originViewType;

    List<string> effectNameList = [];
    if (File.Exists(htmlFilePath)) {
      string htmlContent = File.ReadAllText(htmlFilePath);

      //<img width=554 height=351 src="裁剪.files/image007.jpg" 
      Regex regex = new Regex("<img[^>]+src=\"[^\"]*/([^\"/]+)\"", RegexOptions.IgnoreCase);
      MatchCollection matches = regex.Matches(htmlContent);

      foreach (Match match in matches) {
        effectNameList.Add(match.Groups[1].Value);
      }

      File.Delete(htmlFilePath);
    } else {
      ShowMsgBox.Error("文件未能保存");
      return;
    }

    Directory.Move(filesFolderPath, imagesFolderPath);

    string[] filePaths = Directory.GetFiles(imagesFolderPath);
    foreach (string filePath in filePaths) {
      string fileName = Path.GetFileName(filePath);
      if (!effectNameList.Contains(fileName)) {
        File.Delete(filePath);
      }
    }

    List<string> effectivePathList = effectNameList.Select(effectName => Path.Combine(imagesFolderPath, effectName)).ToList();

    for (int i = 0; i < effectivePathList.Count; i++) {
      string extension = Path.GetExtension(effectivePathList[i]);
      string newFilePath = Path.Combine(imagesFolderPath, $"image{i + 1}{extension}");
      File.Move(effectivePathList[i], newFilePath);
    }

    DialogResult dialogResult = ShowMsgBox.QuestionOkCancel($"图片成功提取至{imagesFolderPath}，是否打开所在文件夹？\r\n点击“确定”打开；\r\n点击“取消”放弃操作。");
    if (dialogResult == DialogResult.OK) {
      Process.Start(imagesFolderPath);
    }
  }

  private void btnTextExtractImagesZip_Click(object sender, RibbonControlEventArgs e) {
    if (ActiveDoc.Saved == false) {
      ShowMsgBox.Warning("请先保存当前文档。");
      return;
    }

    //判断是否是.docx文件
    bool isDocx = DocName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

    string zipFilePath = Path.Combine(DocPath, DocNameNoExt + ".zip");
    if (isDocx) {
      File.Copy(DocFullPath, zipFilePath, true);
    } else {
      string docxFilePath = Path.Combine(DocPath, DocNameNoExt + ".docx");
      if (File.Exists(docxFilePath)) {
        ShowMsgBox.Warning("同名.docx文件已存在，请确保当前文档所在路径下没有同名.docx文件。");
        return;
      }

      string originDocFullPath = Path.Combine(DocPath, DocName);
      ActiveDoc.SaveAs2(docxFilePath, Word.WdSaveFormat.wdFormatXMLDocument);
      ActiveDoc.Close();
      MainWordApp.Documents.Open(originDocFullPath);
      File.Move(docxFilePath, zipFilePath);
    }

    string extractFolder = Path.Combine(DocPath, DocNameNoExt + "_extracted");
    string imagesFolder = Path.Combine(DocPath, "images");
    bool hasExtractFolder = Directory.Exists(extractFolder);
    bool hasImagesFolder = Directory.Exists(imagesFolder);
    if (hasExtractFolder || hasImagesFolder) {
      DialogResult dr = ShowMsgBox.QuestionOkCancel("解压文件夹或图片已存在，是否删除后再操作？\r\n点击“确定”删除现有文件夹；\r\n点击取消放弃操作。");

      if (dr == DialogResult.OK) {
        if (hasExtractFolder) {
          Directory.Delete(extractFolder, true);
        }
        if (hasImagesFolder) {
          Directory.Delete(imagesFolder, true);
        }
      } else {
        return;
      }
    }

    ZipFile.ExtractToDirectory(zipFilePath, extractFolder);

    string mediaFolder = Path.Combine(extractFolder, "word", "media");
    if (Directory.Exists(mediaFolder)) {
      Directory.Move(mediaFolder, imagesFolder);
    } else {
      ShowMsgBox.Error("没有找到图片文件夹。");
      return;
    }

    File.Delete(zipFilePath);
    Directory.Delete(extractFolder, true);

    DialogResult dialogResult = ShowMsgBox.QuestionOkCancel($"图片成功提取至{imagesFolder}，是否打开所在文件夹？\r\n点击“确定”打开；\r\n点击“取消”放弃操作。");
    if (dialogResult == DialogResult.OK) {
      Process.Start(imagesFolder);
    }
  }
  #endregion

  #region 侧边
  private async void tgbPaneOnOff_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    if (tgbPaneOnOff.Checked) {
      await RefreshRightPaneAsync();
    }

    CustomRightTaskPane.Visible = tgbPaneOnOff.Checked;
  }

  #endregion

  #region 翻译
  private void dpdTransWebType_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.TransWebName = dpdTransWebType.SelectedItem.Label;
    Sets.Save();
  }


  private void dpdTransWebScale_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.TransWebScale = int.Parse(dpdTransWebType.SelectedItem.Label);
    Sets.Save();
  }

  private async void btnTransStartWeb_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await OpenRightSideBarAsync();
    RightSideBar.Tabs.SelectedTab = RightSideBar.TabTransWeb;

    string url = TransWebItems.FirstOrDefault(x => x.Name == dpdTransWebType.SelectedItem.Label)?.Content;

    RightSideBar.TransCoreWebView.Navigate(url);
    RightSideBar.TransWebView.ZoomFactor = Sets.TransWebScale / 100.0;
  }


  private void dpdTransApiType_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.TransApiName = dpdTransApiType.SelectedItem.Label;
    Sets.Save();
  }

  private void btnTransApiDevelop_Click(object sender, RibbonControlEventArgs e) {
    TransApiDevelopGuide guide = new();
    guide.Show();
  }

  private async void btnTransApiRequest_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await OpenRightSideBarAsync();

    RightSideBar.Tabs.SelectedTab = RightSideBar.TabTransApi;
  }


  #endregion

  #region AI工具
  private async void btnGptStartWeb_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await OpenRightSideBarAsync();
    RightSideBar.Tabs.SelectedTab = RightSideBar.TabGptWeb;

    string url = GptWebItems.FirstOrDefault(x => x.Name == dpdGptWebType.SelectedItem.Label)?.Content;

    RightSideBar.AiCoreWebView.Navigate(url);
    RightSideBar.GptWebView.ZoomFactor = Sets.GptWebScale / 100.0;
  }


  private void dpdGptWebType_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.GptWebName = dpdGptWebType.SelectedItem.Label;
    Sets.Save();
  }

  private void dpdGptWebScale_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.GptWebScale = int.Parse(dpdGptWebScale.SelectedItem.Label);
    Sets.Save();
  }

  private void dpdGptApiType_SelectionChanged(object sender, RibbonControlEventArgs e) {
    Sets.GptApiName = dpdGptApiType.SelectedItem.Label;
    UpdateGptApiModelAndTem();
    Sets.Save();
  }

  private void dpdGptApiModel_SelectionChanged(object sender, RibbonControlEventArgs e) {
    GeneralGpt.GptApiModel = dpdGptApiModel.SelectedItem.Label;
  }

  private void dpdGptApiTemperature_SelectionChanged(object sender, RibbonControlEventArgs e) {
    GeneralGpt.GptApiTem = int.Parse(dpdGptApiTemperature.SelectedItem.Label) * 0.1f;
  }

  private async Task RefreshRightPaneAsync() {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);

    if (CustomRightTaskPane == null) {
      using WaitDisposable waitDisposable = new();
      RightSideBar = new MainSideBar();
      CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, Paths.EdgeEnvFilesPath);
      await RightSideBar.TransWebView.EnsureCoreWebView2Async(environment);
      await RightSideBar.GptWebView.EnsureCoreWebView2Async(environment);
      CustomRightTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(RightSideBar, "实用工具集");
      CustomRightTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
      CustomRightTaskPane.DockPositionRestrict = Office.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoChange;
      CustomRightTaskPane.Width = Screen.PrimaryScreen.Bounds.Width / 3;

      CustomRightTaskPane.VisibleChanged += CustomRightTaskPane_VisibleChanged;
    }
  }

  private async Task OpenRightSideBarAsync() {
    if (tgbPaneOnOff.Checked)
      return;

    tgbPaneOnOff.Checked = true;
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await RefreshRightPaneAsync();
    CustomRightTaskPane.Visible = true;
    Application.DoEvents();
  }

  private void CustomRightTaskPane_VisibleChanged(object sender, EventArgs e) {
    tgbPaneOnOff.Checked = CustomRightTaskPane.Visible;
    if (!tgbPaneOnOff.Checked) {

      RightSideBar.AiCoreWebView.Navigate(BlankUrl);
      //RightSideBar.WebPictureBox.Image = null;
      tgbPaneOnOff.Label = "展开侧边";
      tgbPaneOnOff.ScreenTip = "点击展开侧边面板";
      RightSideBar.TransApiRequestViewModel.SetEventStateWithVisibility(false);
    } else {
      tgbPaneOnOff.Label = "收起侧边";
      tgbPaneOnOff.ScreenTip = "点击收起侧边面板";
      RightSideBar.TransApiRequestViewModel.SetEventStateWithVisibility(true);
    }
  }


  private async void btnGptApiRequest_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await OpenRightSideBarAsync();

    RightSideBar.Tabs.SelectedTab = RightSideBar.TabGptApi;
  }

  private async void btnGptWriting_Click(object sender, RibbonControlEventArgs e) {
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    await OpenRightSideBarAsync();

    RightSideBar.Tabs.SelectedTab = RightSideBar.TabEditInfo;
  }


  #endregion

  #region ForYearYear
#if ForYearYear
  private int _hasRandomNum;
#endif

  private async void sbtEncourageMyself_Click(object sender, RibbonControlEventArgs e) {
#if ForYearYear
    SynchronizationContext.SetSynchronizationContext(Globals.ThisAddIn.TheWindowsFormsSynchronizationContext);
    if (chkUseLocalData.Checked) {
      string allTexts = Resources.EncourageLib;
      string[] texts = allTexts.Split(["\r\n"], StringSplitOptions.None);
      Random random = new();
      int index = random.Next(texts.Length);
      _hasRandomNum++;
      string selectedString = texts[index];

      if (_hasRandomNum >= Sets.MaximumQuantity) {
        selectedString = texts[0];
      }

      if (selectedString.Contains("我喜欢你")) {
        _hasRandomNum = 0;
      }

      ShowMsgBox.Info("亲爱的小栗子同学：" + selectedString);
    } else {
      using CancellationTokenSource cts = new(TimeSpan.FromSeconds(Sets.ChatWaitTimeSeconds));
      WaitWindow waitWindow = new();
      waitWindow.Show();
      string result;
      try {
        GeneralGpt chatClient = new();
        result = await chatClient.GetResponseAsync("请写一段话鼓励一下现在的我，让我在后面的Presentation中充满信心，请你用“小栗子同学”来称呼我。");
      } catch (Exception ex) {
        ShowMsgBox.Error("对不起，由于未知的原因，未能取得响应，请检查网络连接。(TAT)");
        FileLog.Error(ex);
        return;
      } finally {
        waitWindow.Close();
      }

      ShowMsgBox.Info(result);
    }
#endif
  }

  private void chkUseLocalData_Click(object sender, RibbonControlEventArgs e) {
#if ForYearYear
    Sets.UseLocalData = chkUseLocalData.Checked;
    Sets.Save();
#endif
  }

  #endregion



  private void btnDevTest_Click(object sender, RibbonControlEventArgs e) {

  }

}