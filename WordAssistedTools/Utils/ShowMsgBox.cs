using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordAssistedTools.Utils {
  public static class ShowMsgBox {
    public const string AppName = "实用工具集";

    /// <summary>
    /// exception报错统一化
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="title"></param>
    public static void Error(Exception ex, string title = AppName) {
      MessageBox.Show($"出现异常：{ex.Message}，当前操作未能完成。", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// 错误对话框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    public static void Error(string text, string title = AppName) {
      MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// 警告对话框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    public static void Warning(string text, string title = AppName) {
      MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// 信息对话框
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    public static void Info(string text, string title = AppName) {
      MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static DialogResult QuestionOkCancel(string text, string title = AppName) {
      return MessageBox.Show(text, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

    public static DialogResult WarningOkCancel(string text, string title = AppName) {
      return MessageBox.Show(text, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
    }

    public static DialogResult QuestionYesNoCancel(string text, string title = AppName) {
      return MessageBox.Show(text, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    }

    public static DialogResult QuestionYesNoCancelTopMost(string text, string title = AppName) {
      return MessageBox.Show(text, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3, MessageBoxOptions.DefaultDesktopOnly);
    }
  }
}
