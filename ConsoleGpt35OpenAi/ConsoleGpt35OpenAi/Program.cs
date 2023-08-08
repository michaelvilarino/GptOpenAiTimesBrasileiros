using AzureOpenAIClient.Http;
using ConsoleGpt35OpenAi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OpenAiTest
{
    public class Program
    {
        private readonly GptOpenAIService _gptOpenAIService;

        public Program(GptOpenAIService gptOpenAIService)
        {
            _gptOpenAIService = gptOpenAIService;
        }

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var program = services.GetRequiredService<Program>();
                await program.ObterRespostaTimes("Quem são os rivais entre times paulistas?");
            }
        }

        public async Task ObterRespostaTimes(string pergunta)
        {
            var stream = await _gptOpenAIService.DoWork(pergunta);

            if(stream != null)
              Console.WriteLine(stream.Choices[0].Text);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {                                        
                    services.AddOpenAIClient("https://aitestmichael.openai.azure.com/",
                                              "03757689629940c3ae1c211aa896e91d",
                                              "gpt-chat", 
                                              "2023-03-15-preview");

                    services.AddScoped<GptOpenAIService>();
                    services.AddTransient<Program>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
    }
}
