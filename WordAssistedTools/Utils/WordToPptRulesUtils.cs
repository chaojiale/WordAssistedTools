using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.Utils {
  public static class WordToPptRulesUtils {
    public static bool TryParseWordToPptRules(string wordToPptRules, out List<Dictionary<ProcessType, (string, string)>> allRuleInfos) {
      allRuleInfos = null;

      if (string.IsNullOrWhiteSpace(wordToPptRules)) {
        return true;
      }

      string[] rules = wordToPptRules.Split('|');
      string[] parseResults = new string[rules.Length];
      if (rules.Length > 3) {
        ShowMsgBox.Error("命名习惯规则的数量不能多于3条！");
        return false;
      }

      allRuleInfos = new List<Dictionary<ProcessType, (string, string)>>();
      for (int i = 0; i < rules.Length; i++) {
        Dictionary<ProcessType, (string, string)> ruleInfo = new();

        string rule = rules[i];
        string[] processesKeywords = rule.Split(':');
        string processes = processesKeywords[0];
        int processesNum = processes.Length;
        string allKeywordsConcat = processesKeywords[1];
        string[] keywords = allKeywordsConcat.Split('*');
        int keywordsNum = keywords.Length;

        if (processesNum > 3) {
          ShowMsgBox.Error($"第{i + 1}条命名习惯规则“{rule}”的操作数量不能大于3！");
          return false;
        }

        if (processesNum != keywordsNum) {
          ShowMsgBox.Error($"第{i + 1}条命名习惯规则“{rule}”的操作数量与关键词数量不匹配！");
          return false;
        }

        for (int j = 0; j < processes.Length; j++) {
          char process = processes[j];
          switch (process) {
            case '<':
              ruleInfo.Add(ProcessType.LeftAdd, (keywords[j], string.Empty));
              break;
            case '>':
              ruleInfo.Add(ProcessType.RightAdd, (keywords[j], string.Empty));
              break;
            case '-':
              ruleInfo.Add(ProcessType.Remove, (keywords[j], string.Empty));
              break;
            case '/':
              string[] replaceWords = keywords[j].Split('/');
              if (replaceWords.Length != 2) {
                ShowMsgBox.Error($"第{i + 1}条命名习惯规则“{rule}”的替换关键词数量不为2！");
                return false;
              }
              ruleInfo.Add(ProcessType.Replace, (replaceWords[0], replaceWords[1]));
              break;
          }
        }

        allRuleInfos.Add(ruleInfo);
      }

      return true;
    }

    public static string ToInfoTexts(this List<Dictionary<ProcessType, (string, string)>> allRuleInfos) {
      string result = string.Empty;
      int count = 0;
      foreach (Dictionary<ProcessType, (string, string)> ruleInfo in allRuleInfos) {
        count++;
        result += ruleInfo.ToInfoText(count);
      }

      return result;
    }

    public static string ToInfoText(this Dictionary<ProcessType, (string, string)> ruleInfo, int Id) {
      if (ruleInfo == null || ruleInfo.Count == 0) {
        return string.Empty;
      }

      string result = $"第{Id}条规则：\r\n";
      int i = 0;
      foreach (KeyValuePair<ProcessType, (string, string)> pair in ruleInfo) {
        i++;
        switch (pair.Key) {
          case ProcessType.LeftAdd:
            result += $"第{i}步，左侧增加：“{pair.Value.Item1}”\r\n";
            break;
          case ProcessType.RightAdd:
            result += $"第{i}步，右侧增加：“{pair.Value.Item1}”\r\n";
            break;
          case ProcessType.Remove:
            result += $"第{i}步，移除：“{pair.Value.Item1}”\r\n";
            break;
          case ProcessType.Replace:
            result += $"第{i}步，替换：将“{pair.Value.Item1}”替换为“{pair.Value.Item2}”\r\n";
            break;
        }
      }

      result += "\r\n";
      return result;
    }

  }

  public enum ProcessType {
    LeftAdd,
    RightAdd,
    Remove,
    Replace,
  }
}


