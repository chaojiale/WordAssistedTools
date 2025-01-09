using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using WordAssistedTools.ViewModels;
using WordAssistedTools.Properties;
using static WordAssistedTools.Models.Shared;
using WordAssistedTools.Models;
// ReSharper disable ConvertToAutoProperty

namespace WordAssistedTools.Views;

internal partial class MainSideBar : UserControl {

  public TabControl Tabs => tabs;
  public TabPage TabTransWeb => tabTransWeb;
  public WebView2 TransWebView => transWebView;
  public CoreWebView2 TransCoreWebView => transWebView.CoreWebView2;

  public TabPage TabTransApi => tabTransApi;

  public TabPage TabGptWeb => tabGptWeb;
  public WebView2 GptWebView => gptWebView;
  public CoreWebView2 AiCoreWebView => gptWebView.CoreWebView2;

  public TabPage TabGptApi => tabGptApi;
  public TabPage TabEditInfo => tabEditInfo;

  public TransApiRequestViewModel TransApiRequestViewModel => ((TransApiRequest)hostTransApi.Child).DataContext as TransApiRequestViewModel;
  public GptApiRequestViewModel GptApiRequestViewModel => ((GptApiRequest)hostGptApi.Child).DataContext as GptApiRequestViewModel;
  public EditRequestInfoViewModel EditRequestInfoViewModel => ((EditRequestInfo)hostGptInfo.Child).DataContext as EditRequestInfoViewModel;

  public MainSideBar() {
    InitializeComponent();
    TransApiRequest transApiRequest = new();
    hostTransApi.Child = transApiRequest;

    EditRequestInfo editRequestInfo = new();
    hostGptInfo.Child = editRequestInfo;

    GptApiRequest gptApiRequest = new();
    hostGptApi.Child = gptApiRequest;

    EditRequestInfoViewModel.OnApiRequested += EditRequestInfoViewModel_OnApiRequested;
    EditRequestInfoViewModel.OnCopyWebPage += EditRequestInfoViewModel_OnCopyWebPage;
  }

  private void EditRequestInfoViewModel_OnCopyWebPage(object sender, string e) {
    //string url = Sets.AiWebType switch {
    //  AiWebOpenAi => Sets.AiWebOpenAiUrl,
    //  AiWebMicrosoft => Sets.AiWebMicrosoftUrl,
    //  AiWebBaidu => Sets.AiWebBaiduUrl,
    //  AiWebMoonShot => Sets.AiWebMoonShotUrl,
    //  _ => throw new ArgumentOutOfRangeException()
    //};

    Tabs.SelectedTab = TabGptWeb;
    //AiCoreWebView.Navigate(url);
    Clipboard.SetText(e);
  }

  private async void EditRequestInfoViewModel_OnApiRequested(object sender, string e) {
    Tabs.SelectedTab = TabGptApi;
    await GptApiRequestViewModel.StartRequest(e);
  }

  private void tabRightPanel_SelectedIndexChanged(object sender, EventArgs e) {
    //TabControl tabControl = (TabControl)sender;
    //if (tabControl.SelectedTab == _tabAiApi && hostApi.Child == null) {
      
    //} else if (tabControl.SelectedTab == _tabEditInfo && hostInfo.Child == null) {
      
    //}
  }
}