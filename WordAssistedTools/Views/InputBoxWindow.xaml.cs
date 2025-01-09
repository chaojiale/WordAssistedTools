using System.Windows;

namespace WordAssistedTools.Views {
  /// <summary>
  /// InputBox.xaml 的交互逻辑
  /// </summary>
  public partial class InputBoxWindow : Window {

    public InputBoxWindow() {
      InitializeComponent();
    }

    public string Prompt { get; set; }
    public string DefaultResponse { get; set; }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      prompt.Text = Prompt;
      txtValue.Text = DefaultResponse;
    }

    private void btnConfirm_Click(object sender, RoutedEventArgs e) {
      DialogResult = true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
      DialogResult = false;
    }
  }
}
