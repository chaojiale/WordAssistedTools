namespace WordAssistedTools.Views {
  partial class MainSideBar {
    /// <summary> 
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      this.tabs = new System.Windows.Forms.TabControl();
      this.tabTransWeb = new System.Windows.Forms.TabPage();
      this.transWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
      this.tabTransApi = new System.Windows.Forms.TabPage();
      this.hostTransApi = new System.Windows.Forms.Integration.ElementHost();
      this.tabGptWeb = new System.Windows.Forms.TabPage();
      this.gptWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
      this.tabEditInfo = new System.Windows.Forms.TabPage();
      this.hostGptInfo = new System.Windows.Forms.Integration.ElementHost();
      this.tabGptApi = new System.Windows.Forms.TabPage();
      this.hostGptApi = new System.Windows.Forms.Integration.ElementHost();
      this.tabs.SuspendLayout();
      this.tabTransWeb.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.transWebView)).BeginInit();
      this.tabTransApi.SuspendLayout();
      this.tabGptWeb.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gptWebView)).BeginInit();
      this.tabEditInfo.SuspendLayout();
      this.tabGptApi.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabs
      // 
      this.tabs.Controls.Add(this.tabTransWeb);
      this.tabs.Controls.Add(this.tabTransApi);
      this.tabs.Controls.Add(this.tabGptWeb);
      this.tabs.Controls.Add(this.tabEditInfo);
      this.tabs.Controls.Add(this.tabGptApi);
      this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabs.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
      this.tabs.Location = new System.Drawing.Point(0, 0);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new System.Drawing.Size(720, 720);
      this.tabs.TabIndex = 0;
      this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabRightPanel_SelectedIndexChanged);
      // 
      // tabTransWeb
      // 
      this.tabTransWeb.Controls.Add(this.transWebView);
      this.tabTransWeb.Location = new System.Drawing.Point(4, 39);
      this.tabTransWeb.Name = "tabTransWeb";
      this.tabTransWeb.Size = new System.Drawing.Size(712, 677);
      this.tabTransWeb.TabIndex = 3;
      this.tabTransWeb.Text = " 翻译网页版 ";
      this.tabTransWeb.UseVisualStyleBackColor = true;
      // 
      // transWebView
      // 
      this.transWebView.AllowExternalDrop = true;
      this.transWebView.CreationProperties = null;
      this.transWebView.DefaultBackgroundColor = System.Drawing.Color.White;
      this.transWebView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.transWebView.Location = new System.Drawing.Point(0, 0);
      this.transWebView.Name = "transWebView";
      this.transWebView.Size = new System.Drawing.Size(712, 677);
      this.transWebView.TabIndex = 0;
      this.transWebView.ZoomFactor = 1D;
      // 
      // tabTransApi
      // 
      this.tabTransApi.Controls.Add(this.hostTransApi);
      this.tabTransApi.Location = new System.Drawing.Point(4, 39);
      this.tabTransApi.Name = "tabTransApi";
      this.tabTransApi.Size = new System.Drawing.Size(712, 677);
      this.tabTransApi.TabIndex = 4;
      this.tabTransApi.Text = " 翻译API ";
      this.tabTransApi.UseVisualStyleBackColor = true;
      // 
      // hostTransApi
      // 
      this.hostTransApi.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hostTransApi.Location = new System.Drawing.Point(0, 0);
      this.hostTransApi.Name = "hostTransApi";
      this.hostTransApi.Size = new System.Drawing.Size(712, 677);
      this.hostTransApi.TabIndex = 0;
      this.hostTransApi.Text = "elementHost1";
      this.hostTransApi.Child = null;
      // 
      // tabGptWeb
      // 
      this.tabGptWeb.Controls.Add(this.gptWebView);
      this.tabGptWeb.Location = new System.Drawing.Point(4, 39);
      this.tabGptWeb.Name = "tabGptWeb";
      this.tabGptWeb.Padding = new System.Windows.Forms.Padding(3);
      this.tabGptWeb.Size = new System.Drawing.Size(712, 677);
      this.tabGptWeb.TabIndex = 0;
      this.tabGptWeb.Text = " AI网页版 ";
      this.tabGptWeb.UseVisualStyleBackColor = true;
      // 
      // gptWebView
      // 
      this.gptWebView.AllowExternalDrop = true;
      this.gptWebView.CreationProperties = null;
      this.gptWebView.DefaultBackgroundColor = System.Drawing.Color.White;
      this.gptWebView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gptWebView.Location = new System.Drawing.Point(3, 3);
      this.gptWebView.Name = "gptWebView";
      this.gptWebView.Size = new System.Drawing.Size(706, 671);
      this.gptWebView.TabIndex = 0;
      this.gptWebView.ZoomFactor = 1D;
      // 
      // tabEditInfo
      // 
      this.tabEditInfo.Controls.Add(this.hostGptInfo);
      this.tabEditInfo.Location = new System.Drawing.Point(4, 39);
      this.tabEditInfo.Margin = new System.Windows.Forms.Padding(2);
      this.tabEditInfo.Name = "tabEditInfo";
      this.tabEditInfo.Size = new System.Drawing.Size(712, 677);
      this.tabEditInfo.TabIndex = 2;
      this.tabEditInfo.Text = " AI模板 ";
      this.tabEditInfo.UseVisualStyleBackColor = true;
      // 
      // hostGptInfo
      // 
      this.hostGptInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hostGptInfo.Location = new System.Drawing.Point(0, 0);
      this.hostGptInfo.Margin = new System.Windows.Forms.Padding(2);
      this.hostGptInfo.Name = "hostGptInfo";
      this.hostGptInfo.Size = new System.Drawing.Size(712, 677);
      this.hostGptInfo.TabIndex = 0;
      this.hostGptInfo.Text = "elementHost2";
      this.hostGptInfo.Child = null;
      // 
      // tabGptApi
      // 
      this.tabGptApi.Controls.Add(this.hostGptApi);
      this.tabGptApi.Location = new System.Drawing.Point(4, 39);
      this.tabGptApi.Name = "tabGptApi";
      this.tabGptApi.Padding = new System.Windows.Forms.Padding(3);
      this.tabGptApi.Size = new System.Drawing.Size(712, 677);
      this.tabGptApi.TabIndex = 1;
      this.tabGptApi.Text = " AI API ";
      this.tabGptApi.UseVisualStyleBackColor = true;
      // 
      // hostGptApi
      // 
      this.hostGptApi.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hostGptApi.Location = new System.Drawing.Point(3, 3);
      this.hostGptApi.Margin = new System.Windows.Forms.Padding(2);
      this.hostGptApi.Name = "hostGptApi";
      this.hostGptApi.Size = new System.Drawing.Size(706, 671);
      this.hostGptApi.TabIndex = 0;
      this.hostGptApi.Text = "elementHost1";
      this.hostGptApi.Child = null;
      // 
      // MainSideBar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabs);
      this.Name = "MainSideBar";
      this.Size = new System.Drawing.Size(720, 720);
      this.tabs.ResumeLayout(false);
      this.tabTransWeb.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.transWebView)).EndInit();
      this.tabTransApi.ResumeLayout(false);
      this.tabGptWeb.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.gptWebView)).EndInit();
      this.tabEditInfo.ResumeLayout(false);
      this.tabGptApi.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabs;
    private System.Windows.Forms.TabPage tabGptWeb;
    private Microsoft.Web.WebView2.WinForms.WebView2 gptWebView;
    private System.Windows.Forms.TabPage tabGptApi;
    private System.Windows.Forms.Integration.ElementHost hostGptApi;
    private System.Windows.Forms.TabPage tabEditInfo;
    private System.Windows.Forms.Integration.ElementHost hostGptInfo;
    private System.Windows.Forms.TabPage tabTransWeb;
    private Microsoft.Web.WebView2.WinForms.WebView2 transWebView;
    private System.Windows.Forms.TabPage tabTransApi;
    private System.Windows.Forms.Integration.ElementHost hostTransApi;
  }
}
