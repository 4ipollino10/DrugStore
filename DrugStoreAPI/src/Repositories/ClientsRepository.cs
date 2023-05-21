using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreAPI.src.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ClientsRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
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

        public async Task<IEnumerable<Client>> FindClientsByOrderStatusDelayedMedicamentNameIsTypeIs(MedicamentType type)
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
                         where (orders.OrderStatus == OrderStatus.DELAYED) && (type == MedicamentType.ANY || components.Type == type)
                         select clients;

            return await result.Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Client>> FindClientsByDateAndNameIsAndTypeIs(DateTime @from, DateTime to, string drugName, MedicamentType type)
        {
            var result = from orders in applicationDbContext.Orders
                         join clients in applicationDbContext.Clients
                             on orders.ClientId equals clients.Id
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on orders.Id equals ordersDrugs.OrderId
                         join drugs in applicationDbContext.Drugs
                             on ordersDrugs.DrugId equals drugs.Id
                         where
                         (drugName == string.Empty || drugs.Name == drugName) &&
                         (type == MedicamentType.ANY || drugs.Type == type) &&
                         (orders.OrderDate >= @from) && (orders.OrderDate <= to)
                         select clients;

            return await result.ToListAsync();
        }
    }
}
