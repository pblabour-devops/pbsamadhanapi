using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        public ReportsService(IGeneric_SP_Repository iGeneric_SP_Repository, UserManager<User> userManager, AppDbContext context)
        {
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _userManager = userManager;
            _context = context;
        }

        public async Task<GenericFormModel<List<FormHViewModel>>> FormHDetails(string id, DateTime fromDate, DateTime toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        { 
            GenericFormModel<List<FormHViewModel>> genericFormModel = new GenericFormModel<List<FormHViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                   storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "UserRefId", ParmValue = id.ToString(), isNumber = false },
                       new StoreProcedureParm() { ParmName = "StartDate", ParmValue = fromDate.ToString("yyyy-MM-dd"), isNumber = false },
                       new StoreProcedureParm() { ParmName = "EndDate", ParmValue = toDate.ToString("yyyy-MM-dd"), isNumber = false },

                       new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FormHViewModel>("sp_getFormHDetails", storeProcedureParms);
                genericFormModel.FormModel = new List<FormHViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

    }
           
}
