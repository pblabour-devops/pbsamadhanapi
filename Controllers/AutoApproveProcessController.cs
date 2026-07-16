using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoApproveProcessController : Controller
    {
        private readonly IAutoApproveProcessService _iAutoApproveProcessService;
        public AutoApproveProcessController(IAutoApproveProcessService iAutoApproveProcessService)
        {
            _iAutoApproveProcessService = iAutoApproveProcessService;
        }

        [Route("generateCertificates")]
        [HttpPost]
        public async Task<IActionResult> GenerateCertificates([FromBody] AutoApproveActionParmsViewModel requestData)
        {

            var resp = await _iAutoApproveProcessService.GenerateCertificates(requestData);
            return StatusCode(StatusCodes.Status200OK, requestData);
        }
    }
}
