using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordAssistedTools.Models;
using WordAssistedTools.Properties;
using WordAssistedTools.Utils;

namespace WordAssistedTools.ViewModels;

internal class GptApiConfigViewModel : ObservableObject {
  private static Settings Sets => Settings.Default;

  private GptApiItemViewModel _defaultItem;
  public GptApiItemViewModel DefaultItem {
    get => _defaultItem;
    set => SetProperty(ref _defaultItem, value);
  }

  private GptApiItemViewModel _selectedItem;
  public GptApiItemViewModel SelectedItem {
    get => _selectedItem;
    set {
      if (SetProperty(ref _selectedItem, value)) {
        DeleteItemCommand.NotifyCanExecuteChanged();
        MoveUpItemCommand.NotifyCanExecuteChanged();
        MoveDownItemCommand.NotifyCanExecuteChanged();
      }
    }
  }

  public ObservableCollection<GptApiItemViewModel> Items { get; } = [];

  public RelayCommand WindowLoadedCommand { get; }
  public RelayCommand AddItemCommand { get; }
  public RelayCommand DeleteItemCommand { get; }
  public RelayCommand MoveUpItemCommand { get; }
  public RelayCommand MoveDownItemCommand { get; }

  public RelayCommand<Window> SaveExitCommand { get; }
  public RelayCommand<Window> DirectExitCommand { get; }

  public GptApiConfigViewModel() {
    WindowLoadedCommand = new RelayCommand(WindowLoadedCommand_Execute);
    AddItemCommand = new RelayCommand(AddItemCommand_Execute);
    DeleteItemCommand = new RelayCommand(DeleteItemCommand_Execute, () => SelectedItem != null);
    MoveUpItemCommand = new RelayCommand(MoveUpItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) > 0);
    MoveDownItemCommand = new RelayCommand(MoveDownItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1);
    SaveExitCommand = new RelayCommand<Window>(SaveExitCommand_Execute);
    DirectExitCommand = new RelayCommand<Window>(DirectExitCommand_Execute);
  }

  private void WindowLoadedCommand_Execute() {
    if (!File.Exists(Paths.GptApiConfigJsonPath)) {
      return;
    }

    List<GptApiItemViewModel> configs = JsonTool.LoadJsonFile<GptApiItemViewModel>(Paths.GptApiConfigJsonPath);
    foreach (GptApiItemViewModel config in configs) {
      if (config.Name == Sets.GptApiName) {
        DefaultItem = config;
      }
      Items.Add(config);
    }
  }

  private void AddItemCommand_Execute() {
    // 未选择在末尾创建，否则在选择项后面创建
    if (SelectedItem == null) {
      Items.Add(new GptApiItemViewModel());
    } else {
      int index = Items.IndexOf(SelectedItem);
      Items.Insert(index + 1, new GptApiItemViewModel());
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
      ShowMsgBox.Warning("请选择一个默认模型！");
      return;
    }

    if (Items.Any(item => string.IsNullOrWhiteSpace(item.Name))) {
      ShowMsgBox.Warning("名称不能为空！");
      return;
    }

    if (Items.GroupBy(item => item.Name).Any(group => group.Count() > 1)) {
      ShowMsgBox.Warning("名称不能重复！");
      return;
    }

    if (Items.Any(item => !(item.LowerTem <= item.DefaultTem && item.DefaultTem <= item.UpperTem))) {
      ShowMsgBox.Warning("默认温度必须位于上下界之间！");
      return;
    }

    Sets.GptApiName = DefaultItem.Name;
    JsonTool.SaveJsonFile(Paths.GptApiConfigJsonPath, Items);

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