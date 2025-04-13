using Microsoft.AspNetCore.Mvc;
using MiddlewareService.Application.Services;
using MiddlewareService.Domain.Models.Request;

namespace MiddlewareService.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase{
        private readonly ApiService _service;
        public BankController(ApiService service){
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterRequest request)
        {
            var result = await _service.GetBanks(request);
            return result.Code switch
            {
                "200" => Ok(result),
                "400" => BadRequest(result),
                "404" => NotFound(result),
                _     => StatusCode(500, result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post(BankRequest request)
        {
            var result = await _service.InsertBank(request);
            return result.Code switch
            {
                "200" => Ok(result),
                "400" => BadRequest(result),
                "404" => NotFound(result),
                _     => StatusCode(500, result)
            };
        }
    }
}