using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Windows.Forms;

namespace WordAssistedTools {
  public partial class ThisAddIn {
    private Dispatcher _dispatcher;
    public Dispatcher Dispatcher => _dispatcher;
    public SynchronizationContext TheWindowsFormsSynchronizationContext { get; private set; }
    private void ThisAddIn_Startup(object sender, System.EventArgs e) {
      _dispatcher = Dispatcher.CurrentDispatcher;
      this.TheWindowsFormsSynchronizationContext = SynchronizationContext.Current
                                                   ?? new WindowsFormsSynchronizationContext();
    }

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
    }

    #region VSTO 生成的代码

    /// <summary>
    /// 设计器支持所需的方法 - 不要修改
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(ThisAddIn_Startup);
      this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
    }

    #endregion
  }
}
