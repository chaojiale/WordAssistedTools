using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordAssistedTools.Views;

namespace WordAssistedTools.Utils;

public class ProgressDisposable : IDisposable {

  public static ProgressDisposable Show(string title) {
    return new ProgressDisposable(title);
  }

  public ProgressWindow Window { get; }

  public ProgressDisposable(string title) {
    Window = new ProgressWindow(title);
    Window.Show();
  }

  public void Dispose() {
    Window.Close();
  }
}