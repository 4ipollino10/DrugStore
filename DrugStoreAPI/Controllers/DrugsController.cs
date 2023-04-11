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
        public DrugDTO Add(DrugDTO dto)
        {
            return medicamentService.AddDrug(dto);
        }
        [HttpPost]
        public DrugDTO Update(DrugDTO dto)
        {
            return medicamentService.UpdateDrug(dto);
        }
        [HttpDelete]
        public void Delete(DrugDTO dto) 
        {
            medicamentService.DeleteDrug(dto);
        }
        [HttpGet]
        public DrugDTO GetDrugs()
        {
            return medicamentService.GetDrugs();
        }

    }
}
