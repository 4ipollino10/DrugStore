using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class ComponentAmountUsedReportDTO
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public MedicamentType Type { get; set; }
    }
}
