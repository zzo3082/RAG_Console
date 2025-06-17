using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using RAG_Console.Models.Enums;
using RAG_Console.Plugins.Native;
using RAG_Console.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_Console.Hosts
{
    public class ChatHost : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IChatApiService _chatApiService;
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly string _openApiKey;

        public ChatHost(ILogger logger,
            IChatApiService chatApiService,
            ILoginService loginService,
            IRegisterService registerService,
            IConfiguration config,
            IHostApplicationLifetime hostApplicationLifetime
            )
        {
            _logger = logger;
            _chatApiService = chatApiService;
            _loginService = loginService;
            _registerService = registerService;
            _hostApplicationLifetime = hostApplicationLifetime;
            _openApiKey = config.GetSection("OpenAI")["ApiKey"].ToString();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                ///決定要註冊還是登入
                Console.WriteLine("1. 註冊\r\n2. 登入\r\n請輸入數字1或2");
                var userOption = Console.ReadLine();
                if (!int.TryParse(userOption, out int userOpt))
                {
                    Console.WriteLine("輸入的不是數字");
                    throw new Exception("輸入的不是數字");
                }

                if (userOpt == (int)EUserOption.Register)
                {
                    /// 註冊
                    var registerResult = await _registerService.Register();
                    if (registerResult)
                    {
                        await _chatApiService.ChatApi(_openApiKey);
                    }
                    else
                    {
                        throw new Exception("註冊失敗.");
                    }
                }
                else if (userOpt == (int)EUserOption.Login)
                {
                    /// 登入
                    var loginResult = await _loginService.Login();
                    if (loginResult)
                    {
                        //await _chatApiService.ChatApi(loginResult.apiKey);
                        await _chatApiService.ChatApi(_openApiKey);
                    }
                    else
                    {
                        throw new Exception("登入失敗, 請確認帳號密碼.");
                    }
                }
                else
                {
                    Console.WriteLine("輸入數字不是1或2");
                    throw new Exception("輸入數字不是1或2");
                }

                /// 問完關閉程式
                _hostApplicationLifetime.StopApplication();
            }
            catch (Exception ex)
            {
                _logger.Error($"ChatHost Failed ex : {ex}");
                throw;
            }
        }
    }
}
