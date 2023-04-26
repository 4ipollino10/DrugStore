using DrugStoreAPI.Entities;
using DrugStoreAPI.Data;
using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using System.Runtime.CompilerServices;

namespace DrugStoreAPI.src.Repositories
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
            var result = await FindComponentById(id);

            applicationDbContext.Components.Remove(result);

            await applicationDbContext.SaveChangesAsync();

            return result != null;
        }

        public async Task<Component> FindComponentById(int id)
        {
            return applicationDbContext.Components.FindAsync(id).Result;
        }
        public Component FindComponentByNameIsAndTypeIs(string name, MedicamentType type)
        {
            var component = applicationDbContext.Components.Where(c => c.Name == name && c.Type == type);

            return component.FirstOrDefault();
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
            var result = await FindDrugById(id);

            applicationDbContext.Drugs.Remove(result);

            await applicationDbContext.SaveChangesAsync();

            return result != null;
        }

        public async Task<Drug> FindDrugById(int id)
        {
            return await applicationDbContext.Drugs.FindAsync(id);
        }

        public async Task<Drug> FindDrugByNameIsAndTypeIs(string name, MedicamentType type)
        {
            var drug = applicationDbContext.Drugs.Where(d => d.Name == name && d.Type == type);

            return await drug.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Drug>> GetAllDrugs()
        {
            return await applicationDbContext.Drugs.ToListAsync();
        }

        public async Task<IEnumerable<Component>> FindComponentsByCriticalAmount()
        {
            var result = from component in applicationDbContext.Components
                         where component.Amount <= component.CriticalAmount
                         select component;
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<GetDrugAndComponentsPriceDTO>> FindDrugAndComponentsPrices(int id)
        {
            var result = from drug in applicationDbContext.Drugs
                            join drugsComponents in applicationDbContext.DrugsComponents
                                on drug.Id equals drugsComponents.DrugId
                            join component in applicationDbContext.Components
                                on drugsComponents.ComponentId equals component.Id
                         select new GetDrugAndComponentsPriceDTO() { 
                             DrugName = drug.Name, 
                             DrugPrice = drug.Price, 
                             ComponentName = component.Name, 
                             ComponentPrice = component.Price, 
                             ComponentAmount = component.Amount 
                         };
            return await result.ToListAsync();     
        }

        public async Task<IEnumerable<Drug>> FindDrugsByOrderStatusInProgress()
        {
            var result = from drugs in applicationDbContext.Drugs
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on drugs.Id equals ordersDrugs.DrugId
                         join orders in applicationDbContext.Orders
                             on ordersDrugs.OrderId equals orders.Id
                         where orders.OrderStatus == OrderStatus.IN_PROGRESS
                         select drugs;
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Component>> GetTopUsefulComponetsByTypeIs(MedicamentType type)
        {
            var result = from orders in applicationDbContext.Orders
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on orders.Id equals ordersDrugs.OrderId
                         join drugs in applicationDbContext.Drugs
                             on ordersDrugs.DrugId equals drugs.Id
                         join drugsComponents in applicationDbContext.DrugsComponents
                             on drugs.Id equals drugsComponents.DrugId
                         join components in applicationDbContext.Components
                             on drugsComponents.ComponentId equals components.Id
                         where type == MedicamentType.ANY || components.Type == type
                         select components;

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Drug>> GetDrugsByMinimalAmountAndTypeIs(MedicamentType type)
        {
            var result = from drugs in applicationDbContext.Drugs
                         where type == MedicamentType.ANY || drugs.Type == type
                         orderby drugs.Amount
                         select drugs;
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUsedAmountComponentForPeriodAndTypeIs(DateTime @from, DateTime to, string componentName)
        {
            var result = from orders in applicationDbContext.Orders
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on orders.Id equals ordersDrugs.OrderId
                         join drugs in applicationDbContext.Drugs
                             on ordersDrugs.DrugId equals drugs.Id
                         join drugsComponents in applicationDbContext.DrugsComponents
                             on drugs.Id equals drugsComponents.DrugId
                         join components in applicationDbContext.Components
                             on drugsComponents.ComponentId equals components.Id
                         where 
                         orders.OrderStatus == OrderStatus.COMPLETED 
                         && orders.UsedComponents 
                         && components.Name == componentName 
                         && (orders.OrderDate >= @from && orders.OrderDate <= to)
                         select drugsComponents.Amount;

            return await result.ToListAsync();
        }

    }
}
