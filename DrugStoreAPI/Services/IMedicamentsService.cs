using DrugStoreAPI.DTOs.MedicamentDTOs;

namespace DrugStoreAPI.Services
{
    public interface IMedicamentsService
    {
        ComponentDTO AddComponent(ComponentDTO dto);
        ComponentDTO UpdateComponent(ComponentDTO dto);
        void DeleteComponent(ComponentDTO dto);
        DrugDTO AddDrug(DrugDTO dto);
        DrugDTO UpdateDrug(DrugDTO dto);
        void DeleteDrug(DrugDTO dto);
    }
}
