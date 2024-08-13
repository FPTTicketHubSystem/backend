using backend.Models;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Text;
using System.Collections.Generic;
using backend.DTOs;

namespace backend.Repositories.StatisticRepository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly FpttickethubContext _context;
        private readonly IConverter _converter;

        public StatisticRepository(FpttickethubContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // Thống kê tổng doanh thu theo tháng
        public async Task<IEnumerable<MonthlyRevenueDTO>> GetMonthlyRevenue()
        {
            return await _context.Payments
                .Where(p => p.Status == "Thanh toán thành công" && p.Order.Status == "Thanh toán thành công")
                .GroupBy(p => new { p.PaymentDate.Value.Year, p.PaymentDate.Value.Month })
                .Select(g => new MonthlyRevenueDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(p => p.PaymentAmount ?? 0)
                })
                .OrderByDescending(r => r.Year).ThenByDescending(r => r.Month)
                .ToListAsync();
        }

        // Thống kê số lượng người đang hoạt động theo tháng
        public async Task<IEnumerable<MonthlyRegisteredUsersDTO>> GetMonthlyActiveUsers()
        {
            try
            {
                var data = await _context.Accounts
                    .Where(a => a.Status == "Đang hoạt động")
                    .GroupBy(a => new { a.CreateDate.Value.Year, a.CreateDate.Value.Month })
                    .Select(g => new MonthlyRegisteredUsersDTO
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalRegisteredUsers = g.Count()
                    })
                    .OrderByDescending(r => r.Year).ThenByDescending(r => r.Month)
                    .ToListAsync();
                if (data == null)
                {
                    return null;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Thống kê top năm sự kiện được đánh giá cao nhất
        public async Task<IEnumerable<TopRatedEventDTO>> GetTopRatedEvents()
        {
            return await _context.Events
                .Where(er => er.Status == "Đã duyệt")
                .Include(e => e.Eventratings)
                .Select(g => new TopRatedEventDTO
                {
                    EventName = g.EventName,
                    AverageRating = g.Eventratings.Average(er => er.Rating ?? 0)
                })
                .OrderByDescending(e => e.AverageRating)
                .Take(5)
                .ToListAsync();
        }

        // Thống kê doanh thu của từng sự kiện
        public async Task<IEnumerable<EventRevenueDTO>> GetEventRevenue()
        {
            return await _context.Events
                .Where(e => e.Status == "Đã duyệt")
                .Select(e => new EventRevenueDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    OrganizerName = e.Account.FullName,
                    OrganizerEmail = e.Account.Email,
                    OrganizerPhoneNumber = e.Account.Phone,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    TotalRevenue = e.Tickettypes
                        .SelectMany(tt => tt.Orderdetails)
                        .SelectMany(od => od.Order.Payments)
                        .Where(p => p.Status == "Thanh toán thành công")
                        .Sum(p => p.PaymentAmount ?? 0)
                })
                .OrderByDescending(e => e.TotalRevenue)
                .ToListAsync();
        }

        // Thống kê năm người tham gia nhiều event nhất
        public async Task<IEnumerable<TopParticipantsDTO>> GetTopParticipants()
        {
            return await _context.Orders
                .Where(o => o.Status == "Thanh toán thành công" && o.Payments.Any(p => p.Status == "Thanh toán thành công"))
                .SelectMany(o => o.Orderdetails)
                .Where(od => od.Tickets.Any(t => t.IsCheckedIn == true))
                .GroupBy(od => od.Order.AccountId)
                .Select(g => new
                {
                    AccountId = g.Key,
                    EventsParticipated = g.Count()
                })
                .Join(_context.Accounts,
                      g => g.AccountId,
                      a => a.AccountId,
                      (g, a) => new TopParticipantsDTO
                      {
                          AccountName = a.FullName,
                          EventsParticipated = g.EventsParticipated
                      })
                .OrderByDescending(tp => tp.EventsParticipated)
                .Take(5)
                .ToListAsync();
        }

        // Thống kê năm sự kiện có doanh thu cao nhất
        public async Task<IEnumerable<TopRevenueEventDTO>> GetTopRevenueEvents()
        {
            return await _context.Events
                .Where(e => e.Status == "Đã duyệt")
                .Select(e => new TopRevenueEventDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    TotalRevenue = e.Tickettypes
                        .SelectMany(tt => tt.Orderdetails)
                        .Where(od => od.Order.Payments.Any(p => p.Status == "Thanh toán thành công"))
                        .Sum(od => od.Quantity * od.TicketType.Price ?? 0)
                })
                .OrderByDescending(e => e.TotalRevenue)
                .Take(5)
                .ToListAsync();
        }

        // Thống kê năm sự kiện có người tham gia nhiều nhất
        public async Task<IEnumerable<TopParticipantsEventDTO>> GetTopParticipantsEvents()
        {
            try
            {
                var data = await _context.Tickets
                    .Include(t => t.OrderDetail)
                    .ThenInclude(od => od.Order)
                    .ThenInclude(o => o.Orderdetails)
                    .ThenInclude(od => od.TicketType)
                    .ThenInclude(tt => tt.Event)
                    .Where(t => t.IsCheckedIn == true)
                    .GroupBy(t => t.OrderDetail.TicketType.Event)
                    .Select(g => new TopParticipantsEventDTO
                    {
                        EventName = g.Key.EventName,
                        ParticipantsCount = g.Sum(t => t.OrderDetail.Quantity ?? 0)
                    })
                    .OrderByDescending(e => e.ParticipantsCount)
                    .Take(5)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<byte[]> GenerateEventStatisticsReport()
        {
            // Lấy dữ liệu từ các phương thức thống kê
            var monthlyRevenues = await GetMonthlyRevenue();
            var activeAccountCount = await GetMonthlyActiveUsers();
            var topRatedEvents = await GetTopRatedEvents();
            var topParticipants = await GetTopParticipants();
            var topRevenueEvents = await GetTopRevenueEvents();

            // Tạo nội dung HTML cho báo cáo
            var htmlContent = new StringBuilder();
            htmlContent.AppendLine("<html>");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<style>");
            htmlContent.AppendLine("body { font-family: Arial, sans-serif; }");
            htmlContent.AppendLine("h1, h2 { text-align: center; }");
            htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            htmlContent.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            htmlContent.AppendLine("th { background-color: #f2f2f2; }");
            htmlContent.AppendLine("</style>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");

            //Tiêu đề
            htmlContent.AppendLine("<h1> Bản Báo Cáo Doanh Thu</h1>");

            // Thống kê số lượng người đã đăng ký
            htmlContent.AppendLine("<h1>Số tài khoản đăng ký theo tháng</h1>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Năm</th><th>Tháng</th><th>Tổng Số Tài Khoản</th></tr>");
            foreach (var count in activeAccountCount)
            {
                htmlContent.AppendLine($"<tr><td>{count.Year}</td><td>{count.Month}</td><td>{count.TotalRegisteredUsers}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            // Thống kê tổng doanh thu theo tháng
            htmlContent.AppendLine("<h1>Thống kê doanh thu theo tháng</h1>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Năm</th><th>Tháng</th><th>Tổng Doanh Thu (VND)</th></tr>");
            foreach (var revenue in monthlyRevenues)
            {
                htmlContent.AppendLine($"<tr><td>{revenue.Year}</td><td>{revenue.Month}</td><td>{revenue.TotalRevenue.ToString("N0")}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            // Thống kê top 5 sự kiện được đánh giá cao nhất
            htmlContent.AppendLine("<h2>Top 5 sự kiện được đánh giá cáo nhất</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Tên sự kiện</th><th>Đánh giá trung bình của sự kiện</th></tr>");
            foreach (var eventRating in topRatedEvents)
            {
                htmlContent.AppendLine($"<tr><td>{eventRating.EventName}</td><td>{eventRating.AverageRating:F2}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            // Thống kê top 5 người tham gia nhiều sự kiện nhất
            htmlContent.AppendLine("<h2>Top 5 cá nhân tham gia nhiều sự kiện nhất</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Tên tài khoản</th><th>Số sự kiện tham gia</th></tr>");
            foreach (var participant in topParticipants)
            {
                htmlContent.AppendLine($"<tr><td>{participant.AccountName}</td><td>{participant.EventsParticipated}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            // Thống kê top 5 sự kiện có doanh thu cao nhất
            htmlContent.AppendLine("<h2>Top 5 sự kiện có doanh thu cao nhất</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Tên sự kiện</th><th>Tổng doanh thu</th></tr>");
            foreach (var revenueEvent in topRevenueEvents)
            {
                htmlContent.AppendLine($"<tr><td>{revenueEvent.EventName}</td><td>{revenueEvent.TotalRevenue.ToString("N0")}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            // Tạo tài liệu PDF từ nội dung HTML
            var pdfDocument = new HtmlToPdfDocument
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4
        },
                Objects = {
            new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent.ToString(),
                WebSettings = { DefaultEncoding = "utf-8" }
            }
        }
            };

            return await Task.Run(() => _converter.Convert(pdfDocument));
        }

    }
}
