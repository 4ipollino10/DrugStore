﻿using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrdersService orderService;

        public OrdersController(IOrdersService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public OrderDTO Add(OrderDTO dto)
        {
            return orderService.AddOrder(dto);
        }

        [HttpPost]
        public OrderDTO MakeDrugs(OrderDTO dto)
        {
            return orderService.MakeDrugs(dto);
        }

        [HttpPost]
        public OrderDTO StockComponents(OrderDTO dto)
        {
            return orderService.StockComponents(dto);
        }

        [HttpGet]
        public DateTime Asd()
        {
            return new DateTime();
        }
        [HttpGet]
        public ClientDTO GetClient()
        {
            return orderService.GetClient();
        }
        public OrderDTO GetOrder()
        {
            return orderService.GetOrder();
        }
    }
}
