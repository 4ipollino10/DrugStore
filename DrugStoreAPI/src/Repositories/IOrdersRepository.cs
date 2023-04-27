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

        Task<Client> InsertClient(Client client);
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task<IEnumerable<Client>> FindClientsByOverduedOrders();
        Task<IEnumerable<Client>> FindClientsByMedicamentNameIsOrTypeIs(string drugName, MedicamentType type);
        Task<IEnumerable<Client>> FindClientsByDateAndNameIsAndTypeIs(DateTime from, DateTime to, string drugName, MedicamentType type);

        Task<IEnumerable<Client>> FindClientsByOrderStatusDelayedMedicamentNameIsTypeIs(string drugName, MedicamentType type);
    }
}
