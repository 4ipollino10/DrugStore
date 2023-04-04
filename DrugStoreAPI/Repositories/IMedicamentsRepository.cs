using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;
using System.Xml.Serialization;

namespace DrugStoreAPI.Repositories
{
    public interface IMedicamentsRepository
    {
        Component InsertComponent(Component component);
        Component UpdateComponent(Component component);
        void DeleteComponent(Component component);
        Drug InsertDrug(Drug drug);
        Drug UpdateDrug(Drug drug);
        void DeleteDrug(Drug drug);
        Drug GetDrugById(int id);
        Component GetComponentById(int id);


    }
}
