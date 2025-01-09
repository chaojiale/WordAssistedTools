using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WordAssistedTools.Views {
  public class EstimateParaWordCountValidationRule : ValidationRule {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
      string numberStr = value as string;

      if (!int.TryParse(numberStr, out int number)||number<=0) {
        return new ValidationResult(false, "估计字数必须为正整数！");
      }

      return new ValidationResult(true, null);
    }
  }
}
