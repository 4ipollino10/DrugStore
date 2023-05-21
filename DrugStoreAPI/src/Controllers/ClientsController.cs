using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.src.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await clientService.GetClientById(id);

            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await clientService.GetAllClients();

            return Ok(clients);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsByOverduedOrders()
        {
            var clients = await clientService.GetClientsByOverduedOrders();

            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByMedicaments(ClientsOrderedMedicamentsReportDTO dto)
        {
            var clients = await clientService.GetClientsByMedicaments(dto);

            return Ok(clients);

        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByDelayedOrders(ClientsOrderedMedicamentsReportDTO dto)
        {
            var result = await clientService.GetClientsByDelayedOrders(dto);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetClientsByDate(DrugOrderReportDTO dto)
        {
            var result = await clientService.GetClientsByDate(dto);

            return Ok(result);
        }
    }
}
