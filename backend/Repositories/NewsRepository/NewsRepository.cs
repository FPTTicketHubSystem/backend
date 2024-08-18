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

        public async Task<object> GetAllNewsAdmin(string status = "")
        {
            var query = _context.News
                .Include(n => n.Account)
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
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(n => n.Status == status);
            }

            return await query.ToListAsync();
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
                    CreateDate = DateTime.UtcNow,
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

        /*public object GetNewsById(int newsId)
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
        }*/
        public async Task<object> GetNewsById(int newsId)
        {
            var news = await _context.News
                .Include(n => n.Account)
                .Where(n => n.NewsId == newsId)
                .Select(n => new
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
                })
                .FirstOrDefaultAsync();
            return news;
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
        public List<OtherNews> OtherNews()
        {
            var otherNewsList = _context.News.Take(3).ToList();
            List<OtherNews> otherNews = new List<OtherNews>();

            foreach (var otherNewsItem in otherNewsList)
            {
                OtherNews other = new OtherNews();
                other.NewsId = otherNewsItem.NewsId;

                other.Title = otherNewsItem.Title;
                other.Image = otherNewsItem.CoverImage;
                other.SubTitle = otherNewsItem.Subtitle;
                otherNews.Add(other);
            }
            return otherNews;
        }
        public object GetAllNewsInUserPage()
        {
            try
            {
                var firstNews = _context.News.OrderByDescending(x => x.NewsId).FirstOrDefault();
                var firstNewsDTO = new FirstNews();
                firstNewsDTO.NewsId = firstNews.NewsId;
                firstNewsDTO.Title = firstNews.Title;
                firstNewsDTO.Image = firstNews.CoverImage;
                firstNewsDTO.CreatedDay = firstNews.CreateDate?.ToString("dd/MM/yyyy");
                var dailyNews = DailyNews();
                var otherNews = OtherNews();
                return new
                {
                    message = "Get data successfully",
                    status = 200,
                    firstNew = firstNewsDTO,
                    dailyNew = dailyNews,
                    otherNew = otherNews,
                };
            }
            catch
            {
                return new
                {
                    message = "Get data failed",
                    status = 400,
                };
            }
        }
        public object EditNews(NewsDTO newsDTO)
        {
            var news = _context.News.SingleOrDefault(x => x.NewsId == newsDTO.NewsId);
            if (news == null)
            {
                return new
                {
                    message = "Not found to return",
                    status = 400,
                };
            }
            news.Title = newsDTO.Title;
            news.Subtitle = newsDTO.Subtitle;
            news.CoverImage = newsDTO.CoverImage;
            news.Content = newsDTO.Content;
            _context.SaveChanges();
            return new
            {
                message = "Edit Successfully",
                status = 200,
                data = news,
            };
        }
        public object ChangeStatusNews(int newsId, string status)
        {
            var news = _context.News.FirstOrDefault(x => x.NewsId == newsId);
            if (news == null)
            {
                return new
                {
                    message = "Not found to change",
                    status = 400,
                };
            }
            news.Status = status;
            _context.SaveChanges();
            return new
            {
                message = "Change status successfully",
                status = 200,
            };
        }
        public List<DailyNews> DailyNews()
        {
            var firstNews = _context.News.OrderByDescending(x => x.NewsId).FirstOrDefault();
            var newsList = _context.News.Where(x => x.NewsId != firstNews.NewsId).OrderByDescending(x => x.NewsId).ToList();
            List<DailyNews> dailyNews = new List<DailyNews>();
            var count = 0;
            foreach (var currentNews in newsList)
            {
                DailyNews news = new DailyNews();
                news.NewsId = currentNews.NewsId;
                count++;
                if (count == 3)
                {
                    break;
                }
            }
            return dailyNews;
        }
        public object GetNewDetail(int newsId)
        {
            var dailyNews = DailyNews();
            try
            {
                var news = _context.News.SingleOrDefault(x => x.NewsId == newsId);
                var detailNews = new NewDetail();
                detailNews.NewsId = news.NewsId;
                detailNews.Title = news.Title;
                detailNews.Subtitle = news.Subtitle;
                detailNews.Content = news.Content;
                detailNews.CreateDate = news.CreateDate?.ToString("dd-MM-yyyy");
                if (news == null)
                {
                    return new
                    {
                        message = "No data to return",
                        status = 400,
                    };
                }
                return new
                {
                    message = "Get data successfully",
                    status = 200,
                    data = detailNews,
                    dailyNews = dailyNews,
                };
            }
            catch
            {
                return new
                {
                    message = "No data to return",
                    status = 400,
                };
            }
        }
        public object GetNewsByPage(int page, int pageSize)
        {
            try
            {
                int startIndex = (page - 1) * pageSize;
                var totalNews = GetTotalNewsCount();
                var result = _context.News.Skip(startIndex).Take(pageSize);
                return new
                {
                    message = "Get data succfully",
                    status = 200,
                    totalCount = totalNews,
                    data = result,
                };
            }
            catch
            {
                return new
                {
                    message = "Failed",
                    status = 400,
                };
            }
        }
        public int GetTotalNewsCount()
        {
            return _context.News.Count();
        }

        public object GetNewsForEdit(int newsId)
        {
            var data = _context.News
                .Where(n => n.NewsId == newsId)
                .Select(n =>
                new
                {
                    n.NewsId,
                    n.AccountId,
                    n.CoverImage,
                    n.Title,
                    n.Subtitle,
                    n.Content,
                    n.CreateDate,
                    n.Status
                }).SingleOrDefault();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object EditNew(NewsDTO updateNews)
        {
            try
            {
                var existingNews = _context.News.FirstOrDefault(e => e.NewsId == updateNews.NewsId);

                if (existingNews == null)
                {
                    return new
                    {
                        message = "News Not Found",
                        status = 404
                    };
                }

                existingNews.AccountId = updateNews.AccountId;
                existingNews.CoverImage = updateNews.CoverImage;
                existingNews.Title = updateNews.Title;
                existingNews.Subtitle = updateNews.Subtitle;
                existingNews.Content = updateNews.Content;
                _context.SaveChanges();

                return new
                {
                    message = "News Updated",
                    status = 200,
                    existingNews
                };
            }
            catch
            {
                return new
                {
                    message = "Edit News Fail",
                    status = 400
                };
            }
        }

        public async Task<object> GetNewsByIdUser(int newsId)
        {
            var news = await _context.News
                .Include(n => n.Account)
                .Where(n => n.NewsId == newsId)
                .Select(n => new
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
                })
                .FirstOrDefaultAsync();
            return news;
        }
    }
}

