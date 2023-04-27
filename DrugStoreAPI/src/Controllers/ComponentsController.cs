using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.src.DTOs.QueriesDTOs;

namespace DrugStoreAPI.src.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ComponentsController : Controller
    {
        private readonly IMedicamentsService medicamentService;

        public ComponentsController(IMedicamentsService medicamentService)
        {
            this.medicamentService = medicamentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ComponentDTO dto)
        {
            var component = await medicamentService.AddComponent(dto);

            return Ok(component);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ComponentDTO dto)
        {
            var updatedComponent = await medicamentService.UpdateComponent(dto);

            return Ok(updatedComponent);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await medicamentService.DeleteComponent(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(ComponentDTO dto)
        {
            var component = await medicamentService.GetComponentById(dto.Id);

            return Ok(component);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComponents()
        {
            var components = await medicamentService.GetAllComponents();

            return Ok(components);
        }

        [HttpGet]
        public async Task<IActionResult> GetComponentsByCriticalAmount()
        {
            var components = await medicamentService.GetComponentsByCriticalAmount();

            return Ok(components);
        }

        [HttpPost]
        public async Task<IActionResult> GetTopUsefulComponents(MedicamentTypeDTO dto)
        {
            var components = await medicamentService.GetTopUsefulComponets(dto);

            return Ok(components);
        }

        [HttpPost]
        public async Task<IActionResult> GetUsedAmounComponentForPeriod(ComponentAmountUsedReportDTO dto)
        {
            var amount = await medicamentService.GetUsedAmountComponentForPeriod(dto);

            return Ok(amount);
        }


    }
}
