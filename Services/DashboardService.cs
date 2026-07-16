
using EFCore.BulkExtensions;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Reflection.Emit;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Office2010.Excel;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel;
using System.Text;

namespace pbsamadhannetcoreapi.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly ISystem_O_CommunicationService _iSystem_O_CommunicationService;
        private IAuthService _iAuthService;
        private readonly IDapperRepository _iDapperRepository;
        private IServiceScopeFactory _iServiceScopeFactory;
        public DashboardService(IGeneric_SP_Repository iGeneric_SP_Repository, UserManager<User> userManager, AppDbContext context, ISystem_O_CommunicationService iSystem_O_CommunicationService, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService, IDapperRepository iDapperRepository, IServiceScopeFactory iServiceScopeFactory)
        {
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _userManager = userManager;
            _context = context;
            _iSystem_O_CommunicationService = iSystem_O_CommunicationService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
            _iDapperRepository = iDapperRepository;
            _iServiceScopeFactory = iServiceScopeFactory;
        }

        #region Dashboard Data
        public async Task<GenericFormModel<List<DashboardViewModel>>> GetAllProjectSiteInProcessApplications(Int64 ProjectSiteRefId)
        {
            GenericFormModel<List<DashboardViewModel>> genericFormModel = new GenericFormModel<List<DashboardViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=ProjectSiteRefId.ToString(), isNumber=false}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DashboardViewModel>("dbo.sp_Get_All_ProjectSite_InProcess_Applications", storeProcedureParms);

                if (genericFormModel.FormModel.Count() > 0)
                {
                    genericFormModel.FormModel.Select(
                     x => { x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType)); return x; }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Dashboard Data

        #region AlreadyClearance Details
        public async Task<GenericFormModel<ClearenceFileInfoViewModel>> GetAllProjectSiteAlreadyClearancesApplications(Int64 ProjectSiteRefId)
        {
            GenericFormModel<ClearenceFileInfoViewModel> genericFormModel = new GenericFormModel<ClearenceFileInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=ProjectSiteRefId.ToString(), isNumber=false}
                };

                genericFormModel.FormModel = new ClearenceFileInfoViewModel();
                genericFormModel.FormModel.AlreadyClearancesData = new List<DashboardAlreadyClearenceViewModel>();
                genericFormModel.FormModel.AlreadyClearancesData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DashboardAlreadyClearenceViewModel>("dbo.sp_Get_All_ProjectSite_AlreadyClearances_Applications", storeProcedureParms);

                genericFormModel.FormModel.LegacyApprovedClearenceMappings = new List<LegacyApprovedClearenceMapping>();
                genericFormModel.FormModel.LegacyApprovedClearenceMappings = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LegacyApprovedClearenceMapping>("dbo.sp_Get_All_AppIdsByProjectSiteRefId", storeProcedureParms);

                genericFormModel.FormModel.GetSysNClearancesIssueds = new List<GetSysNClearancesIssuedsViewModel>();
                genericFormModel.FormModel.GetSysNClearancesIssueds = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetSysNClearancesIssuedsViewModel>("dbo.sp_NTF_GetClearanceIssuedsDetails", storeProcedureParms);


                var licenceNumbers = string.Join(", ",genericFormModel.FormModel.AlreadyClearancesData.Select(x => x.LicenceNumber).Distinct());
                genericFormModel.FormModel.WelfareFundDetails = new List<WelfareFundStatusDetailsViewModel>();
                List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="licenceNumber", ParmValue=licenceNumbers.ToString(), isNumber=false}
                };
                genericFormModel.FormModel.WelfareFundDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WelfareFundStatusDetailsViewModel>("dbo.sp_GetWelfareFundDetailsByLicenceNumber", storeProcedureParms2);


                var legacyAppIds = string.Join(", ", genericFormModel.FormModel.WelfareFundDetails.Select(x => x.AppId).Distinct());
                genericFormModel.FormModel.WelfareSchemesDetails =  new List<WelfareSchemesDetailsViewModel>();
                List<StoreProcedureParm> storeProcedureParms3 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="legacyAppId", ParmValue=legacyAppIds.ToString(), isNumber=false}
                };
                genericFormModel.FormModel.WelfareSchemesDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WelfareSchemesDetailsViewModel>("dbo.sp_GetWelfareSchemesDetails", storeProcedureParms3);


                List<StoreProcedureParm> storeProcedureParms4 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="licenceNumber", ParmValue=licenceNumbers.ToString(), isNumber=false}
                };
                genericFormModel.FormModel.InspectionsDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionDetailsBylicenceViewModel>("dbo.sp_GetInspectionDetailsByLicenceNumber", storeProcedureParms4);

               
                genericFormModel.FormModel.UnpaidWagesDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<UnpaidWagesDetailsBylicenceViewModel>("dbo.sp_GetUnpaidWagesDetailsByLicenceNumber", storeProcedureParms4);

               
            }
            catch (Exception ex)
            {   
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }   
        #endregion AlreadyClearance Details

        public async Task<GenericFormModel<OfficialDashboardContentViewModel>> LoadRecordsTypesMenusCounts(string id, DashboardRecordsHeaderTypeEnum DashboardRecordsHeaderType, ApplicationTypeEnum ApplicationType, bool applicationListAlso, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<OfficialDashboardContentViewModel> genericFormModel = new GenericFormModel<OfficialDashboardContentViewModel>();
            genericFormModel.FormModel = new OfficialDashboardContentViewModel();
            Int32 AppType = 0;
            var defaultRecordHeadTypes = new List<RecordTypeMenuCountViewModel>()
            {
                new RecordTypeMenuCountViewModel (){ HeadCode="1", RecordHead="Inbox"},
                //new RecordTypeMenuCountViewModel (){ HeadCode="2", RecordHead="Recently Sent" },
                new RecordTypeMenuCountViewModel (){ HeadCode="3", RecordHead="In-Process" },
                new RecordTypeMenuCountViewModel (){ HeadCode="4", RecordHead="Approved" },
                new RecordTypeMenuCountViewModel (){ HeadCode="5", RecordHead="Rejected" },
                new RecordTypeMenuCountViewModel (){ HeadCode="6", RecordHead="Deemed" },
                new RecordTypeMenuCountViewModel (){ HeadCode="8", RecordHead="Dormant" },
                new RecordTypeMenuCountViewModel (){ HeadCode="9", RecordHead="Withdrawn" }
                //new RecordTypeMenuCountViewModel (){ HeadCode="9", RecordHead="Deemed (Fee Pending)" },
            };
            try
            {
                var user = await _userManager.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).FirstOrDefaultAsync(x => x.Id == id); // .FindByNameAsync(loginViewModel.UserName);
                var rolenames = await _userManager.GetRolesAsync(user);

                if ((rolenames.Contains("ADRF") || rolenames.Contains("DDRF")) && ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    defaultRecordHeadTypes.Add(new RecordTypeMenuCountViewModel() { HeadCode = "7", RecordHead = "At Dealing Hand" });
                }

                if ((rolenames.Contains("ADDF")) && ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    defaultRecordHeadTypes.Remove(new RecordTypeMenuCountViewModel() { HeadCode = "8", RecordHead = "Dormant" });
                }

                //if (rolenames.Contains("ADDF"))
                //{
                //    defaultRecordHeadTypes.Add(new RecordTypeMenuCountViewModel() { HeadCode = "9", RecordHead = "Deemed (Fee Pending)" });
                //}

                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id, isNumber=false}
                };

                genericFormModel.FormModel.RecordTypeMenuCount = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordTypeMenuCountViewModel>("sp_Dashboard_RecordTypeMenu_Counts", storeProcedureParms);

                foreach (var defaultRecordHead in defaultRecordHeadTypes)
                {
                    if (genericFormModel.FormModel.RecordTypeMenuCount.Count(x => x.HeadCode == defaultRecordHead.HeadCode) > 0) { }
                    else
                    {
                        genericFormModel.FormModel.RecordTypeMenuCount.Add(defaultRecordHead);
                    }
                }
                genericFormModel.FormModel.RecordTypeMenuCount = genericFormModel.FormModel.RecordTypeMenuCount.OrderBy(x => x.HeadCode).ToList();

                List<ApplicationTypeEnum> defaultApplicationTypes = new List<ApplicationTypeEnum>();

                if (rolenames.Contains("DLHF") || rolenames.Contains("DDRF") || rolenames.Contains("ADRF") || rolenames.Contains("ADDF") || rolenames.Contains("LBCR"))
                {
                    //defaultApplicationTypes.Add(ApplicationTypeEnum.REG_ESTB);
                    //defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN);
                    //defaultApplicationTypes.Add(ApplicationTypeEnum.COMMON_LICENCE);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.FACTORY_LICENCE);
           
                }
                if (rolenames.Contains("DLHL") || rolenames.Contains("ALLC"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.PRINCIPAL_EMPLOYER);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.CONTRACT_LABOUR);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.ISM_CONTRACT_LABOUR);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.MOTOR_TRANSPORT);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.TRADE_UNION);
      
                }
                if (rolenames.Contains("JDRF") || rolenames.Contains("DTP") || rolenames.Contains("ATP") || rolenames.Contains("ADDF") || rolenames.Contains("LBCR") || rolenames.Contains("ARHQ") || rolenames.Contains("DLBP"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN_HUD);
                }
                if (rolenames.Contains("LBIN"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.SHOP_LICENCE);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.TRADE_UNION);
                }
                if (rolenames.Contains("LBCR") || rolenames.Contains("DPLC"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP);
                }
                if (rolenames.Contains("ADDF") || rolenames.Contains("LBCR"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY);
                }
                if (rolenames.Contains("DPLC") || rolenames.Contains("LBCR"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.PRINCIPAL_EMPLOYER);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.CONTRACT_LABOUR);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.TRADE_UNION);
                }
                if (rolenames.Contains("DLTU") || rolenames.Contains("SOTU") || rolenames.Contains("SPTU") || rolenames.Contains("ADLC"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.TRADE_UNION);
                }
                if (rolenames.Contains("DLHF") || rolenames.Contains("DDRF") || rolenames.Contains("ADRF") || rolenames.Contains("ADDF") || rolenames.Contains("LBCR") || rolenames.Contains("DPLC"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT);
                }
                if (rolenames.Contains("DHLP") || rolenames.Contains("ADLC")) // Whole punjab applications of contract labour
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.CONTRACT_LABOUR);
                }

                //if (rolenames.Contains("JDRF"))
                //{
                //    defaultApplicationTypes.Add(ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE);
                //}

                // Menu tabs For Proposed Building Plan 
                if (rolenames.Contains("ADDF") || rolenames.Contains("DDRF") || rolenames.Contains("ADRF") || rolenames.Contains("LBCR") || rolenames.Contains("DLBP"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN_PROPOSED);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN_EXISTING);
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT);
                }

                if (rolenames.Contains("JDRF_PSIEC") || rolenames.Contains("DL_PSIEC") || rolenames.Contains("CGM") || rolenames.Contains("JDM") || rolenames.Contains("SDO") || rolenames.Contains("DRFT") || rolenames.Contains("EO") || rolenames.Contains("ADDF") || rolenames.Contains("LBCR") || rolenames.Contains("ATP_PSIEC"))
                {
                    defaultApplicationTypes.Add(ApplicationTypeEnum.BUILDING_PLAN_PSIEC);
                }

                List<RecordTypeMenuCountViewModel> RecordTypeMenuCount = new List<RecordTypeMenuCountViewModel>();

                if (genericFormModel.FormModel.RecordTypeMenuCount.Count > 0)
                {
                    var distinctRecordsType = genericFormModel.FormModel.RecordTypeMenuCount.Select(x => new { x.HeadCode, x.RecordHead }).Distinct().ToList();
                    foreach (var recordType in distinctRecordsType)
                    {
                        foreach (var item in defaultApplicationTypes)
                        {
                            RecordTypeMenuCount.Add(new RecordTypeMenuCountViewModel()
                            {
                                ApplicationType = (int)item,
                                ApplicationTypeDesc = "",
                                HeadCode = recordType.HeadCode,
                                RecordCount = genericFormModel.FormModel.RecordTypeMenuCount.Where(x => x.RecordHead == recordType.RecordHead && x.ApplicationType == (int)item).Select(x => x.RecordCount).FirstOrDefault(),
                                RecordHead = recordType.RecordHead
                            });
                        }
                    }
                }
                else
                {
                    foreach (var headType in defaultRecordHeadTypes)
                    {
                        foreach (var item in defaultApplicationTypes)
                        {
                            RecordTypeMenuCount.Add(new RecordTypeMenuCountViewModel()
                            {
                                ApplicationType = (int)item,
                                ApplicationTypeDesc = "",
                                HeadCode = headType.HeadCode,
                                RecordCount = 0,
                                RecordHead = headType.RecordHead
                            });
                        }
                    }
                }

                genericFormModel.FormModel.RecordTypeMenuCount = RecordTypeMenuCount;
                // Update ApplicationTypeEnum Description
                genericFormModel.FormModel.RecordTypeMenuCount.Select(
                     x => { 
                         x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         x.ApplicationTypeShortDesc = EnumOps.GetEnumShortDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         return x; }).ToList();

                if (ApplicationType == ApplicationTypeEnum.DEFAULT && genericFormModel.FormModel.RecordTypeMenuCount.Count > 0)
                {
                    AppType = Convert.ToInt32(defaultApplicationTypes.FirstOrDefault());
                }
                else
                {
                    AppType = Convert.ToInt32(EnumOps.GetEnumValue<ApplicationTypeEnum>(ApplicationType.ToString()));
                }
                genericFormModel.FormModel.ApplicationType = (ApplicationTypeEnum) AppType;
                
                if (applicationListAlso)
                {
                    if (ApplicationType == ApplicationTypeEnum.DEFAULT && genericFormModel.FormModel.RecordTypeMenuCount.Count > 0)
                    {
                        AppType = Convert.ToInt32(defaultApplicationTypes.FirstOrDefault());
                    }
                    else
                    {
                        AppType = Convert.ToInt32(EnumOps.GetEnumValue<ApplicationTypeEnum>(ApplicationType.ToString()));
                    }

                    if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.INBOX)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=string.Empty, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="0", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.SENT)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=string.Empty, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="2", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.IN_PROCESS)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="3", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.APPROVED)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="200", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.REJECTED)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="201", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.DEEMED)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="206", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.PENDING_AT_DEALING_HAND)
                    {
                        var receiverUserRefId = "";
                        List<StoreProcedureParm> procedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="RoleName", ParmValue="DLHF", isNumber=false},
                            new StoreProcedureParm (){ ParmName="CircleType", ParmValue="FACT", isNumber=false},
                            new StoreProcedureParm (){ ParmName="ParentUserRefId", ParmValue=id, isNumber=false},
                        };
                        var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FindUserOfSameCircleViewModel>("dbo.sp_FindUserOfSameCircle", procedureParms);
                        receiverUserRefId = resp[0].UserRefId;

                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=string.Empty, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue= receiverUserRefId.ToString(), isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="0", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }
                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.DORMANT)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="411", isNumber=true},
                            new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                            new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }

                    else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.WITHDRAWN)
                    {
                        storeProcedureParms = new List<StoreProcedureParm>()
                        {
                             new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                             new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                             new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                             new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="210", isNumber=true},
                             new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                             new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                             new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                             new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                             new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                             new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                        };
                    }


                    //else if (DashboardRecordsHeaderType == DashboardRecordsHeaderTypeEnum.DEEMED_WITH_FEE_PENDING)
                    //{
                    //    storeProcedureParms = new List<StoreProcedureParm>()
                    //    {
                    //        new StoreProcedureParm (){ ParmName="SenderUserRefId", ParmValue=id, isNumber=false},
                    //        new StoreProcedureParm (){ ParmName="ReceiverUserRefId", ParmValue=id, isNumber=false},
                    //        new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=AppType.ToString(), isNumber=true},
                    //        new StoreProcedureParm (){ ParmName="AppActionType", ParmValue="214", isNumber=true},
                    //        new StoreProcedureParm (){ ParmName="IsCountOnly", ParmValue="0", isNumber=true},

                    //        new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    //        new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    //        new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    //        new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    //        new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                    //    };
                    //}


                    genericFormModel.FormModel.ApplicationType = (ApplicationTypeEnum) AppType;
                    genericFormModel.FormModel.RecordsTypeList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("sp_Dashboard_RecordsOrCount_AppTypes", storeProcedureParms);
                    genericFormModel.FormModel.RecordsTypeList = genericFormModel.FormModel.RecordsTypeList.Select(x => { x.IsTimeLineElapsed = x.ActionCanTakenUpto == null ? false : DateTime.Now > x.ActionCanTakenUpto; return x; }).ToList();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        #region Application Logs Data
        public async Task<GenericListModel<ApplicationLogsViewModel>> GetApplicationsLogsData(Int64 appRefId, string userId)
        {
            GenericListModel<ApplicationLogsViewModel> genericListModel = new GenericListModel<ApplicationLogsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userId.ToString(), isNumber=false}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationLogsViewModel>("sp_Get_ApplicationsLogs", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        #endregion Application Logs Data

        #region PSL Report Building Plan HUD
        public async Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_BP_HUD(PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PivotColDetailViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate, isNumber=false}
                };
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSLDashboardMajorCountBP_HUD_ViewModel>("dbo.sp_Dashboard_PSL_MajorCounts_BP_HUD", storeProcedureParms);

                List<PivotColDetailViewModel> pivotCols = new List<PivotColDetailViewModel>();
                foreach (var item in data.Select(x => x.CountType).Distinct().ToList())
                {
                    pivotCols.Add(new PivotColDetailViewModel()
                    {
                        ColCode = item,
                        ColName = data.Where(x => x.CountType == item).Select(x => x.ColumnTitle).FirstOrDefault(),
                        RoleDetailList = new List<RoleDetailViewModel>(),

                    });
                }

                foreach (var item in pivotCols.Select(x => x.ColCode))
                {
                    var roleItems = new List<RoleDetailViewModel>();
                    foreach (var d in data.Where(x => x.CountType == item))
                    {
                        roleItems.Add(new RoleDetailViewModel() { Counts = d.Counts, RoleCode = d.RoleCode, RoleName = d.RoleDesc, RoleId = d.RoleId, AppActionType = d.AppActionType, ApplicationLifeCycleStatusType = d.ApplicationLifeCycleStatusType });
                    }

                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].RoleDetailList = roleItems;
                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].ColSpan = roleItems.Count();
                }

                genericServiceResultTemplate.ResponseDataModel = pivotCols;
                //data.Select(x => x.CountType).Distinct().ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_BP_HUD(string colCode, string roleId, int applicationLifeCycleStatusType, int appActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RecordType", ParmValue=colCode, isNumber=false},
                    new StoreProcedureParm (){ ParmName="RoleId", ParmValue=roleId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationLifeCycleStatusType", ParmValue= applicationLifeCycleStatusType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AppActionType", ParmValue=appActionType.ToString(), isNumber=true},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_Dashboard_PSL_Apps_BP_HUD", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<Deemed_ProcessFilesLog>> GetDeemedCalculationDetail(Int64 id)
        {
            GenericResponseTemplateModel<Deemed_ProcessFilesLog> genericServiceResultTemplate = new GenericResponseTemplateModel<Deemed_ProcessFilesLog>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.Deemed_ProcessFilesLogs.Where(x => x.AppRefId == id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericListModel<Deemed_ProcessFilesLogViewModel>> GetDeemedAllActCalculationDetail(Int64 id)
        {
            GenericListModel<Deemed_ProcessFilesLogViewModel> genericListModel = new GenericListModel<Deemed_ProcessFilesLogViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=id.ToString(), isNumber=false}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Deemed_ProcessFilesLogViewModel>("sp_Get_ApplicationsDeemedTimeline", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        #endregion

        public async Task<GenericResponseTemplateModel<List<PaymentDetailsListViewModel>>> GetPaymentDetails(int applicationType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<PaymentDetailsListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PaymentDetailsListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PaymentDetailsListViewModel>("dbo.sp_Dashboard_GetApplicationPaymentDetails", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        #region PSL Report Factory
        public async Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_Factory(PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PivotColDetailViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate, isNumber=false}
                };
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSLDashboardMajorCountBP_HUD_ViewModel>("dbo.sp_Dashboard_PSL_MajorCounts_Factory", storeProcedureParms);

                List<PivotColDetailViewModel> pivotCols = new List<PivotColDetailViewModel>();
                foreach (var item in data.Select(x => x.CountType).Distinct().ToList())
                {
                    pivotCols.Add(new PivotColDetailViewModel()
                    {
                        ColCode = item,
                        ColName = data.Where(x => x.CountType == item).Select(x => x.ColumnTitle).FirstOrDefault(),
                        RoleDetailList = new List<RoleDetailViewModel>(),

                    });
                }

                foreach (var item in pivotCols.Select(x => x.ColCode))
                {
                    var roleItems = new List<RoleDetailViewModel>();
                    foreach (var d in data.Where(x => x.CountType == item))
                    {
                        roleItems.Add(new RoleDetailViewModel() { Counts = d.Counts, RoleCode = d.RoleCode, RoleName = d.RoleDesc, RoleId = d.RoleId, AppActionType = d.AppActionType, ApplicationLifeCycleStatusType = d.ApplicationLifeCycleStatusType });
                    }

                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].RoleDetailList = roleItems;
                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].ColSpan = roleItems.Count();
                }

                genericServiceResultTemplate.ResponseDataModel = pivotCols;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_Factory(string colCode, string roleId, int applicationLifeCycleStatusType, int appActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RecordType", ParmValue=colCode, isNumber=false},
                    new StoreProcedureParm (){ ParmName="RoleId", ParmValue=roleId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationLifeCycleStatusType", ParmValue= applicationLifeCycleStatusType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AppActionType", ParmValue=appActionType.ToString(), isNumber=true},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_Dashboard_PSL_Apps_Factory", storeProcedureParms);


                var groupedResponse = genericServiceResultTemplate.ResponseDataModel
                                    .GroupBy(record => record.CurrentPendingWith)
                                    .Select(group => new
                                    {
                                        CurrentPendingWith = group.Key,
                                        Records = group.ToList()
                                    }).ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region PSL Report Shop
        public async Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_Shop(PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PivotColDetailViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate, isNumber=false}
                };
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSLDashboardMajorCountBP_HUD_ViewModel>("dbo.sp_Dashboard_PSL_MajorCounts_Shop", storeProcedureParms);

                List<PivotColDetailViewModel> pivotCols = new List<PivotColDetailViewModel>();
                foreach (var item in data.Select(x => x.CountType).Distinct().ToList())
                {
                    pivotCols.Add(new PivotColDetailViewModel()
                    {
                        ColCode = item,
                        ColName = data.Where(x => x.CountType == item).Select(x => x.ColumnTitle).FirstOrDefault(),
                        RoleDetailList = new List<RoleDetailViewModel>(),

                    });
                }

                foreach (var item in pivotCols.Select(x => x.ColCode))
                {
                    var roleItems = new List<RoleDetailViewModel>();
                    foreach (var d in data.Where(x => x.CountType == item))
                    {
                        roleItems.Add(new RoleDetailViewModel() { Counts = d.Counts, RoleCode = d.RoleCode, RoleName = d.RoleDesc, RoleId = d.RoleId, AppActionType = d.AppActionType, ApplicationLifeCycleStatusType = d.ApplicationLifeCycleStatusType });
                    }

                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].RoleDetailList = roleItems;
                    pivotCols[pivotCols.FindIndex(x => x.ColCode == item)].ColSpan = roleItems.Count();
                }

                genericServiceResultTemplate.ResponseDataModel = pivotCols;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_Shop(string colCode, string roleId, int applicationLifeCycleStatusType, int appActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RecordType", ParmValue=colCode, isNumber=false},
                    new StoreProcedureParm (){ ParmName="RoleId", ParmValue=roleId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationLifeCycleStatusType", ParmValue= applicationLifeCycleStatusType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AppActionType", ParmValue=appActionType.ToString(), isNumber=true},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_Dashboard_PSL_Apps_Shop", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region Other act clearance
        public async Task<GenericResponseTemplateModel<List<OtherActClearancesDataViewModel>>> GetOtherActClearances(string UserRefId, int applicationType)
        {
            GenericResponseTemplateModel<List<OtherActClearancesDataViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<OtherActClearancesDataViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=UserRefId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<OtherActClearancesDataViewModel>("dbo.sp_GetOtherActClearances", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region PSL Dashboard Act-Wise
        public async Task<GenericResponseTemplateModel<List<PSLDashboardActWiseCountViewModel>>> GetPSLDashboard_ActWiseCount(PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PSLDashboardActWiseCountViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PSLDashboardActWiseCountViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate, isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSLDashboardActWiseCountViewModel>("dbo.sp_Dashboard_PSL_ActWiseData", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region PSL Dashboard Circle-Wise
        public async Task<GenericResponseTemplateModel<List<PSLDashboardCircleWiseCountViewModel>>> GetPSLDashboard_CircleWiseCount(string fromDate, string toDate, int applicationType)
        {
            GenericResponseTemplateModel<List<PSLDashboardCircleWiseCountViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PSLDashboardCircleWiseCountViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=fromDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=toDate, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSLDashboardCircleWiseCountViewModel>("dbo.sp_Dashboard_PSL_CircleWiseCount", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region PSL Dashboard Officer-Wise
        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetPSLDashboard_OfficerWiseCount(string colCode, Int64 circleRefId, Int64 applicationType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RecordType", ParmValue=colCode, isNumber=false},
                    new StoreProcedureParm (){ ParmName="CircleRefId", ParmValue=circleRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_Dashboard_OfficerWiseCount", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region Factory Backlog
        public async Task<GenericFormModel<List<FactoryBacklogCountViewModel>>> FactoryBacklogCount(string id)
        {
            GenericFormModel<List<FactoryBacklogCountViewModel>> genericFormModel = new GenericFormModel<List<FactoryBacklogCountViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "UserId", ParmValue = id.ToString(), isNumber = false },

                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FactoryBacklogCountViewModel>("sp_Dashboard_FactoryBacklogcounts", storeProcedureParms);
                genericFormModel.FormModel = new List<FactoryBacklogCountViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericFormModel<List<FactoryBacklogExcelDataViewModel>>> FactoryBacklogData(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<List<FactoryBacklogExcelDataViewModel>> genericFormModel = new GenericFormModel<List<FactoryBacklogExcelDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "UserId", ParmValue = id.ToString(), isNumber = false },

                       new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FactoryBacklogExcelDataViewModel>("sp_Dashboard_Factory_Backlog", storeProcedureParms);

                //var kk = emps.Where(x => x.ClearanceIssuedOn != null).ToList();
                //emps.Where(x => x.ClearanceIssuedOn != null).Select(x => { x.ClearanceIssuedOn = "HelloS"; return x;});


                genericFormModel.FormModel = new List<FactoryBacklogExcelDataViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_FactoryBacklogDetail(FactoryBacklogDataRequestViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                var profileid = _context.UserProfileMapping.Where(x => x.UserRefId == requestData.Id && x.IsActive == true).FirstOrDefault();
                int? alreadyIterationCount = await _context.AuditLog_Factory_Backlogs.Where(x => x.AppRefId == requestData.AppId).MaxAsync(x => (int?)x.IterationCounter);

                int iterationCounter = alreadyIterationCount.HasValue ? alreadyIterationCount.Value + 1 : 1;

                if (requestData.FactoryHazardousCategoryType_original != requestData.FactoryHazardousCategoryType)
                {
                    AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                    {
                        AppRefId = requestData.AppId,
                        NAR = requestData.NAR,
                        AppFormId = Convert.ToInt32(requestData.AppFormId),
                        IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                        IterationCounter = iterationCounter,
                        FieldName = "FactoryHazardousCategoryType",
                        OldValue = Convert.ToString(requestData.FactoryHazardousCategoryType_original),
                        NewValue = Convert.ToString(requestData.FactoryHazardousCategoryType),
                        CreatedOn = DateTime.Now,
                        UserRefId = requestData.Id,
                        ProfileRefId = profileid.UserProfileRefId,
                        Remarks = requestData.remarks

                    };
                    await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                    await _context.SaveChangesAsync();
                }

                if (requestData.FactorySessionCategoryType_original != requestData.FactorySessionCategoryType)
                {
                    AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                    {
                        AppRefId = requestData.AppId,
                        NAR = requestData.NAR,
                        AppFormId = Convert.ToInt32(requestData.AppFormId),
                        IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                        IterationCounter = iterationCounter,
                        FieldName = "FactorySessionCategoryType",
                        OldValue = Convert.ToString(requestData.FactorySessionCategoryType_original),
                        NewValue = Convert.ToString(requestData.FactorySessionCategoryType),
                        CreatedOn = DateTime.Now,
                        UserRefId = requestData.Id,
                        ProfileRefId = profileid.UserProfileRefId,
                        Remarks = requestData.remarks

                    };
                    await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                    await _context.SaveChangesAsync();
                }

                if (requestData.FactorySectionCategoryType_NVarChar_original != requestData.FactorySectionCategoryType_NVarChar)
                {
                    AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                    {
                        AppRefId = requestData.AppId,
                        NAR = requestData.NAR,
                        AppFormId = Convert.ToInt32(requestData.AppFormId),
                        IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                        IterationCounter = iterationCounter,
                        FieldName = "FactorySectionCategoryType",
                        OldValue = (requestData.FactorySectionCategoryType_NVarChar_original),
                        NewValue = (requestData.FactorySectionCategoryType_NVarChar),
                        CreatedOn = DateTime.Now,
                        UserRefId = requestData.Id,
                        ProfileRefId = profileid.UserProfileRefId,
                        Remarks = requestData.remarks
                    };
                    await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                    await _context.SaveChangesAsync();
                }

                if (requestData.FactoryCategoryType_original != requestData.FactoryCategoryType)
                {
                    AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                    {
                        AppRefId = requestData.AppId,
                        NAR = requestData.NAR,
                        AppFormId = Convert.ToInt32(requestData.AppFormId),
                        IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                        IterationCounter = iterationCounter,
                        FieldName = "FactoryCategoryType",
                        OldValue = Convert.ToString(requestData.FactoryCategoryType_original),
                        NewValue = Convert.ToString(requestData.FactoryCategoryType),
                        CreatedOn = DateTime.Now,
                        UserRefId = requestData.Id,
                        ProfileRefId = profileid.UserProfileRefId,
                        Remarks = requestData.remarks

                    };
                    await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                    await _context.SaveChangesAsync();
                }
                if (requestData.FactoryOrganisationCategoryType != requestData.FactoryOrganisationCategoryType_original)
                {
                    AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                    {
                        AppRefId = requestData.AppId,
                        NAR = requestData.NAR,
                        AppFormId = Convert.ToInt32(requestData.AppFormId),
                        IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                        IterationCounter = iterationCounter,
                        FieldName = "OrganisationType",
                        OldValue = Convert.ToString(requestData.FactoryOrganisationCategoryType_original),
                        NewValue = Convert.ToString(requestData.FactoryOrganisationCategoryType),
                        CreatedOn = DateTime.Now,
                        UserRefId = requestData.Id,
                        ProfileRefId = profileid.UserProfileRefId,
                        Remarks = requestData.remarks

                    };
                    await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                    await _context.SaveChangesAsync();
                }
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "AppId", ParmValue = requestData.AppId.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "ProjectSiteId", ParmValue = requestData.ProjectSiteRefId.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "islegacy", ParmValue = requestData.IsLegacy.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "FactoryHazardousCategoryType", ParmValue = requestData.FactoryHazardousCategoryType.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "FactorySessionCategoryType", ParmValue = requestData.FactorySessionCategoryType.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "FactorySectionCategoryType_NVarChar", ParmValue = requestData.FactorySectionCategoryType_NVarChar, isNumber = true },
                       new StoreProcedureParm() { ParmName = "FactoryCategoryType", ParmValue = requestData.FactoryCategoryType.ToString(), isNumber = true },
                   };

                var result = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_Dashboard_Update_FactoryBacklogDetails", storeProcedureParms);
                int emps = result.Value;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }

            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> DeRegister_FactoryByBacklog(DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                byte[] bytes = Convert.FromBase64String(deRegsiterFactoryRequest.base64);

                string filename = $"FL{DateTime.Now:yyyyMMddHHmmssfff}.pdf";
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + filename;

                File.WriteAllBytes(SaveDirectoryPath, bytes);

                var profileid = _context.UserProfileMapping.Where(x => x.UserRefId == deRegsiterFactoryRequest.id && x.IsActive == true).FirstOrDefault();
                int? alreadyIterationCount = await _context.AuditLog_Factory_Backlogs.Where(x => x.AppRefId == deRegsiterFactoryRequest.appId).MaxAsync(x => (int?)x.IterationCounter);

                int iterationCounter = alreadyIterationCount.HasValue ? alreadyIterationCount.Value + 1 : 1;
                if (deRegsiterFactoryRequest.IsLegacy == 1)
                {
                    var Sys_O_AppId_TokenInfo = await _iSystem_O_CommunicationService.DeRegisterFactoryByFactoryBacklog(deRegsiterFactoryRequest, filename);
                }
                else
                {
                    // update application life cycle
                    //var appDetails = _context.Applications.Where(x => x.AppId == deRegsiterFactoryRequest.appId).FirstOrDefault();
                    var appDetails = _context.Applications.Where(x => x.ProjectSiteRefId == deRegsiterFactoryRequest.ProjectSiteRefId && x.IsDeleted == false).FirstOrDefault();
                    appDetails.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.DEREGISTERED;
                    appDetails.ApplicationLifeCycleLastStatusOn = DateTime.Now;
                    _context.Update<Application>(appDetails);

                    // update application action details
                    var appActionDetails = _context.ApplicationActions.Where(x => x.ApplicationRefId == deRegsiterFactoryRequest.appId).FirstOrDefault();
                    appActionDetails.AppActionType = Convert.ToInt32(AppActionTypeEnum.DEREGISTERED);
                    appActionDetails.ActionDate = DateTime.Now;
                    appActionDetails.Sender_UserRefId = deRegsiterFactoryRequest.id;
                    appActionDetails.Receiver_UserRefId = deRegsiterFactoryRequest.applicantuserid;
                    appActionDetails.Sender_ProfileRefId = profileid.UserProfileRefId;
                    appActionDetails.Receiver_ProfileRefId = deRegsiterFactoryRequest.applicantprofileid;
                    appActionDetails.SenderRoleId = "";
                    appActionDetails.ReceiverRoleId = "";
                    _context.Update<Application>(appDetails);

                    ApplicationActionLog applicationActionLog = new ApplicationActionLog
                    {
                        ActionDate = DateTime.Today,
                        ActionTakenDaysCount = 0,
                        ActionTakenHoursCount = 0,
                        AppActionType = Convert.ToInt32(AppActionTypeEnum.DEREGISTERED),
                        ApplicationRefId = deRegsiterFactoryRequest.appId,
                        Receiver_ProfileRefId = deRegsiterFactoryRequest.applicantprofileid,
                        Receiver_UserRefId = deRegsiterFactoryRequest.applicantuserid,
                        Sender_ProfileRefId = profileid.UserProfileRefId,
                        Sender_UserRefId = deRegsiterFactoryRequest.id,
                        Remarks = deRegsiterFactoryRequest.remarks,
                        ReceiverRoleId = "",
                        SenderRoleId = "",
                        AppDocumentRefId = 0,
                        IsDocumentUploaded = false,
                        Legacy_IsMigrated = true,
                        Legacy_AppId = 0,
                        Legacy_AppFormId = 0,
                        Legacy_NAR = null,
                        Legacy_StatusId = 0
                    };
                    await _context.ApplicationActionLogs.AddAsync(applicationActionLog);
                    await _context.SaveChangesAsync();

                    ApplicationDocument applicationDocument = new ApplicationDocument
                    {
                        AttachmentName = filename,
                        IsUploaded = true,
                        Uploadeddate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        DocumentRefId = 80025,
                        AppRefId = deRegsiterFactoryRequest.appId,
                        IsLocked = true
                    };
                    await _context.ApplicationDocuments.AddAsync(applicationDocument);
                    await _context.SaveChangesAsync();
                }

                AuditLog_Factory_Backlog auditLog_Factory_Backlog = new AuditLog_Factory_Backlog
                {
                    AppRefId = deRegsiterFactoryRequest.appId,
                    NAR = deRegsiterFactoryRequest.nar,
                    AppFormId = Convert.ToInt32(deRegsiterFactoryRequest.appFormId),
                    IsLegacy = Convert.ToBoolean(deRegsiterFactoryRequest.IsLegacy),
                    IterationCounter = iterationCounter,
                    FieldName = "Factory Deregistred",
                    OldValue = "registered",
                    NewValue = Convert.ToString(deRegsiterFactoryRequest.deregistrationnumber),
                    CreatedOn = DateTime.Now,
                    UserRefId = deRegsiterFactoryRequest.id,
                    ProfileRefId = profileid.UserProfileRefId,
                    Remarks = deRegsiterFactoryRequest.remarks

                };
                await _context.AuditLog_Factory_Backlogs.AddAsync(auditLog_Factory_Backlog);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<SysNApplicationDetailsViewModel>> GetApplicationData(Int64 projectSiteRefId, Int64 
            
            _AppId, Int64 pbLabour_AppFormId, string pbLabour_NAR, Int32 IsLegacy, string userId)
        {
            GenericResponseTemplateModel<SysNApplicationDetailsViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<SysNApplicationDetailsViewModel>();
            try
            {
                var appDetails = _context.Applications.Where(x => x.ProjectSiteRefId == projectSiteRefId && x.IsDeleted == false && x.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE).FirstOrDefault();
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "ProjectSiteId", ParmValue = projectSiteRefId.ToString(), isNumber = true },
                        //new StoreProcedureParm() { ParmName = "AppId", ParmValue = pbLabour_AppId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "AppFormId", ParmValue = pbLabour_AppFormId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = IsLegacy.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "NAR", ParmValue = pbLabour_NAR, isNumber = false },
                };

                var currentStatus = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetSysNCurrentStatusViewModel>("sp_SysN_GetCurrentStatusDetails", storeProcedureParms);
                var applicatonDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetApplicationDetailsViewModel>("sp_GetApplicationDetails", storeProcedureParms);
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ProjectSiteId", ParmValue= projectSiteRefId.ToString(), isNumber = true},
                    //new StoreProcedureParm() { ParmName = "AppId", ParmValue = pbLabour_AppId.ToString(), isNumber = true },
                    new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = IsLegacy.ToString(), isNumber = true },
                };
                var clearances = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetSysNClearancesIssuedsViewModel>("sp_SysN_GetClearanceIssuedsDetails", storeProcedureParms);

                var respWelfareFund = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WelfareFundDetailsViewModel>("dbo.sp_SysN_GetWelfareFundDetails", storeProcedureParms);

                var respAnnualReturn = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AnnualReturnDetailsViewModel>("dbo.sp_SysN_GetAnnualReturnDetails", storeProcedureParms);

                var respFactoryInsp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FactoryInspectionDetailsViewModel>("dbo.sp_SysN_GetFactoryInspectionDetails", storeProcedureParms);

                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue= userId, isNumber = false},
                    //new StoreProcedureParm() { ParmName = "AppId", ParmValue = pbLabour_AppId.ToString(), isNumber = true },
                };

                var buildingPlan = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MappedBuildingPlanDetailsViewModel>("sp_GetMappedBuildingPlanDetails", storeProcedureParms);

                SysNApplicationDetailsViewModel applicationDetails = new SysNApplicationDetailsViewModel();
                applicationDetails.CurrentStatus = currentStatus;
                applicationDetails.ApplicationDetails = applicatonDetails;
                applicationDetails.Clearances = clearances;
                applicationDetails.AnnualReturnDetails = respAnnualReturn;
                applicationDetails.WelfareFundDetails = respWelfareFund;
                applicationDetails.FactoryInspectionDetails = respFactoryInsp;
                applicationDetails.BuildingPlanDetails = buildingPlan;

                genericResponseTemplateModel.ResponseDataModel = applicationDetails;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<GetBuildingPlanDetailsViewModel>>> GetBuildingPlanData(string licenceNumber)
        {
            GenericResponseTemplateModel<List<GetBuildingPlanDetailsViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<GetBuildingPlanDetailsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "LicenceNumber", ParmValue = licenceNumber, isNumber = false },
                };

                var currentStatus = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetBuildingPlanDetailsViewModel>("sp_GetBuildingPlanDetails", storeProcedureParms);
                genericResponseTemplateModel.ResponseDataModel = currentStatus;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }
        public async Task<GenericServiceResultTemplate> MapBuildingPlan(MapBuildingPLanRequestViewModel mapBuildingPLanRequestViewModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "UserId", ParmValue = mapBuildingPLanRequestViewModel.id, isNumber = false },
                        new StoreProcedureParm() { ParmName = "Token1", ParmValue = mapBuildingPLanRequestViewModel.Token1, isNumber = false },
                        new StoreProcedureParm() { ParmName = "Token2", ParmValue = mapBuildingPLanRequestViewModel.Token2, isNumber = false },
                        new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = (mapBuildingPLanRequestViewModel.IsLegacy).ToString(), isNumber = true },
                };

                var result = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_MapFactoryBuildingPlan", storeProcedureParms);
                int emps = result.Value;
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<GetApplicationNotingLogsViewModel>>> GetApplicationLogsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<GetApplicationNotingLogsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetApplicationNotingLogsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "Appid", ParmValue = appRefId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "AppFormId", ParmValue = appFormId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "NAR", ParmValue = nar, isNumber = false },
                        new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = isLegacy.ToString(), isNumber = true },
                };
                var NotingLogs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetApplicationNotingLogsViewModel>("sp_GetApplicationNotingLogsByAppId", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = NotingLogs;
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<ApplicationFeeDetailsViewModel>>> GetFeeDetailsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<ApplicationFeeDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ApplicationFeeDetailsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "Appid", ParmValue = appRefId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "AppFormId", ParmValue = appFormId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "NAR", ParmValue = nar, isNumber = false },
                        new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = isLegacy.ToString(), isNumber = true },
                };
                var feeDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationFeeDetailsViewModel>("sp_GetFeeDetailsByAppId", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = feeDetails;
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<List<FactoryBacklogDataViewModel>>> FactoryBacklogCountwiseData(string id, int recordType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<List<FactoryBacklogDataViewModel>> genericFormModel = new GenericFormModel<List<FactoryBacklogDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "UserId", ParmValue = id.ToString(), isNumber = false },
                       new StoreProcedureParm() { ParmName = "RecordType", ParmValue = recordType.ToString(), isNumber = true },

                       new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FactoryBacklogDataViewModel>("sp_Dashboard_FactoryBacklog_CountwiseData", storeProcedureParms);
                genericFormModel.FormModel = new List<FactoryBacklogDataViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<List<LWFMonthWiseContributionViewModel>>> GetLWFMonthWiseData(Int64 pwbCessCollectionId)
        {
            GenericResponseTemplateModel<List<LWFMonthWiseContributionViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<LWFMonthWiseContributionViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "PWBCessCollectionId", ParmValue = pwbCessCollectionId.ToString(), isNumber = true }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LWFMonthWiseContributionViewModel>("sp_SysN_getPWBMonthWiseContribution", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = new List<LWFMonthWiseContributionViewModel>();
                genericServiceResultTemplate.ResponseDataModel = emps;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<LWFEmployeeContributionViewModel>>> GetLWFEmployeeData(string monthlyContributionId)
        {
            GenericResponseTemplateModel<List<LWFEmployeeContributionViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<LWFEmployeeContributionViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "MonthlyContributionId", ParmValue = monthlyContributionId.ToString(), isNumber = false }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LWFEmployeeContributionViewModel>("sp_SysN_getPWBEmployeeContribution", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = new List<LWFEmployeeContributionViewModel>();
                genericServiceResultTemplate.ResponseDataModel = emps;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<GetAppActionDocumentsViewModel>>> GetAppActionDocuments(string nar, Int64 appFormId, Int64 appId, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<GetAppActionDocumentsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetAppActionDocumentsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm() { ParmName = "NAR", ParmValue = nar.ToString(), isNumber = false },
                       new StoreProcedureParm() { ParmName = "AppFormId", ParmValue = appFormId.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "AppId", ParmValue = appId.ToString(), isNumber = true },
                       new StoreProcedureParm() { ParmName = "IsLegacy", ParmValue = isLegacy.ToString(), isNumber = true }
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetAppActionDocumentsViewModel>("sp_GetAppActionDocuments", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = new List<GetAppActionDocumentsViewModel>();
                genericServiceResultTemplate.ResponseDataModel = emps;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region Declaration Stability Certificate
        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetDeclarationStabilityApplications(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id.ToString(), isNumber=false},
                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_GetDeclarationStabilityApplications", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion

        #region Declaration Stability List Excel
        public async Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetDeclarationStabilityApplicationsExcel(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<RecordsTypeListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id.ToString(), isNumber=false},
                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RecordsTypeListViewModel>("dbo.sp_GetDeclarationStabilityApplicationsExcel", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        #endregion


        #region MIS Dashboard
        public async Task<GenericResponseTemplateModel<List<ActAndApplicationPurposeTypeCountsViewModel>>> GetActAndApplicationPurposeTypeCounts(PSLDashboardMajorCountRequestParmsViewModel requestData)
       {
            GenericResponseTemplateModel<List<ActAndApplicationPurposeTypeCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ActAndApplicationPurposeTypeCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                     //new StoreProcedureParm (){ ParmName="RoleName", ParmValue=requestData.RoleName.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="StartDate", ParmValue=requestData.FromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="EndDate", ParmValue=requestData.ToDate.ToString(), isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ActAndApplicationPurposeTypeCountsViewModel>("dbo.sp_MIS_GetServiceWiseData", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<CircleAndApplicationPurposeTypeCountsViewModel>>> GetCircleAndApplicationPurposeTypeCounts(CircleAndApplicationPurposeTypeCountsParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<CircleAndApplicationPurposeTypeCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<CircleAndApplicationPurposeTypeCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=requestData.ApplicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="StartDate", ParmValue=requestData.FromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="EndDate", ParmValue=requestData.ToDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationPurposeType", ParmValue=0.ToString(), isNumber=true},
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<CircleAndApplicationPurposeTypeCountsViewModel>("dbo.sp_MIS_GetCircleWiseData", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<DesignationAndApplicationPurposeTypeCountsViewModel>>> GetDesignationAndApplicationPurposeTypeCounts(CircleAndApplicationPurposeTypeCountsParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<DesignationAndApplicationPurposeTypeCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<DesignationAndApplicationPurposeTypeCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=requestData.ApplicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate.ToString(), isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DesignationAndApplicationPurposeTypeCountsViewModel>("dbo.sp_MIS_getDesignationAndApplicationPurposeTypeCounts", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<FileWiseDataViewModel>>> GetFileWiseData(int applicationType, int statusType, int circleRefId, int applicationPurposeType, string fromDate, string toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<FileWiseDataViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<FileWiseDataViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="StatusType", ParmValue=statusType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="CircleRefId", ParmValue=circleRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ApplicationPurposeType", ParmValue=applicationPurposeType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="StartDate", ParmValue=fromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="EndDate", ParmValue=toDate.ToString(), isNumber=false},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FileWiseDataViewModel>("dbo.sp_MIS_GetFileWiseApplicationData", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = JsonConvert.SerializeObject(ex);
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<List<OfficialsRoleViewModel>>> GetAllDesignatedOfficialsRoleList()
        {
            GenericFormModel<List<OfficialsRoleViewModel>> genericFormModel = new GenericFormModel<List<OfficialsRoleViewModel>>();
            try
            {
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<OfficialsRoleViewModel>("sp_GetAllDesignatedOfficialsRole");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<List<OfficerMISDashboardDetailsByRoleNameViewModel>>> GetOfficerMISDashboardDetailsByRoleName(string roleName)
        {
            GenericResponseTemplateModel<List<OfficerMISDashboardDetailsByRoleNameViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<OfficerMISDashboardDetailsByRoleNameViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(), isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<OfficerMISDashboardDetailsByRoleNameViewModel>("dbo.sp_MIS_GetOfficerDashboardDetailsByRoleName", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<OfficerProfileAndActWiseDashboardDetailsViewModel>>> GetProfileAndActWiseDashboardDetails(Int64 officerProfileRefId)
        {
            GenericResponseTemplateModel<List<OfficerProfileAndActWiseDashboardDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<OfficerProfileAndActWiseDashboardDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="OfficerProfileRefId", ParmValue=officerProfileRefId.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<OfficerProfileAndActWiseDashboardDetailsViewModel>("dbo.sp_MIS_GetActWiseDashboardByOfficerProfileRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<OfficerProfileAndCircleWiseDashboardDetailsViewModel>>> GetProfileAndCircleWiseDashboardDetails(Int64 officerProfileRefId, Int64 applicationType)
        {
            GenericResponseTemplateModel<List<OfficerProfileAndCircleWiseDashboardDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<OfficerProfileAndCircleWiseDashboardDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="OfficerProfileRefId", ParmValue=officerProfileRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<OfficerProfileAndCircleWiseDashboardDetailsViewModel>("dbo.sp_MIS_GetCircletWiseActDashboardByOfficerProfileRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<ProfileAndApplicationPurposeTypeCountsViewModel>>> GetProfileAndApplicationPurposeTypeCounts(int officerProfileRefId)
        {
            GenericResponseTemplateModel<List<ProfileAndApplicationPurposeTypeCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ProfileAndApplicationPurposeTypeCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="OfficerProfileRefId", ParmValue=officerProfileRefId.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProfileAndApplicationPurposeTypeCountsViewModel>("dbo.sp_Mis_GetProfileAndApplicationPurposeTypeCount", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<CurrentlyDesignatedOfficerDetailsViewModel>>> GetCurrentlyDesignatedOfficerDetails(Int64 officerProfileRefId)
        {
            GenericResponseTemplateModel<List<CurrentlyDesignatedOfficerDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<CurrentlyDesignatedOfficerDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="OfficerProfileRefId", ParmValue=officerProfileRefId.ToString(), isNumber=true}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<CurrentlyDesignatedOfficerDetailsViewModel>("dbo.sp_Mis_GetCurrentlyDesignatedOfficerDetails", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<FileWiseDataViewModel>>> GetFileWiseData_AppSearch(int applicationType, int statusType, int circleRefId, int applicationPurposeType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<FileWiseDataViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<FileWiseDataViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="StatusType", ParmValue=statusType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="CircleRefId", ParmValue= circleRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ApplicationPurposeType", ParmValue=applicationPurposeType.ToString(), isNumber=true},
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FileWiseDataViewModel>("dbo.sp_MIS_getFileAndApplicationPurposeTypeCounts", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<LabourWelfareSchemeApplicationCountsViewModel>>> GetLabourWelfareSchemeCounts(LabourWelfareSchemeApplicationParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<LabourWelfareSchemeApplicationCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<LabourWelfareSchemeApplicationCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ReportType", ParmValue=requestData.ReportType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate.ToString(), isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LabourWelfareSchemeApplicationCountsViewModel>("dbo.sp_MIS_getLabourWelfareSchemePerformanceReport", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<EsamikshaCountsViewModel>>> GetEsamikshaData(EsamikshaViewModel requestData)
        {
            GenericResponseTemplateModel<List<EsamikshaCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<EsamikshaCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {

                    new StoreProcedureParm (){ ParmName = "FromDate", ParmValue = requestData.FromDate.ToString("yyyy-MM-dd"), isNumber = false },
                    new StoreProcedureParm (){ ParmName = "ToDate", ParmValue = requestData.ToDate.ToString("yyyy-MM-dd"), isNumber = false }

                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EsamikshaCountsViewModel>("dbo.sp_eSamikshaReport", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<GetPendencyCountViewModel>>> GetPendencyCountServiceWise()
        {
            var genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetPendencyCountViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };

            try
            {
               
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<GetPendencyCountViewModel>("dbo.sp_GetPendencyCountServiceWise");
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }

            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<GetPendingAppCountsViewModel>>> GetPendingAppData(GetPendingAppViewModel requestData)
        {
            GenericResponseTemplateModel<List<GetPendingAppCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetPendingAppCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {

                      new StoreProcedureParm (){ ParmName="ServiceCode", ParmValue=requestData.ServiceCode.ToString(), isNumber=true},
                      new StoreProcedureParm (){ ParmName="PendencyType", ParmValue=requestData.PendencyType.ToString(), isNumber=true},
                      
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetPendingAppCountsViewModel>("dbo.sp_GetPendingApplicationsByServiceCode", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<MisInspectionDashboardDataViewModel>>> GetMis_Inspection(  string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName)
        {
            GenericResponseTemplateModel<List<MisInspectionDashboardDataViewModel>> genericFormModel = new GenericResponseTemplateModel<List<MisInspectionDashboardDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" },
                    new StoreProcedureParm (){isNumber=false, ParmName="UserId", ParmValue=userId.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="RoleName", ParmValue=roleName.ToString()}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MisInspectionDashboardDataViewModel>("sp_MIS_GetInspectionDashboardData", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<List<InspectionApplicationinfoViewModel>>> Get_inspectionsActionWiseData(int inspectionStatus, int randomizationRefId, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionApplicationinfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<InspectionApplicationinfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RandomizationRefId", ParmValue=randomizationRefId.ToString(),isNumber=true},
                    new StoreProcedureParm (){ ParmName="InspectionStatus", ParmValue=inspectionStatus.ToString(),isNumber=true,},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" },
                    new StoreProcedureParm (){isNumber=false, ParmName="UserId", ParmValue=userId.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="RoleName", ParmValue=roleName.ToString()}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionApplicationinfoViewModel>("sp_MIS_GetInspectionActionWiseData", storeProcedureParms);
                if (genericFormModel.ResponseDataModel.Count() > 0)
                {
                    genericFormModel.ResponseDataModel.Select(x => { x.EstablishmentName = x.EstbDetail.Split("|")[0]; x.EstablishmentAddress = x.EstbDetail.Split("|")[1]; return x; }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        #endregion
        public async Task<GenericResponseTemplateModel<List<MPR_Factory_DashboardViewModel>>> Load_Mpr_Factory_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Factory_DashboardViewModel>> genericFormModel = new GenericResponseTemplateModel<List<MPR_Factory_DashboardViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id, isNumber=false},
                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" },
                    new StoreProcedureParm (){ ParmName="Month", ParmValue=month.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="Year", ParmValue=year.ToString(), isNumber=true},
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MPR_Factory_DashboardViewModel>("dbo.sp_Mpr_Factory_GetRecordsByUserId", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        #region Project Profile Dashboard

        public async Task<GenericResponseTemplateModel<bool>> CheckServiceAlreadyApply(Int64 projectSiteId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = "", ResponseDataModel = true };
            try
            {
                genericResponseTemplateModel.ResponseDataModel = _context.Applications.Any(x => x.ProjectSiteRefId == projectSiteId && x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType && x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED);
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }


        #endregion Project Profile Dashboard


        public async Task<GenericResponseTemplateModel<List<AppActionTimeLineDefination>>> GetTimeLinesDetail(Int64 applicationActionLogId)
        {
            GenericResponseTemplateModel<List<AppActionTimeLineDefination>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<AppActionTimeLineDefination>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = await _context.AppActionTimeLineDefinations.Where(x => x.AppActionLogRefId == applicationActionLogId).ToListAsync();

                genericServiceResultTemplate.ResponseDataModel = genericServiceResultTemplate.ResponseDataModel.Select(x => 
                { x.Hours = 
                    x.TotalWorkingTime > Convert.ToDouble(0) &&  TimeSpan.FromHours(x.TotalWorkingTime).Hours == 0 
                    ? Convert.ToInt32(x.TotalWorkingTime) 
                    : TimeSpan.FromHours(x.TotalWorkingTime).Hours; 
                x.Minutes = TimeSpan.FromHours(x.TotalWorkingTime).Minutes; return x; }).ToList();


            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        #region Admin Dashboard

        public async Task<GenericFormModel<List<SearchApplictionDataViewModel>>> SearchApplicationResult( string searchCode)
        {
            GenericFormModel<List<SearchApplictionDataViewModel>> genericFormModel = new GenericFormModel<List<SearchApplictionDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                   {
                       new StoreProcedureParm (){isNumber=false, ParmName="FileNumber",ParmValue= searchCode==null ? "": searchCode},
                   };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SearchApplictionDataViewModel>("sp_GetApplicationDetailsBySearch", storeProcedureParms);
                genericFormModel.FormModel = new List<SearchApplictionDataViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion


        public async Task<GenericFormModel<List<ServiceListViewModel>>> GetAllServiceList(Int64 ProjectSiteRefId)
        {
            GenericFormModel<List<ServiceListViewModel>> genericFormModel = new GenericFormModel<List<ServiceListViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=ProjectSiteRefId.ToString(), isNumber=false}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ServiceListViewModel>("dbo.sp_Get_All_Service_List", storeProcedureParms);


            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericFormModel<List<GetApplciationLogsViewModel>>> TrackApplicationLogs(Int64 appId, Int64 investPunjabAppid, string punblicrefno)
        {
            GenericFormModel<List<GetApplciationLogsViewModel>> genericFormModel = new GenericFormModel<List<GetApplciationLogsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
           {
               new StoreProcedureParm (){ ParmName="AppId", ParmValue=appId.ToString(), isNumber=true},
               new StoreProcedureParm (){ ParmName="InvestPunjabAppId", ParmValue=investPunjabAppid.ToString(), isNumber=true},
               new StoreProcedureParm (){ ParmName="PublicAppRefNum", ParmValue=punblicrefno.ToString(), isNumber=false}
           };

                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetApplciationLogsViewModel>("sp_GetApplicationLogs", storeProcedureParms);
                genericFormModel.FormModel = new List<GetApplciationLogsViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<GetApplciationReActivatedData>>> ReActivateApplication(Int64 appId)
        {
            GenericFormModel<List<GetApplciationReActivatedData>> genericFormModel = new GenericFormModel<List<GetApplciationReActivatedData>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appId", ParmValue=appId.ToString(), isNumber=true}
                };

                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetApplciationReActivatedData>("sp_ActivateApplicationByAppId", storeProcedureParms);

                if (genericFormModel.FormModel.FirstOrDefault().AppActionType == AppActionTypeEnum.APP_OBJECTION)
                {
                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appId, genericFormModel.FormModel.FirstOrDefault().ApplicationType, genericFormModel.FormModel.FirstOrDefault().AppActionType);
                }
                
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<GetDeemedApplciationsData>>> GetDeemedReport(string deemedDate)
        {
            GenericFormModel<List<GetDeemedApplciationsData>> genericFormModel = new GenericFormModel<List<GetDeemedApplciationsData>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="DeemedDate", ParmValue=deemedDate.ToString(), isNumber=false}
                };

                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetDeemedApplciationsData>("sp_GetShopDeemedReport", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericListModel<EmpanelledPersonDetailsByRoleNameViewModel>> GetEmpanelledPersonDetailsByRoleName(string roleName)
        {
            GenericListModel<EmpanelledPersonDetailsByRoleNameViewModel> genericListModel = new GenericListModel<EmpanelledPersonDetailsByRoleNameViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(), isNumber=false}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EmpanelledPersonDetailsByRoleNameViewModel>("sp_GetEmpanelledPersonDetailsByRoleName", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }

        public async Task<GenericListModel<bool>> UpdateEmpanelledPersonStatus(EmpanelledPersonDetailsByRoleNameViewModel requestData)
        {
            GenericListModel<bool> genericListModel = new GenericListModel<bool>();
            try
            {
                var userProfileMapping = await _context.UserProfileMapping
                    .Where(x => x.UserProfileRefId == requestData.UserProfileId)
                    .FirstOrDefaultAsync();

                if (userProfileMapping == null)
                {
                    genericListModel.HasError = true;
                    genericListModel.ErrorDesc = "User profile not found.";
                    genericListModel.ListData = new List<bool> { false };
                    return genericListModel;
                }

                userProfileMapping.IsActive = requestData.Status?.Equals("InActive", StringComparison.OrdinalIgnoreCase) == true ? false : true;
                userProfileMapping.DateOfUserAssign = DateTime.Now;
                userProfileMapping.LastModifiedDate = DateTime.Now;

                _context.Update(userProfileMapping);
                await _context.SaveChangesAsync();

                genericListModel.ListData = new List<bool> { true };
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
                genericListModel.ListData = new List<bool> { false };
            }
            return genericListModel;
        }

        public async Task<GenericResponseTemplateModel<WelfareFundAndWagesDataViewModel>> GetPWBContributionDetails(string startDate, string endDate, int type, string userId)
        {
            GenericResponseTemplateModel<WelfareFundAndWagesDataViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<WelfareFundAndWagesDataViewModel>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {

                      new StoreProcedureParm (){ ParmName="StartDate", ParmValue=startDate, isNumber=false},
                      new StoreProcedureParm (){ ParmName="EndDate", ParmValue=endDate, isNumber=false},
                      new StoreProcedureParm (){ ParmName="Type", ParmValue=type.ToString(), isNumber=true},
                      new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId, isNumber=false},

                };
                 var fundAndWagesCircleWiseData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FundAndWagesCircleWiseDataViewModel>("dbo.sp_GetFundAndWagesCircleWiseData", storeProcedureParms);

                var fundAndUnpaidWagesFileWiseData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FundAndUnpaidWagesViewModel>("dbo.sp_GetFundAndUnpaidWagesData", storeProcedureParms);

                WelfareFundAndWagesDataViewModel welfareFundAndWagesData = new WelfareFundAndWagesDataViewModel();
                welfareFundAndWagesData.FundAndWagesCircleWiseDataViewModel = fundAndWagesCircleWiseData;
                welfareFundAndWagesData.FundAndUnpaidWagesViewModel = fundAndUnpaidWagesFileWiseData;

                genericServiceResultTemplate.ResponseDataModel = welfareFundAndWagesData;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<MonthWiseContributionDetailsViewModel>>> GetMonthlyContributionDetailsById(Int64 pwbCessCollectionId)
        {
            GenericResponseTemplateModel<List<MonthWiseContributionDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<MonthWiseContributionDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
              
                var monthlyContributionDetails = await _iDapperRepository.Get<MonthWiseContributionDetailsViewModel>("SELECT  * FROM PWBMonthlyContributions WHERE PWBCessCollectionId =@PWBCessCollectionId", new { PWBCessCollectionId = pwbCessCollectionId.ToString() }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = monthlyContributionDetails.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<MonthWiseEmployeeDetailsViewModel>>> GetMonthlyEmployeeDetailsById(Int64 monthlyContributionId)
        {
            GenericResponseTemplateModel<List<MonthWiseEmployeeDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<MonthWiseEmployeeDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {

                      new StoreProcedureParm (){ ParmName="PWBMonthlyContributionId", ParmValue=monthlyContributionId.ToString(), isNumber=true}

                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MonthWiseEmployeeDetailsViewModel>("dbo.sp_GetPWBEmployeeDetailsById", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<UnpaidWagesEmployeeDetailsViewModel>>> GetUnpaidWagesEmployeeDetailsById(Int64 unpaidWagesId)
        {
            GenericResponseTemplateModel<List<UnpaidWagesEmployeeDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<UnpaidWagesEmployeeDetailsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {

                var unpaidWagesEmployeeDetails = await _iDapperRepository.Get<UnpaidWagesEmployeeDetailsViewModel>("select EmpName, FatherName, AadharNo, Mobile, Amount, BankAccountNo, BankName, IFSCCode, Remarks  from PWBUnPaidEmployees WHERE PWBUnPaidWagesId = @PWBUnPaidWagesId AND IsDeleted = 0", new { PWBUnPaidWagesId = unpaidWagesId.ToString() }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = unpaidWagesEmployeeDetails.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<FactoriesListViewModel>>> GetRegsiteredFactoriesDetails(int factoryType)
        {
            GenericResponseTemplateModel<List<FactoriesListViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<FactoriesListViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                if (factoryType == 1) 
                { 
                    genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<FactoriesListViewModel>("dbo.sp_GetRegisteredFactoriesDetails");

                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<FactoriesListViewModel>("dbo.sp_GetDeRegisteredFactoriesDetails");
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<GetOtherClearanceDetailsViewModel>> GetOtherClearanceDetails(int projectSiteRefId, Int64 applicationType)
        {
            var response = new GenericResponseTemplateModel<GetOtherClearanceDetailsViewModel>()
            {
                HasError = false,
                ErrorDesc = ""
            };

            try
            {
                var result = new GetOtherClearanceDetailsViewModel();

                using (var conn = _context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "sp_GetOtherClearanceDetails";
                        command.CommandType = CommandType.StoredProcedure;

                        var param1 = command.CreateParameter();
                        param1.ParameterName = "@ProjectSiteRefId";
                        param1.Value = projectSiteRefId;
                        command.Parameters.Add(param1);

                        var param2 = command.CreateParameter();
                        param2.ParameterName = "@ApplicationType";
                        param2.Value = applicationType; 
                        command.Parameters.Add(param2);

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            // 1. Approval Details
                            var approvalDetails = new List<ApprovalDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    approvalDetails.Add(new ApprovalDetailsViewModel
                                    {
                                        SrNo = reader["Sr. No"] != DBNull.Value ? Convert.ToInt32(reader["Sr. No"]) : 0,
                                        ServiceName = reader["ServiceName"]?.ToString(),
                                        ApplicationPurposeType = Convert.ToInt32(reader["ApplicationPurposeType"]),
                                        PublicAppRefNum = reader["PublicAppRefNum"]?.ToString(),
                                        LicenceNumber = reader["LicenceNumber"]?.ToString(),
                                        ApprovalDate = reader["ApprovalDate"] != DBNull.Value ? Convert.ToDateTime(reader["ApprovalDate"]) : default(DateTime),
                                        ClearanceExpiredOn = reader["ClearanceExpiredOn"] != DBNull.Value ? Convert.ToDateTime(reader["ClearanceExpiredOn"]) : default(DateTime),
                                        LicencePath = reader["LicencePath"]?.ToString(),
                                        TransactionAmount = Convert.ToDecimal(reader["TransactionAmount"]),

                                    });
                                }
                            }
                            result.ApprovalDetails = approvalDetails;


                            // 2. Welfare Contribution Details
                            await reader.NextResultAsync();
                            var welfareContributionDetails = new List<AdminDashboardWelfareContributionDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    welfareContributionDetails.Add(new AdminDashboardWelfareContributionDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        FinancialYear = reader["FinancialYear"]?.ToString(),
                                        TimeSlot = reader["TimeSlot"]?.ToString(),
                                        Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                        PWBCessCollectionId = reader["PWBCessCollectionId"] != DBNull.Value ? Convert.ToInt64(reader["PWBCessCollectionId"]) : 0,
                                    });
                                }
                            }
                            result.WelfareContributionDetails = welfareContributionDetails;


                            // 3. Annual Return Details
                            await reader.NextResultAsync();
                            var annualReturnDetails = new List<AdminDashboardAnnualReturnDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    annualReturnDetails.Add(new AdminDashboardAnnualReturnDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        AccNo = reader["AccNo"]?.ToString(),
                                        ReturnYear = reader["ReturnYear"]?.ToString(),
                                        ReturnType = reader["ReturnType"]?.ToString(),
                                        ReturnID = reader["ReturnID"] != DBNull.Value ? Convert.ToInt64(reader["ReturnID"]) : 0,
                                    });
                                }
                            }
                            result.AnnualReturnDetails = annualReturnDetails;


                            // 4. Annual Return Details
                            await reader.NextResultAsync();
                            var inspectionDetails = new List<AdminDashboardInspectionDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    inspectionDetails.Add(new AdminDashboardInspectionDetailsViewModel
                                    {
                                        EstablishmentName = reader["EstablishmentName"]?.ToString(),
                                        LicenceNumber = reader["LicenceNumber"]?.ToString(),
                                        FactoryHazardousCategoryType = reader["FactoryHazardousCategoryType"]?.ToString(),
                                        InspectionDoneOn_Factory_Wing = reader["InspectionDoneOn_Factory_Wing"] != DBNull.Value ? Convert.ToDateTime(reader["InspectionDoneOn_Factory_Wing"]) : default(DateTime),
                                        InspectionSubmittedFactoryWing = reader["InspectionSubmittedFactoryWing"]?.ToString(),
                                        InspectionDoneOn_Labour_Wing = reader["InspectionDoneOn_Labour_Wing"] != DBNull.Value ? Convert.ToDateTime(reader["InspectionDoneOn_Labour_Wing"]) : default(DateTime),
                                        InspectionSubmittedLabourWing = reader["InspectionSubmittedLabourWing"]?.ToString(),
                                    });
                                }
                            }
                            result.InspectionDetails = inspectionDetails;


                        }
                    }
                }

                response.ResponseDataModel = result;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorDesc = ex.Message;
            }

            return response;
        }


        public async Task<GenericResponseTemplateModel<List<MPR_Labour_DashboardViewModel>>> Load_Mpr_Labour_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Labour_DashboardViewModel>> genericFormModel = new GenericResponseTemplateModel<List<MPR_Labour_DashboardViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
         {
             new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id, isNumber=false},
             new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
             new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
             new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
             new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
             new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" },
             new StoreProcedureParm (){ ParmName="Month", ParmValue=month.ToString(), isNumber=true},
             new StoreProcedureParm (){ ParmName="Year", ParmValue=year.ToString(), isNumber=true},
         };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MPR_Labour_DashboardViewModel>("dbo.sp_Mpr_Labour_GetRecordsByUserId", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<MPR_Alc_DashboardViewModel>>> Load_Mpr_Alc_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Alc_DashboardViewModel>> genericFormModel = new GenericResponseTemplateModel<List<MPR_Alc_DashboardViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
  {
      new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=id, isNumber=false},
      new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
      new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
      new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
      new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
      new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" },
      new StoreProcedureParm (){ ParmName="Month", ParmValue=month.ToString(), isNumber=true},
      new StoreProcedureParm (){ ParmName="Year", ParmValue=year.ToString(), isNumber=true},
  };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MPR_Alc_DashboardViewModel>("dbo.sp_Mpr_Alc_GetRecordsByUserId", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<AnnualReturnDashboardViewModel>>> LoadAnnuaReturnData(string userId, Int64 projectSiteRefId)
        {
            var genericFormModel = new GenericResponseTemplateModel<List<AnnualReturnDashboardViewModel>>();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        {
            new StoreProcedureParm(){ ParmName = "ProjectSiteRefId",ParmValue = projectSiteRefId.ToString(),isNumber = false}
        };
                var clearances = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DashboardAlreadyClearenceViewModel>("dbo.sp_Get_All_ProjectSite_AlreadyClearances_Applications", storeProcedureParms);

                var licenceNumbers = clearances.Select(x => x.LicenceNumber).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                var Data = await _context.AnnualReturns.Where(r => (r.UserId == userId && r.ProjectSiteRefId == projectSiteRefId) || (!string.IsNullOrEmpty(r.LicenceNumber) && licenceNumbers.Contains(r.LicenceNumber)))
                    .Select(r => new
                    {
                        r.AppId,
                        r.ReturnYear,
                        r.ActIds,
                        r.CreatedOn,
                        r.AcknowledgementNo,
                        r.IsLocked,
                        r.ProjectSiteRefId,
                        r.LicenceNumber

                    })
                    .ToListAsync();

                var groupData = Data
                    .GroupBy(r => new { r.AppId, r.ReturnYear, r.ActIds, r.ProjectSiteRefId, r.LicenceNumber })
                    .Select(g => new
                    {
                        g.Key.AppId,
                        g.Key.ReturnYear,
                        g.Key.ActIds,
                        g.Key.ProjectSiteRefId,
                        SubmittedOn = g.Min(x => x.CreatedOn),
                        AcknowledgementNo = g.Select(x => x.AcknowledgementNo)
                                             .FirstOrDefault(a => !string.IsNullOrEmpty(a)),
                        IsLocked = g.Any(x => x.IsLocked),
                        g.Key.LicenceNumber

                    })
                    .ToList();

                genericFormModel.ResponseDataModel = groupData
                    .Select(g => new AnnualReturnDashboardViewModel
                    {
                        AppId = g.AppId,
                        ReturnYear = g.ReturnYear,
                        ActIds = g.ActIds,
                        SubmittedOn = g.SubmittedOn,
                        AcknowledgementNo = g.AcknowledgementNo,
                        IsLocked = g.IsLocked,
                        ProjectSiteRefId = g.ProjectSiteRefId,
                        LicenceNumber = g.LicenceNumber

                    })
                    .ToList();

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }




            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<ShopBacklogDataViewModel>>> GetShopBacklogData(string Id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<ShopBacklogDataViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<ShopBacklogDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                       new StoreProcedureParm() { ParmName = "userRefId", ParmValue = Id, isNumber = false },

                       new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                       new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                       new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };

                var shopBacklogData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ShopBacklogDataViewModel>("sp_ShopBacklog", storeProcedureParms);
                genericResponseTemplateModel.ResponseDataModel = shopBacklogData;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<LegacyShopFormDataViewModel>>> GetLegacyShopFormData(Int64 appId, Int64 appFormId, string nar)
        {
            GenericResponseTemplateModel<List<LegacyShopFormDataViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<LegacyShopFormDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "appId", ParmValue = appId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "appFormId", ParmValue = appFormId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "NAR", ParmValue = nar, isNumber = false },
                };

                var shopBacklogData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LegacyShopFormDataViewModel>("sp_GetShopLegacyFormData", storeProcedureParms);
                genericResponseTemplateModel.ResponseDataModel = shopBacklogData;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<PendencyReportViewModel>>> GetPendencyReport()
        {
            GenericResponseTemplateModel<List<PendencyReportViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<PendencyReportViewModel>>();
            try
            {
                var pendencyReport = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<PendencyReportViewModel>("sp_Get_PendencyReport");
                genericResponseTemplateModel.ResponseDataModel = pendencyReport;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericListModel<bool>> MergeLicenceWithExistingUser(MergeLicenceWithUserRequestViewModel requestData)
        {
            GenericListModel<bool> genericListModel = new GenericListModel<bool>();
            try
            {
                //var applications = _context.Applications.Where(x => x.AppId == requestData.AppId).FirstOrDefault();
                //applications.InvestPunjab_Ipin = requestData.InvestPunjabIpin;
                //_context.Update(applications);
                //await _context.SaveChangesAsync();


                //if (applications == null)
                //{
                //    genericListModel.HasError = true;
                //    genericListModel.ErrorDesc = "Application not found.";
                //    genericListModel.ListData = new List<bool> { false };
                //    return genericListModel;
                //}
                //else
                //{
                //    var projectsiteDetails = _context.ProjectSites.Where(x => x.ProjectSiteId == applications.ProjectSiteRefId).FirstOrDefault();
                //    projectsiteDetails.UserRefId = requestData.UserId;
                //    projectsiteDetails.ProjectSiteVersion = projectsiteDetails.ProjectSiteVersion + 1;
                //    _context.Update(projectsiteDetails);
                //    await _context.SaveChangesAsync();
                //    var projectSiteLog = new ProjectSiteLog();
                //    projectSiteLog.ProjectSiteRefId = projectsiteDetails.ProjectSiteId;
                //    projectSiteLog.EstablishmentName = projectsiteDetails.EstablishmentName;
                //    projectSiteLog.Address = projectsiteDetails.Address;
                //    projectSiteLog.VillageOrTown = projectsiteDetails.VillageOrTown;
                //    projectSiteLog.TehsilRefId = Convert.ToInt64(projectsiteDetails.TehsilRefId);
                //    projectSiteLog.DistrictRefId = Convert.ToInt64(projectsiteDetails.DistrictRefId);
                //    projectSiteLog.PinCode = projectsiteDetails.PinCode;
                //    projectSiteLog.IsActive = true;
                //    projectSiteLog.IsDeleted = false;
                //    projectSiteLog.Createddate = DateTime.Now;
                //    projectSiteLog.LastModifiedDate = DateTime.Now;
                //    projectSiteLog.UserRefId = projectsiteDetails.UserRefId;
                //    projectSiteLog.LabourCircleRefId = projectsiteDetails.LabourCircleRefId;
                //    projectSiteLog.FactoryCircleRefId = projectsiteDetails.FactoryCircleRefId;
                //    projectSiteLog.ApplicantAadharNumber = projectsiteDetails.ApplicantAadharNumber;
                //    projectSiteLog.ApplicantAadharAttachment = projectsiteDetails.ApplicantAadharAttachment;
                //    projectSiteLog.ApplicantPanNumber = projectsiteDetails.ApplicantPanNumber;
                //    projectSiteLog.ApplicantPanAttachment = projectsiteDetails.ApplicantPanAttachment;
                //    projectSiteLog.CompanyPanNumber = projectsiteDetails.CompanyPanNumber;
                //    projectSiteLog.CompanyPanAttachment = projectsiteDetails.CompanyPanAttachment;
                //    projectSiteLog.ProjectPurpose = projectsiteDetails.ProjectPurpose;
                //    projectSiteLog.ContactPersonFirstName = projectsiteDetails.ContactPersonFirstName;
                //    projectSiteLog.ContactPersonMiddleName = projectsiteDetails.ContactPersonMiddleName;
                //    projectSiteLog.ContactPersonLastName = projectsiteDetails.ContactPersonLastName == null ? "." : projectsiteDetails.ContactPersonLastName;
                //    projectSiteLog.ContactPersonEmail = projectsiteDetails.ContactPersonEmail;
                //    projectSiteLog.ContactPersonMobileNo = projectsiteDetails.ContactPersonMobileNo;
                //    projectSiteLog.AlternateMobileNo = "NA";
                //    projectSiteLog.AlternateEmail = "NA";
                //    projectSiteLog.ProjectSiteVersion = projectsiteDetails.ProjectSiteVersion;
                //    await _context.ProjectSiteLogs.AddAsync(projectSiteLog);
                //    _context.SaveChanges();

                //    var applicationsData = _context.Applications.Where(x => x.ProjectSiteRefId == projectsiteDetails.ProjectSiteId).ToList();
                //    applicationsData.ForEach(a =>
                //    {
                //        a.ProjectSiteVersion = projectsiteDetails.ProjectSiteVersion;
                //    });
                //    await _context.SaveChangesAsync();
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "licenceNumber", ParmValue = requestData.LicenceNumber.ToString(), isNumber = false },
                        new StoreProcedureParm() { ParmName = "userRefId", ParmValue = requestData.NewUserId.ToString(), isNumber = false }
                };

                var result = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_CreateProjectSiteByLicenceNumber", storeProcedureParms);

                LicenceWiseRelationshipMapping licenceWiseRelationshipMapping = new LicenceWiseRelationshipMapping();

                    licenceWiseRelationshipMapping.LicenceNumber = requestData.LicenceNumber;
                    licenceWiseRelationshipMapping.NewUserId = requestData.NewUserId;
                    licenceWiseRelationshipMapping.NewUserName = requestData.NewuserName;
                    licenceWiseRelationshipMapping.NewInvestPunjab_Ipin = requestData.NewInvestpunjabIpin;
                    licenceWiseRelationshipMapping.NewInvestPunjab_AppId = 0;
                    licenceWiseRelationshipMapping.NewAppId = 0;
                    licenceWiseRelationshipMapping.OldUserId = requestData.OldUserId;
                    licenceWiseRelationshipMapping.OldUserName = requestData.OldUserName;
                    licenceWiseRelationshipMapping.OldAppId = requestData.OldAppId;
                    licenceWiseRelationshipMapping.OldAppFormId = 0;
                    licenceWiseRelationshipMapping.OldNAR = "";
                    licenceWiseRelationshipMapping.OldInvestPunjab_Ipin = "";
                    licenceWiseRelationshipMapping.OldInvestPunjab_AppId = 0;
                licenceWiseRelationshipMapping.ProjectSiteRefId = result.Value;
                await _context.LicenceWiseRelationshipMappings.AddAsync(licenceWiseRelationshipMapping);
                    _context.SaveChanges();

                genericListModel.ListData = new List<bool> { true };
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
                genericListModel.ListData = new List<bool> { false };
            }
            return genericListModel;
        }

        public async Task<GenericResponseTemplateModel<string>> SendOTPToUser(string userId, string userName, string licenceNumber)
        {
            GenericResponseTemplateModel<string> genericResponseTemplateModel = new GenericResponseTemplateModel<string>();
            try
            {
                var userProfile = await _iAuthService.GetUserProfileByUserId(userId);

                var responseId = Guid.NewGuid().ToString();
                var alreadyVerifications = await _context.User2FactorVerifications.Where(x => x.UserRefId == userId).ToListAsync();
                alreadyVerifications = alreadyVerifications.Select(x => { x.User2FactorVerificationType = User2FactorVerificationTypeEnum.DISPOSED; return x; }).ToList();
                _context.UpdateRange(alreadyVerifications);
                await _context.SaveChangesAsync();

                User2FactorVerification user2FactorVerification = new User2FactorVerification()
                {
                    ResponseId = responseId,
                    CreatedOn = DateTime.Now,
                    UserRefId = userId,
                    User2FactorVerificationType = User2FactorVerificationTypeEnum.PENDING
                };
                await _context.AddAsync<User2FactorVerification>(user2FactorVerification);
                await _context.SaveChangesAsync();



                using (var scope = _iServiceScopeFactory.CreateScope())
                {
                    var _iNotificationManagerService = scope.ServiceProvider.GetRequiredService<INotificationManagerService>();
                    List<NotificationViewModel> notifications = new List<NotificationViewModel>()
                    {
                       new NotificationViewModel ()
                       {
                           //Body= "Dear User, Login OTP OTP_TEXT of user id (" + userName +"). Valid for 10 mintues only - PB LABOUR", Title="1407175750163741113",
                           Body= "Dear User, This OTP_TEXT is for verification to merge your License no (" + licenceNumber +") with user id (" + userName +")  -PB LABOUR", Title ="1407173683467097508",
                           NotificationMode=NotificationModeTypeEnum.MOBILE,
                           NotificationPurpose=NotificationPurposeTypeEnum.OTP }
                    };
                    await _iNotificationManagerService.InitiateNotification(userId, notifications);
                }
                genericResponseTemplateModel.ResponseDataModel = responseId;
            }

            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<EstablishmentAndUserDetailViewModel>> GetEstablishmentAndUserDetailsByLicenceNumber(string licenceNumber)
        {
            GenericResponseTemplateModel<EstablishmentAndUserDetailViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<EstablishmentAndUserDetailViewModel>();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "licenceNumber", ParmValue = licenceNumber.ToString(), isNumber = false }
                };
                var pendencyReport = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EstablishmentAndUserDetailViewModel>("sp_GetEstablishmentAndUserDetailsByLicenceNumber", storeProcedureParms);
                genericResponseTemplateModel.ResponseDataModel = pendencyReport.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<ApprovedDataViewModel>>> GetApprovedData(string id, int ServiceCode)
        {
            GenericResponseTemplateModel<List<ApprovedDataViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ApprovedDataViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();

                storeProcedureParms = new List<StoreProcedureParm>()
                {
                  new StoreProcedureParm () { ParmName = "ServiceCode", ParmValue = ServiceCode.ToString(),isNumber = true },
                  new StoreProcedureParm() { ParmName = "UserId", ParmValue = id.ToString(), isNumber = false },
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApprovedDataViewModel>("dbo.sp_GetApprovedDataByService", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }

            return genericServiceResultTemplate;
        }
        
       public async Task<GenericResponseTemplateModel<List<OfflineReportLogs>>> GetOfflineReportLogs(string userRefId)
       {
           GenericResponseTemplateModel<List<OfflineReportLogs>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<OfflineReportLogs>>();

           try
           {
               genericResponseTemplateModel.ResponseDataModel= _context.OfflineReportLogs.Where(x => x.UserRefId == userRefId).ToList();
           }
           catch (Exception ex)
           {
               genericResponseTemplateModel.HasError = true;
               genericResponseTemplateModel.ErrorDesc = ex.Message;
           }

           return genericResponseTemplateModel;
       }

       public async Task<GenericListModel<bool>> AddOfflineReport(OfficerReportLogsRequestViewModel requestData)
       {
           GenericListModel<bool> genericListModel = new GenericListModel<bool>();
           try
           {
               OfflineReportLogs offlineReport = new OfflineReportLogs()
               {
                   ReportId = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "P" + requestData.OfficerProfileId,
                   CreatedOn = DateTime.Now,
                   FromDate = requestData.FromDate,
                   ToDate = requestData.Todate,
                   UserRefId = requestData.UserRefId,
                   Actions = requestData.Actions,
                   ApplicationType = requestData.ApplicationType,
                   RoleId = requestData.RoleId,
                   UserProfileRefId = requestData.OfficerProfileId,
                   ReportStatusType = OfflineReportStatusTypeEnum.INPROCESS
               };
               _context.OfflineReportLogs.Add(offlineReport);
               await _context.SaveChangesAsync();
           }

           catch (Exception ex)
           {
               genericListModel.HasError = true;
               genericListModel.ErrorDesc = ex.Message;
               genericListModel.ListData = new List<bool> { false };
           }
           return genericListModel;
       }


       public async Task<FileDownloadViewModel> DownloadOfflineReport(string reportId)
       {
           var response = new FileDownloadViewModel();

           var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "OfflineReportByScheduler", $"{reportId}.csv");

           if (!File.Exists(filePath))
           {
               response.HasError = true;
               response.ErrorDesc = "File not found";
               return response;
           }

           var bytes = await File.ReadAllBytesAsync(filePath);

           response.Base64Data = Convert.ToBase64String(bytes);
           response.FileName = $"{reportId}.csv";
           response.ContentType = "text/csv";

           return response;
       }

        public async Task<GenericResponseTemplateModel<List<TransperancyReportCountsViewModel>>> GetTransperancyData(TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<TransperancyReportCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<TransperancyReportCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {

                    new StoreProcedureParm (){ ParmName = "FromDate", ParmValue = requestData.FromDate.ToString("yyyy-MM-dd"), isNumber = false },
                    new StoreProcedureParm (){ ParmName = "AsOnDate", ParmValue = requestData.ToDate.ToString("yyyy-MM-dd"), isNumber = false }

                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<TransperancyReportCountsViewModel>("dbo.sp_Get_TransperancyReport", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<FileDownloadViewModel> DownloadRegisteredFactoryCircleWise(string userRefId)
        {
            var response = new FileDownloadViewModel();

            try
            {
                // SP Parameters
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm() { ParmName = "userRefId", ParmValue = userRefId, isNumber = false }
                };
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RegisteredFactoryCircleWiseViewModel>("sp_Dashboard_RegisteredFactoryCircleWise", storeProcedureParms);

                // CSV Builder
                var csv = new StringBuilder();

                // Header Row
                csv.AppendLine(
                    "Sr.No,EstablishmentName,Address,InvestPunjab_Ipin,InvestPunjab_AppId," +
                    "ApplicationPurposeType,PublicAppRefNum,LicenceNumber,RegistrationDate," +
                    "LicenceValidity,AmountCalculated,UniquePaymentGatewayTransactionId," +
                    "ManufacturingProcesses,Workers_MaxDuringYear,PowerKW_Installed," +
                    "ContactPersonName,ContactPersonMobileNo,ContactPersonEmail"
                );

                // Data Rows
                int srNo = 1;

                foreach (var item in data)
                {
                    csv.AppendLine(
                        $"{srNo}," +
                        $"\"{item.EstablishmentName}\"," +
                        $"\"{item.Address}\"," +
                        $"\"{item.InvestPunjab_Ipin}\"," +
                        $"\"{item.InvestPunjab_AppId}\"," +
                        $"\"{item.ApplicationPurposeType}\"," +
                        $"\"{item.PublicAppRefNum}\"," +
                        $"\"{item.LicenceNumber}\"," +
                        $"\"{item.RegistrationDate:yyyy-MM-dd}\"," +
                        $"\"{item.LicenceValidity:yyyy-MM-dd}\"," +
                        $"{item.AmountCalculated}," +
                        $"\"{item.UniquePaymentGatewayTransactionId}\"," +
                        $"\"{item.ManufacturingProcesses}\"," +
                        $"{item.Workers_MaxDuringYear}," +
                        $"{item.PowerKW_Installed}," +
                        $"\"{item.ContactPersonName}\"," +
                        $"\"{item.ContactPersonMobileNo}\"," +
                        $"\"{item.ContactPersonEmail}\""
                    );

                    srNo++;
                }

                // Convert CSV to Base64
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());

                response.Base64Data = Convert.ToBase64String(bytes);
                response.FileName = "RegisteredFactoryCircleWise.csv";
                response.ContentType = "text/csv";
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorDesc = ex.Message;
            }

           return response;
       }

        public async Task<GenericFormModel<WelfareFundReceiptDetailsViewModel>> GetLWBPaymentReceipt(int pwbCessCollectionId)
        {
            GenericFormModel<WelfareFundReceiptDetailsViewModel> genericFormModel = new GenericFormModel<WelfareFundReceiptDetailsViewModel>();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                  new StoreProcedureParm () { ParmName = "pwbCessCollectionId", ParmValue = pwbCessCollectionId.ToString(),isNumber = true },
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WelfareFundReceiptDetailsViewModel>("dbo.sp_LWBGenerateReceipt", storeProcedureParms);
                genericFormModel.FormModel = resp[0];
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<LWFTransperancyReportViewModel>>> GetLWFTransperancyData(TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<LWFTransperancyReportViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<LWFTransperancyReportViewModel>>()
            {
                HasError = false,
                ErrorDesc = ""
            };

            try
            {
                   List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
             {
               new StoreProcedureParm() { ParmName = "FromDate", ParmValue = requestData.FromDate.ToString("yyyy-MM-dd"), isNumber = false },

            new StoreProcedureParm() { ParmName = "AsOnDate", ParmValue = requestData.ToDate.ToString("yyyy-MM-dd"), isNumber = false }


             };

    genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LWFTransperancyReportViewModel>("dbo.sp_Get_LWFTransperancyReport", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }

            return genericServiceResultTemplate;
        }
    }
}