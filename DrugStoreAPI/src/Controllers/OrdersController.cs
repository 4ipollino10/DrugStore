using DrugStoreAPI.DTOs.OrderDTOs;
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

        public OrdersController(IOrdersService orderService)
        {
            this.orderService = orderService;
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

        [HttpPost]
        public async Task<IActionResult> GetOrdersByStatus(OrderStatusDTO dto)
        {
            var orders = await orderService.GetOrderByStatus(dto);

            return Ok(orders);
        }
    }
}








