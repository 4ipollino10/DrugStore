using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMedicamentsRepository medicamentRepository;

        public OrdersRepository(ApplicationDbContext applicationDbContext, IMedicamentsRepository medicamentRepository)
        {
            this.applicationDbContext = applicationDbContext;
            this.medicamentRepository = medicamentRepository;
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
            var currentOrder = applicationDbContext.Orders.Find(order.Id);

            foreach(var orderDrug in currentOrder.OrdersDrugs)
            {
                
            }

            currentOrder.OrderStatus = order.OrderStatus;
            
            applicationDbContext.SaveChanges();

            return currentOrder;
        }

        public Order GetOrderById(int id)
        {
            Order order =  applicationDbContext.Orders.Find(id);
            return order;
        }

        public Client GetClientById(int id)
        {
            return applicationDbContext.Clients.Find(id);
        }
    }
}
