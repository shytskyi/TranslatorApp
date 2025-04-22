using BusinessLogicLayer.Models.DeserializetionModels;
using BusinessLogicLayer.Services.Interfaces;
using System.Text.Json;

namespace BusinessLogicLayer.Services
{
    public class TranslateTol33tsp34kService : ITranslationService
    {
        private readonly HttpClient _client;

        public TranslateTol33tsp34kService(HttpClient client)
            => _client = client;

        public TranslateTol33tsp34kService() : this(new HttpClient())
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<string> TranslateToLeetSpeakAsync(string originalText)
        {
            string apiUrl = "https://api.funtranslations.com/translate/leetspeak.json?text=" + Uri.EscapeDataString(originalText);
            try
            {
                var response = await _client.GetStringAsync(apiUrl);
                var result = JsonSerializer.Deserialize<TranslationResponse>(response);
                return result!.contents.translated;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Translation error: {ex.Message}");
            }
        }
    }
}
