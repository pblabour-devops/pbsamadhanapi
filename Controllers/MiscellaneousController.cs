using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscellaneousController : ControllerBase
    {
        private readonly IMiscellaneousService _iMiscellaneousService;

        public MiscellaneousController(IMiscellaneousService iMiscellaneousService)
        {
            _iMiscellaneousService = iMiscellaneousService;
        }

        [HttpGet("getEstablishmentEpfoById")]
        public async Task<IActionResult> GetEstablishmentEpfoById([FromQuery] string establishmentId)
        {
            var genericServiceResultTemplate = await _iMiscellaneousService.GetEstablishmentEpfoDetails(establishmentId);

            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }

            return Ok(genericServiceResultTemplate);
        }

        [HttpGet, Route("getEstablishmentEpfoLogDetailbyId")]
        public async Task<IActionResult> GetEstablishmentEpfoLogDetailbyId([FromQuery] string establishmentRefId)
        {
            GenericResponseTemplateModel<List<Establishment_EPFO_Logs>> genericFormModel = await _iMiscellaneousService.GetEstablishmentEpfoLogDetailbyId(establishmentRefId); 
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("updateEstablishmentEPFODataLogs")]
        [HttpPost]
        public async Task<IActionResult> UpdateEstablishmentDataLogs([FromBody] Establishment_EPFO_Logs requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMiscellaneousService.UpdateEstablishmentDataLogs(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("updateEstablishmentEPFORemarks")]
        [HttpPost]
        public async Task<IActionResult> UpdateEstablishmentReamrks([FromBody] Establishment_EPFO_Logs requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMiscellaneousService.UpdateEstablishmentReamrks(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("getEstablishmentEPFOReport")]
        public async Task<IActionResult> Get_EstablishmentEPFOReport([FromQuery] string fromdate, string todate, string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<Establishment_EPFO_Report>> genericFormModel = await _iMiscellaneousService.GetEstablishmentEPFOReport(fromdate, todate, id, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
    }
}