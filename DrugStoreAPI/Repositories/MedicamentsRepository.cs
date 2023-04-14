using DrugStoreAPI.Data;
using DrugStoreAPI.DTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

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
            var insertedComponent = await applicationDbContext.Components.AddAsync(component);
            await applicationDbContext.SaveChangesAsync();
            
            return insertedComponent.Entity;
        }

        public async Task<Component> UpdateComponent(Component component)
        {
            var currComponent = await applicationDbContext.Components.FindAsync(component.Id);

            currComponent.Amount = component.Amount;
            currComponent.CriticalAmount = component.CriticalAmount;
            currComponent.Name = component.Name;
            currComponent.Price = component.Price;
            currComponent.Type = component.Type;

            await applicationDbContext.SaveChangesAsync();

            return currComponent;
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var result = await GetComponentById(id);

            applicationDbContext.Components.Remove(result);
            
            await applicationDbContext.SaveChangesAsync();

            return result != null;
        }

        public async Task<Component> GetComponentById(int id)
        {
            return await applicationDbContext.Components.FindAsync(id);
        }

        public async Task<IEnumerable<Component>> GetAllComponents()
        {
            return await applicationDbContext.Components.ToListAsync();
        }

        public async Task<Drug> InsertDrug(Drug drug)
        {
            var insertedDrug = await applicationDbContext.Drugs.AddAsync(drug);
            await applicationDbContext.SaveChangesAsync();

            return insertedDrug.Entity;
        }

        public async Task<Drug> UpdateDrug(Drug drug)
        {
            var curDrug = await applicationDbContext.Drugs.FindAsync(drug.Id);

            applicationDbContext.DrugsComponents.RemoveRange(applicationDbContext.DrugsComponents.Where(dc => dc.DrugId == drug.Id));
            await applicationDbContext.SaveChangesAsync();

            curDrug.Amount = drug.Amount;
            curDrug.CookingTime = drug.CookingTime;
            curDrug.CriticalAmount = drug.CriticalAmount;
            curDrug.DrugsComponents = drug.DrugsComponents;
            curDrug.Name = drug.Name;
            curDrug.Price = drug.Price;
            curDrug.Technology = drug.Technology;
            curDrug.Type = drug.Type;

            await applicationDbContext.SaveChangesAsync();

            return curDrug;
        }

        public async Task<bool> DeleteDrug(int id)
        {
            var result = await GetDrugById(id);

            applicationDbContext.Drugs.Remove(result);

            await applicationDbContext.SaveChangesAsync();

            return result != null;
        }


        public async Task<Drug> GetDrugById(int id)
        {
            return await applicationDbContext.Drugs.FindAsync(id);
        }

        public async Task<IEnumerable<Drug>> GetAllDrugs()
        {
            return await applicationDbContext.Drugs.ToListAsync();
        }

    }
}
