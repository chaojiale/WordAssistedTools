using System;
using System.Globalization;
using System.Windows.Data;

namespace WordAssistedTools.Converters {
  public class DoubleSecondsToTimeStrConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is not double d) {
        return null;
      }

      if (d < 0) {
        return "null";
      }

      if (d == 0) {
        return string.Empty;
      }

      int allSeconds = (int)Math.Round(d);
      int minutes = allSeconds / 60;
      int seconds = allSeconds % 60;
      return $"{minutes}’{seconds:00}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return null;
    }
  }
}
