using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
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
    [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
    public class MPRController : ControllerBase
    {
        private IMPRService _iMPRService;
        private readonly UserManager<User> _userManager;
        public MPRController(IMPRService iMPRService, UserManager<User> userManager)
        {
            _iMPRService = iMPRService;
            _userManager = userManager;
        }

        [HttpGet, Route("getMprFactoryDetail")]
        public async Task<IActionResult> GetMprFactoryDetail([FromQuery] Int64 id, string userId)
        {
            GenericResponseTemplateModel<MPR_Factory> genericFormModel = await _iMPRService.GetMprFactoryDetail(id, userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_mprFactoryDetail")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_MprFactoryDetail([FromBody] MPR_Factory requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMPRService.AddUpdate_MprFactoryDetail(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("checkDuplicateMonthYear")]
        public async Task<IActionResult> CheckDuplicateMonthYear([FromQuery] Int64 id, int month, int year, Int64 factoryCircleRefId, string userId)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iMPRService.CheckDuplicateMonthYear(id, month, year, factoryCircleRefId, userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet("get_mpr_list")]
        public async Task<IActionResult> GetMprLabourDetail([FromQuery] string userId, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Labour>> genericFormModel = await _iMPRService.GetMprLabourDetail(userId, month, year);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpPost("insert_null_json")]
        public async Task<IActionResult> InsertBlankJsonStepsAsync([FromBody] InsertNullJson_ViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMPRService.InsertBlankJsonStepsAsync(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpPost("save_steps_data")]
        public async Task<IActionResult> SaveStepData([FromBody] SaveSteps_ViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMPRService.SaveStepDataAsync(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpPost("insert_null_json_alc")]
        public async Task<IActionResult> InsertBlankJsonStepsAlcAsync([FromBody] InsertNullJsonAlc_ViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMPRService.InsertBlankJsonStepsAlcAsync(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpPost("save_steps_data_alc")]
        public async Task<IActionResult> SaveStepDataAlc([FromBody] SaveStepsAlc_ViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iMPRService.SaveStepDataAlcAsync(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet("get_mpr_list_alc")]
        public async Task<IActionResult> GetMprAlcDetail([FromQuery] string userId, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Alc>> genericFormModel = await _iMPRService.GetMprAlcDetail(userId, month, year);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("Get_MPR_data")]
        public async Task<IActionResult> GetMprDataAppType([FromQuery] string userId, int year, int month, int applicationType)
        {
            GenericResponseTemplateModel<List<MprDataViewModel>> genericResponseTemplate = await _iMPRService.GetMprDataAppType(userId, year, month, applicationType);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }


    }
}
