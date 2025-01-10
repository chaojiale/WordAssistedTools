using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WordAssistedTools.Models;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WordAssistedTools.Utils;

internal class UpdateUtil {
  public static async Task<UpdateInfo> GetUpdateInfoAsync() {
    using WaitDisposable wait = new();
    HttpClient client = new() {
      Timeout = TimeSpan.FromSeconds(5)
    };
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)");
    string response = await client.GetStringAsync(Shared.UpdateApiUrl);
    JObject json = JObject.Parse(response);

    if (json["tag_name"] == null || json["assets"] == null) {
      throw new Exception("未能获取最新版本号或下载链接");
    }

    string latestVersion = json["tag_name"].ToString();
    string downloadUrl = null;

    foreach (JToken asset in json["assets"]) {
      string assetName = asset["name"]?.ToString();
      if (!string.IsNullOrEmpty(assetName) && assetName.Contains("WordAssistedTools-setup")) {
        downloadUrl = asset["browser_download_url"]?.ToString();
        break;
      }
    }

    if (string.IsNullOrEmpty(downloadUrl)) {
      throw new Exception("未找到符合的下载链接");
    }

    return new UpdateInfo(latestVersion, downloadUrl);
  }


  private static bool _isUpdatingNow = false;

  public static async Task<string> DownloadNewestVersionAsync(string url) {
    try {
      HttpClient client = new();
      client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)");
      if (_isUpdatingNow) {
        ShowMsgBox.Warning("正在更新中，请不要重复点击！");
        return null;
      }

      string downloadFolder = GetSystemDownloadPath();
      string fileName = Path.GetFileName(url);
      string filePath = Path.Combine(downloadFolder, fileName);
      if (string.IsNullOrEmpty(downloadFolder)) {
        SaveFileDialog saveFileDialog = new() {
          Filter = "可执行文件|*.exe",
          FileName = fileName,
          Title = "请选择下载路径",
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK) {
          filePath = saveFileDialog.FileName;
        } else {
          return null;
        }
      }

      _isUpdatingNow = true;
      HttpResponseMessage headResponse = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
      headResponse.EnsureSuccessStatusCode();
      long totalBytes = headResponse.Content.Headers.ContentLength ?? 0;

      using Stream stream = await client.GetStreamAsync(url);
      using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
      const int bufferSize = 8192;
      byte[] buffer = new byte[bufferSize];

      using ProgressDisposable progress = ProgressDisposable.Show("下载更新");
      int bytesRead;
      int count = 0;
      while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, progress.Window.CancelToken.Token)) > 0) {
        if (progress.Window.CancelToken.Token.IsCancellationRequested) {
          throw new Exception("操作被取消！");
        }

        await fileStream.WriteAsync(buffer, 0, bytesRead, progress.Window.CancelToken.Token);
        count++;
        progress.Window.SetProgress((int)(count * bufferSize * 100 / totalBytes));
      }

      progress.Window.SetProgress(100);
      await Task.Delay(200);
      _isUpdatingNow = false;
      return filePath;
    } catch (Exception e) {
      Shared.FileLog.Error(e);
      _isUpdatingNow = false;
      return null;
    }
  }

  [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
  static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);
  public static readonly Guid Downloads = new("374DE290-123F-4565-9164-39C4925E467B");
  public static string GetSystemDownloadPath() {
    try {
      SHGetKnownFolderPath(Downloads, 0, IntPtr.Zero, out string path);
      return path;
    } catch {
      return null;
    }
  }
}

public record UpdateInfo(string LatestVersion, string DownloadUrl) {
  public string LatestVersion { get; set; } = LatestVersion;
  public string DownloadUrl { get; set; } = DownloadUrl;
}