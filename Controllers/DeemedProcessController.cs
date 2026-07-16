using Microsoft.AspNetCore.Authorization;
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
    public class DeemedProcessController : ControllerBase
    {
        private readonly IDeemedProcessService _iDeemedProcessService;
        private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        public DeemedProcessController(IDeemedProcessService iDeemedProcessService, IApplicationManagementService<ApplicationAction> iapplicationManagementService) 
        {
            _iDeemedProcessService = iDeemedProcessService;
            _iApplicationMamnagementService = iapplicationManagementService;
        }

        [Route("generateCertificates")]
        [HttpPost]
        public async Task<IActionResult> GenerateCertificates([FromBody] DeemedActionParmsViewModel requestData)
        {
            var resp = await _iDeemedProcessService.GenerateCertificates(requestData);
            return StatusCode(StatusCodes.Status200OK, requestData);
        }

        [Route("dormantApplications")]
        [HttpPost]
        public async Task<IActionResult> DormantApplications([FromBody] DormantParmsViewModel requestData)
        {
            var resp = await _iDeemedProcessService.DormantApplications(requestData);
            return StatusCode(StatusCodes.Status200OK, requestData);
        }

        [HttpGet, Route("esclationApplications")]
        public async Task<IActionResult> EsclationApplications()
        {
            GenericFormModel<bool> genericFormModel = await _iDeemedProcessService.EsclationApplications();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("deemedAllActCertificate")]
        public async Task<IActionResult> DeemedAllActCertificate()
        {
            GenericFormModel<bool> genericFormModel = await _iDeemedProcessService.DeemedAllActCertificates();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDeemedApplications")]
        public async Task<IActionResult> DeemedApplications([FromQuery] DateTime deemedDate , String type)
        {
            GenericResponseTemplateModel<List<All_Act_Deemed_ProcessEngineLogsViewModel>> genericFormModel = await _iDeemedProcessService.GetDeemedApplications(deemedDate, type);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDeemedTimeLineApplications")]
        public async Task<IActionResult> DeemedTimeLineApplications()
        {
            GenericResponseTemplateModel<List<DeemedApplicationTimeLineViewModel>> genericFormModel = await _iDeemedProcessService.GetDeemedTimeLineApplications();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getEscalationApplications")]
        public async Task<IActionResult> GetEscalationApplicationWiseData(Int64 escalationProcessEngineRefId, string userId)
        {
            GenericResponseTemplateModel<EscalationFileAndActWiseDataViewModel> genericFormModel = await _iDeemedProcessService.GetEscalationApplicationWiseData(escalationProcessEngineRefId, userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



    }
}
