using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Mappers.MedicamentsMappers;

namespace DrugStoreAPI.Mappers.OrderMappers
{
    public class OrdersMapper
    {
        public ClientDTO ClientToClientDTO(Client client)
        {
            ClientDTO clientDTO = new()
            {
                Id = client.Id,
                Address = client.Address,
                Name = client.Name,
                PhoneNumber = client.PhoneNumber,
            };

            return clientDTO;
        }

        public Client ClientDTOtoClient(ClientDTO dto)
        {
            Client client = new()
            {
                Id = dto.Id,
                Name = dto.Name,
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

            order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);

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

        public OrderDTO OrderToOrderDTO(Order order)
        {
            MedicamentsMapper medicamentsMapper = new();

            OrderDTO orderDTO = new()
            {
                AppointedDate = order.AppointedDate,
                Client = ClientToClientDTO(order.Client),
                Id = order.Id,
                OrderDate = order.OrderDate,
                RecievingDate = order.ReceivingDate,
                OrderStatus = order.OrderStatus
            };

            List<DrugOrderDTO> drugOrderDTOs = new();

            foreach(var ordersDrugs in order.OrdersDrugs)
            {
                drugOrderDTOs.Add(new DrugOrderDTO()
                {
                    Drug = medicamentsMapper.DrugToDrugDTO(ordersDrugs.Drug),
                });
            }

            orderDTO.Drugs = drugOrderDTOs;

            return orderDTO;
        }
    }
}
