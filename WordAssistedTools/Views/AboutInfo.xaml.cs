using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordAssistedTools.Utils;

namespace WordAssistedTools.Views {
  /// <summary>
  /// AboutInfo.xaml 的交互逻辑
  /// </summary>
  public partial class AboutInfo  {
    private void OpenHyperlink(object sender, ExecutedRoutedEventArgs e) {
      Process.Start(e.Parameter.ToString());
    }

    private void ClickOnImage(object sender, ExecutedRoutedEventArgs e) {
      MessageBox.Show($"URL: {e.Parameter}");
    }

    public AboutInfo() {
      InitializeComponent();

      HelpViewer.Markdown = Properties.Resources.Readme;
      LicenseViewer.Markdown = Properties.Resources.License;
    }
  }
}
