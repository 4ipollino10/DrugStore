using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Services;

namespace DrugStoreAPI.src.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ComponentsController : Controller
    {
        private readonly IComponentsService componentsService;

        public ComponentsController(IComponentsService componentsService)
        {
            this.componentsService = componentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ComponentDTO dto)
        {
            var component = await componentsService.AddComponent(dto);

            return Ok(component);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ComponentDTO dto)
        {
            var updatedComponent = await componentsService.UpdateComponent(dto);

            return Ok(updatedComponent);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await componentsService.DeleteComponent(id);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(ComponentDTO dto)
        {
            var component = await componentsService.GetComponentById(dto.Id);

            return Ok(component);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComponents()
        {
            var components = await componentsService.GetAllComponents();

            return Ok(components);
        }

        [HttpGet]
        public async Task<IActionResult> GetComponentsByCriticalAmount()
        {
            var components = await componentsService.GetComponentsByCriticalAmount();

            return Ok(components);
        }

        [HttpPost]
        public async Task<IActionResult> GetTopUsefulComponents(MedicamentTypeDTO dto)
        {
            var components = await componentsService.GetTopUsefulComponets(dto);

            return Ok(components);
        }

        [HttpPost]
        public async Task<IActionResult> GetUsedAmountComponentForPeriod(ComponentAmountUsedReportDTO dto)
        {
            var amount = await componentsService.GetUsedAmountComponentForPeriod(dto);

            return Ok(amount);
        } 


    }
}
