using Azure;
using backend.DTOs;
using backend.Models;
using backend.Services.NewsService;
using backend.Services.OtherService;
using backend.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using System.Globalization;

namespace backend.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly PayOS _payOS;
        private readonly FpttickethubContext _context;
        public PaymentController(IPaymentService paymentService,FpttickethubContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost("paymentForUser")]
        public async Task<ActionResult> PaymentForUser(PaymentDTO paymentDTO)
        {
            try
            {
                var result = _paymentService.Payment(paymentDTO);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("returnAvaliableTicket")]
        public async Task<ActionResult> ReturnAvaliableTicket(int eventId)
        {
            try
            {
                var result = _paymentService.ReturnAvaliableTicket(eventId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("returnPaymentUrl")]
        public async Task<ActionResult> ReturnPaymentUrl(ReturnPaymentURL returnPaymentURL)
        {
            try
            {
                var result = _paymentService.ReturnPaymentUrl(HttpContext, returnPaymentURL.OrderId, returnPaymentURL.DiscountCode);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("paymentCallBack")]
        public async Task<ActionResult> PaymentCallBack()
        {
            try
            {
                var result = _paymentService.PaymentExcute(Request.Query);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("deleteTimeOutOrder")]
        public async Task<ActionResult> DeleteTimeOutOrder(PaymentDTO paymentDTO)
        {
            try
            {
                var result = _paymentService.DeleteTimeOutOrder(paymentDTO);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("checkInputCoupon")]
        public async Task<ActionResult> CheckInputCoupon(int eventId, string coupon)
        {
            try
            {
                var result = _paymentService.CheckInputCoupon(eventId, coupon);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("CheckOrderdOfUser")]
        public async Task<ActionResult> CheckOrderdOfUser(int userId, int eventId)
        {
            try
            {
                var result = _paymentService.CheckOrderdOfUser(userId, eventId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("CancelOrderOfUser")]
        public async Task<ActionResult> CancelOrderOfUser(int userId)
        {
            try
            {
                var result = _paymentService.CancelOrderOfUser(userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("createPaymentWithPayos")]
        public async Task<ActionResult> CreatePaymentWithPayos(ReturnPaymentURL returnPaymentURL)
        {

            try
            {
                int _orderId = returnPaymentURL.OrderId;
                string _discountCode = returnPaymentURL.DiscountCode;
                Order _order = _context.Orders.Where(x => x.OrderId == _orderId).SingleOrDefault();
                Payment paymentSignUp = new Payment();


                var checkOrderDetails = _context.Orderdetails.FirstOrDefault(x => x.OrderId == returnPaymentURL.OrderId);
                var checkTicketType = _context.Tickettypes.SingleOrDefault(x => x.TicketTypeId == checkOrderDetails.TicketTypeId);
                if (checkTicketType.Price > 0)
                {

                    PayOS _payOS = new PayOS("02e24a69-a908-4188-8f97-3bc3706857ae", "4fcc829a-c2af-431a-ad3f-b86584287b56", "e4ea17f84e63d87e2ccdbbf8e88e443207cdaaa62be0a1e7bc8bca05b62e368e");
                    Discountcode discountCode = _context.Discountcodes.SingleOrDefault(x => x.Code == _discountCode);
                    paymentSignUp.OrderId = _order.OrderId;
                    paymentSignUp.Status = "0";
                    if (discountCode != null)
                    {
                        paymentSignUp.DiscountCodeId = discountCode.DiscountCodeId;
                        paymentSignUp.PaymentAmount = _order.Total * (discountCode.DiscountAmount / 100);
                    }
                    else
                    {
                        paymentSignUp.PaymentAmount = _order.Total;
                    }
                    paymentSignUp.PaymentDate = DateTime.UtcNow;
                    paymentSignUp.PaymentMethodId = 2;

                    var orderDetailList = _context.Orderdetails.Where(x => x.OrderId == _order.OrderId).ToList();
                    List<ItemData> items = new List<ItemData>();
                    if (orderDetailList != null)
                    {
                        foreach (var orderDetail in orderDetailList)
                        {
                            var ticketType = _context.Tickettypes.SingleOrDefault(x => x.TicketTypeId == orderDetail.TicketTypeId);
                            var ticketName = "No Name";
                            if (ticketType != null)
                            {
                                ticketName = ticketType.TypeName;
                            }
                            var quantity = orderDetail.Quantity != null ? orderDetail.Quantity : 0;
                            var price = orderDetail.Subtotal > 0 ? orderDetail.Subtotal : 0;
                            ItemData item = new ItemData(ticketName, (int)quantity, (int)price);

                            items.Add(item);
                        }
                        //PaymentData paymentData = new PaymentData(_order.OrderId, (int)_order.Total, _order.Status, items, body.cancelUrl, body.returnUrl);
                        var returnUrl = "http://localhost:3000/payment-success/" + _order.OrderId;
                        var statusPayment = "Don hang " + _order.OrderId;
                        PaymentData paymentData = new PaymentData(_order.OrderId, (int)_order.Total, statusPayment, items, returnUrl, returnUrl);
                        CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                        return Ok(new
                        {
                            status = 200,
                            message = "Thanh toan thanh cong",
                            createPayment = createPayment, 
                            paymentMethod = 0 // thanh toan bang tien
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            status = 400,
                            message = "Thanh toan that bai"
                        });
                    }
                } else
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Thanh toan thanh cong",
                        paymentMethod = 1 // thanh toan bang tien
                    });

                }
                
                
            }
            catch (System.Exception exception)
            {
                return Ok(new
                {
                    status = 400,
                    message = "Thanh toan that bai"
                });
            }
        }

        [HttpGet("checkOrderId")]
        public async Task<ActionResult> CheckOrderId(int orderId)
        {
            try
            {
                int _orderId = orderId;
                Order _order = _context.Orders.Where(x => x.OrderId == _orderId).SingleOrDefault();
                Payment paymentSignUp = new Payment();


                //var checkOrderDetails = _context.Orderdetails.FirstOrDefault(x => x.OrderId == orderId);
                //var checkTicketType = _context.Tickettypes.SingleOrDefault(x => x.TicketTypeId == checkOrderDetails.TicketTypeId);
                var orderDetails = _context.Orderdetails.Where(x => x.OrderId == orderId).ToList();
                bool isFree = true;
                foreach (var detail in orderDetails)
                {
                    var ticketType = _context.Tickettypes.SingleOrDefault(x => x.TicketTypeId == detail.TicketTypeId);
                    if (ticketType != null && ticketType.Price > 0)
                    {
                        isFree = false;
                        break;
                    }
                }

                if (!isFree)
                {
                    PayOS _payOS = new PayOS("02e24a69-a908-4188-8f97-3bc3706857ae", "4fcc829a-c2af-431a-ad3f-b86584287b56", "e4ea17f84e63d87e2ccdbbf8e88e443207cdaaa62be0a1e7bc8bca05b62e368e");
                    PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                    if (paymentLinkInformation.status == "PAID")
                    {
                        var getOrderDetail = _context.Orderdetails.Where(x => x.OrderId == Convert.ToInt32(paymentLinkInformation.orderCode)).ToList();
                        foreach (var orderDetail in getOrderDetail)
                        {
                            Ticket addNewTicket = new Ticket();
                            addNewTicket.OrderDetailId = orderDetail.OrderDetailId;
                            addNewTicket.Status = "";
                            addNewTicket.IsCheckedIn = false;
                            addNewTicket.CheckInDate = null;
                            _context.Tickets.Add(addNewTicket);
                            _context.SaveChanges();
                        }
                        var order = _context.Orders.Include(o => o.Account)
                            .Include(o => o.Orderdetails)
                                .ThenInclude(od => od.TicketType)
                                    .ThenInclude(tt => tt.Event)
                            .Include(o => o.Payments).FirstOrDefault(x => x.OrderId == Convert.ToInt32(paymentLinkInformation.orderCode));
                        if (order != null)
                        {
                            order.Status = "Đã thanh toán";
                            _context.SaveChanges();

                            //update send ticket email for payos payment
                            var email = order.Account?.Email;
                            var fullName = order.Account?.FullName;
                            var payment = order.Payments.SingleOrDefault(x => x.OrderId == paymentLinkInformation.orderCode);
                            var paymentAmount = paymentLinkInformation?.amount ;
                            string paymentAmountVND = paymentAmount?.ToString("#,##0") + " ₫";
                            var orderDetail = order.Orderdetails.FirstOrDefault();
                            var eventInfo = orderDetail?.TicketType?.Event;

                            if (email != null && fullName != null && eventInfo != null)
                            {
                                var eventName = eventInfo.EventName;
                                var eventStartTime = eventInfo.StartTime.Value;
                                var eventLocation = eventInfo.Location;
                                var eventAddress = eventInfo.Address;

                                foreach (var detail in getOrderDetail)
                                {
                                    var ticket = detail.Tickets.FirstOrDefault();
                                    if (ticket != null)
                                    {
                                        var ticketId = ticket.TicketId;
                                        var ticketType = detail.TicketType?.TypeName ?? "??";
                                        var quantity = detail.Quantity.Value;
                                        var fthOrderId = "FTH" + DateTime.Now.Year + order.OrderId;

                                        //await EmailService.Instance.SendTicketEmail(
                                        //    email,
                                        //    fullName,
                                        //    ticketId,
                                        //    ticketType,
                                        //    quantity,
                                        //    fthOrderId,
                                        //    paymentAmountVND,
                                        //    eventName,
                                        //    eventStartTime,
                                        //    eventLocation,
                                        //    eventAddress
                                        //);
                                        _ = Task.Run(() => EmailService.Instance.SendTicketEmail(
                                            email,
                                            fullName,
                                            ticketId,
                                            ticketType,
                                            quantity,
                                            fthOrderId,
                                            paymentAmountVND,
                                            eventName,
                                            eventStartTime,
                                            eventLocation,
                                            eventAddress
                                        ));
                                    }
                                }
                            }
                        }


                    }
                    return Ok(new
                    {
                        status = 200,
                        paymentLinkInformation = paymentLinkInformation,
                        paymentMethod = 0 
                    });
                }
                else
                {
                    var getOrderDetail = _context.Orderdetails.Where(x => x.OrderId == orderId).ToList();
                    foreach (var orderDetail in getOrderDetail)
                    {
                        Ticket addNewTicket = new Ticket();
                        addNewTicket.OrderDetailId = orderDetail.OrderDetailId;
                        addNewTicket.Status = "";
                        addNewTicket.IsCheckedIn = false;
                        addNewTicket.CheckInDate = null;
                        _context.Tickets.Add(addNewTicket);
                        _context.SaveChanges();
                    }
                    var order = _context.Orders.Include(o => o.Account)
                            .Include(o => o.Orderdetails)
                                .ThenInclude(od => od.TicketType)
                                    .ThenInclude(tt => tt.Event)
                            .Include(o => o.Payments).FirstOrDefault(x => x.OrderId == orderId);
                    if (order != null)
                    {

                        order.Status = "Đã thanh toán";
                        _context.SaveChanges();

                        //update send ticket email for free ticket
                        var email = order.Account?.Email;
                        var fullName = order.Account?.FullName;
                        //var payment = order.Payments.SingleOrDefault(x => x.OrderId == order.OrderId);
                        var paymentAmount = "Miễn phí";
                        var orderDetail = order.Orderdetails.FirstOrDefault();
                        var eventInfo = orderDetail?.TicketType?.Event;

                        if (email != null && fullName != null && eventInfo != null)
                        {
                            var eventName = eventInfo.EventName;
                            var eventStartTime = eventInfo.StartTime.Value;
                            var eventLocation = eventInfo.Location;
                            var eventAddress = eventInfo.Address;

                            foreach (var detail in getOrderDetail)
                            {
                                var ticket = detail.Tickets.FirstOrDefault();
                                if (ticket != null)
                                {
                                    var ticketId = ticket.TicketId;
                                    var ticketType = detail.TicketType?.TypeName ?? "??";
                                    var quantity = detail.Quantity.Value;
                                    var fthOrderId = "FTH" + DateTime.Now.Year + order.OrderId;

                                    //await EmailService.Instance.SendTicketEmail(
                                    //    email,
                                    //    fullName,
                                    //    ticketId,
                                    //    ticketType,
                                    //    quantity,
                                    //    fthOrderId,
                                    //    paymentAmount,
                                    //    eventName,
                                    //    eventStartTime,
                                    //    eventLocation,
                                    //    eventAddress
                                    //);
                                    _ = Task.Run(() => EmailService.Instance.SendTicketEmail(
                                        email,
                                        fullName,
                                        ticketId,
                                        ticketType,
                                        quantity,
                                        fthOrderId,
                                        paymentAmount,
                                        eventName,
                                        eventStartTime,
                                        eventLocation,
                                        eventAddress
                                    ));
                                }
                            }
                        }
                    }
                    return Ok(new
                    {
                        status = 200,
                        paymentMethod = 1
                    });
                } 
                    
                   
            }
            catch (System.Exception exception)
            {
                return Ok(new
                {
                    status = 400,
                    message = exception.Message,
                });
            }

        }

    }
}
