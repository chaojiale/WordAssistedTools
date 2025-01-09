using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WordAssistedTools.Models;
using WordAssistedTools.Properties;
using WordAssistedTools.Utils;

namespace WordAssistedTools.ViewModels;

internal class TransApiConfigViewModel:ObservableObject {

  private static Settings Sets => Settings.Default;

  private TransApiItemViewModel _defaultItem;
  public TransApiItemViewModel DefaultItem {
    get => _defaultItem;
    set => SetProperty(ref _defaultItem, value);
  }

  private TransApiItemViewModel _selectedItem;
  public TransApiItemViewModel SelectedItem {
    get => _selectedItem;
    set {
      if (SetProperty(ref _selectedItem, value)) {
        SeeInFolderCommand.NotifyCanExecuteChanged();
        RestoreItemCommand.NotifyCanExecuteChanged();
        MoveUpItemCommand.NotifyCanExecuteChanged();
        MoveDownItemCommand.NotifyCanExecuteChanged();
      }
    }
  }

  public ObservableCollection<TransApiItemViewModel> Items => TransApiManager.TransApiItems;

  public RelayCommand SeeInFolderCommand { get; }
  public RelayCommand RestoreItemCommand { get; }
  public RelayCommand MoveUpItemCommand { get; }
  public RelayCommand MoveDownItemCommand { get; }
  public RelayCommand ReloadItemsCommand { get; }

  public RelayCommand<Window> SaveExitCommand { get; }
  public RelayCommand<Window> DirectExitCommand { get; }

  public TransApiConfigViewModel() {
    SeeInFolderCommand = new RelayCommand(SeeInFolderCommand_Execute, () => SelectedItem != null);
    RestoreItemCommand = new RelayCommand(RestoreItemCommand_Execute, () => SelectedItem != null);
    MoveUpItemCommand = new RelayCommand(MoveUpItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) > 0);
    MoveDownItemCommand = new RelayCommand(MoveDownItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1);
    ReloadItemsCommand = new RelayCommand(ReloadItemsCommand_Execute);
    SaveExitCommand = new RelayCommand<Window>(SaveExitCommand_Execute);
    DirectExitCommand = new RelayCommand<Window>(DirectExitCommand_Execute);

    foreach (TransApiItemViewModel item in Items) {
      if (item.Name == Sets.TransApiName) {
        DefaultItem = item;
        break;
      }
    }
  }

  private void SeeInFolderCommand_Execute() {
    Process.Start("explorer", $"/e,/select,{SelectedItem.DllPath}");
  }

  private void RestoreItemCommand_Execute() {
    SelectedItem.Name = SelectedItem.Translator.DefaultName;
    SelectedItem.Key = SelectedItem.Translator.DefaultKeyForm;
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

  private void ReloadItemsCommand_Execute() {
    DialogResult result = ShowMsgBox.QuestionOkCancel("确定重新加载吗？");
    if (result == DialogResult.OK) {
      TransApiManager.ReloadTransApiItems();
      OnPropertyChanged(nameof(Items));
    }
  }

  private void SaveExitCommand_Execute(Window window) {
    if (DefaultItem == null) {
      ShowMsgBox.Warning("请选择一个默认模型！");
      return;
    }

    if (Items.Any(item => string.IsNullOrWhiteSpace(item.Name))) {
      //todo:添加其他的
      ShowMsgBox.Warning($"名称不能为空！");
      return;
    }

    Sets.TransApiName = DefaultItem.Name;
    JsonTool.SaveJsonFile(Paths.TransApiConfigJsonPath, Items);

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