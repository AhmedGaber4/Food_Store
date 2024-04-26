using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.OrderService.Dto;
using Store.Service.Services.BasketService.Dots;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Store.API.Controllers
{

    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string endpointSecret = "whsec_4c26913062cc3850bd2139910905a8f7c95b1fcb6989986e531f7dd486227ede";
        public PaymentController( IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [HttpPost("{basketId}")]
      
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
             =>Ok( await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(input));


        [HttpPost("{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketID)
             => Ok(await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketID));

        //[HttpPost("webhook")]
    
        //public class WebhookController : Controller
        //{

            // This is your Stripe CLI webhook secret for testing your endpoint locally.
            //  const string endpointSecret = "whsec_4c26913062cc3850bd2139910905a8f7c95b1fcb6989986e531f7dd486227ede";

            [HttpPost("webhook")]
            public async Task<IActionResult> Index()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], endpointSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                    {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed :", paymentIntent.Id) ;
                    order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Update To Payment Failed :", order.Id);


                }
                    else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                    {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeed :", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Update To Payment Succeed :", order.Id);

                }
                else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }

                    return Ok();
                }
                catch (StripeException e)
                {
                    return BadRequest();
                }
            }
        //}


    }
}
