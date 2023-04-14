using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order> InsertOrder(Order order);
        Task<Order> UpdateOrder(Order order);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrders();
        
        Task<Client> InsertClient(Client client);
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
    }
}
