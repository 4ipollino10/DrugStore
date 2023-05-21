using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.Repositories
{
    public interface IClientsRepository
    {
        Task<Client> InsertClient(Client client);
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task<IEnumerable<Client>> FindClientsByOverduedOrders();
        Task<IEnumerable<Client>> FindClientsByMedicamentNameIsOrTypeIs(string drugName, MedicamentType type);
        Task<IEnumerable<Client>> FindClientsByDateAndNameIsAndTypeIs(DateTime from, DateTime to, string drugName, MedicamentType type);
        Task<IEnumerable<Client>> FindClientsByOrderStatusDelayedMedicamentNameIsTypeIs(MedicamentType type);
    }
}
