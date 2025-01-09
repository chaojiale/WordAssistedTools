using System;
using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace WordAssistedTools.ViewModels;

internal class NameContentItemViewModel : ObservableObject {

  public event EventHandler<string> OnIsCheckedChanged;

  private bool _isChecked;
  [JsonIgnore]
  public bool IsChecked {
    get => _isChecked;
    set {
      if (SetProperty(ref _isChecked, value) && value) {
        // 触发选中事件
        OnIsCheckedChanged?.Invoke(this, Content);
      }
    }
  }

  private string _name;
  public string Name {
    get => _name;
    set => SetProperty(ref _name, value);
  }

  private string _content;
  public string Content {
    get => _content;
    set => SetProperty(ref _content, value);
  }

  public NameContentItemViewModel() {

  }

  public NameContentItemViewModel(string name, string content) {
    Name = name;
    Content = content;
  }

  public override string ToString() {
    return $"{{{Name}: {Content}}}";
  }
}

