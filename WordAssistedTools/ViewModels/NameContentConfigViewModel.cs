using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WordAssistedTools.Models;
using WordAssistedTools.Utils;
using System.ComponentModel;
using System.IO;
using WordAssistedTools.Properties;

namespace WordAssistedTools.ViewModels;

internal class NameContentConfigViewModel : ObservableObject {
  private static Settings Sets => Settings.Default;

  public ThemeTypeEnum ThemeType { get; }
  public string Theme { get; }

  private string _nameHeader;
  public string NameHeader {
    get => _nameHeader;
    set => SetProperty(ref _nameHeader, value);
  }

  private string _contentHeader;
  public string ContentHeader {
    get => _contentHeader;
    set => SetProperty(ref _contentHeader, value);
  }


  private NameContentItemViewModel _defaultItem;
  public NameContentItemViewModel DefaultItem {
    get => _defaultItem;
    set => SetProperty(ref _defaultItem, value);
  }

  private NameContentItemViewModel _selectedItem;
  public NameContentItemViewModel SelectedItem {
    get => _selectedItem;
    set {
      if (SetProperty(ref _selectedItem, value)) {
        DeleteItemCommand.NotifyCanExecuteChanged();
        MoveUpItemCommand.NotifyCanExecuteChanged();
        MoveDownItemCommand.NotifyCanExecuteChanged();
      }
    }
  }

  public ObservableCollection<NameContentItemViewModel> Items { get; } = [];

  public RelayCommand WindowLoadedCommand { get; }
  public RelayCommand AddItemCommand { get; }
  public RelayCommand DeleteItemCommand { get; }
  public RelayCommand MoveUpItemCommand { get; }
  public RelayCommand MoveDownItemCommand { get; }

  public RelayCommand<Window> SaveExitCommand { get; }
  public RelayCommand<Window> DirectExitCommand { get; }

  [Obsolete]
  public NameContentConfigViewModel() {

  }

  public NameContentConfigViewModel(ThemeTypeEnum themeType, string nameHeader, string contentHeader) {
    ThemeType = themeType;
    Theme = themeType switch {
      ThemeTypeEnum.Template => "模板",
      ThemeTypeEnum.TransWebType => "翻译网页",
      ThemeTypeEnum.GptWebType => "AI网页",
      _ => throw new InvalidEnumArgumentException()
    };
    NameHeader = nameHeader;
    ContentHeader = contentHeader;

    WindowLoadedCommand = new RelayCommand(WindowLoadedCommand_Execute);
    AddItemCommand = new RelayCommand(AddItemCommand_Execute);
    DeleteItemCommand = new RelayCommand(DeleteItemCommand_Execute, () => SelectedItem != null);
    MoveUpItemCommand = new RelayCommand(MoveUpItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) > 0);
    MoveDownItemCommand = new RelayCommand(MoveDownItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1);

    SaveExitCommand = new RelayCommand<Window>(SaveExitCommand_Execute);
    DirectExitCommand = new RelayCommand<Window>(DirectExitCommand_Execute);
  }

  private void WindowLoadedCommand_Execute() {
    switch (ThemeType) {
      case ThemeTypeEnum.Template:
        if (!File.Exists(Paths.GptTemplateConfigJsonPath)) {
          return;
        }

        List<NameContentItemViewModel> templates = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.GptTemplateConfigJsonPath);
        foreach (NameContentItemViewModel template in templates) {
          if (template.Name == Sets.GptApiTemplateName) {
            DefaultItem = template;
          }
          Items.Add(template);
        }
        break;
      case ThemeTypeEnum.TransWebType:
        if (!File.Exists(Paths.TransWebConfigJsonPath)) {
          return;
        }

        List<NameContentItemViewModel> transWebConfigs = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.TransWebConfigJsonPath);
        foreach (NameContentItemViewModel type in transWebConfigs) {
          if (type.Name == Sets.TransWebName) {
            DefaultItem = type;
          }
          Items.Add(type);
        }

        break;
      case ThemeTypeEnum.GptWebType:
        if (!File.Exists(Paths.GptWebConfigJsonPath)) {
          return;
        }

        List<NameContentItemViewModel> gptWebConfigs = JsonTool.LoadJsonFile<NameContentItemViewModel>(Paths.GptWebConfigJsonPath);
        foreach (NameContentItemViewModel type in gptWebConfigs) {
          if (type.Name == Sets.GptWebName) {
            DefaultItem = type;
          }
          Items.Add(type);
        }

        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void AddItemCommand_Execute() {
    // 未选择在末尾创建，否则在选择项后面创建
    if (SelectedItem == null) {
      Items.Add(new NameContentItemViewModel(NameHeader, ContentHeader));
    } else {
      int index = Items.IndexOf(SelectedItem);
      Items.Insert(index + 1, new NameContentItemViewModel(NameHeader, ContentHeader));
    }
  }

  private void DeleteItemCommand_Execute() {
    DialogResult result = ShowMsgBox.QuestionOkCancel("确定删除选中的行？");
    if (result == DialogResult.OK) {
      Items.Remove(SelectedItem);
    }
  }

  private void MoveUpItemCommand_Execute() {
    int index = Items.IndexOf(SelectedItem);
    Items.Move(index, index - 1);
    if (index == 1) {
      MoveUpItemCommand.NotifyCanExecuteChanged();
    }
    if (index == Items.Count - 1) {
      MoveDownItemCommand.NotifyCanExecuteChanged();
    }
  }

  private void MoveDownItemCommand_Execute() {
    int index = Items.IndexOf(SelectedItem);
    Items.Move(index, index + 1);
    if (index == 0) {
      MoveUpItemCommand.NotifyCanExecuteChanged();
    }
    if (index == Items.Count - 2) {
      MoveDownItemCommand.NotifyCanExecuteChanged();
    }
  }


  private void SaveExitCommand_Execute(Window window) {
    if (DefaultItem == null) {
      ShowMsgBox.Warning($"请选择一个默认{Theme}！");
      return;
    }

    if (Items.Any(item => string.IsNullOrWhiteSpace(item.Name) || string.IsNullOrWhiteSpace(item.Content))) {
      ShowMsgBox.Warning($"{NameHeader}和{ContentHeader}不能为空！");
      return;
    }

    switch (ThemeType) {
      case ThemeTypeEnum.Template:
        Sets.GptApiTemplateName = DefaultItem.Name;
        JsonTool.SaveJsonFile(Paths.GptTemplateConfigJsonPath, Items);
        break;
      case ThemeTypeEnum.TransWebType:
        Sets.TransWebName = DefaultItem.Name;
        JsonTool.SaveJsonFile(Paths.TransWebConfigJsonPath, Items);
        break;
      case ThemeTypeEnum.GptWebType:
        Sets.GptWebName = DefaultItem.Name;
        JsonTool.SaveJsonFile(Paths.GptWebConfigJsonPath, Items);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    Sets.Save();
    window.Close();
    OnSaveEvent?.Invoke(this, EventArgs.Empty);
  }

  private void DirectExitCommand_Execute(Window window) {
    DialogResult result = ShowMsgBox.QuestionOkCancel("确定退出而不保存吗？");
    if (result == DialogResult.OK)
      window.Close();
  }

  public event EventHandler OnSaveEvent;
}

public enum ThemeTypeEnum {
  Template,
  GptWebType,
  TransWebType
}