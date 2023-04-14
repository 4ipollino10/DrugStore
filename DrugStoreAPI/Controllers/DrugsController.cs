using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DrugsController : Controller
    {
        private readonly IMedicamentsService medicamentService;

        public DrugsController(IMedicamentsService medicamentService) 
        {
            this.medicamentService = medicamentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(DrugDTO dto)
        {
            var drug = await medicamentService.AddDrug(dto);

            return Ok(drug);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DrugDTO dto)
        {
            var updatedDrug = await medicamentService.UpdateDrug(dto);

            return Ok(updatedDrug);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id) 
        {
            return await medicamentService.DeleteDrug(id);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drug = await medicamentService.GetDrugById(id);

            return Ok(drug);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrugs()
        {
            var drugs = await medicamentService.GetAllDrugs();

            return Ok(drugs);
        }

    }
}
