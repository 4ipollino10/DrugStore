using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Repositories
{
    public interface IOrdersRepository
    {
        Client InsertClient(Client client);
        Order InsertOrder(Order order);
        Order GetOrderById(int id);
        Order UpdateOrder(Order order);
        Client GetClientById(int id);
    }
}
