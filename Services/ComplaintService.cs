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
            GenericFormModel<List<Complaint_EmployerORContractorDetail>> genericFormModel =
                new GenericFormModel<List<Complaint_EmployerORContractorDetail>>();

            try
            {
                genericFormModel.FormModel = await _context.Complaint_EmployerORContractorDetails
                    .Where(x => x.AppRefId == id)
                    .ToListAsync();
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


                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SAMADHAN_COMPLAINTS, genericFormModel.FormModel?.Id ?? id, "EED");
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
                    SelectListTypeCode = "MaternityDischargeOptionEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MaternityDischargeOptionEnum>()
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
                        .FirstOrDefaultAsync(x => x.AppId == id);

                    if (application != null)
                    {
                        genericFormModel.FormModel = new ComplaintDetailViewModel
                        {
                            WorkerDetail = application.WorkerDetail,
                            Complaint_EmployerORContractorDetails = application.Complaint_EmployerORContractorDetails.ToList(),
                            Complaint_WorkplaceDetails = application.Complaint_WorkplaceDetails,
                            Complaint_EstablishmentDetails = application.Complaint_EstablishmentDetails,
                            Complaint_GratuityClaims = application.Complaint_GratuityClaims
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
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id,ApplicationTypeEnum.SAMADHAN_COMPLAINTS,id,"LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }
        #endregion

    }
}
