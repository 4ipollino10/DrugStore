using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class DrugTechnologyQueryDTO
    {
        public string DrugName { get; set; }
        public MedicamentType Type { get; set; }
        public bool IsInProggress { get; set; }
    }
}
