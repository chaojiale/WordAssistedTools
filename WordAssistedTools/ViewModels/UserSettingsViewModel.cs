using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WordAssistedTools.Models;
using WordAssistedTools.Properties;
using WordAssistedTools.Utils;
using WordAssistedTools.Views;

namespace WordAssistedTools.ViewModels;

internal class UserSettingsViewModel : ObservableObject {

  public event EventHandler<SettingsUpdateEventArgs> OnConfirm;

  private static Settings Sets => Settings.Default;
  private List<NameContentItemViewModel> TransWebItems => JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.TransWebConfigJsonPath);
  public List<string> TransWebNames => TransWebItems.Select(x => x.Name).ToList();
  public List<string> TransApiNames => TransApiManager.TransApiItems.Select(x => x.Name).ToList();
  public List<NameContentItemViewModel> GptWebItems => JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.GptWebConfigJsonPath);
  public List<string> GptWebNames => GptWebItems.Select(x => x.Name).ToList();
  public List<GptApiItemViewModel> GptApiItems => JsonTool.LoadJsonFile<GptApiItemViewModel>(Paths.GptApiConfigJsonPath);
  public List<string> GptApiNames => GptApiItems.Select(x => x.Name).ToList();

  private readonly SettingsUpdateEventArgs _updateSettingsArgs = new();

  public RelayCommand TryParseWordToPptRulesCommand { get; }
  public RelayCommand ConfigTransWebCommand { get; }
  public RelayCommand ConfigTransApiCommand { get; }
  public RelayCommand ConfigGptWebCommand { get; }
  public RelayCommand ConfigGptApiCommand { get; }
  public RelayCommand RestoreCommand { get; }
  public RelayCommand<Window> ConfirmCommand { get; }

  public UserSettingsViewModel() {
    TryParseWordToPptRulesCommand = new RelayCommand(TryParseWordToPptRulesCommand_Execute);
    ConfigTransWebCommand = new RelayCommand(ConfigTransWebCommand_Execute);
    ConfigTransApiCommand = new RelayCommand(ConfigTransApiCommand_Execute);
    ConfigGptWebCommand = new RelayCommand(ConfigGptWebCommand_Execute);
    ConfigGptApiCommand = new RelayCommand(ConfigGptApiCommand_Execute);
    RestoreCommand = new RelayCommand(RestoreCommand_Execute);
    ConfirmCommand = new RelayCommand<Window>(ConfirmCommand_Execute);
    Sets.PropertyChanged += Sets_PropertyChanged;
  }

  private void Sets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
    switch (e.PropertyName) {
      case nameof(Sets.TransWebName):
        _updateSettingsArgs.IsUpdateTransWeb = true;
        break;
      case nameof(Sets.TransApiName):
        _updateSettingsArgs.IsUpdateTransApi = true;
        break;
      case nameof(Sets.GptWebName):
        _updateSettingsArgs.IsUpdateGptWeb = true;
        break;
      case nameof(Sets.GptApiName):
        _updateSettingsArgs.IsUpdateGptApi = true;
        break;
    }
  }

  private void ConfigTransWebCommand_Execute() {
    NameContentConfigViewModel viewModel = new(ThemeTypeEnum.TransWebType, "名称", "链接");
    viewModel.OnSaveEvent += (_, _) => {
      OnPropertyChanged(nameof(TransWebItems));
      _updateSettingsArgs.IsUpdateTransWeb = true;
    };
    WebServiceConfig config = new() {
      DataContext = viewModel
    };
    config.ShowDialog();
  }


  private void ConfigTransApiCommand_Execute() {
    TransApiConfigViewModel viewModel = new();
    viewModel.OnSaveEvent += (_, _) => {
      OnPropertyChanged(nameof(TransApiNames));
      _updateSettingsArgs.IsUpdateTransApi = true;
    };
    TransApiConfig config = new() {
      DataContext = viewModel
    };
    config.ShowDialog();
  }


  private void ConfigGptWebCommand_Execute() {
    NameContentConfigViewModel viewModel = new(ThemeTypeEnum.GptWebType, "名称", "链接");
    viewModel.OnSaveEvent += (_, _) => {
      OnPropertyChanged(nameof(GptWebItems));
      _updateSettingsArgs.IsUpdateGptWeb = true;
    };
    WebServiceConfig config = new() {
      DataContext = viewModel
    };
    config.ShowDialog();
  }

  private void ConfigGptApiCommand_Execute() {
    GptApiConfigViewModel viewApiConfigViewModel = new();
    viewApiConfigViewModel.OnSaveEvent += (_, _) => {
      OnPropertyChanged(nameof(GptApiNames));
      _updateSettingsArgs.IsUpdateGptApi = true;
    };

    GptApiConfig config = new() {
      DataContext = viewApiConfigViewModel
    };
    config.ShowDialog();
  }

  private void TryParseWordToPptRulesCommand_Execute() {
    bool flag = WordToPptRulesUtils.TryParseWordToPptRules(Sets.WordToPptRules, out List<Dictionary<ProcessType, (string, string)>> allRuleInfos);
    if (flag) {
      ShowMsgBox.Info(allRuleInfos.ToInfoTexts());
    }
  }

  private void RestoreCommand_Execute() {
    Settings.Default.Reset();
  }


  private void ConfirmCommand_Execute(Window window) {
    if (!WordToPptRulesUtils.TryParseWordToPptRules(Sets.WordToPptRules, out List<Dictionary<ProcessType, (string, string)>> _)) {
      return;
    }

    Sets.Save();
    OnConfirm?.Invoke(this, _updateSettingsArgs);
    window.Close();
  }
}

public class SettingsUpdateEventArgs : EventArgs {
  public bool IsUpdateTransWeb { get; set; }
  public bool IsUpdateTransApi { get; set; }
  public bool IsUpdateGptWeb { get; set; }
  public bool IsUpdateGptApi { get; set; }
}