using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using WordAssistedTools.SDK;

namespace CommonTranslators;

internal class BaiduTranslator : BaseTranslator {
  public override string DefaultName => "百度";
  public override string BaseUrl => "http://api.fanyi.baidu.com/api/trans/vip/translate?";
  public override string DefaultKeyForm => "appid#secretKey";

  protected override Dictionary<Language, string> LanguageCodeMap { get; } = new() {
    [Language.Auto] = "auto",
    [Language.Chinese] = "zh",
    [Language.ChineseT] = "cht",
    [Language.English] = "en",
    [Language.French] = "fra",
    [Language.Russian] = "ru",
    [Language.Spanish] = "spa",
    [Language.German] = "de",
    [Language.Japanese] = "jp",
    [Language.Korean] = "kor",
    [Language.Arabic] = "ara",
    [Language.Portuguese] = "pt",
  };

  public override async Task<string> TranslateAsync(string key, string text, Language source, Language target) {
    try {
      if (target == Language.Auto) {
        throw new ArgumentException("Target language should not be 'auto'.");
      }

      string salt = new Random().Next(100000).ToString();
      // 假设传入的 Key 格式为 "appid#secretKey"
      string[] keyParts = key.Split('#');
      if (keyParts.Length != 2) {
        throw new ArgumentException("Invalid key format. Expected 'appid#secretKey'.");
      }

      string appId = keyParts[0];
      string secretKey = keyParts[1];

      string sign = EncryptString(appId + text + salt + secretKey);
      string url = BaseUrl;
      url += "q=" + HttpUtility.UrlEncode(text);
      url += "&from=" + LanguageCodeMap[source];
      url += "&to=" + LanguageCodeMap[target];
      url += "&appid=" + appId;
      url += "&salt=" + salt;
      url += "&sign=" + sign;

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = "GET";
      request.ContentType = "text/html;charset=UTF-8";
      request.Timeout = 6000;

      using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
      using Stream myResponseStream = response.GetResponseStream();
      using StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
      string jsonResponse = await myStreamReader.ReadToEndAsync();

      JObject jsonObject = JObject.Parse(jsonResponse);

      JArray transResultArray = (JArray)jsonObject["trans_result"];
      if (transResultArray == null) {
        throw new Exception(jsonObject.ToString());
      }

      string translatedText = transResultArray[0]["dst"].ToString();

      return translatedText;
    } catch (Exception ex) {
      return $"Error: {ex.Message}";
    }
  }

  // MD5加密
  public static string EncryptString(string str) {
    using MD5 md5 = MD5.Create();
    byte[] byteOld = Encoding.UTF8.GetBytes(str);
    byte[] byteNew = md5.ComputeHash(byteOld);
    StringBuilder sb = new StringBuilder();
    foreach (byte b in byteNew) {
      sb.Append(b.ToString("x2"));
    }

    return sb.ToString();
  }

  public override void GetHelp() {
    Process.Start("https://api.fanyi.baidu.com/doc/21");
  }
}