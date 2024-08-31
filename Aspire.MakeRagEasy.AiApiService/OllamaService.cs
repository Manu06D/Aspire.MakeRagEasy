using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Aspire.MakeRagEasy.AiApiService
{
    public class OllamaService
    {
        private Kernel _kernel;
        private IChatCompletionService _chatCompletionService;

        public OllamaService(Kernel kernel)
        {
            _kernel = kernel;
            _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetSingleChatMessageContentAsync(string prompt)
        {
            KernelArguments arguments = new(new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.5 }) {  };

            //one shot question
            //var response = await _kernel.InvokePromptAsync(prompt, arguments);

            var chatHistory = new ChatHistory("""
                                           You are a friendly assistant who helps users by answering their question.
                                           You will always answer in French.
                                           Respond as JSON in the following form : {
                                            "answer" : "string"
                                           }
                                           """);

            var response = await _chatCompletionService.GetChatMessageContentAsync(
                chatHistory: chatHistory,
                //executionSettings: settings,
                kernel: _kernel);

            return response.ToString();

            ChatMessageContent chatResponse = await _chatCompletionService.GetChatMessageContentAsync(prompt);
            return chatResponse.ToString();
        }
    }
}
