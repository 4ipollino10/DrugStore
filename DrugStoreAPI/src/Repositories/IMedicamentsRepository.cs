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
        IQueryable<Component> FindComponentsByCriticalAmount();
        IQueryable<GetDrugAndComponentsPriceDTO> FindDrugAndComponentsPrices(int id);

        Task<Drug> InsertDrug(Drug drug);
        Task<Drug> UpdateDrug(Drug drug);
        Task<bool> DeleteDrug(int id);
        Task<Drug> FindDrugById(int id);
        Drug FindDrugByNameIsAndTypeIs(string name, MedicamentType type);
        Task<IEnumerable<Drug>> GetAllDrugs();


    }
}
