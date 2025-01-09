# 翻译API开发指南
开发者需要引用WordAssistedTools.SDK，想要实现几个平台的翻译API，就创建几个类，每个类需要继承BaseTranslator类，然后实现其中的抽象属性和方法。  
开发完毕后，将类库文件复制到WordAssistedTools的Translators文件夹下，插件在加载时能够自动检测可用的翻译API。  
该BaseTranslator类的定义如下：  
```cs
public abstract class BaseTranslator {
  /// <summary>
  /// 翻译API的默认名称，后期可由用户定义
  /// </summary>
  public abstract string DefaultName { get; }

  /// <summary>
  /// 翻译API的请求链接
  /// </summary>
  public abstract string BaseUrl { get; }

  /// <summary>
  /// 指示翻译API的密钥填写格式
  /// </summary>
  public abstract string DefaultKeyForm { get; }

  /// <summary>
  /// 必须指定以下常见的语言在该翻译API中的语言代码：自动检测、中文（简体）、中文（繁体）、英语、法语、俄语、西班牙语、德语、日语、韩语、阿拉伯语、葡萄牙语
  /// </summary>
  protected abstract Dictionary<Language, string> LanguageCodeMap { get; }

  /// <summary>
  /// 翻译文本
  /// </summary>
  /// <param name="key">按DefaultKeyForm填写的密钥</param>
  /// <param name="text">原始文本</param>
  /// <param name="source">源语言</param>
  /// <param name="target">目标语言</param>
  /// <returns></returns>
  public abstract Task<string> TranslateAsync(string key, string text, Language source, Language target);

  /// <summary>
  /// 获取帮助信息
  /// </summary>
  public abstract void GetHelp();
}
```

