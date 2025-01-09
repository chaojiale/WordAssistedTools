using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordAssistedTools.Views;
using Newtonsoft.Json;

namespace WordAssistedTools.ViewModels;

internal class GptApiItemViewModel : ObservableObject {

  private string _name;
  public string Name {
    get => _name;
    set => SetProperty(ref _name, value);
  }

  private string _url;
  public string Url {
    get => _url;
    set => SetProperty(ref _url, value);
  }

  public ObservableCollection<string> Models { get; } = [];

  private string _defaultModel;
  public string DefaultModel {
    get => _defaultModel;
    set => SetProperty(ref _defaultModel, value);
  }

  private int _lowerTem = 0;
  public int LowerTem {
    get => _lowerTem;
    set => SetProperty(ref _lowerTem, value);
  }

  private int _upperTem = 10;
  public int UpperTem {
    get => _upperTem;
    set => SetProperty(ref _upperTem, value);
  }

  private int _defaultTem = 3;
  public int DefaultTem {
    get => _defaultTem;
    set => SetProperty(ref _defaultTem, value);
  }

  private string _key;
  public string Key {
    get => _key;
    set => SetProperty(ref _key, value);
  }

  private string _system = "You are a helpful assistant.";
  public string System {
    get => _system;
    set => SetProperty(ref _system, value);
  }

  [JsonIgnore]
  public RelayCommand EditModelsCommand { get; }

  public GptApiItemViewModel() {
    EditModelsCommand = new RelayCommand(EditModelsCommand_Execute);
  }

  private void EditModelsCommand_Execute() {
    EditListWindowViewModel viewModel = new(Models, DefaultModel);
    EditListWindow window = new() {
      DataContext = viewModel
    };
    viewModel.Confirmed += (_, e) => {
      DefaultModel = e;
    };
    window.ShowDialog();
  }


}