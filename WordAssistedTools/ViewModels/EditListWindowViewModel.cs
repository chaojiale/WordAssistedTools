using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WordAssistedTools.Utils;

namespace WordAssistedTools.ViewModels;

internal class EditListWindowViewModel : ObservableObject {
  private string _newAdd;
  public string NewAdd {
    get => _newAdd;
    set {
      if (SetProperty(ref _newAdd, value)) {
        AddItemCommand.NotifyCanExecuteChanged();
      }
    }
  }

  private string _selectedItem;
  public string SelectedItem {
    get => _selectedItem;
    set {
      if (SetProperty(ref _selectedItem, value)) {
        DeleteItemCommand.NotifyCanExecuteChanged();
        MoveUpItemCommand.NotifyCanExecuteChanged();
        MoveDownItemCommand.NotifyCanExecuteChanged();
      }
    }
  }

  public ObservableCollection<string> Items { get; }
  public RelayCommand AddItemCommand { get; }
  public RelayCommand DeleteItemCommand { get; }
  public RelayCommand MoveUpItemCommand { get; }
  public RelayCommand MoveDownItemCommand { get; }
  public RelayCommand<Window> ConfirmCommand { get; }

  public event EventHandler<string> Confirmed;

  public EditListWindowViewModel(ObservableCollection<string> items, string defaultItem) {
    AddItemCommand = new RelayCommand(AddItemCommand_Execute, () => NewAdd != null);
    DeleteItemCommand = new RelayCommand(DeleteItemCommand_Execute, () => SelectedItem != null);
    MoveUpItemCommand = new RelayCommand(MoveUpItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) > 0);
    MoveDownItemCommand = new RelayCommand(MoveDownItemCommand_Execute, () => SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1);
    ConfirmCommand = new RelayCommand<Window>(ConfirmCommand_Execute);

    Items = items;
    if (items.Count != 0 && items.Contains(defaultItem)) {
      SelectedItem = defaultItem;
    }
  }

  private void AddItemCommand_Execute() {
    if (Items.Contains(NewAdd)) {
      ShowMsgBox.Warning("已存在相同的项！");
      return;
    }

    Items.Add(NewAdd);
    NewAdd = null;
  }

  private void DeleteItemCommand_Execute() {
    Items.Remove(SelectedItem);
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

  private void ConfirmCommand_Execute(Window window) {
    if (SelectedItem == null) {
      ShowMsgBox.Warning("请选择一个默认项！");
      return;
    }

    Confirmed?.Invoke(this, SelectedItem);
    window.Close();
  }
}
