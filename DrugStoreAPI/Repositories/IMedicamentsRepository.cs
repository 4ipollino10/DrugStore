using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;
using System.Xml.Serialization;

namespace DrugStoreAPI.Repositories
{
    public interface IMedicamentsRepository
    {
        Task<Component> InsertComponent(Component component);
        Task<Component> UpdateComponent(Component component);
        Task<bool> DeleteComponent(int id);
        Task<Component> GetComponentById(int id);
        Task<IEnumerable<Component>> GetAllComponents();

        Task<Drug> InsertDrug(Drug drug);
        Task<Drug> UpdateDrug(Drug drug);
        Task<bool> DeleteDrug(int id);
        Task<Drug> GetDrugById(int id);
        Task<IEnumerable<Drug>> GetAllDrugs();


    }
}
