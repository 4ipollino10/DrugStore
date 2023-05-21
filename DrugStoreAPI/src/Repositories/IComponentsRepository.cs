using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.Repositories
{
    public interface IComponentsRepository
    {
        Task<Component> InsertComponent(Component component);
        Task<Component> UpdateComponent(Component component);
        Task<bool> DeleteComponent(int id);
        Task<Component> FindComponentById(int id);
        Component FindComponentByNameIsAndTypeIs(string name, MedicamentType type);
        Task<IEnumerable<Component>> GetAllComponents();
        Task<IEnumerable<Component>> FindComponentsByCriticalAmount();
        Task<IEnumerable<Component>> GetTopUsefulComponetsByTypeIs(MedicamentType type);
        Task<IEnumerable<int>> GetUsedAmountComponentForPeriodAndTypeIs(DateTime from, DateTime to, string name);
    }
}
