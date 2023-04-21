namespace DrugStoreAPI.src.DTOs.QueriesDTOs
{
    public class GetDrugAndComponentsPriceDTO
    {
        public string DrugName { get; set; }
        public double DrugPrice { get; set; }
        public string ComponentName { get; set; }
        public double ComponentPrice { get; set; }
        public int ComponentAmount { get; set; }
    }
}
