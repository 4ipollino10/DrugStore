using DrugStoreAPI.DTOs.MedicamentDTOs;

namespace DrugStoreAPI.DTOs.OrderDTOs
{
    public class DrugOrderDTO
    {
        public DrugDTO Drug { get; set; }
        public bool IsReady { get; set; }
        public bool IsEnough { get; set; }
    }
}
