using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WordAssistedTools.SDK;

namespace WordAssistedTools.Converters;

internal class LanguageEnumStringToDisplayConverter : IValueConverter {
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    if (value is not string languageStr) {
      return null;
    }

    Language language = (Language)Enum.Parse(typeof(Language), languageStr);

    return language switch {
      Language.Auto => "自动检测",
      Language.Chinese => "中文（简体）",
      Language.ChineseT => "中文（繁体）",
      Language.English => "英语",
      Language.French => "法语",
      Language.Russian => "俄语",
      Language.Spanish => "西班牙语",
      Language.German => "德语",
      Language.Japanese => "日语",
      Language.Korean => "韩语",
      Language.Arabic => "阿拉伯语",
      Language.Portuguese => "葡萄牙语",
      _ => null
    };
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}