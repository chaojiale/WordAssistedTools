using System.Windows;

namespace WordAssistedTools.Views;

public static class InputBox { 
  public static string Show(string prompt, string title = "", string defaultResponse = "", Window owner = null) {
      
    var messageBox = new InputBoxWindow();
    if (owner != null) {
      messageBox.Owner = owner;
      messageBox.Icon = owner.Icon;
    }

    messageBox.Prompt = prompt;
    messageBox.Title = title;
    messageBox.DefaultResponse = defaultResponse;

    bool? dialogResult = messageBox.ShowDialog();
    switch (dialogResult) {
      case true:
        return messageBox.txtValue.Text;
      case null:
      default:
        return string.Empty;
    }
  }
}