using System;
using System.Globalization;
using System.Windows.Data;

namespace WordAssistedTools.Converters {
  public class AutoWrapConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is not string s) {
        return null;
      }

      string result = string.Empty;
      for (int i = 0; i < s.Length; i++) {
        if (i % 10 == 0 && i != 0) {
          result += Environment.NewLine;
        }
        result += s[i];
      }
      return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return null;
    }
  }
}
