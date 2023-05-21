using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order> InsertOrder(Order order);
        Task<Order> UpdateOrder(Order order);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> FindOrderByTypeIs(OrderStatus type);

        
    }
}
