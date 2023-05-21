using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Configuration.Exceptions;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Mappers.MedicamentsMappers;
using DrugStoreAPI.src.Repositories;

namespace DrugStoreAPI.src.Services
{
    public class DrugsService : IDrugsService
    {
        private readonly IComponentsRepository componentsRepository;
        private readonly IDrugsRepository drugsRepository;

        public DrugsService(IComponentsRepository componentsRepository, IDrugsRepository drugsRepository)
        {
            this.componentsRepository = componentsRepository;
            this.drugsRepository = drugsRepository;
        }

        public async Task<DrugDTO> AddDrug(DrugDTO dto)
        {
            var medicamentsMapper = new MedicamentsMapper();
            var drug = medicamentsMapper.DrugDTOtoDrug(dto);

            var result = await IsDrugHasDuplicates(drug);
            if (!result)
            {
                throw new DuplicateDrugException($"Drug with such name: '{dto.Name}' and type: '{dto.Type}' is already exists!");
            }

            foreach (var drugComponentDTO in dto.Components)
            {
                if (await componentsRepository.FindComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: '{drugComponentDTO.Component.Id}' do not exists!");
                }
            }

            var drugsComponents = await GetDrugsComponents(drug, dto.Components);
            drug.DrugsComponents = drugsComponents;

            drug = await drugsRepository.InsertDrug(drug);

            return medicamentsMapper.DrugToDrugDTO(drug);
        }

        private async Task<bool> IsDrugHasDuplicates(Drug drug)
        {
            var result = await drugsRepository.FindDrugByNameIsAndTypeIs(drug.Name, drug.Type);

            return result == null;
        }

        public async Task<DrugDTO> UpdateDrug(DrugDTO dto)
        {
            var currentDrug = await drugsRepository.FindDrugById(dto.Id);

            if (currentDrug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: '{dto.Id}' do not exists!");
            }

            foreach (var drugComponentDTO in dto.Components)
            {
                if (await componentsRepository.FindComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: '{drugComponentDTO.Component.Id}' do not exists!");
                }
            }

            var medicamentsMapper = new MedicamentsMapper();
            var updatedDrug = medicamentsMapper.DrugDTOtoDrug(dto);

            var drugsComponents = await GetDrugsComponents(updatedDrug, dto.Components);

            updatedDrug.DrugsComponents = drugsComponents;

            updatedDrug = await drugsRepository.UpdateDrug(updatedDrug);

            return medicamentsMapper.DrugToDrugDTO(updatedDrug);
        }

        public async Task<bool> DeleteDrug(int id)
        {
            var deleteResult = await drugsRepository.DeleteDrug(id);

            if (!deleteResult)
            {
                throw new DrugNotFoundException($"Drug with such id: '{id}' do not exists!");
            }

            return deleteResult;
        }
        public async Task<DrugDTO> GetDrugById(int id)
        {
            var drug = await drugsRepository.FindDrugById(id);

            if (drug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: '{id}' do not exists!");
            }

            var mapper = new MedicamentsMapper();

            return mapper.DrugToDrugDTO(drug);
        }

        public async Task<IEnumerable<DrugDTO>> GetAllDrugs()
        {
            var currentDrugs = await drugsRepository.GetAllDrugs();

            if (currentDrugs == null)
            {
                throw new DrugNotFoundException("There are no drugs!");
            }

            var mapper = new MedicamentsMapper();

            var drugs = new List<DrugDTO>();
            foreach (var drug in currentDrugs)
            {
                drugs.Add(mapper.DrugToDrugDTO(drug));
            }

            return drugs;
        }

        public async Task<ICollection<DrugsComponents>> GetDrugsComponents(Drug drug, List<DrugComponentDTO> drugComponentDTOs)
        {
            var mapper = new MedicamentsMapper();

            var drugsComponents = new List<DrugsComponents>();
            foreach (var drugComponentDTO in drugComponentDTOs)
            {
                var component = await componentsRepository.FindComponentById(mapper.ComponentDTOtoComponent(drugComponentDTO.Component).Id);

                drugsComponents.Add(new DrugsComponents()
                {
                    Drug = drug,
                    DrugId = drug.Id,
                    Component = component,
                    ComponentId = component.Id,
                    Amount = drugComponentDTO.Amount
                });
            }

            return drugsComponents;
        }

        public async Task<IEnumerable<DrugComponentsPricesReportDTO>> GetDrugAndComponentsPrices(int id)
        {
            var result = await drugsRepository.FindDrugAndComponentsPrices(id);

            return result;
        }

        public async Task<IEnumerable<DrugDTO>> GetDrugsInOrdersInProgress()
        {
            var result = await drugsRepository.FindDrugsByOrderStatusInProgress();

            var medicamentsMapper = new MedicamentsMapper();
            var drugs = new List<DrugDTO>();
            foreach (var drug in result)
            {
                drugs.Add(medicamentsMapper.DrugToDrugDTO(drug));
            }

            return drugs;
        }

        public async Task<IEnumerable<DrugDTO>> GetDrugsByMinimalAmount(MedicamentTypeDTO dto)
        {
            var result = await drugsRepository.GetDrugsByMinimalAmountAndTypeIs(dto.Type);

            var medicamentsMapper = new MedicamentsMapper();
            var drugs = new List<DrugDTO>();
            foreach (var drug in result)
            {
                drugs.Add(medicamentsMapper.DrugToDrugDTO(drug));
            }
            return drugs;
        }

        public async Task<IEnumerable<DrugTechnologyDTO>> GetDrugsTechnologies(DrugInProgressTechnologyReportDTO dto)
        {
            var result = await drugsRepository.GetDrugsTechnologiesByNameIsAndTypeIs(dto.DrugName, dto.Type, dto.IsInProggress);

            var technologies = new List<DrugTechnologyDTO>();
            foreach (var technology in result)
            {
                technologies.Add(new DrugTechnologyDTO()
                {
                    Technology = technology,
                });
            }

            return technologies;
        }
    }
}

