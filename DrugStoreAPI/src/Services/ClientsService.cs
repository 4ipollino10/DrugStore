using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.src.Configuration.Exceptions;
using DrugStoreAPI.Mappers.OrderMappers;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Repositories;

namespace DrugStoreAPI.src.Services
{
    public class ClientsService : IClientService
    {
        private readonly IClientsRepository clientsRepository;

        public ClientsService(IClientsRepository clientsRepository)
        {
            this.clientsRepository = clientsRepository;
        }

        public async Task<ClientDTO> GetClientById(int id)
        {
            var client = await clientsRepository.GetClientById(id);

            if (client == null)
            {
                throw new ClientNotFoundException($"Client with such id: \"{id}\" do not exists!");
            }

            var mapper = new OrdersMapper();
            return mapper.ClientToClientDTO(client);
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClients()
        {
            var currentClients = await clientsRepository.GetAllClients();

            if (currentClients == null)
            {
                throw new OrderNotFoundException("There are no clients!");
            }

            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();

            foreach (var client in currentClients)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }
            return clients;
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByOverduedOrders()
        {
            var currentClients = await clientsRepository.FindClientsByOverduedOrders();

            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();

            foreach (var client in currentClients)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }

            return clients;
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByMedicaments(ClientsOrderedMedicamentsReportDTO dto)
        {
            var result = await clientsRepository.FindClientsByMedicamentNameIsOrTypeIs(dto.MedicamentName, dto.MedicamentType);

            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();

            foreach (var client in result)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }

            return clients;
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByDelayedOrders(ClientsOrderedMedicamentsReportDTO dto)
        {
            var result = await clientsRepository.FindClientsByOrderStatusDelayedMedicamentNameIsTypeIs(dto.MedicamentType);

            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();

            foreach (var client in result)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }

            return clients;
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByDate(DrugOrderReportDTO dto)
        {
            var result = await clientsRepository.FindClientsByDateAndNameIsAndTypeIs(dto.From, dto.To, dto.Name, dto.Type);

            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();
            foreach (var client in result)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }

            return clients;
        }
    }
}
