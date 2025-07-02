using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskTracker.Domain;

namespace TaskTracker.Application
{
    public class KeywordTaskCategorizer : ITaskCategorizer
    {
        #region Properties

        private readonly HttpClient httpClient;
        private readonly string apiToken;
        private readonly string modelName;
        private readonly string baseUrl;

        #endregion

        #region Constructor

        public KeywordTaskCategorizer(IConfiguration config)
        {
            httpClient = new HttpClient();

            apiToken = config["ApiToken"] ?? throw new ArgumentNullException("ApiToken not configured");
            modelName = config["Model"] ?? AIConstants.DefaultModel;
            baseUrl = config["ApiUrl"] ?? AIConstants.DefaultBaseUrl;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                AIConstants.AuthHeaderScheme, apiToken);
        }

        #endregion

        #region Public Methods

        public async Task<string> CategorizeAsync(string description)
        {
            var payload = new
            {
                inputs = description,
                parameters = new
                {
                    candidatelabels = AIConstants.CandidateLabels,
                    hypothesistemplate = AIConstants.HypothesisTemplate,
                    multilabel = false,
                    waitformodel = true
                },
                options = new
                {
                    usecache = true,
                    waitformodel = true
                }
            };

            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/{modelName}", payload);

            if (!response.IsSuccessStatusCode)
                return "Other";

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var labels = json.GetProperty("labels");
            var scores = json.GetProperty("scores");

            int best = 0;
            for (int i = 1; i < scores.GetArrayLength(); i++)
            {
                if (scores[i].GetDouble() > scores[best].GetDouble())
                    best = i;
            }

            return labels[best].GetString() ?? "Other";
        }

        #endregion

    }
}
