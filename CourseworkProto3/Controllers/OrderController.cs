using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Library.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService
                                ,ProductRepository productRepository
                                ,ProductService productService)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _productService = productService;
        }
        
        public async Task<IActionResult> Index()
        {
            await _orderService.CheckOrdersAsync();
            return View("Index", await _productRepository.GetFullForOrder());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Product(int id){
            if(!await _productRepository.ProductExists(id) || !await _productService.IsProductForOrder(id)){
                return NotFound();
            }
            else{
                var order = await _orderService.GetPendingOrder(id);
                var userId = JwtService.GetUserIdFromToken(Request.Cookies["access-cookie"]) ?? -1;
                if(userId == -1){
                    return NotFound();
                }
                var dto = new OrderDTO{
                    UserId = userId,
                    ProductId = id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(14),
                    PaymentAmount = order.PaymentAmount ?? 0m
                };

                return View(dto);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Product(OrderDTO dto){
            await _orderService.MakeOrder(dto);
            return RedirectToAction("Index", "Catalog");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Return(int id){
            if(!await _productRepository.ProductExists(id) || !await _productService.IsProductOrdered(id)){
                return NotFound();
            }
            else {
                await _orderService.ReturnOrder(id);
                return RedirectToAction("Index", "Catalog");
            }
        }
    }
}
