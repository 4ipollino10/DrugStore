﻿using DrugStoreAPI.src.Configuration.Exceptions;
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
        private readonly IOrdersRepository orderRepository;
        private readonly IComponentsRepository componentsRepository;
        private readonly IDrugsRepository drugsRepository;
        private readonly IClientsRepository clientsRepository;
        private readonly IDrugsService drugsService;

        public OrdersService(IOrdersRepository orderRepository, 
                            IComponentsRepository componentsRepository, 
                            IDrugsRepository drugsRepository, 
                            IClientsRepository clientsRepository,
                            IDrugsService drugsService)
        {
            this.clientsRepository = clientsRepository;
            this.drugsService = drugsService;
            this.orderRepository = orderRepository;
            this.componentsRepository = componentsRepository;
            this.drugsRepository = drugsRepository;
        }

        public async Task<OrderDTO> AddOrder(OrderDTO dto)
        {
            var mapper = new OrdersMapper();
            var client = await clientsRepository.GetClientById(dto.Client.Id);
            if (client == null)
            {
                client = await clientsRepository.InsertClient(mapper.ClientDTOtoClient(dto.Client));
            }
            Console.WriteLine(client.Name);

            var order = mapper.OrderDTOtoOrder(dto);

            order.Client = client;
            order.ClientId = client.Id;
            order.OrdersDrugs = await GetOrdersDrugs(order, dto);
            order.UsedComponents = true;

            await SetMedicationReadiness(dto.Drugs);

            if (IsEnoughDrugs(dto.Drugs))
            {
                order.OrderStatus = Utils.OrderStatus.COMPLETED;
                order.AppointedDate = order.OrderDate;
                order.UsedComponents = false;
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
            await SetMedicationReadiness(insertedOrderDTO.Drugs);
            Console.WriteLine(insertedOrder.OrdersDrugs);
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

            await SetMedicationReadiness(orderDTO.Drugs);

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

                var component = await componentsRepository
                    .UpdateComponent(medicamentsMapper.ComponentDTOtoComponent(drugComponentDTO.Component));

                drugComponentDTO.Component = medicamentsMapper.ComponentToComponentDTO(component);
            }

            var drug = medicamentsMapper.DrugDTOtoDrug(drugOrderDTO.Drug);
            drug.DrugsComponents = await drugsService.GetDrugsComponents(drug, drugOrderDTO.Drug.Components);
            drug = await drugsRepository.UpdateDrug(drug);

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

            await SetMedicationReadiness(orderDTO.Drugs);

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

                var component = await componentsRepository
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
                var drug = await drugsRepository.FindDrugById(drugOrderDTO.Drug.Id);

                drug.Amount -= 1;

                drug.DrugsComponents = await drugsService.GetDrugsComponents(drug, drugOrderDTO.Drug.Components);
                drug = await drugsRepository.UpdateDrug(drug);

                drugOrderDTO.Drug = medicamentsMapper.DrugToDrugDTO(drug);
            }

            order.ReceivingDate = dto.ReceivingDate;
            order.OrderStatus = OrderStatus.ISSUED;

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
                var drug = await drugsRepository.FindDrugById(orderDTO.Drug.Id);

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

        private async Task<bool> SetMedicationReadiness(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                await SetDrugReadiness(drugOrderDTO);
                await SetComponentsReadiness(drugOrderDTO);
            }

            return true;
        }

        private async Task<bool> SetComponentsReadiness(DrugOrderDTO dto)
        {
            foreach (var drugComponentDTO in dto.Drug.Components)
            {
                var component = await componentsRepository.FindComponentById(drugComponentDTO.Component.Id);

                if (component.Amount >= drugComponentDTO.Amount)
                {
                    drugComponentDTO.IsReady = true;
                }
                else
                {
                    drugComponentDTO.IsReady = false;
                }
                
            }
            return true;
        }

        private async Task<bool> SetDrugReadiness(DrugOrderDTO dto)
        {
            var drug = await drugsRepository.FindDrugById(dto.Drug.Id);

            if (drug.Amount > 0)
            {
                dto.IsEnough = true;
            }

            return true;
        }

        private bool IsEnoughComponents(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach (var drugOrderDTO in drugOrderDTOs)
            {
                foreach (var drugComponentDTO in drugOrderDTO.Drug.Components)
                {
                    Console.WriteLine(drugComponentDTO.IsReady);
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

        public async Task<IEnumerable<OrderDTO>> GetOrderByStatus(OrderStatusDTO dto)
        {
            var result = await ordersRepository.FindOrderByTypeIs(dto.status);

            var ordersMapper = new OrdersMapper();
            var orders = new List<OrderDTO>();

            foreach(var order in result)
            {
                orders.Add(ordersMapper.OrderToOrderDTO(order));
            }

            return orders;
        }
    }
}
