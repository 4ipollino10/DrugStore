﻿using DrugStoreAPI.Exceptions;
using DrugStoreAPI.src.Mappers.MedicamentsMappers;
using DrugStoreAPI.Mappers.OrderMappers;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Repositories;
using DrugStoreAPI.Services;
using DrugStoreAPI.src.Utils;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IMedicamentsRepository medicamentsRepository;
        private readonly IMedicamentsService medicamentsService;

        public OrdersService(IOrdersRepository orderRepository, IMedicamentsRepository medicamentsRepository, IMedicamentsService medicamentsService)
        {
            ordersRepository = orderRepository;
            this.medicamentsRepository = medicamentsRepository;
            this.medicamentsService = medicamentsService;
        }

        public async Task<OrderDTO> AddOrder(OrderDTO dto)
        {
            var mapper = new OrdersMapper();
            var client = await ordersRepository.GetClientById(dto.Client.Id);
            if (client == null)
            {
                client = await ordersRepository.InsertClient(mapper.ClientDTOtoClient(dto.Client));
            }

            var order = mapper.OrderDTOtoOrder(dto);

            order.Client = client;
            order.ClientId = client.Id;
            order.OrdersDrugs = await GetOrdersDrugs(order, dto);

            SetMedicationReadiness(dto.Drugs);

            if (IsEnoughDrugs(dto.Drugs))
            {
                order.OrderStatus = Utils.OrderStatus.COMPLETED;
                order.AppointedDate = order.OrderDate;
                order.ReceivingDate = order.OrderDate;
            }
            else if (IsEnoughComponents(dto.Drugs))
            {
                double maxCookingTime = FindMaxCookingTime(dto.Drugs);

                order.OrderStatus = Utils.OrderStatus.IN_PROGRESS;
                order.AppointedDate = order.OrderDate.AddMinutes(maxCookingTime);
            }
            else
            {
                order.OrderStatus = Utils.OrderStatus.DELAYED;
                order.AppointedDate = order.OrderDate.AddDays(1);
            }

            var insertedOrder = await ordersRepository.InsertOrder(order);
            var insertedOrderDTO = mapper.OrderToOrderDTO(insertedOrder);
            SetMedicationReadiness(insertedOrderDTO.Drugs);

            return insertedOrderDTO;
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await ordersRepository.GetOrderById(id);

            if (order == null)
            {
                throw new OrderNotFoundException($"Order with such id: \"{id}\" do not exists!");
            }

            var mapper = new OrdersMapper();

            return mapper.OrderToOrderDTO(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var currentOrders = await ordersRepository.GetAllOrders();

            if (currentOrders == null)
            {
                throw new OrderNotFoundException("There are no orders!");
            }

            var mapper = new OrdersMapper();

            var orders = new List<OrderDTO>();

            foreach (var order in currentOrders)
            {
                orders.Add(mapper.OrderToOrderDTO(order));
            }

            return orders;
        }

        public async Task<ClientDTO> GetClientById(int id)
        {
            var client = await ordersRepository.GetClientById(id);

            if (client == null)
            {
                throw new ClientNotFoundException($"Client with such id: \"{id}\" do not exists!");
            }

            var mapper = new OrdersMapper();

            return mapper.ClientToClientDTO(client);
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClients()
        {
            var currentClients = await ordersRepository.GetAllClients();

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

        public IEnumerable<ClientDTO> GetClientsByOverduedOrders()
        {
            var currentClients = ordersRepository.FindClientsByOverduedOrders();
            
            var ordersMapper = new OrdersMapper();
            var clients = new List<ClientDTO>();

            foreach(var client in currentClients)
            {
                clients.Add(ordersMapper.ClientToClientDTO(client));
            }

            return clients;
        }

        public async Task<OrderDTO> MakeOrder(OrderDTO dto)
        {
            var orderDTO = await GetOrderById(dto.Id);

            if (orderDTO.OrderStatus == Utils.OrderStatus.COMPLETED)
            {
                throw new OrderBadRequestException("This order is completed!");
            }
            if (orderDTO.OrderStatus == Utils.OrderStatus.DELAYED)
            {
                throw new OrderBadRequestException("This order is delayed!");
            }

            var ordersMapper = new OrdersMapper();

            SetMedicationReadiness(orderDTO.Drugs);

            foreach (var drugOrderDTO in orderDTO.Drugs)
            {
                if (drugOrderDTO.IsEnough)
                {
                    continue;
                }

                await MakeDrug(drugOrderDTO);
            }

            orderDTO.OrderStatus = Utils.OrderStatus.COMPLETED;

            await ordersRepository.UpdateOrder(ordersMapper.OrderDTOtoOrder(orderDTO));

            return orderDTO;
        }

        private async Task<bool> MakeDrug(DrugOrderDTO drugOrderDTO)
        {
            var medicamentsMapper = new MedicamentsMapper();

            drugOrderDTO.Drug.Amount += 1;

            foreach (var drugComponentDTO in drugOrderDTO.Drug.Components)
            {
                drugComponentDTO.Component.Amount -= drugComponentDTO.Amount;

                var component = await medicamentsRepository
                    .UpdateComponent(medicamentsMapper.ComponentDTOtoComponent(drugComponentDTO.Component));

                drugComponentDTO.Component = medicamentsMapper.ComponentToComponentDTO(component);
            }

            var drug = medicamentsMapper.DrugDTOtoDrug(drugOrderDTO.Drug);
            drug.DrugsComponents = await medicamentsService.GetDrugsComponents(drug, drugOrderDTO.Drug.Components);
            drug = await medicamentsRepository.UpdateDrug(drug);

            drugOrderDTO.Drug = medicamentsMapper.DrugToDrugDTO(drug);

            return true;
        }

        public async Task<OrderDTO> StockComponents(OrderDTO dto)
        {
            var orderDTO = await GetOrderById(dto.Id);


            if (orderDTO.OrderStatus == Utils.OrderStatus.COMPLETED)
            {
                throw new OrderBadRequestException("This order is completed!");
            }
            if (orderDTO.OrderStatus == Utils.OrderStatus.IN_PROGRESS)
            {
                throw new OrderBadRequestException("This order is in progress!");
            }

            var mapper = new OrdersMapper();

            SetMedicationReadiness(orderDTO.Drugs);

            foreach (var drugOrderDTO in orderDTO.Drugs)
            {
                if (drugOrderDTO.IsReady)
                {
                    continue;
                }

                await StockComponent(drugOrderDTO);
            }

            orderDTO.OrderStatus = Utils.OrderStatus.IN_PROGRESS;

            await ordersRepository.UpdateOrder(mapper.OrderDTOtoOrder(orderDTO));

            return orderDTO;
        }

        private async Task<bool> StockComponent(DrugOrderDTO drugOrderDTO)
        {
            var medicamentsMapper = new MedicamentsMapper();

            foreach (var drugComponentDTO in drugOrderDTO.Drug.Components)
            {
                if (drugComponentDTO.IsReady)
                {
                    continue;
                }

                drugComponentDTO.Component.Amount += drugComponentDTO.Amount * 10;

                var component = await medicamentsRepository
                    .UpdateComponent(medicamentsMapper.ComponentDTOtoComponent(drugComponentDTO.Component));

                drugComponentDTO.Component = medicamentsMapper.ComponentToComponentDTO(component);
            }

            return true;
        }

        public async Task<bool> CompleteOrder(OrderDTO dto)
        {
            var order = await ordersRepository.GetOrderById(dto.Id);

            if (order.OrderStatus == Utils.OrderStatus.DELAYED)
            {
                throw new OrderBadRequestException("This order is delayed!");
            }
            if (order.OrderStatus == Utils.OrderStatus.IN_PROGRESS)
            {
                throw new OrderBadRequestException("This order is in progress!");
            }

            var medicamentsMapper = new MedicamentsMapper();

            foreach (var drugOrderDTO in dto.Drugs)
            {
                var drug = await medicamentsRepository.FindDrugById(drugOrderDTO.Drug.Id);

                drug.Amount -= 1;

                drug.DrugsComponents = await medicamentsService.GetDrugsComponents(drug, drugOrderDTO.Drug.Components);
                drug = await medicamentsRepository.UpdateDrug(drug);

                drugOrderDTO.Drug = medicamentsMapper.DrugToDrugDTO(drug);
            }

            order.ReceivingDate = dto.ReceivingDate;

            await ordersRepository.UpdateOrder(order);

            return true;
        }

        private double FindMaxCookingTime(List<DrugOrderDTO> drugOrderDTOs)
        {
            double maxcookingTime = 0;

            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                if (maxcookingTime < drugOrderDTO.Drug.CookingTime)
                {
                    maxcookingTime = drugOrderDTO.Drug.CookingTime;
                }
            }

            return maxcookingTime;
        }

        private async Task<ICollection<OrdersDrugs>> GetOrdersDrugs(Order order, OrderDTO dto)
        {
            var ordersDrugs = new List<OrdersDrugs>();

            foreach (var orderDTO in dto.Drugs)
            {
                var drug = await medicamentsRepository.FindDrugById(orderDTO.Drug.Id);

                ordersDrugs.Add(new OrdersDrugs()
                {
                    DrugId = drug.Id,
                    Drug = drug,
                    Order = order,
                    OrderId = order.Id
                });
            }

            return ordersDrugs;
        }

        private void SetMedicationReadiness(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                SetDrugReadiness(drugOrderDTO);
                SetComponentsReadiness(drugOrderDTO);
            }
        }

        private async void SetComponentsReadiness(DrugOrderDTO dto)
        {
            foreach (var drugComponentDTO in dto.Drug.Components)
            {
                var component = await medicamentsRepository.FindComponentById(drugComponentDTO.Component.Id);

                if (component.Amount >= drugComponentDTO.Amount)
                {
                    drugComponentDTO.IsReady = true;
                }
                else
                {
                    drugComponentDTO.IsReady = false;
                }
            }
        }

        private async void SetDrugReadiness(DrugOrderDTO dto)
        {
            var drug = await medicamentsRepository.FindDrugById(dto.Drug.Id);

            if (drug.Amount > 0)
            {
                dto.IsEnough = true;
            }
        }

        private bool IsEnoughComponents(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                foreach (var drugComponentDTO in drugOrderDTO.Drug.Components)
                {
                    if (!drugComponentDTO.IsReady)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsEnoughDrugs(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                if (!drugOrderDTO.IsEnough)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<OrderDTO> GetOrderByType(OrderStatus type)
        {
            var result = ordersRepository.FindOrderByTypeIs(type);

            var ordersMapper = new OrdersMapper();
            var orders = new List<OrderDTO>();

            foreach(var order in result)
            {
                orders.Add(ordersMapper.OrderToOrderDTO(order));
            }

            return orders;
        }

        public IEnumerable<ClientDTO> GetClientsByMedicaments(GetClientsByMedicamentsDTO dto)
        {
            var result = ordersRepository.FindClientsByMedicamentNameIsOrTypeIs(dto.DrugName, dto.MedicamentType);

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