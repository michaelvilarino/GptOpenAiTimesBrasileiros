using AzureOpenAIClient.Http;

namespace ConsoleGpt35OpenAi
{
    public class GptOpenAIService
    {
        private readonly OpenAIClient _client;

        public GptOpenAIService(OpenAIClient client)
        {
            _client = client;
        }

        public async Task<CompletionResponse?> DoWork(string input)
        {
            var completionRequest = new CompletionRequest()
            {
                Prompt = input,
                MaxTokens = 100
            };
            CompletionResponse? completionResponse = await _client.GetTextCompletionResponseAsync(completionRequest);

            return completionResponse;
        }
    }
}
