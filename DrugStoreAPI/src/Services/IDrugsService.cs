using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Services
{
    public interface IDrugsService
    {
        Task<DrugDTO> AddDrug(DrugDTO dto);
        Task<DrugDTO> UpdateDrug(DrugDTO dto);
        Task<bool> DeleteDrug(int id);
        Task<DrugDTO> GetDrugById(int id);
        Task<IEnumerable<DrugDTO>> GetAllDrugs();
        Task<IEnumerable<DrugComponentsPricesReportDTO>> GetDrugAndComponentsPrices(int id);
        Task<IEnumerable<DrugDTO>> GetDrugsInOrdersInProgress();
        Task<IEnumerable<DrugDTO>> GetDrugsByMinimalAmount(MedicamentTypeDTO dto);
        Task<IEnumerable<DrugTechnologyDTO>> GetDrugsTechnologies(DrugInProgressTechnologyReportDTO dto);
        Task<ICollection<DrugsComponents>> GetDrugsComponents(Drug drug, List<DrugComponentDTO> drugComponentDTOs);
    }
}
