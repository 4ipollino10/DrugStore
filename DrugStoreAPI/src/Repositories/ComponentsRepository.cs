using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreAPI.src.Repositories
{
    public class ComponentsRepository : IComponentsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ComponentsRepository(ApplicationDbContext applicationDbContext)
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
            var result = from component in applicationDbContext.Components
                         orderby component.Id
                         select component;

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Component>> FindComponentsByCriticalAmount()
        {
            var result = from component in applicationDbContext.Components
                         where component.Amount <= component.CriticalAmount
                         select component;
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
                         where (type == MedicamentType.ANY || components.Type == type)
                         select components;

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUsedAmountComponentForPeriodAndTypeIs(DateTime fromm, DateTime to, string componentName)
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
                         (orders.OrderStatus == OrderStatus.ISSUED)
                         && (orders.UsedComponents)
                         && (components.Name == componentName)
                         && (orders.OrderDate >= fromm && orders.OrderDate <= to)
                         select drugsComponents.Amount;

            return await result.ToListAsync();
        }
    }
}
