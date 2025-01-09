using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WordAssistedTools.Views {
  public class CustomDataGrid : DataGrid {
    public CustomDataGrid() {
      this.SelectionChanged += CustomDataGrid_SelectionChanged;
    }

    private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      this.SelectedItemsList = this.SelectedItems;
    }

    #region SelectedItemsList

    public IList SelectedItemsList {
      get => (IList)GetValue(SelectedItemsListProperty);
      set => SetValue(SelectedItemsListProperty, value);
    }

    public static readonly DependencyProperty SelectedItemsListProperty =
      DependencyProperty.Register(nameof(SelectedItemsList), typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));

    #endregion
  }
}
