using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using DrugStoreAPI.Exceptions;
using DrugStoreAPI.DTOs.MedicamentDTOs;

namespace DrugStoreAPI.Controllers
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
        public ComponentDTO Add(ComponentDTO dto)
        {
            return medicamentService.AddComponent(dto); 
        }

        [HttpPost]
        public ComponentDTO Update(ComponentDTO dto)
        {
            return medicamentService.UpdateComponent(dto);
        }

        [HttpDelete]
        public void Delete(ComponentDTO dto)
        {
            medicamentService.DeleteComponent(dto);
        }



    }
}
