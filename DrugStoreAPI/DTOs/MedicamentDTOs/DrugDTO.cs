using DrugStoreAPI.Entities;

namespace DrugStoreAPI.DTOs.MedicamentDTOs
{
    public class DrugDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }
        public string Technology { get; set; }
        public double CookingTime { get; set; }
        public List<DrugComponentDTO> Components { get; set; }
    }
}
