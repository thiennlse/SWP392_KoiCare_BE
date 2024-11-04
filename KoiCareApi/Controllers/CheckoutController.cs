﻿using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;


namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;


        public CheckoutController(PayOS payOS, IPaymentService paymentService, IOrderService orderService, IProductService productService, IEmailService emailService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _orderService = orderService;
            _productService = productService;
            _emailService = emailService;
        }

        [HttpPost("create-payment-link")]
        public async Task<IActionResult> Checkout([FromBody] List<PaymentRequestModel> orderRequest, string cancelUrl, string returnUrl)
        {
            try
            {
                List<ItemData> items = new List<ItemData>();
                int ordercode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                int totalAmount = 0;
                foreach (var request in orderRequest)
                {
                    Product product = await _productService.GetProductById(request.ProductId);

                    if (product == null)
                    {
                        return BadRequest($"Product or order not found for orderId {request.ProductId}");
                    }
                    if (product.InStock < request.quantity)
                    {
                        return BadRequest("Not enough product");
                    }
                    int price = (int)(request.Cost * request.quantity);
                    totalAmount += price;
                    ItemData item = new ItemData(product.Name, request.quantity, price);
                    items.Add(item);
                }
                PaymentData paymentData = new PaymentData
                (
                    ordercode,
                    totalAmount,
                    "payment order",
                    items,
                    cancelUrl,
                    returnUrl
                );
                CreatePaymentResult paymentResult = await _paymentService.createPaymentLink(paymentData);
                return Ok(new
                {
                    Url = paymentResult.checkoutUrl,
                    orderCode = paymentResult.orderCode

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send-email/{orderCode}")]
        public async Task<IActionResult> SendOrderEmail([FromBody] EmailRequestModel emailRequest, string orderCode)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.RecipientEmail))
            {
                return BadRequest("Invalid email request.");
            }
            try
            {
                // Send the email asynchronously
                await _emailService.SendVerifyAccountEmail(emailRequest.RecipientEmail, orderCode);

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}
