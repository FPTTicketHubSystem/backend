using backend.DTOs;

namespace backend.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        /*Task<object> GetAllNews();*/
        Task<object> GetAllNews(string status = "");
        Task<object> GetNewsByAccount(int accountId);
        object AddNews(NewsDTO newsDTO);
        /*object GetNewsById(int newsId);*/
        Task<object> GetNewsById(int newsId);
        object ChangeStatusNews(int newsId, string status);
        /*object GetAllNewsInUserPage();*/
        object GetNewDetail(int newsId);
        object GetNewsByPage(int page, int pageSize);
    }
}
