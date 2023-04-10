using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Mappers.MedicamentsMappers;
using DrugStoreAPI.Mappers.OrderMappers;
using DrugStoreAPI.Repositories;
using System.Runtime.CompilerServices;

namespace DrugStoreAPI.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IMedicamentsRepository medicamentsRepository;

        public OrdersService(IOrdersRepository orderRepository, IMedicamentsRepository medicamentsRepository) 
        {
            this.ordersRepository = orderRepository;
            this.medicamentsRepository = medicamentsRepository;
        }
        
        public OrderDTO AddOrder(OrderDTO dto)
        {
            OrdersMapper mapper = new();

            Client client = mapper.ClientDTOtoClient(dto.Client);

            client = ordersRepository.InsertClient(client);

            SetMedicationReadiness(dto.Drugs);

            var order = mapper.OrderDTOtoOrder(dto);

            order.Client = client;
            order.ClientId = client.Id;
            order.OrdersDrugs = GetOrdersDrugs(order, dto);

            
            if (IsEnoughDrugs(dto.Drugs))
            {
                order.OrderStatus = Utils.OrderStatus.COMPLETED;
                order.AppointedDate = order.OrderDate;
                order.ReceivingDate = order.OrderDate;

                dto = mapper.OrderToOrderDTO(ordersRepository.InsertOrder(order));
                
                SetMedicationReadiness(dto.Drugs);
                
                return dto;
            }

            if (IsEnoughComponents(dto.Drugs))
            {
                double maxCookingTime = FindMaxCookingTime(dto.Drugs);

                order.OrderStatus = Utils.OrderStatus.IN_PROGRESS;
                order.AppointedDate = order.OrderDate.AddMinutes(maxCookingTime);

                dto = mapper.OrderToOrderDTO(ordersRepository.InsertOrder(order));

                SetMedicationReadiness(dto.Drugs);

                return dto;
            }
            
            order.OrderStatus = Utils.OrderStatus.DELAYED;
            order.AppointedDate = order.OrderDate.AddDays(1);

            dto = mapper.OrderToOrderDTO(ordersRepository.InsertOrder(order));

            SetMedicationReadiness(dto.Drugs);

            return dto;
        }

        public OrderDTO MakeDrugs(OrderDTO dto)
        {
            OrdersMapper ordersMapper = new();

            dto = ordersMapper.OrderToOrderDTO(GetOrder(dto));

            SetMedicationReadiness(dto.Drugs);

            foreach(var drugOrderDTO in dto.Drugs)
            {
                if(!drugOrderDTO.IsEnough)
                {
                    drugOrderDTO.Drug.Amount += 1;

                    foreach(var drugComponentDTO in drugOrderDTO.Drug.Components)
                    {
                        drugComponentDTO.Component.Amount -= drugComponentDTO.Amount;
                    }
                }
            }

            dto = ordersMapper.OrderToOrderDTO(ordersRepository.UpdateOrder(ordersMapper.OrderDTOtoOrder(dto)));
            
            dto.OrderStatus = Utils.OrderStatus.COMPLETED;

            return dto;
        }

        public OrderDTO StockComponents(OrderDTO dto)
        {
            OrdersMapper ordersMapper = new();

            dto = ordersMapper.OrderToOrderDTO(GetOrder(dto));

            foreach(var OrderDrugDTO in dto.Drugs)
            {
                if (!OrderDrugDTO.IsReady)
                {
                    foreach(var componentDTO in OrderDrugDTO.Drug.Components)
                    {
                        if (!componentDTO.IsReady)
                        {
                            componentDTO.Component.Amount += componentDTO.Amount * 10;
                        }
                    }
                }
            }

            ordersRepository.UpdateOrder(ordersMapper.OrderDTOtoOrder(dto));

            return dto;
        }

        private double FindMaxCookingTime(List<DrugOrderDTO> drugOrderDTOs)
        {
            double maxcookingTime = 0;

            foreach(var drugOrderDTO in drugOrderDTOs)
            {
                if(maxcookingTime < drugOrderDTO.Drug.CookingTime)
                {
                    maxcookingTime = drugOrderDTO.Drug.CookingTime;
                }
            }

            return maxcookingTime;
        }

        private List<OrdersDrugs> GetOrdersDrugs(Order order, OrderDTO dto)
        {
            List<OrdersDrugs> ordersDrugs = new();

            foreach(var orderDTO in dto.Drugs)
            {
                Drug drug = GetDrug(orderDTO.Drug);

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

        private Drug GetDrug(DrugDTO drugDTO) 
        {
            return medicamentsRepository.GetDrugById(drugDTO.Id);
        }

        private Component GetComponent(ComponentDTO dto)
        {
            return medicamentsRepository.GetComponentById(dto.Id);
        }

        private Order GetOrder(OrderDTO dto)
        {
            return ordersRepository.GetOrderById(dto.Id);
        }

        private void SetMedicationReadiness(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach(var drugOrderDTO in drugOrderDTOs)
            {
                SetDrugReadiness(drugOrderDTO);
                SetComponentsReadiness(drugOrderDTO);
            }
        }

        private void SetComponentsReadiness(DrugOrderDTO dto)
        {
            dto.IsReady = true;

            foreach(var drugComponentDTO in dto.Drug.Components)
            {
                if(drugComponentDTO.Component.Amount >= drugComponentDTO.Amount)
                {
                    drugComponentDTO.IsReady = true;
                }
                else
                {
                    dto.IsReady = false;
                }
            }
        }

        private void SetDrugReadiness(DrugOrderDTO dto)
        {
            if(dto.Drug.Amount > 0)
            {
                dto.IsEnough = true;
            }
        }

        private bool IsEnoughComponents(List<DrugOrderDTO> drugOrderDTOs) 
        {
            foreach(var drugOrderDTO in drugOrderDTOs)
            {
                if(!drugOrderDTO.IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEnoughDrugs(List<DrugOrderDTO> drugOrderDTOs)
        {
            foreach(var drugOrderDTO in drugOrderDTOs)
            {
                if (!drugOrderDTO.IsEnough)
                {
                    return false;
                }
            }

            return true;
        }

        public ClientDTO GetClient()
        {
            OrdersMapper ordersMapper = new OrdersMapper();

            return ordersMapper.ClientToClientDTO(ordersRepository.GetClientById(1));
        }

        public OrderDTO GetOrder()
        {
            OrdersMapper ordersMapper = new();

            return ordersMapper.OrderToOrderDTO(ordersRepository.GetOrderById(2));
        }

    }
}
