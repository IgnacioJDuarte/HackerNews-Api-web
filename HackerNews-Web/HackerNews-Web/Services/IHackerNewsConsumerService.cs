using HackerNews_Web.Models;

namespace HackerNews_Web.Services
{
    public interface IHackerNewsConsumerService
    {
        Task<Stories[]?> GetStories();
    }
}