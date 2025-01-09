using System;
using System.Globalization;
using System.Windows.Data;

namespace WordAssistedTools.Converters {
  public class BoolToOnOffStateConverter: IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is bool b) {
        return b ? "开" : "关";
      }

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return null;
    }
  }
}
