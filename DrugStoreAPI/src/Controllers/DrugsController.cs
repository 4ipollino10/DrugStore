using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Services;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.src.Controllers
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

        [HttpGet("{id}")]
        public IActionResult GetDrugAndComponentsPrices(int id)
        {
            var result = medicamentService.GetDrugAndComponentsPrices(id);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetDrugsInOrdersInProgress()
        {
            var result = medicamentService.GetDrugsInOrdersInProgress();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDrugsByMinimalAmount(MedicamentTypeDTO dto)
        {
            var result = await medicamentService.GetDrugsByMinimalAmount(dto);

            return Ok(result);
        }
    }
}
