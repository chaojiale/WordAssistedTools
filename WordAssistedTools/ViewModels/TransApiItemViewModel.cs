using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Input;
using WordAssistedTools.Models;
using WordAssistedTools.SDK;

namespace WordAssistedTools.ViewModels;

internal class TransApiItemViewModel : ObservableObject, ITransApiConfigBase {
  private string _name;
  public string Name {
    get => _name;
    set => SetProperty(ref _name, value);
  }

  [JsonIgnore]
  public string DllPath { get; }

  [JsonIgnore]
  public Type ApiClass { get; }

  [JsonIgnore]
  public string ClassName => ApiClass.Name;

  public string ClassFullName => ApiClass.FullName;

  [JsonIgnore]
  public BaseTranslator Translator { get; }

  [JsonIgnore]
  public string Url => Translator.BaseUrl;

  private string _key;
  public string Key {
    get => _key;
    set => SetProperty(ref _key, value);
  }

  [JsonIgnore]
  public RelayCommand GetHelpCommand { get; }

  public TransApiItemViewModel(string dllPath, Type type) {
    GetHelpCommand = new RelayCommand(GetHelpCommand_Execute);
    DllPath = dllPath;
    ApiClass = type;
    Translator = (BaseTranslator)Activator.CreateInstance(type);
  }

  private void GetHelpCommand_Execute() {
    Translator.GetHelp();
  }

  public async Task<string> TranslateAsync(string text, Language source, Language target) {
    string key = Key.StartsWith("$") ? Environment.GetEnvironmentVariable(Key.Substring(1)) : Key;
    return await Translator.TranslateAsync(key, text, source, target);
  }

}