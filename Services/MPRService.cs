using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class MPRService : IMPRService
    {

        private readonly IGenericRepository<MPR_Factory> _iGR_MPR_Factory;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public MPRService(IGenericRepository<MPR_Factory> iGR_MPR_Factory, AppDbContext context,IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _iGR_MPR_Factory = iGR_MPR_Factory;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }

        public async Task<GenericResponseTemplateModel<MPR_Factory>> GetMprFactoryDetail(long id, string userId)
        {
            GenericResponseTemplateModel<MPR_Factory> genericFormModel = new GenericResponseTemplateModel<MPR_Factory>();
            try
            {
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_MPR_Factory
                       .GetAsync(x => x.Id == id)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.ResponseDataModel = parentWithChildObject.FirstOrDefault();
                }

                else //New Record
                {

                    genericFormModel.ResponseDataModel = new MPR_Factory();
                    genericFormModel.ResponseDataModel.FactoryCircleRefId = (Int64)_context.UserCircleMappings.Where(x => x.UserRefId == userId && x.Version == 2).Select(x => x.FactoryCircleRefId).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_MprFactoryDetail(MPR_Factory formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                formModel.LastModifiedOn = DateTime.Now;
                formModel.SubmittedBy_RoleRefId = _context.UserRoles.Where(x => x.UserId == formModel.SubmittedBy_UserRefId).Select(x => x.RoleId).FirstOrDefault();
                formModel.FactoryCircleRefId = (Int64) _context.UserCircleMappings.Where(x => x.UserRefId == formModel.SubmittedBy_UserRefId && x.Version == 2).Select(x => x.FactoryCircleRefId).FirstOrDefault();

                if (formModel.Id != 0) //Existing record
                {
                    _iGR_MPR_Factory.Update(formModel);
                    await _iGR_MPR_Factory.SavechangeAsync();

                }
                else //New record
                {
                    _iGR_MPR_Factory.Insert(formModel);
                    await _iGR_MPR_Factory.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> CheckDuplicateMonthYear(long id, int month, int year, Int64 factoryCircleRefId, string uid)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                if (factoryCircleRefId == 0)
                {
                    factoryCircleRefId = (Int64)_context.UserCircleMappings.Where(x => x.UserRefId == uid && x.Version == 2).Select(x => x.FactoryCircleRefId).FirstOrDefault();
                }

                var parentWithChildObject = await _iGR_MPR_Factory
                            .GetAsync(x => x.Month == month && x.Year == year && x.FactoryCircleRefId == factoryCircleRefId)      //Includes
                            .ConfigureAwait(false);
                
                genericFormModel.ResponseDataModel = true;


                if (parentWithChildObject.Count() == 0)
                {
                    genericFormModel.ResponseDataModel = false;
                }
                else if (parentWithChildObject.Count() > 0 && parentWithChildObject.FirstOrDefault().Id>0 && parentWithChildObject.FirstOrDefault().Id==id)
                {
                    genericFormModel.ResponseDataModel = false;
                }
                
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<MPR_Labour>>> GetMprLabourDetail(string userId, int month, int year)
        {
            var genericFormModel = new GenericResponseTemplateModel<List<MPR_Labour>>();
            try
            {
                var records = await _context.MPR_Labours
                    .Where(x => x.SubmittedBy_UserRefId == userId &&
                                x.Month == month &&
                                x.Year == year &&
                              ((!string.IsNullOrEmpty(x.JsonData)) || (x.StepCodes == "LOCK" && x.IsLocked == true)))
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

        public async Task<GenericServiceResultTemplate> InsertBlankJsonStepsAsync(InsertNullJson_ViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                var labourCircleRefId = (Int64)_context.UserCircleMappings
                    .Where(x => x.UserRefId == formModel.SubmittedBy_UserRefId && x.Version == 2)
                    .Select(x => x.LabourCircleRefId)
                    .FirstOrDefault();

                var roleId = _context.UserRoles
                    .Where(x => x.UserId == formModel.SubmittedBy_UserRefId)
                    .Select(x => x.RoleId)
                    .FirstOrDefault();

                foreach (var stepCode in formModel.StepCodes)
                {
                    bool exists = await _context.MPR_Labours.AnyAsync(x =>
                        x.LabourCircleRefId == labourCircleRefId &&
                        x.Month == formModel.Month &&
                        x.Year == formModel.Year &&
                        x.StepCodes == stepCode);

                    if (!exists)
                    {
                        var record = new MPR_Labour
                        {
                            LabourCircleRefId = labourCircleRefId,
                            Month = formModel.Month,
                            Year = formModel.Year,
                            StepCodes = stepCode,
                            JsonData = null,
                            SubmittedBy_UserRefId = formModel.SubmittedBy_UserRefId,
                            SubmittedBy_ProfileRefId = formModel.SubmittedBy_ProfileRefId,
                            SubmittedBy_RoleRefId = roleId,
                            CreatedOn = DateTime.Now
                        };

                        _context.MPR_Labours.Add(record);
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

        public async Task<GenericServiceResultTemplate> SaveStepDataAsync(SaveSteps_ViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                var labourCircleRefId = (Int64)_context.UserCircleMappings
                    .Where(x => x.UserRefId == formModel.SubmittedBy_UserRefId && x.Version == 2)
                    .Select(x => x.LabourCircleRefId)
                    .FirstOrDefault();

                var roleId = _context.UserRoles
                     .Where(x => x.UserId == formModel.SubmittedBy_UserRefId)
                     .Select(x => x.RoleId)
                     .FirstOrDefault();

                if (formModel.IsLocked == true)
                {
                    var records = _context.MPR_Labours.Where(x =>
                        x.LabourCircleRefId == labourCircleRefId &&
                        x.Month == formModel.Month &&
                        x.Year == formModel.Year);

                    foreach (var record in records)
                    {
                        record.IsLocked = true;
                    }

                    await _context.SaveChangesAsync();
                    return genericServiceResultTemplate;
                }


                var existing = await _context.MPR_Labours.FirstOrDefaultAsync(x =>
                    x.LabourCircleRefId == labourCircleRefId &&
                   x.Month == formModel.Month &&
                   x.Year == formModel.Year &&
                  x.StepCodes == formModel.StepCodes);

                if (existing != null)
                {
                    existing.JsonData = formModel.JsonData;
                    // existing.LastModifiedOn = DateTime.Now;
                    _context.MPR_Labours.Update(existing);
                }
                else
                {
                    var newRecord = new MPR_Labour
                    {
                        LabourCircleRefId = labourCircleRefId,
                        Month = formModel.Month,
                        Year = formModel.Year,
                        StepCodes = formModel.StepCodes,
                        JsonData = formModel.JsonData,
                        SubmittedBy_UserRefId = formModel.SubmittedBy_UserRefId,
                        SubmittedBy_RoleRefId = roleId,
                        CreatedOn = DateTime.Now
                    };

                    _context.MPR_Labours.Add(newRecord);
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


        public async Task<GenericResponseTemplateModel<List<MPR_Alc>>> GetMprAlcDetail(string userId, int month, int year)
        {
            var genericFormModel = new GenericResponseTemplateModel<List<MPR_Alc>>();
            try
            {
                var records = await _context.MPR_Alcs
                    .Where(x => x.SubmittedBy_UserRefId == userId &&
                                x.Month == month &&
                                x.Year == year &&
                              ((!string.IsNullOrEmpty(x.JsonData)) || (x.StepCodes == "LOCK" && x.IsLocked == true)))
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
        public async Task<GenericServiceResultTemplate> InsertBlankJsonStepsAlcAsync(InsertNullJsonAlc_ViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                var alcCircleRefId = (Int64)_context.UserCircleMappings
                    .Where(x => x.UserRefId == formModel.SubmittedBy_UserRefId && x.Version == 2)
                    .Select(x => x.AlcCircleRefId)
                    .FirstOrDefault();

                var roleId = _context.UserRoles
                    .Where(x => x.UserId == formModel.SubmittedBy_UserRefId)
                    .Select(x => x.RoleId)
                    .FirstOrDefault();

                foreach (var stepCode in formModel.StepCodes)
                {
                    bool exists = await _context.MPR_Alcs.AnyAsync(x =>
                        x.AlcCircleRefId == alcCircleRefId &&
                        x.Month == formModel.Month &&
                        x.Year == formModel.Year &&
                        x.StepCodes == stepCode);

                    if (!exists)
                    {
                        var record = new MPR_Alc
                        {
                            AlcCircleRefId = alcCircleRefId,
                            Month = formModel.Month,
                            Year = formModel.Year,
                            StepCodes = stepCode,
                            JsonData = null,
                            SubmittedBy_UserRefId = formModel.SubmittedBy_UserRefId,
                            SubmittedBy_ProfileRefId = formModel.SubmittedBy_ProfileRefId,
                            SubmittedBy_RoleRefId = roleId,
                            CreatedOn = DateTime.Now
                        };

                        _context.MPR_Alcs.Add(record);
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

        public async Task<GenericServiceResultTemplate> SaveStepDataAlcAsync(SaveStepsAlc_ViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                var alcCircleRefId = (Int64)_context.UserCircleMappings
                    .Where(x => x.UserRefId == formModel.SubmittedBy_UserRefId && x.Version == 2)
                    .Select(x => x.AlcCircleRefId)
                    .FirstOrDefault();

                var roleId = _context.UserRoles
                     .Where(x => x.UserId == formModel.SubmittedBy_UserRefId)
                     .Select(x => x.RoleId)
                     .FirstOrDefault();

                if (formModel.IsLocked == true)
                {
                    var records = _context.MPR_Alcs.Where(x =>
                        x.AlcCircleRefId == alcCircleRefId &&
                        x.Month == formModel.Month &&
                        x.Year == formModel.Year);

                    foreach (var record in records)
                    {
                        record.IsLocked = true;
                    }

                    await _context.SaveChangesAsync();
                    return genericServiceResultTemplate;
                }


                var existing = await _context.MPR_Alcs.FirstOrDefaultAsync(x =>
                    x.AlcCircleRefId == alcCircleRefId &&
                   x.Month == formModel.Month &&
                   x.Year == formModel.Year &&
                  x.StepCodes == formModel.StepCodes);

                if (existing != null)
                {
                    existing.JsonData = formModel.JsonData;
                    // existing.LastModifiedOn = DateTime.Now;
                    _context.MPR_Alcs.Update(existing);
                }
                else
                {
                    var newRecord = new MPR_Alc
                    {
                        AlcCircleRefId = alcCircleRefId,
                        Month = formModel.Month,
                        Year = formModel.Year,
                        StepCodes = formModel.StepCodes,
                        JsonData = formModel.JsonData,
                        SubmittedBy_UserRefId = formModel.SubmittedBy_UserRefId,
                        SubmittedBy_RoleRefId = roleId,
                        CreatedOn = DateTime.Now
                    };

                    _context.MPR_Alcs.Add(newRecord);
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

        public async Task<GenericResponseTemplateModel<List<MprDataViewModel>>> GetMprDataAppType(string userId, int year, int month, int applicationType)
        {
            GenericResponseTemplateModel<List<MprDataViewModel>> genericFormModel = new GenericResponseTemplateModel<List<MprDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
 {
     new StoreProcedureParm (){ ParmName="userId", ParmValue=userId, isNumber=false},
     new StoreProcedureParm (){ ParmName="applicationType", ParmValue=applicationType.ToString(), isNumber=true},
     new StoreProcedureParm (){ ParmName="month", ParmValue=month.ToString(), isNumber=true},
     new StoreProcedureParm (){ ParmName="year", ParmValue=year.ToString(), isNumber=true},
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MprDataViewModel>("dbo.sp_MPR_GetApplicationTypeData", storeProcedureParms);

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
