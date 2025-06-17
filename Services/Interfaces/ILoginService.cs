namespace RAG_Console.Services.Interfaces
{
    public interface ILoginService
    {
        Task<(bool isLogin, string? apiKey)> Login_OpenApikey();

        Task<bool> Login();
    }
}