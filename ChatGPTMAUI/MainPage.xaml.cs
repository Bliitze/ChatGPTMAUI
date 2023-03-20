using System.Text;
using System.Text.Json;

namespace ChatGPTMAUI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        try
        {
            CompletionRequest completionRequest = new CompletionRequest
            {
                Model = "text-davinci-003",
                Prompt = Entry1.Text,
                MaxTokens = 500,
                
            };
            CompletionResponse completionResponse = new CompletionResponse();
            string apiKey = "YOUR-API-KEY";
            using (HttpClient httpClient = new HttpClient())
            {
                using (var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions"))
                {
                    httpReq.Headers.Add("Authorization", $"Bearer {apiKey}");
                    httpReq.Headers.Add("OpenAI-Organization", "YOUR-ORGANIZATION-ID");
                    string requestString = JsonSerializer.Serialize(completionRequest);
                    httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage? httpResponse = await httpClient.SendAsync(httpReq))
                    {
                        if (httpResponse is not null)
                        {
                            if (httpResponse.IsSuccessStatusCode)
                            {
                                string responseString = await httpResponse.Content.ReadAsStringAsync();
                                {
                                    if (!string.IsNullOrWhiteSpace(responseString))
                                    {
                                        completionResponse = JsonSerializer.Deserialize<CompletionResponse>(responseString);
                                    }
                                }
                            }
                            else
                            {
                                string responseString = await httpResponse.Content.ReadAsStringAsync();
                                await DisplayAlert("Error", responseString, "Acept");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error","IS NULL",  "Acept");
                        }
                        if (completionResponse is not null)
                        {
                            string? completionText = completionResponse.Choices?[0]?.Text;
                            Label1.Text = completionText;
                           // Console.WriteLine(completionText);
                           //Entry1.Text = completionText;
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Acept");
        }
	}
}

