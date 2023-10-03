using HackerNews_Web.Models;
using HackerNews_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews_Web.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class HackerNewsController : ControllerBase
    {
        private readonly ILogger<HackerNewsController> _logger;
        private readonly IHackerNewsConsumerService _hackerNewsConsumerService;
        private readonly IMemoryCache _cache;
        private const string storiesListCacheKey = "storiesList";

        public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsConsumerService hackerNewsConsumerService, IMemoryCache cache)
        {
            _logger = logger;
            _hackerNewsConsumerService = hackerNewsConsumerService;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Stories[]> Get()
        {
            try
            {
                if (_cache.TryGetValue(storiesListCacheKey, out Stories[] storiesResponse))
                {
                    return storiesResponse;
                }

                var response = await _hackerNewsConsumerService.GetStories();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
                _cache.Set(storiesListCacheKey, response, cacheEntryOptions);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured when excetued the request");
                throw;
            }
        }
    }
}
