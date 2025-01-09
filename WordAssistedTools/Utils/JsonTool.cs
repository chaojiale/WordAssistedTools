using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.ViewModels;
using static WordAssistedTools.Models.Shared;

namespace WordAssistedTools.Utils;

internal static class JsonTool {

  public static void SaveJsonFile<T>(string path, IEnumerable<T> templates) {
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(templates, Newtonsoft.Json.Formatting.Indented);
    File.WriteAllText(path, json);
  }

  public static List<T> LoadJsonFile<T>(string path) {
    try {
      string json = File.ReadAllText(path);
      return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
    } catch (Exception ex) {
      //ShowMsgBox.Error(path);
      FileLog.Error(ex);
      return null;
    }
  }
}