using DrugStoreAPI.Data;
using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Repositories
{
    public class MedicamentsRepository : IMedicamentsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MedicamentsRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Component InsertComponent(Component component)
        {
            Component insertComponent = applicationDbContext.Components.Add(component).Entity;
            applicationDbContext.SaveChanges();
            
            return insertComponent;
        }

        public Component UpdateComponent(Component component)
        {
            var curComponent = applicationDbContext.Components.Find(component.Id);
            curComponent = component;
            applicationDbContext.SaveChanges();

            return curComponent;
        }

        public void DeleteComponent(Component component)
        {
            applicationDbContext.Components.Remove(component);

            applicationDbContext.SaveChanges();
        }

        public Drug InsertDrug(Drug drug)
        {
            Drug insertDrug = applicationDbContext.Drugs.Add(drug).Entity;
            applicationDbContext.SaveChanges();
            
            return insertDrug;
        }

        public Drug UpdateDrug(Drug drug)
        {
            var curDrug = applicationDbContext.Drugs.Find(drug.Id);
            curDrug = drug;
            applicationDbContext.SaveChanges();

            return curDrug;
        }

        public void DeleteDrug(Drug drug)
        {
            applicationDbContext.Drugs.Remove(drug);

            applicationDbContext.SaveChanges();
        }

        public Component GetComponentById(int id)
        {
            return applicationDbContext.Components.Find(id);
        }

        public Drug GetDrugById(int id)
        {
            return applicationDbContext.Drugs.Find(id);
        }
    }
}
