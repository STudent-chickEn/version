using System;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace AIChatBotWinForms
{
    public partial class Form1 : Form
    {
        private const string apiKey = "YOUR_OPENAI_API_KEY";
        private const string apiUrl = "https://api.openai.com/v1/chat/completions";

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userInput = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            rtbChatHistory.AppendText("You: " + userInput + "\n");

            string aiResponse = await GetAIResponse(userInput);
            rtbChatHistory.AppendText("AI: " + aiResponse + "\n\n");

            txtUserInput.Clear();
        }

        private async Task<string> GetAIResponse(string prompt)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var requestBody = new
            {
                model = "your AI version( Exam:gpt-4,...",
                messages = new[]
                {
                    new { role = "system", content = "content" },
                    new { role = "user", content = prompt }
                }
            };

            request.AddJsonBody(requestBody);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                return jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim() ?? "error";
            }
            return "error connect";
        }
    }
}
