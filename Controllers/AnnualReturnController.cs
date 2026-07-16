using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomFillters.AuthorizeAttribute("INDL")]
    public class AnnualReturnController : ControllerBase
    {
        private IAnnualReturnService _iAnnualReturnService;
        private readonly UserManager<User> _userManager;
        public AnnualReturnController(IAnnualReturnService iAnnualReturnService, UserManager<User> userManager)
        {
            _iAnnualReturnService = iAnnualReturnService;
            _userManager = userManager;
        }

        [HttpGet("get_AnnualReturnList")]
        public async Task<IActionResult> GetAnnualReturnList([FromQuery] string userId, Int64 projectSiteRefId)
        {
            var genericResponseTemplate = await _iAnnualReturnService.GetAnnualReturnList(userId, projectSiteRefId);

            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }

            return Ok(genericResponseTemplate);
        }

        [HttpPost("initiate_AnnualReturn")]
        public async Task<IActionResult> InitiateAnnualReturn([FromBody] InitiateAnnualReturnViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAnnualReturnService.InitiateAnnualReturn(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpPost("save_StepsDataReturn")]
        public async Task<IActionResult> SaveStepDataReturn([FromBody] SaveStepsReturnViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAnnualReturnService.SaveStepDataReturn(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("Get_EstablishmentDetails")]
        public async Task<IActionResult> GetEstablishmentDetails([FromQuery] string licenceNo)
        {
            GenericResponseTemplateModel<List<EstablishmentDataViewModel>> genericResponseTemplate = await _iAnnualReturnService.GetEstablishmentDetails(licenceNo);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }
    }
}