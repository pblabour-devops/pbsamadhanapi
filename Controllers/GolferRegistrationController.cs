using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GolferRegistrationController : ControllerBase
    {
        private readonly IGolferRegistrationService _iGolferRegistrationService;
        public GolferRegistrationController(IGolferRegistrationService iGolferRegistrationService)
        {
            _iGolferRegistrationService = iGolferRegistrationService;
        }

        [HttpGet, Route("checkMobileNo")]
        public async Task<IActionResult> CheckMobileNo([FromQuery] string mobileNo)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iGolferRegistrationService.CheckMobileNo(mobileNo);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("addGolferRegistrationDetails")]
        [HttpPost]
        public async Task<IActionResult> AddGolferRegistrationDetails([FromBody] GolferRegistration formModel)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iGolferRegistrationService.AddGolferRegistrationDetails(formModel);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

    }
}

