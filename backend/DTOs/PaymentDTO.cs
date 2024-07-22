using backend.Models;
using System.Security.Cryptography.Pkcs;

namespace backend.DTOs
{
    public class PaymentDTO
    {
        public int AccountId { get; set; }
        public int EventId { get; set; }
        public decimal TotalPayment { get; set; }

        public List <TicketBuyed> TicketBuyeds { get; set; }
        public DateTime? OrderDate { get; set; }
    }

    public class TicketBuyed
    {
        public int TicketTypeId {  get; set; }
        public decimal PriceTicket { get; set; }
        public int Quantity { get; set; }
    }

    public class ReturnPaymentURL
    {
        public int OrderId { get; set; }
        public string DiscountCode { get; set; }
    }
}
