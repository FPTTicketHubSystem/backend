using backend.DTOs;

namespace backend.Services.NewsService
{
    public interface INewsService
    {
        Task<object> GetAllNews();
        Task<object> GetAllNewsAdmin(string status = "");
        Task<object> GetNewsByAccount(int accountId);
        Task<object> GetNewsById(int newsId);
        object AddNews(NewsDTO newsDTO);
        public object ChangeStatusNews(int newsId, string status);
        /*public object GetAllNewsInUserPage();*/
        public object GetNewDetail(int newsId);
        public object GetNewsByPage(int page, int pageSize);
        object GetNewsForEdit(int newsId);
        object EditNew(NewsDTO updateNews);
        Task<object> GetLastestNews();
        Task<object> GetNewsByIdUser(int newsId);
    }
}

