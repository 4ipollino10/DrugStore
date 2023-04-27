using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.Repositories
{
    public interface IMedicamentsRepository
    {
        Task<Component> InsertComponent(Component component);
        Task<Component> UpdateComponent(Component component);
        Task<bool> DeleteComponent(int id);
        Task<Component> FindComponentById(int id);
        Component FindComponentByNameIsAndTypeIs(string name, MedicamentType type);
        Task<IEnumerable<Component>> GetAllComponents();
        Task<IEnumerable<Component>> FindComponentsByCriticalAmount();
        Task<IEnumerable<GetDrugAndComponentsPriceDTO>> FindDrugAndComponentsPrices(int id);
        Task<IEnumerable<Component>> GetTopUsefulComponetsByTypeIs(MedicamentType type);
        Task<IEnumerable<Drug>> GetDrugsByMinimalAmountAndTypeIs(MedicamentType type);
        Task<IEnumerable<int>> GetUsedAmountComponentForPeriodAndTypeIs(DateTime from, DateTime to, string name);

        Task<Drug> InsertDrug(Drug drug);
        Task<Drug> UpdateDrug(Drug drug);
        Task<bool> DeleteDrug(int id);
        Task<Drug> FindDrugById(int id);
        Task<Drug> FindDrugByNameIsAndTypeIs(string name, MedicamentType type);
        Task<IEnumerable<Drug>> GetAllDrugs();
        Task<IEnumerable<Drug>> FindDrugsByOrderStatusInProgress();
        Task<IEnumerable<string>> GetDrugsTechnologiesByNameIsAndTypeIs(string drugName, MedicamentType type, bool isInProggress);

    }
}
