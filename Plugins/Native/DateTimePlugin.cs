using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_Console.Plugins.Native
{
    public class DateTimePlugin
    {
        [KernelFunction("GetCurrentDateTime")]
        [Description("Get Current DateTime")]
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString();
        }

        [KernelFunction("GetMinRayBirthDay")]
        [Description("Get MinRay's BirthDay")]
        public string GetMinRayBirthDay()
        {
            return "1996/11/25";
        }

    }



}
