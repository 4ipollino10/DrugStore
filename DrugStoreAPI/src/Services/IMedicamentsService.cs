using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using LanguageExt.Common;

namespace DrugStoreAPI.Services
{
    public interface IMedicamentsService
    {
        Task<ComponentDTO> AddComponent(ComponentDTO dto);
        Task<ComponentDTO> UpdateComponent(ComponentDTO dto);
        Task<bool> DeleteComponent(int id);
        Task<ComponentDTO> GetComponentById(int id);
        Task<IEnumerable<ComponentDTO>> GetAllComponents();
        Task<IEnumerable<ComponentDTO>> GetComponentsByCriticalAmount();
        Task<IEnumerable<ComponentDTO>> GetTopUsefulComponets(MedicamentTypeDTO dto);
        Task<>

        Task<DrugDTO> AddDrug(DrugDTO dto);
        Task<DrugDTO> UpdateDrug(DrugDTO dto);
        Task<bool> DeleteDrug(int id);
        Task<DrugDTO> GetDrugById(int id);
        Task<IEnumerable<DrugDTO>> GetAllDrugs();
        Task<IEnumerable<GetDrugAndComponentsPriceDTO>> GetDrugAndComponentsPrices(int id);
        Task<IEnumerable<DrugDTO>> GetDrugsInOrdersInProgress();
        Task<IEnumerable<DrugDTO>> GetDrugsByMinimalAmount(MedicamentTypeDTO dto);

        Task<ICollection<DrugsComponents>> GetDrugsComponents(Drug drug, List<DrugComponentDTO> drugComponentDTOs);
    }
}
