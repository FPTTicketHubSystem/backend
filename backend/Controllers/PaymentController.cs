using Azure;
using backend.DTOs;
    using backend.Helper;
using backend.Models;
using backend.Services.NewsService;
using backend.Services.OtherService;
using backend.Services.PaymentService;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Net.payOS;
using Net.payOS.Types;
using Net.payOS.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

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
        public PaymentController(IPaymentService paymentService, FpttickethubContext context, PayOS payOS)
        {
            _paymentService = paymentService;
            _context = context;
            _payOS = payOS;
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

        [HttpPost("deleteTimeOutOrder")]
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


                var checkOrderDetails = _context.Orderdetails.Where(x => x.OrderId == returnPaymentURL.OrderId).ToList();
                bool isFree = true;
                foreach (var detail in checkOrderDetails)
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

                    //PayOS _payOS = new PayOS("b2514e3c-d2d6-432a-b1a4-eaaa8b989c88", "f8879890-fd24-41db-bc96-aa21ec3f7abd", "100f66bc876b8ed977fa4cde1864a4065394dc0e82e7f8a8b37a8e74d07da637");
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
                    _context.Payments.Add(paymentSignUp);
                    _context.SaveChanges();

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
                        var returnUrl = "https://frontend-nine-brown-60.vercel.app/payment-success/" + _order.OrderId;
                        var statusPayment = "Don hang " + _order.OrderId;
                        var cancelUrl = "https://frontend-nine-brown-60.vercel.app/payment-success/" + _order.OrderId;
                        var merchantId = "b2514e3c-d2d6-432a-b1a4-eaaa8b989c88";
                        var transactionId = "100f66bc876b8ed977fa4cde1864a4065394dc0e82e7f8a8b37a8e74d07da637";
                        PaymentData paymentData = new PaymentData(_order.OrderId, (int)_order.Total, statusPayment, items, returnUrl, returnUrl);
                        string signature = SignatureControl.CreateSignatureOfPaymentRequest(paymentData, "100f66bc876b8ed977fa4cde1864a4065394dc0e82e7f8a8b37a8e74d07da637");
                        PaymentData paymentData2 = new PaymentData(_order.OrderId, (int)_order.Total, statusPayment, items, returnUrl, returnUrl, signature);
                        CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData2);
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
                }
                else
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
                Order _order = _context.Orders.Where(x => x.OrderId == _orderId).FirstOrDefault();
                if (_order.Status == "Đã thanh toán")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Checked"
                    });
                }
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
                            paymentSignUp = _context.Payments.SingleOrDefault(x => x.OrderId == orderId);
                            if (paymentSignUp != null)
                            {
                                paymentSignUp.Status = "1";
                            }
                            _context.SaveChanges();

                            //update send ticket email for payos payment
                            var email = order.Account?.Email;
                            var fullName = order.Account?.FullName;
                            var payment = order.Payments.SingleOrDefault(x => x.OrderId == paymentLinkInformation.orderCode);
                            var paymentAmount = paymentLinkInformation?.amount;
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
                    paymentSignUp = _context.Payments.SingleOrDefault(x => x.OrderId == orderId);
                    if (paymentSignUp != null)
                    {
                        paymentSignUp.Status = "1";
                        _context.SaveChanges();
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
