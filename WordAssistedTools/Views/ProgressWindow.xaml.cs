using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WordAssistedTools.Views {
  /// <summary>
  /// ProgressWindow.xaml 的交互逻辑
  /// </summary>
  public partial class ProgressWindow {
    public CancellationTokenSource CancelToken { get; set; } = new();

    public ProgressWindow(string title) {
      InitializeComponent();
      WindowTitle.Text = $"请等待程序执行{title}任务……";
    }

    public void SetProgress(int value) {
      Progress.Value = value;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) {
      CancelToken?.Cancel();
    }
  }
}
