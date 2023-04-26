using DrugStoreAPI.Data;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Repositories;
using DrugStoreAPI.src.Utils;
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

        public async Task<IEnumerable<Client>> FindClientsByOverduedOrders()
        {
            var clients = from client in applicationDbContext.Clients
                        join order in applicationDbContext.Orders
                            on client.Id equals order.ClientId
                   where order.ReceivingDate > order.AppointedDate 
                        select client;

            return await clients.ToListAsync();
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
            currentOrder.AppointedDate = order.AppointedDate;
            currentOrder.ReceivingDate = order.ReceivingDate;
            
            await applicationDbContext.SaveChangesAsync();

            return currentOrder;
        }

        public async Task<IEnumerable<Order>> FindOrderByTypeIs(OrderStatus type)
        {
            var result = applicationDbContext.Orders.Where(o => o.OrderStatus == type);
            
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Client>> FindClientsByMedicamentNameIsOrTypeIs(string drugName, MedicamentType type)
        {
            var result = from c in applicationDbContext.Clients
                            join orders in applicationDbContext.Orders
                                on c.Id equals orders.ClientId
                            join ordersDrugs in applicationDbContext.OrdersDrugs
                                on orders.Id equals ordersDrugs.OrderId
                            join drugs in applicationDbContext.Drugs
                                on ordersDrugs.DrugId equals drugs.Id
                            where drugs.Name == drugName && (type == MedicamentType.ANY || drugs.Type == type) 
                         select c;

            return await result.Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Client>> FindClientsByOrderStatusDelayedMedicamentNameIsTypeIs(string medicamentName, MedicamentType type)
        {
            var result = from clients in applicationDbContext.Clients
                         join orders in applicationDbContext.Orders
                             on clients.Id equals orders.ClientId
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on orders.Id equals ordersDrugs.OrderId
                         join drugs in applicationDbContext.Drugs
                             on ordersDrugs.DrugId equals drugs.Id
                         join drugsComponents in applicationDbContext.DrugsComponents
                             on drugs.Id equals drugsComponents.DrugId
                         join components in applicationDbContext.Components
                             on drugsComponents.ComponentId equals components.Id
                         where components.Name == medicamentName && type == MedicamentType.ANY || components.Type == type
                         select clients;
            
            return await result.Distinct().ToListAsync();
        }
    }
}
