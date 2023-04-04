using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Mappers.MedicamentsMappers;

namespace DrugStoreAPI.Mappers.OrderMappers
{
    public class OrdersMapper
    {

        public Client ClientDTOtoClient(ClientDTO dto)
        {
            Client client = new()
            {
                Id = dto.Id,
                Name = dto.ClientName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
            };

            return client;
        }

        public Order OrderDTOtoOrder(OrderDTO dto)
        {
            MedicamentsMapper medicamentsMapper = new();

            Order order = new() 
            {
                Id = dto.Id,
                OrderDate = dto.OrderDate,
                AppointedDate = dto.AppointedDate,
                ClientId = dto.Client.Id,
                OrderStatus = dto.OrderStatus,
                ReceivingDate = dto.RecievingDate,
                
            };

            List<OrdersDrugs> ordersDrugs = new();

            foreach(var drugOrderDTO in dto.Drugs)
            {
                Drug drug = medicamentsMapper.DrugDTOtoDrug(drugOrderDTO.Drug);

                ordersDrugs.Add(new OrdersDrugs()
                {
                    Order = order,
                    OrderId = order.Id,
                    Drug = drug,
                    DrugId = drug.Id
                });
            }

            order.OrdersDrugs = ordersDrugs;

            return order;
        }

        public OrderDTO OrderToOrderDTO(Order dto)
        {
            return new OrderDTO();
        }
    }
}
