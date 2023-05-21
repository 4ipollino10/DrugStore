using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Configuration.Exceptions;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Mappers.MedicamentsMappers;
using DrugStoreAPI.src.Repositories;

namespace DrugStoreAPI.src.Services
{
    public class ComponentsService : IComponentsService
    {
        private readonly IComponentsRepository componentsRepository;

        public ComponentsService(IComponentsRepository componentsRepository)
        {
            this.componentsRepository = componentsRepository;
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

            var insertedComponent = await componentsRepository.InsertComponent(component);
            return medicamentsMapper.ComponentToComponentDTO(insertedComponent);
        }

        private bool IsComponentHasDuplicates(Component component)
        {
            var result = componentsRepository.FindComponentByNameIsAndTypeIs(component.Name, component.Type);

            return result == null;
        }

        public async Task<ComponentDTO> UpdateComponent(ComponentDTO dto)
        {
            var currentComponent = componentsRepository.FindComponentById(dto.Id);

            if (currentComponent == null)
            {
                throw new ComponentNotFoundException($"Component with such id: '{dto.Id}' do not exists!");
            }

            var mapper = new MedicamentsMapper();
            var updatedOrder = await componentsRepository.UpdateComponent(mapper.ComponentDTOtoComponent(dto));

            return mapper.ComponentToComponentDTO(updatedOrder);
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var deleteResult = await componentsRepository.DeleteComponent(id);

            if (!deleteResult)
            {
                throw new ComponentNotFoundException($"Component with such id: '{id}' do not exists!");
            }

            return deleteResult;
        }

        public async Task<ComponentDTO> GetComponentById(int id)
        {
            var component = await componentsRepository.FindComponentById(id);

            if (component == null)
            {
                throw new ComponentNotFoundException($"Component with such id: '{id}' do not exist!");
            }

            var mapper = new MedicamentsMapper();
            return mapper.ComponentToComponentDTO(component);

        }

        public async Task<IEnumerable<ComponentDTO>> GetAllComponents()
        {
            var currentComponents = await componentsRepository.GetAllComponents();

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

        public async Task<IEnumerable<ComponentDTO>> GetComponentsByCriticalAmount()
        {
            var result = await componentsRepository.FindComponentsByCriticalAmount();

            var medicamentsMapper = new MedicamentsMapper();
            var components = new List<ComponentDTO>();

            foreach (var component in result)
            {
                components.Add(medicamentsMapper.ComponentToComponentDTO(component));
            }

            return components;
        }

        public async Task<IEnumerable<ComponentDTO>> GetTopUsefulComponets(MedicamentTypeDTO dto)
        {
            var result = await componentsRepository.GetTopUsefulComponetsByTypeIs(dto.Type);

            var tmpMap = new Dictionary<Component, int>();

            foreach (var componet in result)
            {
                if (tmpMap.ContainsKey(componet))
                {
                    tmpMap[componet]++;
                    continue;
                }

                tmpMap.Add(componet, 1);
            }

            var sortedTmpMap = from entry in tmpMap orderby entry.Value descending select entry;

            var medicametsMapper = new MedicamentsMapper();
            var components = new List<ComponentDTO>();
            foreach (var component in sortedTmpMap)
            {
                components.Add(medicametsMapper.ComponentToComponentDTO(component.Key));
            }

            return components;
        }

        public async Task<ComponentAmountDTO> GetUsedAmountComponentForPeriod(ComponentAmountUsedReportDTO dto)
        {
            var result = await componentsRepository.GetUsedAmountComponentForPeriodAndTypeIs(dto.From, dto.To, dto.Name);

            var amount = 0;
            foreach (var componentsAmount in result)
            {
                amount += componentsAmount;
            }

            var componentAmount = new ComponentAmountDTO()
            {
                Amount = amount
            };

            return componentAmount;
        }
    }
}
