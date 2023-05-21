using DrugStoreAPI.DTOs.MedicamentDTOs;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Services;
using DrugStoreAPI.src.DTOs.QueriesDTOs;
using DrugStoreAPI.src.Services;
using DrugStoreAPI.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrugStoreAPI.src.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DrugsController : Controller
    {
        private readonly IDrugsService drugsService;

        public DrugsController(IDrugsService drugsService)
        {
            this.drugsService = drugsService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(DrugDTO dto)
        {
            var drug = await drugsService.AddDrug(dto);

            return Ok(drug);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DrugDTO dto)
        {
            var updatedDrug = await drugsService.UpdateDrug(dto);

            return Ok(updatedDrug);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await drugsService.DeleteDrug(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drug = await drugsService.GetDrugById(id);

            return Ok(drug);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrugs()
        {
            var drugs = await drugsService.GetAllDrugs();

            return Ok(drugs);
        }

        [HttpGet("{id}")]
        public IActionResult GetDrugAndComponentsPrices(int id)
        {
            var result = drugsService.GetDrugAndComponentsPrices(id);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetDrugsInOrdersInProgress()
        {
            var result = drugsService.GetDrugsInOrdersInProgress();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDrugsByMinimalAmount(MedicamentTypeDTO dto)
        {
            var result = await drugsService.GetDrugsByMinimalAmount(dto);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetDrugsTechnologies(DrugInProgressTechnologyReportDTO dto)
        {
            var result = await drugsService.GetDrugsTechnologies(dto);

            return Ok(result);
        }
    }
}
