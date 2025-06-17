using System;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAG_Console.Services.Interfaces;
using RAG_Console.Plugins.Native;

namespace RAG_Console.Services
{
    public class ChatApiService : IChatApiService
    {
        public ChatApiService()
        {

        }

        public async Task Chat(string userInput, Kernel kernel)
        {
            OpenAIPromptExecutionSettings setting = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            };
            Console.WriteLine("ChatGpt > ");
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            var streamingResult = chatCompletionService.GetStreamingChatMessageContentsAsync(userInput, setting, kernel);
            StringBuilder resultString = new();
            await foreach (var item in streamingResult)
            {
                resultString.Append(item);
                Console.Write(item);
            }
            Console.WriteLine();

        }

        public async Task ChatApi(string apiKey)
        {
            /// 登入成功建立kernel
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion("gpt-3.5-turbo-0125", apiKey);
            var kernel = builder.Build();

            /// add plugins
            kernel.Plugins.AddFromType<DateTimePlugin>();

            /// 開始用自己的kernel講話
            Console.WriteLine("=====開始對話, 想要退出的話請輸入 \"q\" =====");
            ChatHistory chatHistory = new ChatHistory();

            /// 給他一個角色定位
            //chatHistory.AddMessage(AuthorRole.System, "你是教授");

            /// 額外資訊 可以是json字串
            chatHistory.AddMessage(AuthorRole.Tool, "{  \"漢神DB的連線字串\": \"hanshin.db.connect\",  \"AIA DB的連線字串\": \"aia.db.connect\"}");

            while (true)
            {
                Console.WriteLine("User > ");
                var userInput = Console.ReadLine();

                /// 問問題
                chatHistory.AddMessage(AuthorRole.User, userInput);
                if (!String.IsNullOrEmpty(userInput) && userInput != "q")
                {
                    OpenAIPromptExecutionSettings setting = new()
                    {
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    };
                    Console.WriteLine("Gpt > ");
                    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                    //var streamingResult = chatCompletionService.GetStreamingChatMessageContentsAsync(userInput, setting, kernel);
                    var streamingResult = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, setting, kernel);
                    StringBuilder resultString = new();
                    await foreach (var item in streamingResult)
                    {
                        resultString.Append(item);
                        Console.Write(item);
                    }

                    /// 將回答存到這個session內
                    chatHistory.AddMessage(AuthorRole.Assistant, resultString.ToString());
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("=====結束對話=====");
                    break;
                }
            }

        }
    }
}

