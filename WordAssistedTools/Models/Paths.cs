using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.Models {
  internal static class Paths {
    public static readonly string DataPath = AppDomain.CurrentDomain.BaseDirectory;

    private const string TransWebConfigJson = "TransWebConfig.json";
    private const string TransApiConfigJson = "TransApiConfig.json";
    private const string GptTemplatesJson = "QuestionTemplates.json";
    private const string GptWebConfigJson = "GptWebConfig.json";
    private const string GptApiConfigJson = "GptApiConfig.json";

    private const string EdgeEnvFiles = nameof(EdgeEnvFiles);
    public static readonly string EdgeEnvFilesPath = Path.Combine(DataPath, EdgeEnvFiles);
    public static readonly string ProjectName = Assembly.GetExecutingAssembly().GetName().Name;
    public static readonly string LogFileName = $"{ProjectName}_{DateTime.Now:yyyy-MM-dd}.log";
    public static readonly string LogFilePath = Path.Combine(DataPath, "Logs", LogFileName);
    public static readonly string TransApiDllPath = Path.Combine(DataPath, "Translators");

#if DEBUG
    public static readonly string ResourcesPath = $"..\\..\\{ProjectName}\\Resources";
  #else
    public static readonly string ResourcesPath = "Resources";
  #endif
    public static string TransWebConfigJsonPath => Path.Combine(DataPath, ResourcesPath, TransWebConfigJson);
    public static string TransApiConfigJsonPath => Path.Combine(DataPath, ResourcesPath, TransApiConfigJson);
    public static string GptWebConfigJsonPath => Path.Combine(DataPath, ResourcesPath, GptWebConfigJson);
    public static string GptApiConfigJsonPath => Path.Combine(DataPath, ResourcesPath, GptApiConfigJson);
    public static string GptTemplateConfigJsonPath => Path.Combine(DataPath, ResourcesPath, GptTemplatesJson);

  }
}
