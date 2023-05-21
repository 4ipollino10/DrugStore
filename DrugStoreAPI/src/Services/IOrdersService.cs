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
        Task<IEnumerable<OrderDTO>> GetOrderByStatus(OrderStatusDTO dto);

        Task<OrderDTO> MakeOrder(OrderDTO dto);
        Task<OrderDTO> StockComponents(OrderDTO dto);
        Task<bool> CompleteOrder(OrderDTO dto);
    };
}
