using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.Utils;

public static class Methods {
  public static bool TryConvertTimeStrToDouble(string timeStr, out double timeSeconds) {
    timeSeconds = -1;
    if (!timeStr.Contains('’')) {
      return false;
    }

    string[] timeInfo = timeStr.Split('’');
    if (timeInfo.Length != 2) {
      return false;
    }

    try {
      int min = int.Parse(timeInfo[0]);
      int s = int.Parse(timeInfo[1]);
      if (min < 0 || s < 0 || s > 59) {
        return false;
      }

      timeSeconds = min * 60 + s;
      if (timeSeconds == 0) {
        //避免被converter以默认值过滤
        timeSeconds = 0.01;
      }
    } catch {
      return false;
    }

    return true;
  }

  public static string ConvertSecondsToTimeStr(double seconds) {
    int secondsInt = (int)Math.Round(seconds);
    int min = secondsInt / 60;
    int s = secondsInt % 60;
    return $"{min}’{s:00}";
  }
}