namespace DrugStoreAPI.DTOs.MedicamentDTOs
{
    public class DrugComponentDTO
    {
        public ComponentDTO Component { get; set; }
        public int Amount { get; set; }
        public bool IsReady { get; set; }

    }
}
