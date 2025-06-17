using Microsoft.SemanticKernel;

namespace RAG_Console.Services.Interfaces
{
    public interface IChatApiService
    {
        Task Chat(string userInput, Kernel kernel);

        Task ChatApi(string apiKey);
    }
}