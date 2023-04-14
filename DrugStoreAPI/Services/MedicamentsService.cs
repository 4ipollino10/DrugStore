using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Exceptions;
using DrugStoreAPI.Mappers.MedicamentsMappers;
using DrugStoreAPI.Repositories;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.Services
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
            var currentComponents = await GetAllComponents();

            foreach(var currentComponent in currentComponents)
            {
                if(currentComponent.Name == dto.Name)
                {
                    throw new DuplicateComponentException($"Component with such name: \"{dto.Name}\" is already exists!");
                }
            }

            var mapper = new MedicamentsMapper();

            var insertedComponent = await medicamentsRepository.InsertComponent(mapper.ComponentDTOtoComponent(dto));

            return mapper.ComponentToComponentDTO(insertedComponent);
        }

        public async Task<ComponentDTO> UpdateComponent(ComponentDTO dto)
        {
            var currentComponent = medicamentsRepository.GetComponentById(dto.Id);

            if(currentComponent == null)
            {
                throw new ComponentNotFoundException($"Component with such id: \"{dto.Id}\" do not exists!");
            }

            var mapper = new MedicamentsMapper();

            var updatedOrder = await medicamentsRepository.UpdateComponent(mapper.ComponentDTOtoComponent(dto));

            return mapper.ComponentToComponentDTO(updatedOrder);
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var deleteResult = await medicamentsRepository.DeleteComponent(id);

            if(deleteResult == false)
            {
                throw new ComponentNotFoundException($"Component with such id: \"{id}\" do not exists!");
            }

            return deleteResult;
        }

        public async Task<ComponentDTO> GetComponentById(int id)
        {
            var component = await medicamentsRepository.GetComponentById(id);

            if(component == null)
            {
                throw new ComponentNotFoundException($"Component with such id: \"{id}\" do not exist!");
            }

            var mapper = new MedicamentsMapper();
            
            return mapper.ComponentToComponentDTO(component);

        }

        public async Task<IEnumerable<ComponentDTO>> GetAllComponents()
        {
            var currentComponents = await medicamentsRepository.GetAllComponents();

            if(currentComponents == null)
            {
                throw new ComponentNotFoundException("There are no components!");
            }

            var mapper = new MedicamentsMapper();
            
            var components = new List<ComponentDTO>();
            
            foreach(var component in currentComponents)
            {
                components.Add(mapper.ComponentToComponentDTO(component));
            }
            
            return components;
        }

        public async Task<DrugDTO> AddDrug(DrugDTO dto)
        {
            var currentDrugs = await GetAllDrugs();

            foreach(var currentDrug in currentDrugs)
            {
                if(dto.Name == currentDrug.Name)
                {
                    throw new DuplicateDrugException($"Drug with such name: \"{dto.Name}\" is already exists!");
                }
            }

            foreach (var drugComponentDTO in dto.Components)
            {
                if (await medicamentsRepository.GetComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: \"{drugComponentDTO.Component.Id}\" do not exists!");
                }
            }

            var mapper = new MedicamentsMapper();
            var drug = mapper.DrugDTOtoDrug(dto);

            var drugsComponents = await GetDrugsComponents(drug, dto.Components);
            drug.DrugsComponents = drugsComponents;

            drug = await medicamentsRepository.InsertDrug(drug);

            return mapper.DrugToDrugDTO(drug);
        }

        public async Task<DrugDTO> UpdateDrug(DrugDTO dto)
        {
            var currentDrug = await medicamentsRepository.GetDrugById(dto.Id);
            
            if(currentDrug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: \"{dto.Id}\" do not exists!");
            }
            
            foreach(var drugComponentDTO in dto.Components)
            {
                if (await medicamentsRepository.GetComponentById(drugComponentDTO.Component.Id) == null)
                {
                    throw new DrugNotFoundException($"Component with such id: \"{drugComponentDTO.Component.Id}\" do not exists!");
                }
            }

            var mapper = new MedicamentsMapper();

            var updatedDrug = mapper.DrugDTOtoDrug(dto);

            var drugsComponents = await GetDrugsComponents(updatedDrug, dto.Components);

            updatedDrug.DrugsComponents = drugsComponents;

            updatedDrug = await medicamentsRepository.UpdateDrug(updatedDrug);

            return mapper.DrugToDrugDTO(updatedDrug);
        }

        public async Task<bool> DeleteDrug(int id)
        {
            var deleteResult = await medicamentsRepository.DeleteDrug(id);

            if (deleteResult == false)
            {
                throw new DrugNotFoundException($"Drug with such id: \"{id}\" do not exists!");
            }

            return deleteResult;
        }
        public async Task<DrugDTO> GetDrugById(int id)
        {
            var drug = await medicamentsRepository.GetDrugById(id);

            if(drug == null)
            {
                throw new DrugNotFoundException($"Drug with such id: \"{id}\" do not exists!");
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
            foreach(var drugComponentDTO  in drugComponentDTOs)
            {
                var component = await medicamentsRepository.GetComponentById(mapper.ComponentDTOtoComponent(drugComponentDTO.Component).Id);

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
    }
}
