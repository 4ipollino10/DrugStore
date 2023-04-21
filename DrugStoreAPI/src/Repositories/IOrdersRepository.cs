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
        IEnumerable<Order> FindOrderByTypeIs(OrderStatus type);

        Task<Client> InsertClient(Client client);
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
        IQueryable<Client> FindClientsByOverduedOrders();
        IQueryable<Client> FindClientsByMedicamentNameIsOrTypeIs(string drugName, MedicamentType type);
    }
}
