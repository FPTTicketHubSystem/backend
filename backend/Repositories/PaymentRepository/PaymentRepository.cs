using Azure;
using backend.DTOs;
using backend.Helper;
using backend.Models;
using backend.Repositories.EventRepository;
using Org.BouncyCastle.Asn1.X9;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace backend.Repositories.PaymentRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FpttickethubContext _context;

        public PaymentRepository(FpttickethubContext context)
        {
            _context = context;
        }
        public object Payment(PaymentDTO paymentDTO)
        {
            DateTime dateTime = DateTime.UtcNow;    
            Order orderSignUp = new Order();
            Payment paymentSignUp = new Payment();
            decimal paymentAmount = 0;
            try
            {
                //Sign Up Order
                orderSignUp.AccountId = paymentDTO.AccountId;
                orderSignUp.OrderDate = dateTime;
                orderSignUp.Total = paymentDTO.TotalPayment;
                orderSignUp.Status = "Đang xử lý";
                _context.Add(orderSignUp);
                _context.SaveChanges();

                Order order = _context.Orders.Where(x => x.AccountId == paymentDTO.AccountId && x.OrderDate == dateTime).SingleOrDefault();

                //Sign Up OderDetail
                foreach (var orderDeteail in paymentDTO.TicketBuyeds)
                {
                    Orderdetail orderdetailSignUp = new Orderdetail();
                    orderdetailSignUp.TicketTypeId = orderDeteail.TicketTypeId;
                    orderdetailSignUp.OrderId = order.OrderId;
                    orderdetailSignUp.Quantity = orderDeteail.Quantity;

                    //Sum up subtotal
                    Tickettype getPriceOfTicketType = _context.Tickettypes.Where(x => x.TicketTypeId == orderDeteail.TicketTypeId).SingleOrDefault();
                    var subToTal = orderdetailSignUp.Quantity * getPriceOfTicketType.Price;
                    //Descreate Quantity Of Ticket
                    getPriceOfTicketType.Quantity = getPriceOfTicketType.Quantity - orderdetailSignUp.Quantity;

                    orderdetailSignUp.Subtotal = subToTal;

                    paymentAmount = (decimal)(paymentAmount + orderdetailSignUp.Subtotal);
                    _context.Add(orderdetailSignUp);
                    _context.SaveChanges();
                }

                //Sign Up Payment
                //paymentSignUp.OrderId = order.OrderId;
                //paymentSignUp.PaymentMethodId = 2;
                //if (disCountCodeId != 0)
                //{
                //    paymentSignUp.DiscountCodeId = disCountCodeId;
                //}
                //paymentSignUp.PaymentDate = dateTime;
                //paymentSignUp.Status = "Thanh toán thành công";
                //paymentSignUp.PaymentAmount = paymentAmount;
                //if(discountcode != null)
                //{
                //    paymentSignUp.DiscountCodeId = disCountCodeId;
                //}
                //if(paymentSignUp.DiscountCodeId != 0 || paymentSignUp.DiscountCodeId != 0)
                //{
                //    Discountcode discountCode = _context.Discountcodes.Where(x => x.DiscountCodeId == paymentSignUp.DiscountCodeId).SingleOrDefault();
                //    if(discountcode != null)
                //    {
                //        paymentSignUp.PaymentAmount = paymentAmount * discountcode.DiscountAmount;
                //    } else
                //    {
                //        paymentSignUp.PaymentAmount = paymentAmount;
                //    }
                //}
                return new
                {
                    status = 200,
                    orderId = order.OrderId,
                    message = "Thanh toán thành công",
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                    message = "Thanh toán thất bại",
                };
            }
        }

        public object ReturnAvaliableTicket(int eventId)
        {
            try
            {
                var result = _context.Tickettypes.Where(x => x.EventId == eventId).ToList();
                return new
                {
                    result = result,
                    status = 200,
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                };
            }
            
        }

        public object ReturnPaymentUrl(HttpContext context, int _orderId, string _discounrCode)
        {
            string vnp_Returnurl = "http://localhost:3000/myticket";
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string vnp_TmnCode = "DH72XW45";
            string vnp_HashSecret = "8BNW3HO6R9QA8W8HDLWCET6TJLP6WDU5";

            Order _order = _context.Orders.Where(x => x.OrderId == _orderId).SingleOrDefault();
            Discountcode discountCode = _context.Discountcodes.SingleOrDefault(x => x.Code == _discounrCode);

            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            { 
                return new
                {
                    message = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config",
                    status = 400
                };
            }
            Payment paymentSignUp = new Payment();
            paymentSignUp.OrderId = _order.OrderId;
            paymentSignUp.Status = "0";
            if (discountCode != null)
            {
                paymentSignUp.DiscountCodeId = discountCode.DiscountCodeId;
                paymentSignUp.PaymentAmount = _order.Total * (discountCode.DiscountAmount/100);
            }
            else
            {
                paymentSignUp.PaymentAmount = _order.Total;
            }
            paymentSignUp.PaymentDate = DateTime.Now;
            paymentSignUp.PaymentMethodId = 2;
            string locale = "vn";
            VnPayLibrary vnpay = new VnPayLibrary();
            double amount = Convert.ToDouble(paymentSignUp.PaymentAmount);
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", paymentSignUp.PaymentDate?.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", locale);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + DateTime.Now.Ticks);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", Convert.ToString(_order.OrderId));

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //Response.Redirect(paymentUrl);
            return new
            {
                paymentUrl = paymentUrl,
                status = 200,
            };
        }

        public object PaymentExcute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach(var (key, value) in collections)
            {
                if(!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TramsactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, "8BNW3HO6R9QA8W8HDLWCET6TJLP6WDU5");
            if(!checkSignature)
            {
                return new
                {
                    status = 400,
                    message = "Thanh toan that bai",
                };
            }
            var getOrderDetail = _context.Orderdetails.Where(x => x.OrderId == Convert.ToInt32(vnp_orderId));
            foreach(var orderDetail in getOrderDetail)
            {
                Ticket addNewTicket = new Ticket();
                addNewTicket.OrderDetailId = orderDetail.OrderDetailId;
                addNewTicket.Status = "";
                addNewTicket.IsCheckedIn = false;
                addNewTicket.CheckInDate = null;
                _context.Add(addNewTicket);
                _context.SaveChanges();
            }
            
            return new
            {
                status = 200,
                VnPayResponse = vnp_ResponseCode
            };
        }

        public object DeleteTimeOutOrder(PaymentDTO paymentDTO)
        {
            try
            {
                Orderdetail deleteOrderDetail = new Orderdetail();
                foreach (var returnQuantity in paymentDTO.TicketBuyeds)
                {
                    var returnQuantityOfTicket = _context.Tickettypes.SingleOrDefault(x => x.TicketTypeId == returnQuantity.TicketTypeId);
                    if (returnQuantityOfTicket != null)
                    {
                        returnQuantityOfTicket.Quantity += returnQuantity.Quantity;
                        _context.SaveChanges();
                    }
                }
                var deleteOrder = _context.Orders.SingleOrDefault(x => x.OrderDate == paymentDTO.OrderDate && x.AccountId == paymentDTO.AccountId);
                if (deleteOrder != null)
                {
                    var deleteOrderDetails = _context.Orderdetails.Where(x => x.OrderId == deleteOrder.OrderId);
                    foreach (var delete in deleteOrderDetails)
                    {
                        var deleteDetail = _context.Orderdetails.SingleOrDefault(x => x.OrderDetailId == delete.OrderDetailId);
                        if (deleteDetail != null)
                        {
                            _context.Remove(deleteDetail);
                            _context.SaveChanges();
                        }
                    }
                }
                _context.Remove(deleteOrder);
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "Delete success",
                };
            }
            catch(Exception ex)
            {
                return new
                {
                    status = 400,
                    message = "Delete failed",
                };
            }
            
        }

        public object CheckInputCoupon(int eventId, string coupon)
        {
            var avaliableCoupon = _context.Discountcodes.SingleOrDefault(x => x.Code.Equals(coupon) && x.Quantity > 0);
            if(avaliableCoupon != null)
            {
                return new
                {
                    status = 200,
                    message = "Avaliable coupon",
                    discountAmount = avaliableCoupon.DiscountAmount,
                };
            } else
            {
                return new
                {
                    status = 400,
                    message = "Sold out coupon",
                };
            }

        }
    }
}
