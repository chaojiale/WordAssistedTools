using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WordAssistedTools.SDK;

namespace CommonTranslators;

internal class YoudaoTranslator : BaseTranslator {
  public override string DefaultName => "有道";
  public override string BaseUrl => "https://openapi.youdao.com/api";
  public override string DefaultKeyForm => "appid#appsecret#vocabid(optional)";

  protected override Dictionary<Language, string> LanguageCodeMap { get; } = new() {
    [Language.Auto] = "auto",
    [Language.Chinese] = "zh-CHS",
    [Language.ChineseT] = "zh-CHT",
    [Language.English] = "en",
    [Language.French] = "fr",
    [Language.Russian] = "ru",
    [Language.Spanish] = "es",
    [Language.German] = "de",
    [Language.Japanese] = "ja",
    [Language.Korean] = "ko",
    [Language.Arabic] = "ar",
    [Language.Portuguese] = "pt",
  };

  public override async Task<string> TranslateAsync(string key, string text, Language source, Language target) {
    try {
      // 添加鉴权相关参数
      string[] keyParts = key.Split('#');
      if (keyParts.Length != 2 && keyParts.Length != 3) {
        throw new ArgumentException("Invalid key format. Expected 'appid#secretKey' or 'appid#appsecret#vocabid(optional)'.");
      }

      string appKey = keyParts[0];
      string appSecret = keyParts[1];

      // 添加请求参数
      Dictionary<string, string[]> paramsMap = new Dictionary<string, string[]>() {
        {"q", [text] },
        {"from", [LanguageCodeMap[source]] },
        {"to", [LanguageCodeMap[target]] },
      };

      if (keyParts.Length == 3) {
        paramsMap.Add("vocabId", [keyParts[2]]);
      }

      AddAuthParams(appKey, appSecret, paramsMap);
      Dictionary<string, string[]> header = new() {
        ["Content-Type"] = ["application/x-www-form-urlencoded"]
      };
      // 请求api服务
      byte[] result = await DoPost("https://openapi.youdao.com/api", header, paramsMap, "application/json");

      string resStr = Encoding.UTF8.GetString(result);

      // 解析 JSON 字符串
      JObject jsonObject = JObject.Parse(resStr);

      // 提取 "translation" 中的值
      JArray translationArray = (JArray)jsonObject["translation"];
      string translation = translationArray[0].ToString();
      return translation;
    } catch (Exception ex) {
      return $"Error: {ex.Message}";
    }
  }

  /// <summary>
  /// 添加鉴权相关参数
  /// appKey : 应用ID
  /// salt : 随机值
  /// curtime : 当前时间戳(秒)
  /// signType : 签名版本
  /// sign : 请求签名
  /// </summary>
  /// <param name="appKey">应用ID</param>
  /// <param name="appSecret">应用密钥</param>
  /// <param name="paramsMap">请求参数表</param>
  private static void AddAuthParams(string appKey, string appSecret, Dictionary<string, string[]> paramsMap) {
    string q = "";
    string[] qArray = paramsMap.TryGetValue("q", out var value) ? value : paramsMap["img"];

    foreach (string item in qArray) {
      q += item;
    }
    string salt = Guid.NewGuid().ToString();
    string curtime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + "";
    string sign = CalculateSign(appKey, appSecret, q, salt, curtime);
    paramsMap.Add("appKey", [appKey]);
    paramsMap.Add("salt", [salt]);
    paramsMap.Add("curtime", [curtime]);
    paramsMap.Add("signType", ["v3"]);
    paramsMap.Add("sign", [sign]);
  }

  /// <summary>
  /// 计算鉴权签名 -
  /// 计算方式 : sign = sha256(appKey + input(q) + salt + curtime + appSecret)
  /// </summary>
  /// <param name="appKey">您的应用ID</param>
  /// <param name="appSecret">您的应用密钥</param>
  /// <param name="q">请求内容</param>
  /// <param name="salt">随机值</param>
  /// <param name="curtime">当前时间戳(秒)</param>
  /// <returns></returns>
  private static string CalculateSign(string appKey, string appSecret, string q, string salt, string curtime) {
    string strSrc = appKey + GetInput(q) + salt + curtime + appSecret;
    return Encrypt(strSrc);
  }

  private static string Encrypt(string strSrc) {
    byte[] inputBytes = Encoding.UTF8.GetBytes(strSrc);
    HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
    byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
    return BitConverter.ToString(hashedBytes).Replace("-", "");
  }

  private static string GetInput(string q) {
    if (q == null) {
      return "";
    }
    int len = q.Length;
    return len <= 20 ? q : q.Substring(0, 10) + len + q.Substring(len - 10, 10);
  }

  private static async Task<byte[]> DoPost(string url, Dictionary<string, string[]> header, Dictionary<string, string[]> param, string expectContentType) {
    StringBuilder content = new StringBuilder();
    using HttpClient client = new HttpClient();
    if (param != null) {
      int i = 0;
      foreach (KeyValuePair<string, string[]> p in param) {
        foreach (string v in p.Value) {
          if (i > 0) {
            content.Append("&");
          }
          content.AppendFormat("{0}={1}", p.Key, System.Web.HttpUtility.UrlEncode(v));
          i++;
        }

      }
    }

    StringContent para = new StringContent(content.ToString());
    if (header != null) {
      para.Headers.Clear();
      foreach (KeyValuePair<string, string[]> h in header) {
        foreach (string v in h.Value) {
          para.Headers.Add(h.Key, v);
        }
      }
    }
    HttpResponseMessage res = await client.PostAsync(url, para);
    bool suc = res.Content.Headers.TryGetValues("Content-Type", out IEnumerable<string> contentTypeHeader);

    if (suc && !((string[])contentTypeHeader)[0].Contains(expectContentType)) {
      string result = await res.Content.ReadAsStringAsync();
      throw new Exception(result);
    }

    return await res.Content.ReadAsByteArrayAsync();
  }

  public override void GetHelp() {
    Process.Start("https://fanyi.youdao.com/openapi/");
  }

}