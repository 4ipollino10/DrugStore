using DrugStoreAPI.DTOs.OrderDTOs;

namespace DrugStoreAPI.Services
{
    public interface IOrdersService
    {
        Task<OrderDTO> AddOrder(OrderDTO dto);
        Task<OrderDTO> GetOrderById(int id);
        Task<IEnumerable<OrderDTO>> GetAllOrders();

        Task<OrderDTO> MakeOrder(OrderDTO dto);
        Task<OrderDTO> StockComponents(OrderDTO dto);
        Task<bool> CompleteOrder(OrderDTO dto);

        Task<ClientDTO> GetClientById(int id);
        Task<IEnumerable<ClientDTO>> GetAllClients();
    }
}
