using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAG_Console.Services.Interfaces;

namespace RAG_Console.Services
{
    public class RegisterService : IRegisterService
    {
        public RegisterService()
        {

        }

        public async Task<bool> Register()
        {
            /// 註冊功能
            Console.WriteLine("=====開始註冊===== ");
            Console.WriteLine("請輸入 userName > ");
            var userName = Console.ReadLine();
            Console.WriteLine("請輸入 password > ");
            var password = Console.ReadLine();

            /// todo : 把註冊資料存到DB
            Console.WriteLine("=====註冊成功===== ");
            return true;
        }
    }
}
