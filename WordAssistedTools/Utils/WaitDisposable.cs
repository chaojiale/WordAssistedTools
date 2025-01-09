using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.Views;

namespace WordAssistedTools.Utils;

public class WaitDisposable : IDisposable {
  private readonly WaitWindow _waitWindow;

  public static WaitDisposable Show() {
    return new WaitDisposable();
  }

  public WaitDisposable() {
    _waitWindow = new WaitWindow();
    _waitWindow.Show();
  }

  public void Dispose() {
    _waitWindow.Close();
  }
}