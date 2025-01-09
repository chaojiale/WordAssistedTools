using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.Models;
using WordAssistedTools.Properties;
using WordAssistedTools.SDK;
using WordAssistedTools.ViewModels;

namespace WordAssistedTools.Utils;

internal class TransApiManager {
  public static ObservableCollection<TransApiItemViewModel> TransApiItems { get; } = [];
  public static TransApiItemViewModel CurrentTranslator => TransApiItems.FirstOrDefault(x => x.Name == Settings.Default.TransApiName);

  static TransApiManager() {
    ReloadTransApiItems();
  }

  public static void ReloadTransApiItems() {
    TransApiItems.Clear();
    if (!Directory.Exists(Paths.TransApiDllPath)) {
      return;
    }

    List<TransApiUserConfig> userConfigItems = null;
    if (File.Exists(Paths.TransApiConfigJsonPath)) {
      userConfigItems = JsonTool.LoadJsonFile<TransApiUserConfig>(Paths.TransApiConfigJsonPath);
    }

    string[] dllFiles = Directory.GetFiles(Paths.TransApiDllPath, "*.dll");
    foreach (string dllFile in dllFiles) {
      Assembly assembly = Assembly.LoadFrom(dllFile);
      Type[] types = assembly.GetTypes();
      foreach (Type type in types) {
        if (!type.IsSubclassOf(typeof(BaseTranslator)) || type.IsAbstract) {
          continue;
        }

        TransApiItemViewModel dllItem = new(dllFile, type);
        TransApiUserConfig userInfoItem = userConfigItems?.FirstOrDefault(x => x.ClassFullName == dllItem.ClassFullName);
        if (userInfoItem != null) {
          dllItem.Name = userInfoItem.Name;
          dllItem.Key = userInfoItem.Key;
        } else {
          dllItem.Name = dllItem.Translator.DefaultName;
          dllItem.Key = dllItem.Translator.DefaultKeyForm;
        }
        TransApiItems.Add(dllItem);
      }
    }

    JsonTool.SaveJsonFile(Paths.TransApiConfigJsonPath, TransApiItems);
  }
}