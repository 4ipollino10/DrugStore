using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Services
{
    public interface IComponentsService
    {
        Task<ComponentDTO> AddComponent(ComponentDTO dto);
        Task<ComponentDTO> UpdateComponent(ComponentDTO dto);
        Task<bool> DeleteComponent(int id);
        Task<ComponentDTO> GetComponentById(int id);
        Task<IEnumerable<ComponentDTO>> GetAllComponents();
        Task<IEnumerable<ComponentDTO>> GetComponentsByCriticalAmount();
        Task<IEnumerable<ComponentDTO>> GetTopUsefulComponets(MedicamentTypeDTO dto);
        Task<ComponentAmountDTO> GetUsedAmountComponentForPeriod(ComponentAmountUsedReportDTO dto);
    }
}
