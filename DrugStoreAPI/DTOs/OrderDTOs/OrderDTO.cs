using DrugStoreAPI.Entities;
using DrugStoreAPI.Utils;

namespace DrugStoreAPI.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime AppointedDate { get; set; }
        public DateTime RecievingDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ClientDTO Client { get; set; }
        public List<DrugOrderDTO> Drugs { get; set; }
    }
}
