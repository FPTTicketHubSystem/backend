using backend.DTOs;

namespace backend.Services.NewsService
{
    public interface INewsService
    {
        Task<object> GetAllNews();
        Task<object> GetNewsByAccount(int accountId);
        object AddNews(NewsDTO newsDTO);
        object GetNewsById(int newsId);
        Task<object> GetLastestNews();
    }
}
