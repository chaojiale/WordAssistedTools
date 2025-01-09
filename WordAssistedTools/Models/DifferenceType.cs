using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.Models {
  public enum DifferenceType {
    OnlyText,
    TimeAndText
  }

  public static class DifferenceTypeSwitch {
    public static DifferenceType ToDifferenceType(this string text) {
      return text switch {
        "OnlyText" => DifferenceType.OnlyText,
        "TimeAndText" => DifferenceType.TimeAndText,
        _ => throw new ArgumentOutOfRangeException(nameof(text), text, null)
      };
    }
  }


}
