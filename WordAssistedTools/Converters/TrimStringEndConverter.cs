using System;
using System.Globalization;
using System.Windows.Data;

namespace WordAssistedTools.Converters {
  public class TrimStringEndConverter:IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return value is string s ? s.TrimEnd() : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return null;
    }
  }
}
