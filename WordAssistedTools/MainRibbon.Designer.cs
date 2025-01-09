namespace WordAssistedTools {
  partial class MainRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase {
    /// <summary>
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public MainRibbon()
        : base(Globals.Factory.GetRibbonFactory()) {
      InitializeComponent();
    }

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region 组件设计器生成的代码

    /// <summary>
    /// 设计器支持所需的方法 - 不要修改
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent() {
      this.tabTools = this.Factory.CreateRibbonTab();
      this.groupSets = this.Factory.CreateRibbonGroup();
      this.btnSetsSettings = this.Factory.CreateRibbonButton();
      this.btnSetsBrowseProject = this.Factory.CreateRibbonButton();
      this.btnSetsCheckUpdate = this.Factory.CreateRibbonButton();
      this.btnSetsAbout = this.Factory.CreateRibbonButton();
      this.groupPre = this.Factory.CreateRibbonGroup();
      this.btnPreAutoPlan = this.Factory.CreateRibbonButton();
      this.btnPreDeletePlan = this.Factory.CreateRibbonButton();
      this.btnPreExportPpt = this.Factory.CreateRibbonButton();
      this.groupText = this.Factory.CreateRibbonGroup();
      this.btnTextExtractTitle = this.Factory.CreateRibbonButton();
      this.btnTextExtractContent = this.Factory.CreateRibbonButton();
      this.btnTextMarkRevisions = this.Factory.CreateRibbonButton();
      this.btnTextExtractImagesHtml = this.Factory.CreateRibbonButton();
      this.btnTextExtractImagesZip = this.Factory.CreateRibbonButton();
      this.group1 = this.Factory.CreateRibbonGroup();
      this.tgbPaneOnOff = this.Factory.CreateRibbonToggleButton();
      this.groupTrans = this.Factory.CreateRibbonGroup();
      this.dpdTransWebType = this.Factory.CreateRibbonDropDown();
      this.dpdTransWebScale = this.Factory.CreateRibbonDropDown();
      this.btnTransStartWeb = this.Factory.CreateRibbonButton();
      this.dpdTransApiType = this.Factory.CreateRibbonDropDown();
      this.btnTransApiRequest = this.Factory.CreateRibbonButton();
      this.groupGpt = this.Factory.CreateRibbonGroup();
      this.dpdGptWebType = this.Factory.CreateRibbonDropDown();
      this.dpdGptWebScale = this.Factory.CreateRibbonDropDown();
      this.btnGptStartWeb = this.Factory.CreateRibbonButton();
      this.dpdGptApiType = this.Factory.CreateRibbonDropDown();
      this.dpdGptApiModel = this.Factory.CreateRibbonDropDown();
      this.dpdGptApiTemperature = this.Factory.CreateRibbonDropDown();
      this.btnGptWriting = this.Factory.CreateRibbonButton();
      this.btnGptApiRequest = this.Factory.CreateRibbonButton();
      this.groupYearYear = this.Factory.CreateRibbonGroup();
      this.sbtEncourageMyself = this.Factory.CreateRibbonSplitButton();
      this.chkUseLocalData = this.Factory.CreateRibbonCheckBox();
      this.groupDev = this.Factory.CreateRibbonGroup();
      this.btnDevTest = this.Factory.CreateRibbonButton();
      this.btnTransApiDevelop = this.Factory.CreateRibbonButton();
      this.tabTools.SuspendLayout();
      this.groupSets.SuspendLayout();
      this.groupPre.SuspendLayout();
      this.groupText.SuspendLayout();
      this.group1.SuspendLayout();
      this.groupTrans.SuspendLayout();
      this.groupGpt.SuspendLayout();
      this.groupYearYear.SuspendLayout();
      this.groupDev.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabTools
      // 
      this.tabTools.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
      this.tabTools.Groups.Add(this.groupSets);
      this.tabTools.Groups.Add(this.groupPre);
      this.tabTools.Groups.Add(this.groupText);
      this.tabTools.Groups.Add(this.group1);
      this.tabTools.Groups.Add(this.groupTrans);
      this.tabTools.Groups.Add(this.groupGpt);
      this.tabTools.Groups.Add(this.groupYearYear);
      this.tabTools.Groups.Add(this.groupDev);
      this.tabTools.Label = "实用工具集";
      this.tabTools.Name = "tabTools";
      // 
      // groupSets
      // 
      this.groupSets.Items.Add(this.btnSetsSettings);
      this.groupSets.Items.Add(this.btnSetsBrowseProject);
      this.groupSets.Items.Add(this.btnSetsCheckUpdate);
      this.groupSets.Items.Add(this.btnSetsAbout);
      this.groupSets.Label = "插件管理";
      this.groupSets.Name = "groupSets";
      // 
      // btnSetsSettings
      // 
      this.btnSetsSettings.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnSetsSettings.Label = "插件设置";
      this.btnSetsSettings.Name = "btnSetsSettings";
      this.btnSetsSettings.OfficeImageId = "AddInCommandsMenu";
      this.btnSetsSettings.ShowImage = true;
      this.btnSetsSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetsSettings_Click);
      // 
      // btnSetsBrowseProject
      // 
      this.btnSetsBrowseProject.Label = "本地";
      this.btnSetsBrowseProject.Name = "btnSetsBrowseProject";
      this.btnSetsBrowseProject.OfficeImageId = "FileSaveDatabaseAsLocal";
      this.btnSetsBrowseProject.ShowImage = true;
      this.btnSetsBrowseProject.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetsBrowseProject_Click);
      // 
      // btnSetsCheckUpdate
      // 
      this.btnSetsCheckUpdate.Label = "更新";
      this.btnSetsCheckUpdate.Name = "btnSetsCheckUpdate";
      this.btnSetsCheckUpdate.OfficeImageId = "TemplatesMenu";
      this.btnSetsCheckUpdate.ShowImage = true;
      this.btnSetsCheckUpdate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetsCheckUpdate_Click);
      // 
      // btnSetsAbout
      // 
      this.btnSetsAbout.Label = "关于";
      this.btnSetsAbout.Name = "btnSetsAbout";
      this.btnSetsAbout.OfficeImageId = "About";
      this.btnSetsAbout.ShowImage = true;
      this.btnSetsAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetsAbout_Click);
      // 
      // groupPre
      // 
      this.groupPre.Items.Add(this.btnPreAutoPlan);
      this.groupPre.Items.Add(this.btnPreDeletePlan);
      this.groupPre.Items.Add(this.btnPreExportPpt);
      this.groupPre.Label = "Pre工具";
      this.groupPre.Name = "groupPre";
      // 
      // btnPreAutoPlan
      // 
      this.btnPreAutoPlan.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnPreAutoPlan.Label = "演讲时间自动规划";
      this.btnPreAutoPlan.Name = "btnPreAutoPlan";
      this.btnPreAutoPlan.OfficeImageId = "TimeScaleMenu";
      this.btnPreAutoPlan.ShowImage = true;
      this.btnPreAutoPlan.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPreAutoPlan_Click);
      // 
      // btnPreDeletePlan
      // 
      this.btnPreDeletePlan.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnPreDeletePlan.Label = "清除所有规划信息";
      this.btnPreDeletePlan.Name = "btnPreDeletePlan";
      this.btnPreDeletePlan.OfficeImageId = "ViewDeleteCurrent";
      this.btnPreDeletePlan.ShowImage = true;
      this.btnPreDeletePlan.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPreDeletePlan_Click);
      // 
      // btnPreExportPpt
      // 
      this.btnPreExportPpt.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnPreExportPpt.Label = "导出至PPT";
      this.btnPreExportPpt.Name = "btnPreExportPpt";
      this.btnPreExportPpt.OfficeImageId = "ExportFile";
      this.btnPreExportPpt.ShowImage = true;
      this.btnPreExportPpt.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPreExportPpt_Click);
      // 
      // groupText
      // 
      this.groupText.Items.Add(this.btnTextExtractTitle);
      this.groupText.Items.Add(this.btnTextExtractContent);
      this.groupText.Items.Add(this.btnTextMarkRevisions);
      this.groupText.Items.Add(this.btnTextExtractImagesHtml);
      this.groupText.Items.Add(this.btnTextExtractImagesZip);
      this.groupText.Label = "内容处理";
      this.groupText.Name = "groupText";
      // 
      // btnTextExtractTitle
      // 
      this.btnTextExtractTitle.Label = "提取标题";
      this.btnTextExtractTitle.Name = "btnTextExtractTitle";
      this.btnTextExtractTitle.OfficeImageId = "ControlTitle";
      this.btnTextExtractTitle.ShowImage = true;
      this.btnTextExtractTitle.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTextExtractTitle_Click);
      // 
      // btnTextExtractContent
      // 
      this.btnTextExtractContent.Label = "提取正文";
      this.btnTextExtractContent.Name = "btnTextExtractContent";
      this.btnTextExtractContent.OfficeImageId = "TemplatesMenu";
      this.btnTextExtractContent.ShowImage = true;
      this.btnTextExtractContent.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTextExtractContent_Click);
      // 
      // btnTextMarkRevisions
      // 
      this.btnTextMarkRevisions.Label = "标记修订";
      this.btnTextMarkRevisions.Name = "btnTextMarkRevisions";
      this.btnTextMarkRevisions.OfficeImageId = "SparklineMarkerColorPicker";
      this.btnTextMarkRevisions.ShowImage = true;
      this.btnTextMarkRevisions.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTextMarkRevisions_Click);
      // 
      // btnTextExtractImagesHtml
      // 
      this.btnTextExtractImagesHtml.Label = "提取图片(HTML)";
      this.btnTextExtractImagesHtml.Name = "btnTextExtractImagesHtml";
      this.btnTextExtractImagesHtml.OfficeImageId = "EmptyPictureInsert";
      this.btnTextExtractImagesHtml.ShowImage = true;
      this.btnTextExtractImagesHtml.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTextExtractImagesHtml_Click);
      // 
      // btnTextExtractImagesZip
      // 
      this.btnTextExtractImagesZip.Label = "提取图片(ZIP)";
      this.btnTextExtractImagesZip.Name = "btnTextExtractImagesZip";
      this.btnTextExtractImagesZip.OfficeImageId = "EmptyPictureInsert";
      this.btnTextExtractImagesZip.ShowImage = true;
      this.btnTextExtractImagesZip.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTextExtractImagesZip_Click);
      // 
      // group1
      // 
      this.group1.Items.Add(this.tgbPaneOnOff);
      this.group1.Label = "侧边";
      this.group1.Name = "group1";
      // 
      // tgbPaneOnOff
      // 
      this.tgbPaneOnOff.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.tgbPaneOnOff.Label = "展开侧边";
      this.tgbPaneOnOff.Name = "tgbPaneOnOff";
      this.tgbPaneOnOff.OfficeImageId = "FillLeft";
      this.tgbPaneOnOff.ShowImage = true;
      this.tgbPaneOnOff.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.tgbPaneOnOff_Click);
      // 
      // groupTrans
      // 
      this.groupTrans.Items.Add(this.dpdTransWebType);
      this.groupTrans.Items.Add(this.dpdTransWebScale);
      this.groupTrans.Items.Add(this.btnTransStartWeb);
      this.groupTrans.Items.Add(this.dpdTransApiType);
      this.groupTrans.Items.Add(this.btnTransApiDevelop);
      this.groupTrans.Items.Add(this.btnTransApiRequest);
      this.groupTrans.Label = "翻译";
      this.groupTrans.Name = "groupTrans";
      // 
      // dpdTransWebType
      // 
      this.dpdTransWebType.Label = "网站";
      this.dpdTransWebType.Name = "dpdTransWebType";
      this.dpdTransWebType.SizeString = "OpenAI-C";
      this.dpdTransWebType.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdTransWebType_SelectionChanged);
      // 
      // dpdTransWebScale
      // 
      this.dpdTransWebScale.Label = "缩放";
      this.dpdTransWebScale.Name = "dpdTransWebScale";
      this.dpdTransWebScale.SizeString = "OpenAI-C";
      this.dpdTransWebScale.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdTransWebScale_SelectionChanged);
      // 
      // btnTransStartWeb
      // 
      this.btnTransStartWeb.Label = "开始翻译";
      this.btnTransStartWeb.Name = "btnTransStartWeb";
      this.btnTransStartWeb.OfficeImageId = "TranslateMenu";
      this.btnTransStartWeb.ShowImage = true;
      this.btnTransStartWeb.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTransStartWeb_Click);
      // 
      // dpdTransApiType
      // 
      this.dpdTransApiType.Label = "请求";
      this.dpdTransApiType.Name = "dpdTransApiType";
      this.dpdTransApiType.SizeString = "OpenAI-C";
      this.dpdTransApiType.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdTransApiType_SelectionChanged);
      // 
      // btnTransApiRequest
      // 
      this.btnTransApiRequest.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnTransApiRequest.Label = "API请求";
      this.btnTransApiRequest.Name = "btnTransApiRequest";
      this.btnTransApiRequest.OfficeImageId = "WebQueryAccess";
      this.btnTransApiRequest.ShowImage = true;
      this.btnTransApiRequest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTransApiRequest_Click);
      // 
      // groupGpt
      // 
      this.groupGpt.Items.Add(this.dpdGptWebType);
      this.groupGpt.Items.Add(this.dpdGptWebScale);
      this.groupGpt.Items.Add(this.btnGptStartWeb);
      this.groupGpt.Items.Add(this.dpdGptApiType);
      this.groupGpt.Items.Add(this.dpdGptApiModel);
      this.groupGpt.Items.Add(this.dpdGptApiTemperature);
      this.groupGpt.Items.Add(this.btnGptWriting);
      this.groupGpt.Items.Add(this.btnGptApiRequest);
      this.groupGpt.Label = "GPT";
      this.groupGpt.Name = "groupGpt";
      // 
      // dpdGptWebType
      // 
      this.dpdGptWebType.Label = "网站";
      this.dpdGptWebType.Name = "dpdGptWebType";
      this.dpdGptWebType.SizeString = "OpenAI-C";
      this.dpdGptWebType.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdGptWebType_SelectionChanged);
      // 
      // dpdGptWebScale
      // 
      this.dpdGptWebScale.Label = "缩放";
      this.dpdGptWebScale.Name = "dpdGptWebScale";
      this.dpdGptWebScale.SizeString = "OpenAI-C";
      this.dpdGptWebScale.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdGptWebScale_SelectionChanged);
      // 
      // btnGptStartWeb
      // 
      this.btnGptStartWeb.Label = "开始会话";
      this.btnGptStartWeb.Name = "btnGptStartWeb";
      this.btnGptStartWeb.OfficeImageId = "SpeakCells";
      this.btnGptStartWeb.ShowImage = true;
      this.btnGptStartWeb.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGptStartWeb_Click);
      // 
      // dpdGptApiType
      // 
      this.dpdGptApiType.Label = "请求";
      this.dpdGptApiType.Name = "dpdGptApiType";
      this.dpdGptApiType.SizeString = "OpenAI-C";
      this.dpdGptApiType.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdGptApiType_SelectionChanged);
      // 
      // dpdGptApiModel
      // 
      this.dpdGptApiModel.Label = "模型";
      this.dpdGptApiModel.Name = "dpdGptApiModel";
      this.dpdGptApiModel.SizeString = "OpenAI-C";
      this.dpdGptApiModel.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdGptApiModel_SelectionChanged);
      // 
      // dpdGptApiTemperature
      // 
      this.dpdGptApiTemperature.Label = "T×0.1";
      this.dpdGptApiTemperature.Name = "dpdGptApiTemperature";
      this.dpdGptApiTemperature.SizeString = "OpenAI-C";
      this.dpdGptApiTemperature.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dpdGptApiTemperature_SelectionChanged);
      // 
      // btnGptWriting
      // 
      this.btnGptWriting.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnGptWriting.Label = "创作工具";
      this.btnGptWriting.Name = "btnGptWriting";
      this.btnGptWriting.OfficeImageId = "VirtualListToolEditItemInError";
      this.btnGptWriting.ShowImage = true;
      this.btnGptWriting.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGptWriting_Click);
      // 
      // btnGptApiRequest
      // 
      this.btnGptApiRequest.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnGptApiRequest.Label = "API请求";
      this.btnGptApiRequest.Name = "btnGptApiRequest";
      this.btnGptApiRequest.OfficeImageId = "WebQueryAccess";
      this.btnGptApiRequest.ShowImage = true;
      this.btnGptApiRequest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGptApiRequest_Click);
      // 
      // groupYearYear
      // 
      this.groupYearYear.Items.Add(this.sbtEncourageMyself);
      this.groupYearYear.Label = "ForYearYear";
      this.groupYearYear.Name = "groupYearYear";
      // 
      // sbtEncourageMyself
      // 
      this.sbtEncourageMyself.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.sbtEncourageMyself.Image = global::WordAssistedTools.Properties.Resources.image;
      this.sbtEncourageMyself.Items.Add(this.chkUseLocalData);
      this.sbtEncourageMyself.Label = "鼓励一下自己";
      this.sbtEncourageMyself.Name = "sbtEncourageMyself";
      this.sbtEncourageMyself.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.sbtEncourageMyself_Click);
      // 
      // chkUseLocalData
      // 
      this.chkUseLocalData.Label = "使用本地数据";
      this.chkUseLocalData.Name = "chkUseLocalData";
      this.chkUseLocalData.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.chkUseLocalData_Click);
      // 
      // groupDev
      // 
      this.groupDev.Items.Add(this.btnDevTest);
      this.groupDev.Label = "开发";
      this.groupDev.Name = "groupDev";
      // 
      // btnDevTest
      // 
      this.btnDevTest.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnDevTest.Label = "测试";
      this.btnDevTest.Name = "btnDevTest";
      this.btnDevTest.OfficeImageId = "ScriptDebugger";
      this.btnDevTest.ShowImage = true;
      this.btnDevTest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDevTest_Click);
      // 
      // btnTransApiDevelop
      // 
      this.btnTransApiDevelop.Label = "开发指南";
      this.btnTransApiDevelop.Name = "btnTransApiDevelop";
      this.btnTransApiDevelop.OfficeImageId = "DeveloperReference";
      this.btnTransApiDevelop.ShowImage = true;
      this.btnTransApiDevelop.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTransApiDevelop_Click);
      // 
      // MainRibbon
      // 
      this.Name = "MainRibbon";
      this.RibbonType = "Microsoft.Word.Document";
      this.Tabs.Add(this.tabTools);
      this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonTools_Load);
      this.tabTools.ResumeLayout(false);
      this.tabTools.PerformLayout();
      this.groupSets.ResumeLayout(false);
      this.groupSets.PerformLayout();
      this.groupPre.ResumeLayout(false);
      this.groupPre.PerformLayout();
      this.groupText.ResumeLayout(false);
      this.groupText.PerformLayout();
      this.group1.ResumeLayout(false);
      this.group1.PerformLayout();
      this.groupTrans.ResumeLayout(false);
      this.groupTrans.PerformLayout();
      this.groupGpt.ResumeLayout(false);
      this.groupGpt.PerformLayout();
      this.groupYearYear.ResumeLayout(false);
      this.groupYearYear.PerformLayout();
      this.groupDev.ResumeLayout(false);
      this.groupDev.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    internal Microsoft.Office.Tools.Ribbon.RibbonTab tabTools;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupPre;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPreAutoPlan;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPreExportPpt;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupSets;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetsSettings;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPreDeletePlan;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDevTest;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupDev;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetsAbout;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupYearYear;
    internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton sbtEncourageMyself;
    internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox chkUseLocalData;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTextMarkRevisions;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupText;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupGpt;
    internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tgbPaneOnOff;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdGptWebType;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGptStartWeb;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdGptApiType;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdGptApiModel;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdGptApiTemperature;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGptWriting;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGptApiRequest;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetsCheckUpdate;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetsBrowseProject;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTextExtractTitle;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTextExtractContent;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTextExtractImagesHtml;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTextExtractImagesZip;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupTrans;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdTransWebType;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdGptWebScale;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdTransWebScale;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTransStartWeb;
    internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dpdTransApiType;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTransApiRequest;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTransApiDevelop;
  }

  partial class ThisRibbonCollection {
    internal MainRibbon RibbonTools {
      get { return this.GetRibbon<MainRibbon>(); }
    }
  }
}
