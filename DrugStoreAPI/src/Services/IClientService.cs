using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Services
{
    public interface IClientService
    {
        Task<ClientDTO> GetClientById(int id);
        Task<IEnumerable<ClientDTO>> GetAllClients();
        Task<IEnumerable<ClientDTO>> GetClientsByOverduedOrders();
        Task<IEnumerable<ClientDTO>> GetClientsByMedicaments(ClientsOrderedMedicamentsReportDTO dto);
        Task<IEnumerable<ClientDTO>> GetClientsByDelayedOrders(ClientsOrderedMedicamentsReportDTO dto);
        Task<IEnumerable<ClientDTO>> GetClientsByDate(DrugOrderReportDTO dto);
    }
}
