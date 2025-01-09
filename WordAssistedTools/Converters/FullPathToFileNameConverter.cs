using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WordAssistedTools.Converters {
  internal class FullPathToFileNameConverter:IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return value is not string s ? null : System.IO.Path.GetFileName(s);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
