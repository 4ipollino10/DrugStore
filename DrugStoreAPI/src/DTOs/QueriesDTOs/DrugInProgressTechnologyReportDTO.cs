using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class DrugInProgressTechnologyReportDTO
    {
        public string DrugName { get; set; }
        public MedicamentType Type { get; set; }
        public bool IsInProggress { get; set; }
    }
}
