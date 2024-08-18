using backend.DTOs;
using backend.Models;
using backend.Repositories.NewsRepository;
using backend.Repositories.PaymentRepository;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;

namespace backend.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public object CancelOrderOfUser(int userId)
        {
            return _paymentRepository.CancelOrderOfUser(userId);
        }

        public object CheckInputCoupon(int eventId, string coupon)
        {
            return _paymentRepository.CheckInputCoupon(eventId, coupon);
        }

        public object CheckOrderdOfUser(int userId, int eventId)
        {
            return _paymentRepository.CheckOrderdOfUser(userId, eventId);
        }

        public object DeleteTimeOutOrder(PaymentDTO paymentDTO)
        {
            return _paymentRepository.DeleteTimeOutOrder(paymentDTO);
        }

        public object Payment(PaymentDTO paymentDTO)
        {
            return _paymentRepository.Payment(paymentDTO);
        }

        public object PaymentExcute(IQueryCollection collections)
        {
            return _paymentRepository.PaymentExcute(collections);
        }

        public object ReturnAvaliableTicket(int eventId)
        {
            return _paymentRepository.ReturnAvaliableTicket(eventId);
        }

        public object ReturnPaymentUrl(HttpContext context, int _orderId, string _discounrCode)
        {
            return _paymentRepository.ReturnPaymentUrl(context, _orderId, _discounrCode); 
        }
    }
}
