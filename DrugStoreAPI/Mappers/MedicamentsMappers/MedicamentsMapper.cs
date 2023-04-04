using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Entities;

namespace DrugStoreAPI.Mappers.MedicamentsMappers
{
    public class MedicamentsMapper
    {
        public Component ComponentDTOtoComponent(ComponentDTO dto)
        {
            Component component = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Price = dto.Price,
                Amount = dto.Amount,
                CriticalAmount = dto.CriticalAmount,
            };

            return component;
        }

        public ComponentDTO ComponentToComponentDTO(Component component)
        {
            ComponentDTO componentDTO = new()
            {
                Id = component.Id,
                Name = component.Name,
                Type = component.Type,
                Price = component.Price,
                Amount = component.Amount,
                CriticalAmount = component.CriticalAmount,
            };

            return componentDTO;
        }

        public Drug DrugDTOtoDrug(DrugDTO dto)
        {
            Drug drug = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Price = dto.Price,
                Amount = dto.Amount,
                CriticalAmount = dto.CriticalAmount,
                Technology = dto.Technology,
                CookingTime = dto.CookingTime,
            };

            List<DrugsComponents> drugsComponentsList = new();

            foreach (var componentDTO in dto.Components)
            {
                Component component = ComponentDTOtoComponent(componentDTO.Component);

                DrugsComponents drugsComponentsEntity = new()
                {
                    DrugId = dto.Id,
                    Drug = drug,
                    Component = component,
                    ComponentId = component.Id,
                    Amount = componentDTO.Amount
                };

                drugsComponentsList.Add(drugsComponentsEntity);
            }

            drug.DrugsComponents = drugsComponentsList;

            return drug;
        }

        public DrugDTO DrugToDrugDTO(Drug drug)
        {
            List<DrugComponentDTO> components = new();

            foreach(var drugComponent in drug.DrugsComponents)
            {
                components.Add(new DrugComponentDTO() 
                { 
                    Amount = drugComponent.Amount, 
                    Component = ComponentToComponentDTO(drugComponent.Component) 
                });

            }

            DrugDTO drugDTO = new()
            {
                Id = drug.Id,
                Name = drug.Name,
                Type = drug.Type,
                Price = drug.Price,
                Amount = drug.Amount,
                CriticalAmount = drug.CriticalAmount,
                Technology = drug.Technology,
                Components = components,
                CookingTime = drug.CookingTime,
            };

            return drugDTO;
        }

        /*public List<DrugComponentDTO> DrugDTOtoListDrugComponentDTOs(DrugDTO dto)
        {
            List<DrugComponentDTO> components = new();
            
            foreach(var drugComponentDTO in dto.Components)
            {
                components.Add(drugComponentDTO.Component);
            }
            return components;
        }*/
    }
}
