using DrugStoreAPI.Exceptions;
using DrugStoreAPI.src.Mappers.MedicamentsMappers;
using DrugStoreAPI.Services;
using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Repositories;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Services
{
    public class MedicamentsService : IMedicamentsService
    {
        private readonly IMedicamentsRepository medicamentsRepository;

        public MedicamentsService(IMedicamentsRepository medicamentsRepository)
        {
            this.medicamentsRepository = medicamentsRepository;
        }

        public async Task<ComponentDTO> AddComponent(ComponentDTO dto)
        {
            var medicamentsMapper = new MedicamentsMapper();
            var component = medicamentsMapper.ComponentDTOtoComponent(dto);

            var result = IsComponentHasDuplicates(component);
            if (!result)
            {
                throw new DuplicateComponentException($"Component with such name: '{dto.Name}' and type: '{dto.Type}' is already exists!");
            }

            var insertedComponent = await medicamentsRepository.InsertComponent(component);

            return medicamentsMapper.ComponentToComponentDTO(insertedComponent);
        }

        private bool IsComponentHasDuplicates(Component component)
        {
            var result = medicamentsRepository.FindComponentByNameIsAndTypeIs(component.Name, component.Type);

            return result == null;
        }

        public async Task<ComponentDTO> UpdateComponent(ComponentDTO dto)
        {
            var currentComponent = medicamentsRepository.FindComponentById(dto.Id);

            if (currentComponent == null)
            {
                throw new ComponentNotFoundException($"Component with such id: '{dto.Id}' do not exists!");
            }

            var mapper = new MedicamentsMapper();
            var updatedOrder = await medicamentsRepository.UpdateComponent(mapper.ComponentDTOtoComponent(dto));

            return mapper.ComponentToComponentDTO(updatedOrder);
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var deleteResult = await medicamentsRepository.DeleteComponent(id);

            if (!deleteResult)
            {
                throw new ComponentNotFoundException($"Component with such id: '{id}' do not exists!");
            }

            return deleteResult;
        }

        public async Task<ComponentDTO> GetComponentById(int id)
        {
            var component = await medicamentsRepository.FindComponentById(id);

            if (component == null)
            {
                throw new ComponentNotFoundException($"Component with such id: '{id}' do not exist!");
            }

            var mapper = new MedicamentsMapper();

            return mapper.ComponentToComponentDTO(component);

        }

        public async Task<IEnumerable<ComponentDTO>> GetAllComponents()
        {
            var currentComponents = await medicamentsRepository.GetAllComponents();

            if (currentComponents == null)
            {
                throw new ComponentNotFoundException("There are no components!");
            }

            var mapper = new MedicamentsMapper();

            var components = new List<ComponentDTO>();

            foreach (var component in currentComponents)
            {
                components.Add(mapper.ComponentToComponentDTO(component));
            }

            return components;
        }

        public async Task<DrugDTO> AddDrug(DrugDTO dto)
        {
            var medicamentsMapper = new MedicamentsMapper();
            var drug = medicamentsMapper.DrugDTOtoDrug(dto);

            var result = IsDrugHasDuplicates(drug);
            if (!result)
            {
                throw new DuplicateDrugException($"Drug with such name: '{dto.Name}' and type: '{dto.Type}' is already exists!");
            }

            foreach (var drugComponentDTO in dto.Components)
            {
                if (await medicamentsRepository.FindComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: '{drugComponentDTO.Component.Id}' do not exists!");
                }
            }

            var drugsComponents = await GetDrugsComponents(drug, dto.Components);
            drug.DrugsComponents = drugsComponents;

            drug = await medicamentsRepository.InsertDrug(drug);

            return medicamentsMapper.DrugToDrugDTO(drug);
        }

        private bool IsDrugHasDuplicates(Drug drug)
        {
            var result = medicamentsRepository.FindDrugByNameIsAndTypeIs(drug.Name, drug.Type);

            return result == null;
        }

        public async Task<DrugDTO> UpdateDrug(DrugDTO dto)
        {
            var currentDrug = await medicamentsRepository.FindDrugById(dto.Id);

            if (currentDrug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: '{dto.Id}' do not exists!");
            }

            foreach (var drugComponentDTO in dto.Components)
            {
                if (await medicamentsRepository.FindComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: '{drugComponentDTO.Component.Id}' do not exists!");
                }
            }

            var medicamentsMapper = new MedicamentsMapper();
            var updatedDrug = medicamentsMapper.DrugDTOtoDrug(dto);

            var drugsComponents = await GetDrugsComponents(updatedDrug, dto.Components);

            updatedDrug.DrugsComponents = drugsComponents;

            updatedDrug = await medicamentsRepository.UpdateDrug(updatedDrug);

            return medicamentsMapper.DrugToDrugDTO(updatedDrug);
        }

        public async Task<bool> DeleteDrug(int id)
        {
            var deleteResult = await medicamentsRepository.DeleteDrug(id);

            if (!deleteResult)
            {
                throw new DrugNotFoundException($"Drug with such id: '{id}' do not exists!");
            }

            return deleteResult;
        }
        public async Task<DrugDTO> GetDrugById(int id)
        {
            var drug = await medicamentsRepository.FindDrugById(id);

            if (drug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: '{id}' do not exists!");
            }

            var mapper = new MedicamentsMapper();

            return mapper.DrugToDrugDTO(drug);
        }

        public async Task<IEnumerable<DrugDTO>> GetAllDrugs()
        {
            var currentDrugs = await medicamentsRepository.GetAllDrugs();

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
                var component = await medicamentsRepository.FindComponentById(mapper.ComponentDTOtoComponent(drugComponentDTO.Component).Id);

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

        public IEnumerable<ComponentDTO> GetComponentsByCriticalAmount()
        {
            var result = medicamentsRepository.FindComponentsByCriticalAmount();

            var medicamentsMapper = new MedicamentsMapper();
            var components = new List<ComponentDTO>();

            foreach(var component in result)
            {
                components.Add(medicamentsMapper.ComponentToComponentDTO(component));
            }

            return components;
        }

        public IQueryable<GetDrugAndComponentsPriceDTO> GetDrugAndComponentsPrices(int id)
        {
            
            var result = medicamentsRepository.FindDrugAndComponentsPrices(id);
            
            
            return result;
        
        }
    }
}
