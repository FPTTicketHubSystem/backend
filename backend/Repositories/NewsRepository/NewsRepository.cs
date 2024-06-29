using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.NewsRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly FpttickethubContext _context;

        public NewsRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public async Task<object> GetAllNews()
        {
            var data = _context.News
                .Include(n => n.Account)
                .Where(n => n.Status == "Đã duyệt")
                .OrderByDescending(n => n.CreateDate)
                .Select(n =>
                new
                {
                    n.NewsId,
                    n.Account.FullName,
                    n.Account.Avatar,
                    n.CoverImage,
                    n.Title,
                    n.Subtitle,
                    n.Content,
                    n.CreateDate,
                    n.Status,
                });
            return data;
        }

        public async Task<object> GetNewsByAccount(int accountId)
        {
            var data = _context.News
                .Include(n => n.Account)
                .Where(n => n.AccountId == accountId)
                .OrderByDescending(n => n.CreateDate)
                .Select(n =>
                new
                {
                    n.NewsId,
                    n.Account.FullName,
                    n.Account.Avatar,
                    n.CoverImage,
                    n.Title,
                    n.Subtitle,
                    n.Content,
                    n.CreateDate,
                    n.Status,
                });
            return data;
        }

        public object AddNews(NewsDTO newsDTO)
        {
            try
            {
                var newNews = new News
                {
                    AccountId = newsDTO.AccountId,
                    CoverImage = newsDTO.CoverImage,
                    Title = newsDTO.Title,
                    Subtitle = newsDTO.Subtitle,
                    Content = newsDTO.Content,
                    CreateDate = DateTime.Now,
                    Status = "Chờ duyệt",
                };
                _context.News.Add(newNews);
                _context.SaveChanges();
                return new
                {
                    message = "News Added",
                    status = 200,
                    newNews
                };
            }
            catch
            {
                return new
                {
                    message = "Add News Fail",
                    status = 400
                };
            }
        }

        public object GetNewsById(int newsId)
        {
            var data = _context.News
                .Include(n => n.Account)
                .Where(n => n.NewsId == newsId)
                .Select(n =>
                new
                {
                    n.NewsId,
                    n.Account.FullName,
                    n.Account.Avatar,
                    n.CoverImage,
                    n.Title,
                    n.Subtitle,
                    n.Content,
                    n.CreateDate,
                    n.Status,
                }).SingleOrDefault();
            if (data == null )
            {
                return null;
            }
            return data;
        }

        public async Task<object> GetLastestNews()
        {
            var data = _context.News
                .Include(n => n.Account)
                .Where(n => n.Status == "Đã duyệt")
                .OrderByDescending(n => n.CreateDate)
                .Take(4)
                .Select(n =>
                new
                {
                    n.NewsId,
                    n.Account.FullName,
                    n.Account.Avatar,
                    n.CoverImage,
                    n.Title,
                    n.Subtitle,
                    n.Content,
                    n.CreateDate,
                    n.Status,
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

    }
}
