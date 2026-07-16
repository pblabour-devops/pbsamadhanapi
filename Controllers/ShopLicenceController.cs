using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ShopLicenceController : ControllerBase
    {
        private IShopLicenceService _iShopLicenceService;
        private readonly UserManager<User> _userManager;
        public ShopLicenceController(IShopLicenceService iShopLicenceService, UserManager<User> userManager)
        {
            _iShopLicenceService = iShopLicenceService;
            _userManager = userManager;
        }
        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<ShopLicence_GeneralDetail> genericFormModel = await _iShopLicenceService.GetShopLicenceGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        //***** SHIFTTED TO CRUD 
        //[Route("addupdate_generaldetails")]
        //[HttpPost]
        //public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] ShopLicence_GeneralDetail requestData)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iShopLicenceService.AddUpdate_ShopLicenceDetail(requestData, user.Id);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }
        //    else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}

        [HttpGet, Route("getemployeedetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_EmployeesDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<EmployeeDetailViewModel> genericFormModel = await _iShopLicenceService.GetShopLicenceEmlpyeeDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getShopLicenceEmlpyeesList")]
        public async Task<IActionResult> Get_ShopLicenceEmlpyeesList([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericFormModel<List<ShopEmployeeDetailViewModel>> genericFormModel = await _iShopLicenceService.GetShopLicenceEmlpyeesList(dataTableParams.AppRefId, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        //***** SHIFTTED TO CRUD 
        //[Route("addupdate_employeedetails")]
        //[HttpPost]
        //public async Task<IActionResult> AddUpdate_EmployeeDetail([FromBody] EmployeeDetailViewModel requestData)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iShopLicenceService.AddUpdate_ShopLicenceEmployeeDetail(requestData);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}
        #endregion

        #region ShopLicence Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<ShopLicenceViewModel> genericFormModel = await _iShopLicenceService.GetShopLicenceDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion ShopLicence Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iShopLicenceService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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
        #endregion Lock Application

        #region Find Duplicate
        [HttpGet, Route("findDuplicateGST")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> FindDuplicateGST([FromQuery] string gstNumber, Int64 shopLicenceId)
        {
            GenericFormModel<IntReturn> genericFormModel = await _iShopLicenceService.FindDuplicateGST(gstNumber, shopLicenceId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("findDuplicatePan")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> FindDuplicatePAN([FromQuery] string panOrTanNumber, Int64 shopLicenceId)
        {
            GenericFormModel<IntReturn> genericFormModel = await _iShopLicenceService.FindDuplicatePanNo(panOrTanNumber, shopLicenceId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("findDuplicateAadharNo")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> FindDuplicateAadharNo([FromQuery] string aadharNumber, Int64 projectSiteRefId)
        {
            GenericResponseTemplateModel<AdhaarVerifyViewModel> genericFormModel = await _iShopLicenceService.FindDuplicateAadharNumber(aadharNumber, projectSiteRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("removeEmployee")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> RemoveEmployee([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iShopLicenceService.RemoveEmployee(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion
    }
}
