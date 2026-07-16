using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessEntityController : ControllerBase
    {
    //    private IBusinessEntityService _iBusinessEntityService;
    //    public BusinessEntityController(IBusinessEntityService iBusinessEntityService)
    //    {
    //        _iBusinessEntityService = iBusinessEntityService;
    //    }

    //    #region Business Entity
    //    [HttpGet, Route("getbusinessentity")]
    //    public async Task<IActionResult> Get_businessentity([FromQuery] Int64 id)
    //    {
    //        GenericFormModel<BusinessEntity> genericFormModel = await _iBusinessEntityService.GetBusinessEntityDetail(id);
    //        if (genericFormModel.FormModel == null && id != 0)
    //        {
    //            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
    //            genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
    //            genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
    //            genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages = "Invalid establishment identity";
    //            return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
    //        }
    //        return Ok(genericFormModel);
    //    }

    //    [Route("addupdate_businessentitydetails")]
    //    [HttpPost]
    //    public async Task<IActionResult> AddUpdate_GeneralDetail([FromForm] string requestData)
    //    {
    //        GenericServiceResultTemplate genericServiceResultTemplate = await _iBusinessEntityService.AddUpdate_BusinessEntityDetail(requestData);
    //        if (genericServiceResultTemplate.HasException)
    //        {
    //            return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
    //        }
    //        else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
    //        {
    //            return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
    //        }
    //        return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
    //    }
        //#endregion
    }
}
