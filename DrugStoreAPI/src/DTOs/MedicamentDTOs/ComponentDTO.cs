using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.DTOs.MedicamentDTOs
{
    public class ComponentDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public MedicamentType Type { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }

    }
}
