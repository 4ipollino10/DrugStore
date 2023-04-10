using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public OrdersRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public Client InsertClient(Client client)
        {
            var insertedClient = applicationDbContext.Clients.Add(client).Entity;
            applicationDbContext.SaveChanges();

            return insertedClient;
        }

        public Order InsertOrder(Order order)
        {
            var insertedOrder = applicationDbContext.Orders.Add(order).Entity;
            applicationDbContext.SaveChanges();

            return insertedOrder;
        }

        public Order UpdateOrder(Order order)
        {
            var updatedOrder = applicationDbContext.Orders.Find(order.Id);
            updatedOrder = order;

            applicationDbContext.SaveChanges();

            return updatedOrder;
        }

        public Order GetOrderById(int id)
        {
            Order order =  applicationDbContext.Orders.Find(id);
            order.Client = GetClientById(order.ClientId);
            return order;
        }

        public Client GetClientById(int id)
        {
            return applicationDbContext.Clients.Find(id);
        }
    }
}
