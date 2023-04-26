using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class GetClientsByMedicamentsDTO
    {
        public string MedicamentName { get; set; }
        public MedicamentType MedicamentType { get; set; }
    }
}
