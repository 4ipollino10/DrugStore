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
        public IActionResult GetClientsByOverduedOrders()
        {
            var clients = orderService.GetClientsByOverduedOrders();

            return Ok(clients);
        }

        [HttpGet("{type}")]
        public IActionResult GetOrdersByType(OrderStatus type)
        {
            var orders = orderService.GetOrderByType(type);

            return Ok(orders);
        }

        [HttpGet]
        public IActionResult GetClientsByMedicaments([FromBody] GetClientsByMedicamentsDTO dto) 
        {
            var clients = orderService.GetClientsByMedicaments(dto);

            return Ok(clients);

        }

    }
}








