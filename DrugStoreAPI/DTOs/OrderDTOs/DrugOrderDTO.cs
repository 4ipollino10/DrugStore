using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;

namespace DrugStoreAPI.DTOs.OrderDTOs
{
    public class DrugOrderDTO
    {
        public DrugDTO Drug { get; set; }
        public bool IsReady { get; set; }
        public bool IsEnough { get; set; }
    }
}
