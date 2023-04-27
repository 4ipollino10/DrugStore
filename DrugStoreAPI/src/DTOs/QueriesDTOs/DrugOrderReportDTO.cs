using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class DrugOrderReportDTO
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Name { get; set; }
        public MedicamentType Type { get; set; }
    }
}
