using DrugStoreAPI.DTOs.MedicamentDTOs;
using LanguageExt.Common;

namespace DrugStoreAPI.Services
{
    public interface IMedicamentsService
    {
        Task<ComponentDTO> AddComponent(ComponentDTO dto);
        Task<IEnumerable<ComponentDTO>> GetAllComponents();
        Task<ComponentDTO> UpdateComponent(ComponentDTO dto);
        Task<bool> DeleteComponent(int id);
        DrugDTO AddDrug(DrugDTO dto);
        DrugDTO UpdateDrug(DrugDTO dto);
        void DeleteDrug(DrugDTO dto);
        DrugDTO GetDrugs();
    }
}
