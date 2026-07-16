using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
    public class ReportsController : ControllerBase
    {
        private IReportsService _iReportsService;
        private readonly UserManager<User> _userManager;
        public ReportsController(IReportsService iReportsService, UserManager<User> userManager)
        {
            _iReportsService = iReportsService;
            _userManager = userManager;
        }

        [HttpGet, Route("formH")]
        public async Task<IActionResult> FormHDetails([FromQuery] string Id, DateTime fromDate, DateTime toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            GenericFormModel<List<FormHViewModel>> genericFormModel = await _iReportsService.FormHDetails(Id, fromDate, toDate, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        


    }
}
