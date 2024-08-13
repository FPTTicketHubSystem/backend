using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class EventDTO
    {
        public int EventId { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string EventName { get; set; }
        public string ThemeImage { get; set; }
        public string EventDescription { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        //public List<EventImageDTO> EventImages { get; set; }
        public List<TicketTypeDTO> TicketTypes { get; set; }
        //public List<DiscountCodeDTO> DiscountCodes { get; set; }
    }

    //public class EventImageDTO
    //{
    //    public string ImageUrl { get; set; }
    //    public string Status { get; set; }
    //}

    public class TicketTypeDTO
    {
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class DiscountCodeDTO
    {
        //public int DiscountCodeId { get; set; }

        public int AccountId { get; set; }

        public int EventId { get; set; }

        public string Code { get; set; }

        public int DiscountAmount { get; set; }

        public int Quantity { get; set; }
        public string Status { get; set; }
    }
    public class EventRevenueDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class EventParticipantsDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TotalParticipants { get; set; }
    }
    public class EventRatingDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public double AverageRating { get; set; }
    }
    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class MonthlyParticipantsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalParticipants { get; set; }
    }
}
