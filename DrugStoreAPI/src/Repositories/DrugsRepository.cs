using DrugStoreAPI.Data;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreAPI.src.Repositories
{
    public class DrugsRepository : IDrugsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public DrugsRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
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
            var result = from drug in applicationDbContext.Drugs
                         orderby drug.Id
                         select drug;
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<DrugComponentsPricesReportDTO>> FindDrugAndComponentsPrices(int id)
        {
            var result = from drug in applicationDbContext.Drugs
                         join drugsComponents in applicationDbContext.DrugsComponents
                             on drug.Id equals drugsComponents.DrugId
                         join component in applicationDbContext.Components
                             on drugsComponents.ComponentId equals component.Id
                         select new DrugComponentsPricesReportDTO()
                         {
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

        public async Task<IEnumerable<Drug>> GetDrugsByMinimalAmountAndTypeIs(MedicamentType type)
        {
            //todo minimal;
            var result = from drugs in applicationDbContext.Drugs
                         where type == MedicamentType.ANY || drugs.Type == type
                         orderby drugs.Amount
                         select drugs;
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDrugsTechnologiesByNameIsAndTypeIs(string drugName, MedicamentType type, bool isInProggress)
        {
            var result = from drugs in applicationDbContext.Drugs
                         join ordersDrugs in applicationDbContext.OrdersDrugs
                             on drugs.Id equals ordersDrugs.DrugId
                         join orders in applicationDbContext.Orders
                             on ordersDrugs.OrderId equals orders.Id
                         where (drugName == string.Empty || drugs.Name == drugName)
                         && (type == MedicamentType.ANY || drugs.Type == type)
                         && (!isInProggress || orders.OrderStatus == OrderStatus.IN_PROGRESS)
                         select drugs.Technology;

            return await result.ToListAsync();
        }
    }
}
