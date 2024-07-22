using backend.DTOs;
using backend.Repositories.EventRepository;
using backend.Repositories.NewsRepository;

namespace backend.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<object> GetAllNewsAdmin(string status = "")
        {
            return await _newsRepository.GetAllNewsAdmin(status);
        }
        public async Task<object> GetAllNews()
        {
            return await _newsRepository.GetAllNews();
        }
        public async Task<object> GetNewsByAccount(int accountId)
        {
            return await _newsRepository.GetNewsByAccount(accountId);
        }
        public object AddNews(NewsDTO newsDTO)
        {
            var result = _newsRepository.AddNews(newsDTO);
            return result;
        }
        /* public object GetNewsById(int newsId)
         {
             var result = _newsRepository.GetNewsById(newsId);
             return result;
         }*/
        public Task<object> GetNewsById(int newsId)
        {
            var result = _newsRepository.GetNewsById(newsId);
            return result;
        }
        /*        public object GetAllNewsInUserPage()
                {
                    return _newsRepository.GetAllNewsInUserPage();
                }*/

        public object GetNewDetail(int newsId)
        {
            return _newsRepository.GetNewDetail(newsId);
        }

        public object GetNewsByPage(int page, int pageSize)
        {
            return _newsRepository.GetNewsByPage(page, pageSize);
        }
        public object ChangeStatusNews(int newsId, string status)
        {
            return _newsRepository.ChangeStatusNews(newsId, status);
        }
    }
}

