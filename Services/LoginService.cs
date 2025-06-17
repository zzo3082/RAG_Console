using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAG_Console.Services.Interfaces;
using Serilog;

namespace RAG_Console.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger _logger;

        public LoginService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///   登入後回傳apiKey
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool isLogin, string? apiKey)> Login_OpenApikey()
        {
            Console.WriteLine("=====開始登入===== ");
            Console.WriteLine("請輸入 password > ");
            var password = Console.ReadLine();
            try
            {
                /// todo : 去DB驗證是否這這個人 可以用密碼當作api key呼叫api驗證是否登入成功



                /// 符合的話回傳他自己的apiKey
                return (true, "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");
            }
            catch (Exception ex)
            {

                _logger.Error($"登入失敗 ex : {ex}");
                return (false, null);
            }
        }


        public async Task<bool> Login()
        {
            Console.WriteLine("=====開始登入===== ");
            Console.WriteLine("請輸入 password > ");
            var password = Console.ReadLine();
            try
            {
                /// todo : 去DB驗證是否這這個人 可以用密碼當作api key呼叫api驗證是否登入成功



                /// 符合的話回傳他自己的apiKey
                return true;
            }
            catch (Exception ex)
            {

                _logger.Error($"登入失敗 ex : {ex}");
                return false;
            }
        }
    }
}
