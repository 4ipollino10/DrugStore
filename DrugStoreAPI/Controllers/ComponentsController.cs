using DrugStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using DrugStoreAPI.Exceptions;
using DrugStoreAPI.DTOs.MedicamentDTOs;
using System.Net;
using LanguageExt.Common;

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
        public async Task<IActionResult> Add(ComponentDTO dto)
        {
            var result = await medicamentService.AddComponent(dto);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<ComponentDTO> Update(ComponentDTO dto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<bool> Delete(ComponentDTO dto)
        {
            throw new NotImplementedException(nameof(dto));
        }



    }
}
