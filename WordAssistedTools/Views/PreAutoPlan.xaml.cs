using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WordAssistedTools.Views {
  /// <summary>
  /// AutoPlan.xaml 的交互逻辑
  /// </summary>
  public partial class PreAutoPlan  {
    public PreAutoPlan() {
      InitializeComponent();
      this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
    }

    private int _errorCount;
    private void OnErrorEvent(object sender, RoutedEventArgs e) {
      if (e is not ValidationErrorEventArgs validationEventArgs)
        return;

      switch (validationEventArgs.Action) {
        case ValidationErrorEventAction.Added: {
          _errorCount++;
          break;
        }
        case ValidationErrorEventAction.Removed: {
          _errorCount--;
          break;
        }
        default:
          return;
      }

      ButtonRefreshPlanningResults.IsEnabled = _errorCount == 0;
      ButtonUpdateWordDocument.IsEnabled = _errorCount == 0;
    }
  }
}
