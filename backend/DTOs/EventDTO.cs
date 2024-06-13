using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class EventDTO
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string EventName { get; set; }
        public string ThemeImage { get; set; }
        public string EventDescription { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TicketQuantity { get; set; }
        public string Status { get; set; }
        public List<EventImageDTO> EventImages { get; set; }
        public List<TicketTypeDTO> TicketTypes { get; set; }
        public List<DiscountCodeDTO> DiscountCodes { get; set; }
    }

    public class EventImageDTO
    {
        public string ImageUrl { get; set; }
        public string Status { get; set; }
    }

    public class TicketTypeDTO
    {
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class DiscountCodeDTO
    {
        public string Code { get; set; }
        public int DiscountAmount { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
