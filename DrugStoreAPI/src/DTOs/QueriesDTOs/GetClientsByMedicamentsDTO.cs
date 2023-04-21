using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class GetClientsByMedicamentsDTO
    {
        public string DrugName { get; set; }
        public MedicamentType MedicamentType { get; set; }
    }
}
