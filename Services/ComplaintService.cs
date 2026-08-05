using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class ComplaintService : IComplaintService
    {
        private readonly AppDbContext _context;
        private readonly IApplicationManagementService<WorkerDetail> _iApplicationMamnagementService;


        public ComplaintService(
        AppDbContext context,
        IApplicationManagementService<WorkerDetail> IApplicationMamnagementService)
        {
            _context = context;
            _iApplicationMamnagementService = IApplicationMamnagementService;
        }

        public async Task<GenericFormModel<object>> Get_ComplaintsCategories()
        {
            GenericFormModel<object> genericFormModel = new GenericFormModel<object>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                var parentWithChildObject = await _context.ComplaintsCategories.AsNoTracking().OrderBy(x => x.ComplaintCategoryType).ThenBy(x => x.Id).ToListAsync();
                genericFormModel.FormModel = parentWithChildObject;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        #region Worker Details
        public async Task<GenericFormModel<WorkerDetail>> GetWorkerDetails(long id, long projectSiteId)
        {
            GenericFormModel<WorkerDetail> genericFormModel = new GenericFormModel<WorkerDetail>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.WorkerDetails.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new WorkerDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Id : id), "WD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

        #region App Complaint Mapping
        public async Task<GenericServiceResultTemplate> CreateAppComplaintTypeMapping(AppComplaintTypeMapping requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                AppComplaintTypeMapping appComplaintTypeMapping = new AppComplaintTypeMapping()
                {
                    AppRefId = requestData.AppRefId,
                    ComplaintsCategoryRefId = requestData.ComplaintsCategoryRefId
                };

                await _context.AppComplaintTypeMappings.AddAsync(appComplaintTypeMapping);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }

            return genericServiceResultTemplate;
        }
        #endregion

        #region Get Employer OR Contractor Details
        public async Task<GenericFormModel<List<Complaint_EmployerORContractorDetail>>> Get_EmployerOrContractorDetails(long id)
        {
            GenericFormModel<List<Complaint_EmployerORContractorDetail>> genericFormModel = new GenericFormModel<List<Complaint_EmployerORContractorDetail>>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_EmployerORContractorDetails.AsNoTracking().Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();
                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_EmployerORContractorDetail>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "SamadhaanEstablishmentTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<SamadhaanEstablishmentTypeEnum>()
                });


                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id,ApplicationTypeEnum.SAMADHAN_COMPLAINTS,id,"EED");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        #endregion

        #region  Get WorkPlace
        public async Task<GenericFormModel<Complaint_WorkplaceDetail>> Get_WorkPlaceDetail(long id)
        {
            GenericFormModel<Complaint_WorkplaceDetail> genericFormModel =
                new GenericFormModel<Complaint_WorkplaceDetail>();

            try
            {
                genericFormModel.FormModel = await _context.Complaint_WorkplaceDetails
                    .Where(x => x.AppRefId == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        #endregion

        #region Get Establishment Details

        public async Task<GenericFormModel<Complaint_EstablishmentDetail>> Get_EstablishmentDetails(long id)
        {
            GenericFormModel<Complaint_EstablishmentDetail> genericFormModel = new GenericFormModel<Complaint_EstablishmentDetail>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_EstablishmentDetails.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_EstablishmentDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ComplaintCategoryTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ComplaintCategoryTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "WagePeriodtypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<WagePeriodtypeEnum>()
                });
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Get Gratuity claim details
        public async Task<GenericFormModel<Complaint_GratuityClaim>> Get_GratuityClaimDetails(long id)
        {
            GenericFormModel<Complaint_GratuityClaim> genericFormModel = new GenericFormModel<Complaint_GratuityClaim>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_GratuityClaims.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_GratuityClaim();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "GratuityClaimBasisTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<GratuityClaimBasisTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MaritalStatusTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MaritalStatusTypeEnum>()
                });


                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, genericFormModel.FormModel?.Id ?? id, "GC");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Get Maternity Benefits Complaint detail

        public async Task<GenericFormModel<Complaint_MaternityBenefitComplaint>> Get_MaternityBenefitsComplaintDetails(long id)
        {
            GenericFormModel<Complaint_MaternityBenefitComplaint> genericFormModel = new GenericFormModel<Complaint_MaternityBenefitComplaint>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_MaternityBenefitComplaints.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_MaternityBenefitComplaint();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MaternityDischargeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MaternityDischargeTypeEnum>()
                });

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, genericFormModel.FormModel?.Id ?? id, "MBC");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim under code on wages

        public async Task<GenericFormModel<Complaint_Claim_CodeOnWage>> Get_ClaimUnderCodeOnWagesDetails(long id)
        {
            GenericFormModel<Complaint_Claim_CodeOnWage> genericFormModel = new GenericFormModel<Complaint_Claim_CodeOnWage>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Claim_CodeOnWages.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Claim_CodeOnWage();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "AllowanceTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<AllowanceTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "PlaceOfWorkTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<PlaceOfWorkTypeEnum>()
                });

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, genericFormModel.FormModel?.Id ?? id, "CCOW");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Minimum wages not paid

        public async Task<GenericFormModel<Complaint_MinimumWage>> Get_MinimumWagesNotPaidDetails(long id)
        {
            GenericFormModel<Complaint_MinimumWage> genericFormModel = new GenericFormModel<Complaint_MinimumWage>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_MinimumWages.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_MinimumWage();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion


        #region Minimum wages not paid period amount 

        public async Task<GenericFormModel<List<Complaint_MinimumWagesPeriodAmt>>> Get_MinimumWagesNotPaidPeriodAmountDetails(long id)
        {
            GenericFormModel<List<Complaint_MinimumWagesPeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_MinimumWagesPeriodAmt>>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_MinimumWagesPeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();
                    //var Complaint_MinimumWagesNotPaidPeriodAmount = _context.Complaint_MinimumWagesNotPaidPeriodAmounts.Where(x => x.AppRefId == id).ToListAsync();


                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_MinimumWagesPeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim wages not paid for on weekly day of rest

        public async Task<GenericFormModel<Complaint_Wages_WkDay>> Get_WagesNotPaidWeekDayDetail(long id)
        {
            GenericFormModel<Complaint_Wages_WkDay> genericFormModel = new GenericFormModel<Complaint_Wages_WkDay>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_WkDays.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Wages_WkDay();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Complaint_Wages_WkDay_PeriodAmt>>> Get_WagesNotPaidWeekDayPeriodAmountDetails(long id)
        {
            GenericFormModel<List<Complaint_Wages_WkDay_PeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_Wages_WkDay_PeriodAmt>>(); try

            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_WkDay_PeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();
                    //var Complaint_MinimumWagesNotPaidPeriodAmount = _context.Complaint_MinimumWagesNotPaidPeriodAmounts.Where(x => x.AppRefId == id).ToListAsync();


                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_Wages_WkDay_PeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim wages not paid for working overtime

        public async Task<GenericFormModel<Complaint_Wages_OT>> Get_WagesWorkingOvertimeDetail(long id)
        {
            GenericFormModel<Complaint_Wages_OT> genericFormModel = new GenericFormModel<Complaint_Wages_OT>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_OTs.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Wages_OT();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Complaint_Wages_OT_PeriodAmt>>> Get_WagesWorkingOvertimePerAmtDetail(long id)
        {
            GenericFormModel<List<Complaint_Wages_OT_PeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_Wages_OT_PeriodAmt>>(); try

            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_OT_PeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();

                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_Wages_OT_PeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim wages not paid for all

        public async Task<GenericFormModel<Complaint_Wages_Not_Paid>> Get_WagesNotPaidDetail(long id)
        {
            GenericFormModel<Complaint_Wages_Not_Paid> genericFormModel = new GenericFormModel<Complaint_Wages_Not_Paid>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_Not_Paids.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Wages_Not_Paid();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Complaint_Wages_Not_Paid_PeriodAmt>>> Get_WagesNotPaidPerAmtDetail(long id)
        {
            GenericFormModel<List<Complaint_Wages_Not_Paid_PeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_Wages_Not_Paid_PeriodAmt>>(); try

            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_Not_Paid_PeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();

                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_Wages_Not_Paid_PeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim wages unauthorised deduction

        public async Task<GenericFormModel<Complaint_Wages_Unauth_Deduct>> Get_UnauthDeductWagesDetail(long id)
        {
            GenericFormModel<Complaint_Wages_Unauth_Deduct> genericFormModel = new GenericFormModel<Complaint_Wages_Unauth_Deduct>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_Unauth_Deducts.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Wages_Unauth_Deduct();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Complaint_Wages_Unauth_Deduct_PeriodAmt>>> Get_UnauthDeductWagesPerAmtDetail(long id)
        {
            GenericFormModel<List<Complaint_Wages_Unauth_Deduct_PeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_Wages_Unauth_Deduct_PeriodAmt>>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Wages_Unauth_Deduct_PeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();

                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_Wages_Unauth_Deduct_PeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Claim wages unauthorised deduction

        public async Task<GenericFormModel<Complaint_Non_Pay_Bonus>> Get_NonPayBonusDetail(long id)
        {
            GenericFormModel<Complaint_Non_Pay_Bonus> genericFormModel = new GenericFormModel<Complaint_Non_Pay_Bonus>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Non_Pay_Bonuses.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Non_Pay_Bonus();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Complaint_Non_Pay_Bonus_PeriodAmt>>> Get_NonPayBonusPerAmtDetail(long id)
        {
            GenericFormModel<List<Complaint_Non_Pay_Bonus_PeriodAmt>> genericFormModel = new GenericFormModel<List<Complaint_Non_Pay_Bonus_PeriodAmt>>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Non_Pay_Bonus_PeriodAmts.Where(x => x.AppRefId == id).Include(x => x.Application).ToListAsync();

                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_Non_Pay_Bonus_PeriodAmt>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BonusClaimTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BonusClaimTypeEnum>()
                });
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Recovery of code
        #region General Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_GeneralDetail>> GetComplaintRecOfMonGeneralDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_GeneralDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_GeneralDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_GeneralDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MoneyDueReasonTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MoneyDueReasonTypeEnum>()
                });
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "RM");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Get Recovery of Money Detail
        public async Task<GenericFormModel<List<Complaint_RecOfMon_MoneyDueDetail>>> GetComplaintRecOfMonDueDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<List<Complaint_RecOfMon_MoneyDueDetail>>();

            try
            {
                if (appRefId != 0) // Existing Record
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_MoneyDueDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).ToListAsync();


                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                    }
                }
                else // New Record
                {
                    genericFormModel.FormModel = new List<Complaint_RecOfMon_MoneyDueDetail>();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Money Due Detail

        //public async Task<GenericFormModel<Complaint_RecOfMon_MoneyDueDetail>> GetComplaintRecOfMonMoneyDueDetail(long appRefId)
        //{
        //    var genericFormModel = new GenericFormModel<Complaint_RecOfMon_MoneyDueDetail>();
        //    try
        //    {
        //        if (appRefId != 0)
        //        {
        //            var parentWithChildObject = await _context.Complaint_RecOfMon_MoneyDueDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

        //            if (parentWithChildObject != null)
        //            {
        //                genericFormModel.FormModel = parentWithChildObject;
        //                genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
        //                genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
        //                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
        //            }
        //        }
        //        else
        //        {
        //            genericFormModel.FormModel = new Complaint_RecOfMon_MoneyDueDetail();
        //            genericFormModel.IsEditAllowed = true;
        //            genericFormModel.IsLocked = false;
        //            genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
        //        }

        //        genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "RM");
        //    }
        //    catch (Exception ex)
        //    {
        //        genericFormModel.HasError = true;
        //        genericFormModel.ErrorDesc = ex.Message;
        //        throw;
        //    }

        //    return genericFormModel;
        //}

        #endregion

        #region Settlement Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_SettlementDetail>> GetComplaintRecOfMonSettlementDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_SettlementDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_SettlementDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_SettlementDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate
                {
                    SelectListTypeCode = "SettlementTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<SettlementTypeEnum>()
                });

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "SETL");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Award Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_AwardDetail>> GetComplaintRecOfMonAwardDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_AwardDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_AwardDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_AwardDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "AWRD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Notice Pay Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_NoticePayDetail>> GetComplaintRecOfMonNoticePayDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_NoticePayDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_NoticePayDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_NoticePayDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "NoticePayPeriodTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<NoticePayPeriodTypeEnum>()
                });

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "NOTP");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Retrenchment Compensation Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_RetrenchmentCompDetail>> GetComplaintRecOfMonRetrenchmentCompDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_RetrenchmentCompDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_RetrenchmentCompDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_RetrenchmentCompDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "RETR");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Lay-Off Detail

        public async Task<GenericFormModel<Complaint_RecOfMon_LayOffDetail>> GetComplaintRecOfMonLayOffDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<Complaint_RecOfMon_LayOffDetail>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_LayOffDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_RecOfMon_LayOffDetail();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "LOFF");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion

        #region Lay-Off Compensation Detail

        public async Task<GenericFormModel<List<Complaint_RecOfMon_LayOffCompDetail>>> GetComplaintRecOfMonLayOffCompDetail(long appRefId)
        {
            var genericFormModel = new GenericFormModel<List<Complaint_RecOfMon_LayOffCompDetail>>();
            try
            {
                if (appRefId != 0)
                {
                    var parentWithChildObject = await _context.Complaint_RecOfMon_LayOffCompDetails.AsNoTracking().Where(x => x.AppRefId == appRefId).Include(x => x.Application).ToListAsync();

                    if (parentWithChildObject != null && parentWithChildObject.Any())
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.First().Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.First().Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.First().Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new List<Complaint_RecOfMon_LayOffCompDetail>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, appRefId, "LOFC");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }

        #endregion
        #endregion

        #region Complaint details
        public async Task<GenericFormModel<ComplaintDetailViewModel>> Get_ComplaintDetail(long id)
        {
            GenericFormModel<ComplaintDetailViewModel> genericFormModel =
                new GenericFormModel<ComplaintDetailViewModel>();

            try
            {
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                if (id != 0)
                {
                    var application = await _context.Applications
                        .Include(x => x.WorkerDetail)
                        .Include(x => x.Complaint_EmployerORContractorDetails)
                        .Include(x => x.Complaint_WorkplaceDetails)
                        .Include(x => x.Complaint_EstablishmentDetails)
                        .Include(x => x.Complaint_GratuityClaims)
                        .Include(x => x.Complaint_Claim_CodeOnWages)
                        .Include(x => x.Complaint_MinimumWage)
                        .Include(x => x.Complaint_MinimumWage)
                        .Include(x => x.Complaint_MinimumWagesPeriodAmts)
                        .Include(x => x.Complaint_Wages_WkDay)
                        .Include(x => x.Complaint_Wages_WkDay_PeriodAmts)
                        .Include(x => x.Complaint_Wages_WkDay_PeriodAmts)
                        .Include(x => x.Complaint_Wages_OT)
                        .Include(x => x.Complaint_Wages_Not_Paid)
                        .Include(x => x.Complaint_Wages_Not_Paid_PeriodAmts)
                        .Include(x => x.Complaint_Wages_Unauth_Deduct)
                        .Include(x => x.Complaint_Wages_Unauth_Deduct_PeriodAmts)
                        .Include(x => x.Complaint_Non_Pay_Bonus)
                        .Include(x => x.Complaint_Non_Pay_Bonus_PeriodAmts)
                        .Include(x => x.Complaint_MaternityBenefitComplaints)
                        .FirstOrDefaultAsync(x => x.AppId == id);

                    if (application != null)
                    {
                        genericFormModel.FormModel = new ComplaintDetailViewModel
                        {
                            WorkerDetail = application.WorkerDetail,
                            Complaint_EmployerORContractorDetails = application.Complaint_EmployerORContractorDetails.ToList(),
                            Complaint_WorkplaceDetails = application.Complaint_WorkplaceDetails,
                            Complaint_EstablishmentDetails = application.Complaint_EstablishmentDetails,

                            Complaint_GratuityClaims = application.Complaint_GratuityClaims,
                            Complaint_Claim_CodeOnWages = application.Complaint_Claim_CodeOnWages,
                            Complaint_MinimumWage = application.Complaint_MinimumWage,
                            Complaint_MinimumWagesPeriodAmts = application.Complaint_MinimumWagesPeriodAmts,
                            Complaint_Wages_WkDay = application.Complaint_Wages_WkDay,
                            Complaint_Wages_WkDay_PeriodAmts = application.Complaint_Wages_WkDay_PeriodAmts,
                            Complaint_Wages_OT = application.Complaint_Wages_OT,
                            Complaint_Wages_OT_PeriodAmts = application.Complaint_Wages_OT_PeriodAmts,
                            Complaint_Wages_Not_Paid = application.Complaint_Wages_Not_Paid,
                            Complaint_Wages_Not_Paid_PeriodAmts = application.Complaint_Wages_Not_Paid_PeriodAmts,
                            Complaint_Wages_Unauth_Deduct = application.Complaint_Wages_Unauth_Deduct,
                            Complaint_Wages_Unauth_Deduct_PeriodAmts = application.Complaint_Wages_Unauth_Deduct_PeriodAmts,
                            Complaint_Non_Pay_Bonus = application.Complaint_Non_Pay_Bonus,
                            Complaint_Non_Pay_Bonus_PeriodAmts = application.Complaint_Non_Pay_Bonus_PeriodAmts,
                            Complaint_MaternityBenefitComplaints = application.Complaint_MaternityBenefitComplaints
                        };

                        genericFormModel.IsEditAllowed = application.IsAllowEdit;
                        genericFormModel.IsLocked = application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new ComplaintDetailViewModel();

                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "GratuityClaimBasisTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<GratuityClaimBasisTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MaritalStatusTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MaritalStatusTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "WorkerCategoryTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<WorkerCategoryTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MaternityDischargeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MaternityDischargeTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "AllowanceTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<AllowanceTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "PlaceOfWorkTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<PlaceOfWorkTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BonusClaimTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BonusClaimTypeEnum>()
                });
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, id, "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }
        #endregion

        #region Review Of Dismissal

        public async Task<GenericFormModel<Complaint_Review_OfDismissal>> Get_ReviewofDismissalDetail(long id)
        {
            var genericFormModel = new GenericFormModel<Complaint_Review_OfDismissal>();
            try
            {
                if (id != 0)
                {
                    var parentWithChildObject = await _context.Complaint_Review_OfDismissals.AsNoTracking().Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefaultAsync();

                    if (parentWithChildObject != null)
                    {
                        genericFormModel.FormModel = parentWithChildObject;
                        genericFormModel.IsEditAllowed = parentWithChildObject.Application.IsAllowEdit;
                        genericFormModel.IsLocked = parentWithChildObject.Application.IsLocked;
                        genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.Application.ApplicationLifeCycleStatusType;
                    }
                }
                else
                {
                    genericFormModel.FormModel = new Complaint_Review_OfDismissal();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, id, "ROD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw;
            }

            return genericFormModel;
        }
        #endregion

        #region Get Draft application

        public async Task<GenericResponseTemplateModel<List<Application>>> Get_ComplaintsDraftApplication()
        {
            GenericResponseTemplateModel<List<Application>> genericRespModel = new GenericResponseTemplateModel<List<Application>>();
            try
            {
                genericRespModel.ResponseDataModel = await _context.Applications.Where(x => !x.IsDeleted && !x.IsLocked).OrderByDescending(x => x.AppId).ToListAsync();
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }
        #endregion

        #region GET LOCK APPLICATION

        public async Task<GenericResponseTemplateModel<List<Application>>> Get_AllApplication()
        {
            GenericResponseTemplateModel<List<Application>> genericRespModel = new GenericResponseTemplateModel<List<Application>>();
            try
            {
                genericRespModel.ResponseDataModel = await _context.Applications.OrderByDescending(x => x.AppId).ToListAsync();
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }
        #endregion


    }
}
