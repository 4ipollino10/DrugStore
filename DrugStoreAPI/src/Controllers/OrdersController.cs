﻿using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Services;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.src.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrdersService orderService;
        private readonly IMedicamentsService medicamentService;

        public OrdersController(IOrdersService orderService, IMedicamentsService medicamentService)
        {
            this.orderService = orderService;
            this.medicamentService = medicamentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDTO dto)
        {
            var order = await orderService.AddOrder(dto);

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await orderService.GetOrderById(id);

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var order = await orderService.GetAllOrders();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder(OrderDTO dto)
        {
            var order = await orderService.MakeOrder(dto);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> StockComponents(OrderDTO dto)
        {
            var order = await orderService.StockComponents(dto);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(OrderDTO dto)
        {
            var result = await orderService.CompleteOrder(dto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await orderService.GetClientById(id);

            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await orderService.GetAllClients();

            return Ok(clients);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsByOverduedOrders()
        {
            var clients = await orderService.GetClientsByOverduedOrders();

            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrdersByStatus(OrderStatusDTO dto)
        {
            var orders = await orderService.GetOrderByStatus(dto);

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByMedicaments(GetClientsByMedicamentsDTO dto) 
        {
            var clients = await orderService.GetClientsByMedicaments(dto);

            return Ok(clients);

        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByDelayedOrders(GetClientsByMedicamentsDTO dto)
        {
            var result = await orderService.GetClientsByDelayedOrders(dto);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByDate(DrugOrderReportDTO dto)
        {
            var result = await orderService.GetClientsByDate(dto);

            return Ok(result);
        }
    }
}








