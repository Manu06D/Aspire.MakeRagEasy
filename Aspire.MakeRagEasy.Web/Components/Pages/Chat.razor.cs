using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Aspire.MakeRagEasy.Web.Components.Pages
{
    public partial class Chat
    {
        [Inject]
        private IJSRuntime JS { get; set; }
        [Inject]
        private AiApiClient aiApiClient { get; set; }

        private List<ChatMessage> messages = [];
        private string userInput = "";
        private bool isLoading = false;

        private async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(userInput) && !isLoading)
            {
                messages.Add(new ChatMessage { Content = userInput, IsUser = true });

                isLoading = true;
                var result = await aiApiClient.AskQuestionToAI(userInput);
                userInput = "";

                StateHasChanged();

                isLoading = false;
                messages.Add(new ChatMessage { Content = result, IsUser = false });
                StateHasChanged();

                await JS.InvokeVoidAsync("scrollToBottom", "chat-messages");
            }
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendMessage();
            }
        }

        private class ChatMessage
        {
            public string Content { get; set; }
            public bool IsUser { get; set; }
        }
    }
}