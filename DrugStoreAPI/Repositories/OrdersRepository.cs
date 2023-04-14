using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Client> InsertClient(Client client)
        {
            var insertedClient = await applicationDbContext.Clients.AddAsync(client);
            await applicationDbContext.SaveChangesAsync();

            return insertedClient.Entity;
        }
        public async Task<Client> GetClientById(int id)
        {
            return await applicationDbContext.Clients.FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await applicationDbContext.Clients.ToListAsync();
        }

        public async Task<Order> InsertOrder(Order order)
        {
            var insertedOrder = await applicationDbContext.Orders.AddAsync(order);
            await applicationDbContext.SaveChangesAsync();

            return insertedOrder.Entity;
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await applicationDbContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await applicationDbContext.Orders.ToListAsync();
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var currentOrder = await applicationDbContext.Orders.FindAsync(order.Id);
            
            foreach(var orderDrug in order.OrdersDrugs)
            {
                orderDrug.Drug = await applicationDbContext.Drugs.FindAsync(orderDrug.DrugId);
            }

            currentOrder.OrderStatus = order.OrderStatus;
            currentOrder.OrdersDrugs = order.OrdersDrugs;
            
            await applicationDbContext.SaveChangesAsync();

            return currentOrder;
        }
    }
}
