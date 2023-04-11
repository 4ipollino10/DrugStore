using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;
using System.Xml.Serialization;

namespace DrugStoreAPI.Repositories
{
    public interface IMedicamentsRepository
    {
        Task<Component> InsertComponent(Component component);
        Component UpdateComponent(Component component);
        Task<Component> GetComponentById(int id);
        Task<IEnumerable<Component>> GetAllComponents();
        Task<bool> DeleteComponent(int id);
        Drug InsertDrug(Drug drug);
        Drug UpdateDrug(Drug drug);
        void DeleteDrug(Drug drug);
        Drug GetDrugById(int id);


    }
}
