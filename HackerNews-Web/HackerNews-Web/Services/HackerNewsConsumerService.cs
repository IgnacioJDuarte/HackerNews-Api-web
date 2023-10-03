using HackerNews_Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HackerNews_Web.Services
{
    public class HackerNewsConsumerService : IHackerNewsConsumerService
    {
        private readonly ILogger<HackerNewsConsumerService> _logger;
        private readonly IConfiguration _configuration;

        public HackerNewsConsumerService(ILogger<HackerNewsConsumerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Stories[]?> GetStories()
        {
            try
            {

                var newstories = await GetLastestStories();
                var storiesArray = new List<Stories>();
                for (int i = 0; i < newstories.Stories.Count; i++)
                {
                    var story = await GetStoryById(newstories.Stories[i]);
                    if (story.type == "story" && !String.IsNullOrEmpty(story.url))
                    {
                        storiesArray.Add(story);
                    }
                }
                return storiesArray.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurs");
            }

            return null;
        }


        private async Task<NewStories> GetLastestStories()
        {
            var url = $"{_configuration.GetValue<string>("HackerNewsUrl")}/newstories.json";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = String.Empty;
                jsonResponse = await response.Content.ReadAsStringAsync();
                var newStories = new NewStories
                {
                    Stories = JsonSerializer.Deserialize<List<int>>(jsonResponse)
                };
                return newStories;
            }

            return null;
        }

        private async Task<Stories> GetStoryById(int id)
        {
            var url = $"{_configuration.GetValue<string>("HackerNewsUrl")}/item/{id}.json";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var story = JsonSerializer.Deserialize<Stories>(jsonResponse);
                return story;
            }

            _logger.LogTrace($"Id [{id}], returns [{response.StatusCode}]");
            return null;
        }
    }
}
