using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.Properties;

namespace WordAssistedTools.Models;

internal static class Shared {
  public static Logger FileLog { get; set; }

  public const string BlankUrl = "about:blank";
  public const string UpdateApiUrl = "https://api.github.com/repos/chaojiale/WordAssistedTools/releases/latest";

  public static readonly ObservableCollection<double> UpperLimitTimes = [4, 5, 8, 10, 12, 15, 20];
  public static readonly ObservableCollection<double> FinalReservedTimes = [2, 5, 10, 15, 20, 30, 60];
  public static readonly ObservableCollection<double> ChangeSlideTimes = [0, 0.5, 1, 1.5, 2, 2.5, 3];
  public static readonly ObservableCollection<int> TimeOutSecondsItems = [10, 15, 20, 25, 30, 45, 60];
  public static readonly ObservableCollection<int> WebScaleItems = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120];
  public static readonly ObservableCollection<int> HistoryNumItems = [0, 1, 2, 3, 5, 10, -1];
}

public enum ReferenceType {
  None,
  Selected,
  CurrentParagraph,
  FullText,
}