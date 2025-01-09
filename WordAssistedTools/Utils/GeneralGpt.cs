using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using WordAssistedTools.Models;
using WordAssistedTools.Properties;

namespace WordAssistedTools.Utils;

internal class GeneralGpt {
  public static string GptApiUrl { get; set; }
  public static string GptApiKey { get; set; }
  public static string GptApiModel { get; set; }
  public static float GptApiTem { get; set; }
  public static string GptApiSystem { get; set; }

  public static Settings Sets => Settings.Default;

  public GeneralGpt() {
    Initialize();
  }

  public ChatClient Client { get; private set; }
  public readonly List<ChatMessage> Messages = [];
  public ChatCompletionOptions CompletionOptions { get; private set; }
  public List<ChatMessage> UsedMessages {
    get {
      if (Sets.GptApiHistoryNum < 0) {
        return Messages;
      }

      List<ChatMessage> results = [Messages[0]];
      int usedNum = Sets.GptApiHistoryNum * 2;
      int skipNum = Messages.Count - 1 - usedNum;

      results.AddRange(skipNum > 1 ? Messages.Skip(skipNum) : Messages.Skip(1));
      return results;
    }
  }

  public void Initialize() {
    if (string.IsNullOrEmpty(GptApiUrl) || string.IsNullOrEmpty(GptApiKey) || string.IsNullOrEmpty(GptApiModel) || GptApiTem <= 0) {
      throw new Exception("GPT API configuration is not complete.");
    }

    OpenAIClientOptions options = new() {
      Endpoint = new Uri(GptApiUrl),
    };

    ApiKeyCredential credential = new(GptApiKey);
    Client = new ChatClient(GptApiModel, credential, options);
    SystemChatMessage startMessage = new(GptApiSystem);
    Messages.Clear();
    Messages.Add(startMessage);

    CompletionOptions = new ChatCompletionOptions {
      Temperature = GptApiTem,
    };
  }

  // 非流式传输
  public async Task<string> GetResponseAsync(string request) {
    Messages.Add(new UserChatMessage(request));
    int timeout = Sets.GptApiTimeoutSecond;
    using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
    ChatCompletion completion = await Client.CompleteChatAsync(UsedMessages, CompletionOptions, cts.Token);
    string result = completion.Content[0].Text;
    Messages.Add(new AssistantChatMessage(result));
    return result;
  }



}