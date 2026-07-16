using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircleManagerController : ControllerBase
    {
        private ICircleManager _iCircleManager;
        public CircleManagerController(ICircleManager ICircleManager)
        {
            _iCircleManager = ICircleManager;
        }

        [HttpGet, Route("getFactoryCircleByDistricRefId")]
        public async Task<IActionResult> Get_FactoryCircleByDistricRefId([FromQuery] int districtRefId)
        {
            GenericFormModel<List<CircleManagerViewModel>> genericFormModel = await _iCircleManager.Get_FactoryCircleByDistricRefId(districtRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLabourCircleByalcCircleRefId")]
        public async Task<IActionResult> Get_LabourCircleByDistricRefId([FromQuery] int districtRefId)
        {
            GenericFormModel<List<LabourCircleViewModel>> genericFormModel = await _iCircleManager.Get_LabourCircleByDistricRefId(districtRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLabourCircleOfficersByAppRefId")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_LabourCircleOfficersByAppRefId([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<List<TransferUserInfoViewModel>> genericFormModel = await _iCircleManager.Get_LabourCircleOfficersByAppRefId(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("verifyAppCircleVersionUpdate")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> VerifyAppCircleVersionUpdate([FromQuery] Int64 projectSiteRefId, string roleCode, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<VerifyAppCircleVersionRespViewModel> genericFormModel = await _iCircleManager.VerifyAppCircleVersionUpdate(projectSiteRefId, roleCode, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getFactoryCircleOfficersByAppRefId")]
        public async Task<IActionResult> Get_FactoryCircleOfficersByAppRefId([FromQuery] Int64 id, string roleName)
        {
            GenericResponseTemplateModel<List<TransferFactoryCircleUserInfoViewModel>> genericFormModel = await _iCircleManager.Get_FactoryCircleOfficersByAppRefId(id, roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("getAlcCircleOfficersByAppRefId")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_ALCCircleOfficersByAppRefId([FromQuery] Int64 id, string roleName)
        {
            GenericResponseTemplateModel<List<TransferALCCircleUserInfoViewModel>> genericFormModel = await _iCircleManager.Get_ALCCircleOfficersByAppRefId(id, roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("getAlcCircleByDistricRefId")]
        public async Task<IActionResult> Get_AlcCircleByDistricRefId([FromQuery] int districtRefId)
        {
            GenericFormModel<List<ALCCircleManagerViewModel>> genericFormModel = await _iCircleManager.Get_AlcCircleByDistricRefId(districtRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


    }
}
