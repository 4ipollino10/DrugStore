using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.Exceptions;
using DrugStoreAPI.Mappers.MedicamentsMappers;
using DrugStoreAPI.Repositories;
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
        
        public ComponentDTO AddComponent(ComponentDTO dto)
        {
            MedicamentsMapper mapper = new MedicamentsMapper();
            Component component = mapper.ComponentDTOtoComponent(dto);

            component = medicamentsRepository.InsertComponent(component);

            return mapper.ComponentToComponentDTO(component);
        }

        public ComponentDTO UpdateComponent(ComponentDTO dto)
        {
            MedicamentsMapper mapper = new();
            Component updateComponent = mapper.ComponentDTOtoComponent(dto);

            return mapper.ComponentToComponentDTO(medicamentsRepository.UpdateComponent(updateComponent));
        }

        public void DeleteComponent(ComponentDTO dto) 
        {
            MedicamentsMapper mapper = new();

            if(GetComponent(dto).DrugsComponents.Count > 0)
            {
                throw new ComponentRelationshipException("asd");
            }

            medicamentsRepository.DeleteComponent(mapper.ComponentDTOtoComponent(dto));
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

        private Drug GetDrug(DrugDTO dto)
        {
            return medicamentsRepository.GetDrugById(dto.Id);
        }
        private Component GetComponent(ComponentDTO dto)
        {
            return medicamentsRepository.GetComponentById(dto.Id);
        }

        private List<DrugsComponents> GetDrugsComponents(Drug drug, List<DrugComponentDTO> drugComponentDTOs)
        {
            List<DrugsComponents> drugsComponents = new();
            
            foreach(var drugComponentDTO in drugComponentDTOs)
            {
                Component component = GetComponent(drugComponentDTO.Component);

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
