using backend.DTOs;

namespace backend.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        Task<object> GetAllNews();
        Task<object> GetNewsByAccount(int accountId);
        object AddNews(NewsDTO newsDTO);
        object GetNewsById(int newsId);
        Task<object> GetLastestNews();
    }
}
