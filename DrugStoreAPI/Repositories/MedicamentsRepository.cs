using DrugStoreAPI.Data;
using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrugStoreAPI.Repositories
{
    public class MedicamentsRepository : IMedicamentsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MedicamentsRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Component> InsertComponent(Component component)
        {
            var insertComponent = await applicationDbContext.Components.AddAsync(component);
            await applicationDbContext.SaveChangesAsync();
            
            return insertComponent.Entity;
        }

        public Component UpdateComponent(Component component)
        {
            var curComponent = applicationDbContext.Components.Update(component).Entity;
            curComponent = component;
            applicationDbContext.SaveChanges();

            return curComponent;
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var result = await GetComponentById(id);

            applicationDbContext.Components.Remove(result);
            
            await applicationDbContext.SaveChangesAsync();

            return result != null ? true : false;
        }

        public Drug InsertDrug(Drug drug)
        {
            Drug insertDrug = applicationDbContext.Drugs.Add(drug).Entity;
            applicationDbContext.SaveChanges();
            
            return insertDrug;
        }
        public async Task<Component> GetComponentById(int id)
        {
            return await applicationDbContext.Components.FindAsync(id);
        }

        public Drug UpdateDrug(Drug drug)
        {
            var curDrug = applicationDbContext.Drugs.Find(drug.Id);

            /*foreach(var drugComponent in drug.DrugsComponents)
            {
                drugComponent.Component = GetComponentById(drugComponent.ComponentId);
            }*/
            
            curDrug.Amount = drug.Amount;
            curDrug.CookingTime = drug.CookingTime;
            curDrug.CriticalAmount = drug.CriticalAmount;
            curDrug.DrugsComponents = drug.DrugsComponents;
            curDrug.Name = drug.Name;
            curDrug.Price = drug.Price;
            curDrug.Technology = drug.Technology;
            curDrug.Type = drug.Type;


            applicationDbContext.SaveChanges();

            return curDrug;
        }

        public void DeleteDrug(Drug drug)
        {
            applicationDbContext.Drugs.Remove(drug);

            applicationDbContext.SaveChanges();
        }


        public Drug GetDrugById(int id)
        {
            return applicationDbContext.Drugs.Find(id);
        }

        public async Task<IEnumerable<Component>> GetAllComponents()
        {
            return await applicationDbContext.Components.ToListAsync();
        }
    }
}
