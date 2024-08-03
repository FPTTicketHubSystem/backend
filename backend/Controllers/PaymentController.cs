using backend.DTOs;
using backend.Models;
using backend.Services.NewsService;
using backend.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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

    }
}
