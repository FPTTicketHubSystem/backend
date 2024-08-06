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
        public async Task<decimal> GetTotalRevenue()
        {
            return await _context.Orders
           .Where(o => o.Status == "Thanh toán thành công")
           .SumAsync(o => o.Total ?? 0);
        }

        public async Task<int> GetTotalParticipants()
        {
            return await _context.Tickets
            .Where(t => t.Status == "Thanh toán thành công")
            .CountAsync();
        }

        public async Task<IEnumerable<EventRatingDto>> GetTopRatedEvents()
        {
            return await _context.Events
                .Where(e => e.Status == "Đã duyệt")
                .Select(e => new EventRatingDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    AverageRating = e.Eventratings.Average(er => er.Rating) ?? 0
                })
                .OrderByDescending(e => e.AverageRating)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventRevenueDto>> GetTopRevenueEvents()
        {
            return await _context.Events
                .Where(e => e.Status == "Đã duyệt")
                .Select(e => new EventRevenueDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    TotalRevenue = e.Tickettypes
                        .SelectMany(tt => tt.Orderdetails)
                        .Sum(od => od.Subtotal ?? 0)
                })
                .OrderByDescending(e => e.TotalRevenue)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventParticipantsDto>> GetTopParticipantsEvents()
        {
            return await _context.Events
                .Where(e => e.Status == "Đã duyệt")
                .Select(e => new EventParticipantsDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    TotalParticipants = e.Tickettypes
                        .SelectMany(tt => tt.Orderdetails)
                        .Sum(od => od.Quantity ?? 0)
                })
                .OrderByDescending(e => e.TotalParticipants)
                .Take(5)
                .ToListAsync();
        }
        public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenue()
        {
            return await _context.Orders
                .Where(o => o.Status == "Thanh toán thành công")
                .GroupBy(o => new { o.OrderDate.Value.Year, o.OrderDate.Value.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.Total ?? 0)
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToListAsync();
        }
        public async Task<IEnumerable<MonthlyParticipantsDto>> GetMonthlyParticipants()
        {
            return await _context.Tickets
                .Where(t => t.Status == "Thanh toán thành công" && t.OrderDetail.Order.Status == "Thanh toán thành công")
                .GroupBy(t => new { t.OrderDetail.Order.OrderDate.Value.Year, t.OrderDetail.Order.OrderDate.Value.Month })
                .Select(g => new MonthlyParticipantsDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalParticipants = g.Count()
                })
                .OrderBy(p => p.Year)
                .ThenBy(p => p.Month)
                .ToListAsync();
        }
        public async Task<byte[]> GenerateEventStatisticsReport()
        {
            var monthlyRevenues = await GetMonthlyRevenue();
            var totalParticipants = await GetTotalParticipants();
            var topRatedEvents = await GetTopRatedEvents();
            var topRevenueEvents = await GetTopRevenueEvents();
            var topParticipantsEvents = await GetTopParticipantsEvents();

            var htmlContent = new StringBuilder();
            htmlContent.AppendLine("<html>");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<style>");
            htmlContent.AppendLine("body { font-family: Arial, sans-serif; }");
            htmlContent.AppendLine("h1 { text-align: center; }");
            htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            htmlContent.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            htmlContent.AppendLine("th { background-color: #f2f2f2; }");
            htmlContent.AppendLine("</style>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");
            htmlContent.AppendLine("<h1>Monthly Revenue Report</h1>");

            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Year</th><th>Month</th><th>Total Revenue (VND)</th></tr>");
            foreach (var revenue in monthlyRevenues)
            {
                htmlContent.AppendLine($"<tr><td>{revenue.Year}</td><td>{revenue.Month}</td><td>{revenue.TotalRevenue}</td></tr>");
            }
            htmlContent.AppendLine($"<p>Total Participants: {totalParticipants}</p>");

            htmlContent.AppendLine("<h2>Top 5 Rated Events</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Event Name</th><th>Average Rating</th></tr>");
            foreach (var eventRating in topRatedEvents)
            {
                htmlContent.AppendLine($"<tr><td>{eventRating.EventName}</td><td>{eventRating.AverageRating}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            htmlContent.AppendLine("<h2>Top 5 Revenue Events</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Event Name</th><th>Total Revenue</th></tr>");
            foreach (var eventRevenue in topRevenueEvents)
            {
                htmlContent.AppendLine($"<tr><td>{eventRevenue.EventName}</td><td>{eventRevenue.TotalRevenue:C}</td></tr>");
            }
            htmlContent.AppendLine("</table>");

            htmlContent.AppendLine("<h2>Top 5 Participants Events</h2>");
            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<tr><th>Event Name</th><th>Total Participants</th></tr>");
            foreach (var eventParticipants in topParticipantsEvents)
            {
                htmlContent.AppendLine($"<tr><td>{eventParticipants.EventName}</td><td>{eventParticipants.TotalParticipants}</td></tr>");
            }
            htmlContent.AppendLine("</table>");
            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

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
