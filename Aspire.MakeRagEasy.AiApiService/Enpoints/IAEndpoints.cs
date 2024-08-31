using Microsoft.SemanticKernel;

namespace Aspire.MakeRagEasy.AiApiService.Enpoints
{
    public static class IAEndpointRegistration
    {
        public class IARequest()
        {
            public string model { get; set; }
            public string prompt { get; set; }
            public bool stream { get; set; }
        }


        public class IAResponse
        {
            public string Response { get; set; }
        }
        public static async Task<T> GetRequestBody<T>(HttpContext context)
        {
            return await context.Request.ReadFromJsonAsync<T>();
        }

        public static void RegisterOllamaEndpoints(this WebApplication endpoints)
        {
            endpoints.MapGet("/api/tags", () => new
            {
                models = new[] {
                    new
                    {
                        name = "phi:latest",
                        model = "phi:latest",
                        modified_at = DateTime.UtcNow,
                        size = 1602463378,
                        digest = "e2fd6321a5fe6bb3ac8a4e6f1cf04477fd2dea2924cf53237a995387e152ee9c",
                        details = new
                        {
                            parent_model = "",
                            format = "gguf",
                            family = "phi2",
                            families = new[] { "phi2" },
                            parameter_size = "3B",
                            quantization_level = "Q4_0"
                        }
                    }
                }
            }).WithName("Tags");

            endpoints.MapGet("/api/version", () => new
            {
                version = "0.1.32"
            }).WithName("Version");

            endpoints.MapPost("/api/GetAnswer", async (HttpContext httpContext, OllamaService ollamaService) =>
            {
                IARequest request = await GetRequestBody<IARequest>(httpContext);
                var response = await ollamaService.GetSingleChatMessageContentAsync(request.prompt);
                return new IAResponse()
                {
                    Response = response
                };
            }).WithName("Generate");

            //endpoints.MapPost("/api/chat", async Task (HttpContext httpContext, OllamaService ollamaservice, CancellationToken ct) =>
            //{
            //    httpContext.Response.Headers.Add("Content-Type", "text/event-stream");


            //    var request = await GetRequestBody<ChatMessageContent>(httpContext);
            //    var responses = await ollamaservice.GetStreamingChatMessageContentsAsync(request);

            //    foreach (var response in responses.Values)
            //    {
            //        // Server-Sent Events
            //        var sseMessage = $"{JsonSerializer.Serialize(response)}";
            //        await httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage));
            //        await httpContext.Response.Body.FlushAsync();
            //    }

            //}).WithName("ChatCompletion");
        }

    }
}
