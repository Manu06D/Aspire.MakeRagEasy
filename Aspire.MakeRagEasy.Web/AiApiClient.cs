using System.Text.Json;
using System.Text;

namespace Aspire.MakeRagEasy.Web
{
    public class AiApiClient(HttpClient httpClient)
    {
        public async Task<string> AskQuestionToAI(string question, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(new IARequest() { model = "phi:latest", prompt= question, stream=false });

            var response = await httpClient.PostAsync("/api/GetAnswer/", new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);

            string answer = await response.Content.ReadAsStringAsync();

            IAResponse result =  await response.Content.ReadFromJsonAsync<IAResponse>();

            return result.response;
        }

        public class IARequest()
        {
            public string model { get; set; }
            public string prompt { get; set; }
            public bool stream { get; set; }
        }

        public class IAResponse()
        {
            public string response { get; set; }
        }
    }
}
