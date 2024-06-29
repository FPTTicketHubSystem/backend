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
        public object GetNewsById(int newsId)
        {
            var result = _newsRepository.GetNewsById(newsId);
            return result;
        }
        public async Task<object> GetLastestNews()
        {
            return await _newsRepository.GetLastestNews();
        }
    }
}
