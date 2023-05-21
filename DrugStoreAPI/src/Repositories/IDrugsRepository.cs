using DrugStoreAPI.Entities;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;

namespace DrugStoreAPI.src.Repositories
{
    public interface IDrugsRepository
    {
        Task<Drug> InsertDrug(Drug drug);
        Task<Drug> UpdateDrug(Drug drug);
        Task<bool> DeleteDrug(int id);
        Task<Drug> FindDrugById(int id);
        Task<Drug> FindDrugByNameIsAndTypeIs(string name, MedicamentType type);
        Task<IEnumerable<Drug>> GetAllDrugs();
        Task<IEnumerable<Drug>> FindDrugsByOrderStatusInProgress();
        Task<IEnumerable<string>> GetDrugsTechnologiesByNameIsAndTypeIs(string drugName, MedicamentType type, bool isInProggress);
        Task<IEnumerable<Drug>> GetDrugsByMinimalAmountAndTypeIs(MedicamentType type);
        Task<IEnumerable<DrugComponentsPricesReportDTO>> FindDrugAndComponentsPrices(int id);


    }
}
