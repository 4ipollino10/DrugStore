using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.Services
{
    public interface IOrdersService
    {
        Task<OrderDTO> AddOrder(OrderDTO dto);
        Task<OrderDTO> GetOrderById(int id);
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        IEnumerable<OrderDTO> GetOrderByType(OrderStatus type);

        Task<OrderDTO> MakeOrder(OrderDTO dto);
        Task<OrderDTO> StockComponents(OrderDTO dto);
        Task<bool> CompleteOrder(OrderDTO dto);

        Task<ClientDTO> GetClientById(int id);
        Task<IEnumerable<ClientDTO>> GetAllClients();
        IEnumerable<ClientDTO> GetClientsByOverduedOrders();
        IEnumerable<ClientDTO> GetClientsByMedicaments(GetClientsByMedicamentsDTO dto);
    };
}
