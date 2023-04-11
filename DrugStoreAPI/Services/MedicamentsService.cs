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
            var mapper = new MedicamentsMapper();
            
            var currentComponents = await GetAllComponents();

            foreach(var currentComponent in currentComponents)
            {
                if(currentComponent.Name == dto.Name)
                {
                    throw new BadRequestException($"Компонента с таким именем: {dto.Name} уже существует");
                }
            }

            var result = await medicamentsRepository.InsertComponent(mapper.ComponentDTOtoComponent(dto));

            return mapper.ComponentToComponentDTO(result);
        }

        public Task<ComponentDTO> UpdateComponent(ComponentDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteComponent(int id)
        {
            var deleteResult = await medicamentsRepository.DeleteComponent(id);

            return deleteResult;
        }

        public async Task<IEnumerable<ComponentDTO>> GetAllComponents()
        {
            var mapper = new MedicamentsMapper();

            var gotComponents = await medicamentsRepository.GetAllComponents();

            var components = new List<ComponentDTO>();
            foreach(var component in gotComponents)
            {
                components.Add(mapper.ComponentToComponentDTO(component));
            }
            
            return components;
        }

        public DrugDTO AddDrug(DrugDTO dto)
        {
            MedicamentsMapper mapper = new();

            Drug drug = mapper.DrugDTOtoDrug(dto);

            List<DrugsComponents> drugsComponents = GetDrugsComponents(drug, dto.Components);

            drug.DrugsComponents = drugsComponents;

            drug = medicamentsRepository.InsertDrug(drug);

            return mapper.DrugToDrugDTO(drug);
        }

        public DrugDTO UpdateDrug(DrugDTO dto)
        {
            MedicamentsMapper mapper = new();

            Drug updatedDrug = mapper.DrugDTOtoDrug(dto);

            return mapper.DrugToDrugDTO(medicamentsRepository.UpdateDrug(updatedDrug));
        }

        public void DeleteDrug(DrugDTO dto)
        {
            MedicamentsMapper mapper = new();

            medicamentsRepository.DeleteDrug(mapper.DrugDTOtoDrug(dto));
        }
        public DrugDTO GetDrugs()
        {
            MedicamentsMapper mapper = new();
            return mapper.DrugToDrugDTO(medicamentsRepository.GetDrugById(1));
        }
        /*private Component GetComponent(ComponentDTO dto)
        {
            return medicamentsRepository.GetComponentById(dto.Id);
        }*/

        private List<DrugsComponents> GetDrugsComponents(Drug drug, List<DrugComponentDTO> drugComponentDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
