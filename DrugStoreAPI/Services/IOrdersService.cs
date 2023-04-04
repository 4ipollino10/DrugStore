using DrugStoreAPI.DTOs.OrderDTOs;

namespace DrugStoreAPI.Services
{
    public interface IOrdersService
    {
        public OrderDTO AddOrder(OrderDTO dto);
        public OrderDTO MakeDrugs(OrderDTO dto);
        public OrderDTO StockComponents(OrderDTO dto);


    }
}
