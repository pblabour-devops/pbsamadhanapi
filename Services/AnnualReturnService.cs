using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace pbsamadhannetcoreapi.Services
{
    public class AnnualReturnService : IAnnualReturnService
    {
        private readonly IGenericRepository<Annual_Return> _iGR_Annual_Return;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public AnnualReturnService(IGeneric_SP_Repository iGeneric_SP_Repository, IGenericRepository<Annual_Return> iGR_Annual_Return, AppDbContext context)
        {
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iGR_Annual_Return = iGR_Annual_Return;
            _context = context;
        }

        public async Task<GenericResponseTemplateModel<List<Annual_Return>>> GetAnnualReturnList(string userId, Int64 projectSiteRefId)
        {
            var genericFormModel = new GenericResponseTemplateModel<List<Annual_Return>>();
            try
            {
                var records = await _context.AnnualReturns
                    .Where(x => x.UserId == userId &&
                         x.ProjectSiteRefId == projectSiteRefId &&
                                (
                                  !string.IsNullOrEmpty(x.JsonData) ||
                                  (x.StepCodes == "LOCK" && x.IsLocked == true)
                                ))
                    .OrderBy(x => x.CreatedOn)
                    .ToListAsync();

                genericFormModel.ResponseDataModel = records;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> InitiateAnnualReturn(InitiateAnnualReturnViewModel formModel)
        {
            var genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                if (formModel == null || formModel.StepCodes == null || !formModel.StepCodes.Any())
                    return genericServiceResultTemplate;

                var userId = formModel.SubmittedBy_UserRefId?.ToString();
                var appId = formModel.AppId;
                var returnYear = formModel.ReturnYear;
                var licenceNumber = formModel.LicenceNumber;


                var selectedActs = (formModel.ActIds ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var stepCode in formModel.StepCodes)
                {
                    bool exists = await _context.AnnualReturns.AnyAsync(x =>
                        x.AppId == appId &&
                        x.ReturnYear == returnYear &&
                        x.StepCodes == stepCode &&
                        x.UserId == userId &&
                        x.ProjectSiteRefId == formModel.ProjectSiteRefId &&
                        x.LicenceNumber == formModel.LicenceNumber
                    );

                    if (!exists)
                    {
                        var record = new Annual_Return
                        {
                            AppId = appId,
                            ReturnYear = returnYear,
                            UserId = userId,
                            StepCodes = stepCode,
                            JsonData = null,
                            ActIds = string.Join(",", selectedActs),
                            IsLocked = false,
                            CreatedOn = DateTime.UtcNow,
                            LastModifiedOn = DateTime.UtcNow,
                            ProjectSiteRefId = formModel.ProjectSiteRefId,
                            LicenceNumber = formModel.LicenceNumber
                        };

                        _context.AnnualReturns.Add(record);
                    }
                }



                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }

            return genericServiceResultTemplate;
        }



        public async Task<GenericServiceResultTemplate> SaveStepDataReturn(SaveStepsReturnViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {

                var roleId = _context.UserRoles
                    .Where(x => x.UserId == formModel.SubmittedBy_UserRefId)
                    .Select(x => x.RoleId)
                    .FirstOrDefault();


                if (formModel.IsLocked == true)
                {
                    var records = _context.AnnualReturns.Where(x =>
                        x.AppId == formModel.AppId &&
                        x.ReturnYear == formModel.ReturnYear);

                    foreach (var record in records)
                    {
                        record.IsLocked = true;
                        record.LastModifiedOn = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                    return genericServiceResultTemplate;
                }


                var existing = await _context.AnnualReturns.FirstOrDefaultAsync(x =>
                    x.AppId == formModel.AppId &&
                    x.ReturnYear == formModel.ReturnYear &&
                    x.StepCodes == formModel.StepCodes &&
                    x.LicenceNumber == formModel.LicenceNumber
                   );

                if (existing != null)
                {
                    existing.JsonData = formModel.JsonData;
                    existing.LastModifiedOn = DateTime.Now;
                    existing.ProjectSiteRefId = formModel.ProjectSiteRefId;
                    _context.AnnualReturns.Update(existing);
                }
                else
                {
                    var newRecord = new Annual_Return
                    {
                        AppId = formModel.AppId,
                        ReturnYear = formModel.ReturnYear,
                        StepCodes = formModel.StepCodes,
                        JsonData = formModel.JsonData,
                        UserId = formModel.SubmittedBy_UserRefId,
                        IsLocked = false,
                        CreatedOn = DateTime.Now,
                        LastModifiedOn = DateTime.Now,
                        ProjectSiteRefId = formModel.ProjectSiteRefId,
                        LicenceNumber = formModel.LicenceNumber
                    };

                    _context.AnnualReturns.Add(newRecord);
                }

                await _context.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }

            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<EstablishmentDataViewModel>>> GetEstablishmentDetails(string licenceNo)
        {
            GenericResponseTemplateModel<List<EstablishmentDataViewModel>> genericFormModel = new GenericResponseTemplateModel<List<EstablishmentDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
      {
             new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo, isNumber=false},

      };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EstablishmentDataViewModel>("dbo.sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);

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
