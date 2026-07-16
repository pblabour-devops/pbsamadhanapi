using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Ocsp;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using Razorpay.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class PaymentManagerService : IPaymentManagerService
    {
        private readonly IGenericRepository<Application> _iGR_Application;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IGenericRepository<AppFeeTransaction> _iGR_AppFeeTransaction;
        private readonly IGenericRepository<AppPaymentSuccessTransactionMapping> _iGR_AppPaymentSuccessTransactionMapping;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IApplicationManagementService<dynamic> _iApplicationMamnagementService;
        private readonly IGenericRepository<AppFeeDetail> _iGR_AppFeeDetail;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IGenericRepository<CommonLicence_GeneralDetail> _iGR_CommonLicence_GeneralDetail;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_BuildingPlanHUDPaymentDetail;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGenericRepository<AppPaymentPart> _iGR_AppPaymentPart;
        private readonly IGenericRepository<ApplicationDocument> _iGR_ApplicationDocument;
        private readonly AppDbContext _context;
        private readonly IAppTimeLineManagerService _iAppTimeLineManagerService;
        public IConfiguration Configuration { get; }
        public PaymentManagerService(IPaymentManagerRepository iPaymentManagerRepository,
            IGenericRepository<Application> iGR_Application, IConfiguration configuration,
            IGenericRepository<AppFeeTransaction> iGR_AppFeeTransaction,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IApplicationManagementService<dynamic> iApplicationMamnagementService,
            IGenericRepository<AppFeeDetail> iGR_AppFeeDetail,
            IProjectSiteService iProjectSiteService,
            IGenericRepository<CommonLicence_GeneralDetail> iGR_CommonLicence_GeneralDetail,
            IGenericRepository<AppPaymentSuccessTransactionMapping> iGR_AppPaymentSuccessTransactionMapping,
            IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_BuildingPlanHUDPaymentDetail,
            IThirdPartyInegrationsService iThirdPartyInegrationsService,
            IGenericRepository<AppPaymentPart> iGR_AppPaymentPart,
            IGenericRepository<ApplicationDocument> iGR_ApplicationDocument,
            AppDbContext context,
            IAppTimeLineManagerService iAppTimeLineManagerService)
        {
            _iPaymentManagerRepository = iPaymentManagerRepository;
            _iGR_Application = iGR_Application;
            Configuration = configuration;
            _iGR_AppFeeTransaction = iGR_AppFeeTransaction;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _iGR_AppFeeDetail = iGR_AppFeeDetail;
            _iProjectSiteService = iProjectSiteService;
            _iGR_CommonLicence_GeneralDetail = iGR_CommonLicence_GeneralDetail;
            _iGR_AppPaymentSuccessTransactionMapping = iGR_AppPaymentSuccessTransactionMapping;
            _iGR_BuildingPlanHUDPaymentDetail = iGR_BuildingPlanHUDPaymentDetail;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGR_AppPaymentPart = iGR_AppPaymentPart;
            _iGR_ApplicationDocument = iGR_ApplicationDocument;
            _context = context;
            _iAppTimeLineManagerService = iAppTimeLineManagerService;
        }
        public async Task<GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>> ApplicationFeeCalculator(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId)
        {
            GenericFormModel<List<FeeCalculatorInfoParmsViewModel>> genericFormModel = new GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>();
            try
            {
                genericFormModel.FormModel = new List<FeeCalculatorInfoParmsViewModel>();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();
                var application = await _iGR_Application.GetAsync(x => x.AppId == appRefId).ConfigureAwait(false);

                if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                {
                    feeHeaderId = 2;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                    //feeHeaderId = 3;
                    //feesHeader = await GetFeeHeader(feeHeaderId);
                    //genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    //{
                    //    FeeHeaderId = feeHeaderId,
                    //    FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                    //    AmountCalculated = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId),
                    //    IsDeduductible = true,
                    //    AppRefId = appRefId
                    //});
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                {
                    var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x => x.AppActionType).FirstOrDefault();
                    if (appActionType == 402)
                    {
                        var feederails30 = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == 30).FirstOrDefault();
                        if(feederails30 != null)
                        {
                            feeHeaderId = 30;
                            feesHeader = await GetFeeHeader(feeHeaderId);
                            var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                            genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                            {
                                FeeHeaderId = feeHeaderId,
                                FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                                AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                                Description = calculatedFeeDetail.Description,
                                IsDeduductible = false,
                                AppRefId = appRefId
                            });
                        }
                        var feederails47 = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == 47).FirstOrDefault();
                        if (feederails47 != null)
                        {
                            feeHeaderId = 47;
                            feesHeader = await GetFeeHeader(feeHeaderId);
                            var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                            genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                            {
                                FeeHeaderId = feeHeaderId,
                                FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                                AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                                Description = calculatedFeeDetail.Description,
                                IsDeduductible = false,
                                AppRefId = appRefId
                            });
                        }

                        
                    }
                    else
                    {

                        feeHeaderId = 11;
                        if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                        {
                            feeHeaderId = 12;
                        }
                        else if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            feeHeaderId = 13;
                        }
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });

                        feeHeaderId = 15;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        }); 
                    }
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                {
                    feeHeaderId = 16;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }
                else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                {
                    var commonApplication = await _iGR_CommonLicence_GeneralDetail.GetAsync(x => x.AppRefId == appRefId, null, x => x.CommonLicences_SelectedLicenceMapping);
                    if (commonApplication.FirstOrDefault().CommonLicences_SelectedLicenceMapping.IsFactory == true)
                    {
                        feeHeaderId = 3;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    if (commonApplication.FirstOrDefault().CommonLicences_SelectedLicenceMapping.IsBeediOrCigar == true)
                    {
                        feeHeaderId = 7;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    if (commonApplication.FirstOrDefault().CommonLicences_SelectedLicenceMapping.IsPrincipalEmployer == true)
                    {
                        feeHeaderId = 11;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    if (commonApplication.FirstOrDefault().CommonLicences_SelectedLicenceMapping.IsPrincipalEmployer == true)
                    {
                        feeHeaderId = 15;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    feeHeaderId = 18;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId,
                        PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                    });
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
                {
                    feeHeaderId = 16;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }
                else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x=> x.AppActionType).FirstOrDefault();
                    if(appActionType == 402)
                    {
                        feeHeaderId = 30;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    else
                    {
                        feeHeaderId = 3;
                        if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                        {
                            feeHeaderId = 4;
                        }
                        else if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            feeHeaderId = 5;
                        }

                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                }
                else if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                {
                    feeHeaderId = 32;
                    if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                    {
                        feeHeaderId = 32;
                    }
                    else if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        feeHeaderId = 33;
                    }
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId,
                        PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                    });
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                {
                    feeHeaderId = 16;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING)
                {
                    feeHeaderId = 16;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    feeHeaderId = 16;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }
                else if (applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                {

                    var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x => x.AppActionType).FirstOrDefault();
                    if (appActionType == 402)
                    {
                        feeHeaderId = 30;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    else
                    {
                        feeHeaderId = 34;
                       if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            feeHeaderId = 35;
                        }
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }

                }
                else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
               {
                   feeHeaderId = 36;
                   feesHeader = await GetFeeHeader(feeHeaderId);
                   var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                   genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                   {
                       FeeHeaderId = feeHeaderId,
                       FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                       AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                       Description = calculatedFeeDetail.Description,
                       IsDeduductible = false,
                       AppRefId = appRefId,
                       PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                   });
               }
                else if (applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                {

                    var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x => x.AppActionType).FirstOrDefault();
                    if (appActionType == 402)
                    {
                        feeHeaderId = 30;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                    else
                    {
                        feeHeaderId = 42;
                        if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            feeHeaderId = 43;
                        }
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }

                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    feeHeaderId = 19; // BOCW-Cess
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = "", //calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId,
                        PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                    });

                    feeHeaderId = 39; // Scrutiny Fee- PSIEC
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = "", //calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId,
                        PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                    });
                }
                else if (applicationType == ApplicationTypeEnum.TRADE_UNION)
                {
                    feeHeaderId = 40;
                    if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        feeHeaderId = 41;
                    }
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId,
                        PaymentBatchCounter = calculatedFeeDetail.PaymentBatchCounter
                    });
                }
                else if (applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                {
                    var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x => x.AppActionType).FirstOrDefault();
                    if (appActionType == 402)
                    {
                        var feeDetail30 = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == 30).FirstOrDefault();
                        if (feeDetail30 != null)
                        {
                            feeHeaderId = 30;
                            feesHeader = await GetFeeHeader(feeHeaderId);
                            var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                            genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                            {
                                FeeHeaderId = feeHeaderId,
                                FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                                AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                                Description = calculatedFeeDetail.Description,
                                IsDeduductible = false,
                                AppRefId = appRefId
                            });
                        }
                        var feeDetail47 = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == 47).FirstOrDefault();
                        if (feeDetail47 != null)
                        {
                            feeHeaderId = 47;
                            feesHeader = await GetFeeHeader(feeHeaderId);
                            var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                            genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                            {
                                FeeHeaderId = feeHeaderId,
                                FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                                AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                                Description = calculatedFeeDetail.Description,
                                IsDeduductible = false,
                                AppRefId = appRefId
                            });
                        }


                    }
                    else
                    {
                        feeHeaderId = 44;
                        if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                        {
                            feeHeaderId = 45;
                        }
                        else if (application.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            feeHeaderId = 46;
                        }
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });

                        feeHeaderId = 15;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount, //await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId).Result.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });
                    }
                }
                else if (applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
                {
                    //var appActionType = _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).Select(x => x.AppActionType).FirstOrDefault();
                    
                        feeHeaderId = 51;
                        feesHeader = await GetFeeHeader(feeHeaderId);
                        var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, application.FirstOrDefault().ApplicationPurposeType);
                        genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                        {
                            FeeHeaderId = feeHeaderId,
                            FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                            AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                            Description = calculatedFeeDetail.Description,
                            IsDeduductible = false,
                            AppRefId = appRefId
                        });

                    
                }
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, applicationType, entityKeyId, "PAYMNT");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<CalculatedFeeInfoViewModel> CalculateFeeAmountHeaderwise(long appRefId, ApplicationTypeEnum applicationType, Int64 feeHeaderId, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            CalculatedFeeInfoViewModel calculatedFeeInfo = new CalculatedFeeInfoViewModel();
            if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH)
            {
                //var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.BP_GeneralDetail);
                //if (parentWithChildObjects.FirstOrDefault().BP_GeneralDetail.IsBuildingContructedBefore_01_10_2008 == YesNoEnum.NO)
                //{
                //    if (parentWithChildObjects.FirstOrDefault().ApplicationPurposeType == ApplicationPurposeTypeEnum.BP_PROPOSED_PLAN)
                //    {
                //        //if(parentWithChildObjects.FirstOrDefault().BP_GeneralDetail.==)
                //        return 500;
                //    }
                //}
                //else
                //{
                //    return 0;
                //}
                if (feeHeaderId == 2)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Establishment_GeneralDetail.Establishment_EmployerDetail);

                    List<EstablishmentRegistrationFeeSlabViewModel> establishmentRegistrationFeeSlabs = new List<EstablishmentRegistrationFeeSlabViewModel>()
                    {
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1000,
                                IsSelected=false
                            }

                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=7500,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=12500,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 750
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=751,
                                MaxVal = 1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=17500,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new EstablishmentRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5001,
                                MaxVal = -1
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        }
                    };

                    int maxEmployees = parentWithChildObjects.FirstOrDefault().Establishment_GeneralDetail.Establishment_EmployerDetail.MaxEmployeesToBeEmployedAnyDay;
                    var selectedSlabAmount = (establishmentRegistrationFeeSlabs = establishmentRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                    && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                    calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString();
                    calculatedFeeInfo.establishmentRegistrationFeeSlabs = establishmentRegistrationFeeSlabs;
                }
            }
            else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
            {
                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_ContractLabour_GeneralDetails);
                if (feeHeaderId == 30) // Additional Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else if (feeHeaderId == 47) // Additional Security Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else
                {
                    if (feeHeaderId == 11 || feeHeaderId == 12 || feeHeaderId == 13)
                    {

                        List<ContractLabourRegistrationFeeSlabViewModel> contractLabourRegistrationFeeSlabs = new List<ContractLabourRegistrationFeeSlabViewModel>()
                        {
                            new ContractLabourRegistrationFeeSlabViewModel ()
                            {
                                NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                                {
                                    MinVal=0,
                                    MaxVal = 200
                                },
                                AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                                {
                                    FeeAmount=1500,
                                    IsSelected=false
                                }
                            },
                            new ContractLabourRegistrationFeeSlabViewModel ()
                            {
                                NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                                {
                                    MinVal=201,
                                    MaxVal = 400
                                },
                                AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                                {
                                    FeeAmount=3000,
                                    IsSelected=false
                                }
                            },
                            new ContractLabourRegistrationFeeSlabViewModel ()
                            {
                                NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                                {
                                    MinVal=400,
                                    MaxVal = 20000
                                },
                                AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                                {
                                    FeeAmount=4500,
                                    IsSelected=false
                                }
                            }
                        };


                        int maxEmployees = 0;
                        var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails;


                        if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            var latestAmendmentData = _context.Licence_ContractLabour_AmendmentDataHistories
                                                 .Where(x => x.AppRefId == appRefId)
                                                 .OrderByDescending(x => x.ModifiedOn)
                                                 .FirstOrDefault();

                            if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                            {
                                maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                                var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                                && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                StringBuilder descStr = new StringBuilder(
                                      "<table class=\"table table-bordered\">" +
                                          "<thead>" +
                                              "<tr>" +
                                                    "<td></td>" +
                                                    "<td class=\"text-right font-weight-bold\">Registration from date:</td>" +
                                                    "<td>" + DateTime.Now.ToLongDateString() + "</td>" +
                                                    "<td class=\"text-right font-weight-bold\">Total no. of Worker</td>" +
                                                    "<td class=\"text-center\">" + maxEmployees.ToString() + "</td>" +
                                              "</tr>" +
                                              "<tr class=\"text-center\">" +
                                                  "<th>Sr No.</th>" +
                                                  "<th>Fee for the year</th>" +
                                                  "<th>Annual Fee<br>(as per fee slab)</th>" +
                                                  "<th>Late fee</th>" +
                                                  "<th>Total fee<br>(Annual fee + late fee)</th>" +
                                              "</tr>" +
                                          "</thead>" +
                                          "<tbody>" +
                                            "TBRS" +
                                          "</tbody>" +
                                      "</table>");

                                StringBuilder rows = new StringBuilder();

                                decimal totAmount = 0;
                                decimal lateFee = 0;
                                DateTime date60DaysAgo = DateTime.Now.AddDays(-30);
                                var licneceyerar = parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.LicenceForYear;
                                var currentYear = DateTime.Now.Year;
                                var decFirst = new DateTime(currentYear, 12, 1);

                                if (licneceyerar > currentYear)
                                {
          
                                    lateFee = Math.Ceiling((selectedSlabAmount * 25) / 100);
                                }
                                else if (licneceyerar == currentYear)
                                {
                                    if (DateTime.Now <= decFirst)
                                    {
                                        lateFee = Math.Ceiling((selectedSlabAmount * 25) / 100);
                                    }
                                    else
                                    {
                                        lateFee = 0;
                                    }
                                }
                                else
                                {
                                    lateFee = 0;
                                }

                                if (latestAmendmentData != null)
                                {
                                    lateFee = lateFee;
                                    totAmount += (selectedSlabAmount + lateFee + 150);
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Renewel" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                                    rows.Append("<tr><td class=\"text-center\">" + 2 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Amendment" + "</td><td class=\"text-right\">" + "150" + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + "150" + "</td></tr>");
                                    calculatedFeeInfo.CalculatedAmount = totAmount;
                                }
                                else
                                {
                                    lateFee = lateFee;
                                    totAmount += (selectedSlabAmount + lateFee);
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Renewel" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                                    calculatedFeeInfo.CalculatedAmount = totAmount;
                                }
                                descStr.Replace("TBRS", rows.ToString());
                                descStr.Replace("GRANDTOTAL", totAmount.ToString());
                                calculatedFeeInfo.Description = descStr.ToString();
                                calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                                calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                            }
                            else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                            {
                                StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                  "<thead>" +
                                  "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                  "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                  "</thead>" +
                                  "<tbody>" +
                                  "TBRS" +
                                  "</tbody>" +
                                  "</table>");

                                StringBuilder rows = new StringBuilder();
                                rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Amendment" + "</td><td class=\"text-right\">" + "150" + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + "150" + "</td></tr>");
                                descStr.Replace("TBRS", rows.ToString());
                                calculatedFeeInfo.Description = descStr.ToString();
                                calculatedFeeInfo.CalculatedAmount = 150;
                                calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                                calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                            }

                        }
                        else
                        {
                            maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                            var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                            && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                           "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                           "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "</table>");

                            StringBuilder rows = new StringBuilder();
                            rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");
                            descStr.Replace("TBRS", rows.ToString());
                            calculatedFeeInfo.Description = descStr.ToString();
                            calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }


                    }
                    else if (feeHeaderId == 15)
                    {
                        int maxEmployees = 0;
                        var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails;
                        List<ContractLabourRegistrationFeeSlabViewModel> contractLabourRegistrationFeeSlabs = new List<ContractLabourRegistrationFeeSlabViewModel>()
                    {
                        new ContractLabourRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=270,
                                IsSelected=false
                            }
                        },
                    };

                        var latestAmendmentData = _context.Licence_ContractLabour_AmendmentDataHistories
                            .Where(x => x.AppRefId == appRefId).ToList();

                        if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                        {
                            maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;

                            var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                            ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            calculatedFeeInfo.CalculatedAmount = selectedSlabAmount * maxEmployees;
                            calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString() + " X 270";
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }
                        else if (latestAmendmentData != null && (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE))
                        {
                            maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                            var sections = latestAmendmentData.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                            if (sections == null)
                            {

                                calculatedFeeInfo.CalculatedAmount = 0;
                                calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString();
                                calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                                calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                            }
                            else
                            {
                                var oldmaxworker = _context.ApplicationLicenceNoMapping
                                    .Where(a => a.LicenceNumber == parentWithChildObjects.FirstOrDefault().Legacy_LicenceNo)
                                    .Join(_context.Licence_ContractLabour_GeneralDetails,
                                          a => a.AppRefId,
                                          al => al.AppRefId,
                                          (a, al) => al.MaximumNumberOfEmployee)
                                    .OrderByDescending(x => x)
                                    .FirstOrDefault();

                                int updatedEmployee = maxEmployees - (int)Convert.ToInt64(oldmaxworker);
                                var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => updatedEmployee >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                                ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                calculatedFeeInfo.CalculatedAmount = (selectedSlabAmount * updatedEmployee);
                                //calculatedFeeInfo.Description = "Old employee as Per Old licence No : " + oldmaxworker + ",Fee calculated as per balance employees (New Employee - Old Employee)" + 
                                //    maxEmployees + " - " + oldmaxworker + "to be employed: " + updatedEmployee.ToString() + " X 270";
                                //calculatedFeeInfo.Description =
                                //$"As per the old licence, the number of employees was {oldmaxworker}. " +
                                //$"The updated employee count is {maxEmployees}. " +
                                //$"Additional employees to be employed: {updatedEmployee}. " +
                                //$"The fee is calculated as {updatedEmployee} × 270.";


                                calculatedFeeInfo.Description =
                                "<b>As per the old licence, the number of worker was:</b> " +
                                "<span style='color:blue;'>" + oldmaxworker + "</span><br/>" +

                                "<b>New Workers:</b> " +
                                "<span style='color:green;'>" + maxEmployees + "</span><br/>" +

                                "<b>Additional Workers:</b> " +
                                "<span style='color:red;'>" + updatedEmployee + "</span><br/>" +

                                "<b>Fee Calculation:</b> " +
                                "<span style='color:purple; font-weight:bold;'>" +
                                updatedEmployee + " × 270</span>";


                                calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                                calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                            }

                        }
                        else
                        {
                            maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                            var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                            ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            calculatedFeeInfo.CalculatedAmount = 0;
                            calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString();
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }
                    }
                }
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
            {
                if (feeHeaderId == 16)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.BuildingPlan.BuildingPlan_AreaDetail);
                    int area = parentWithChildObjects.FirstOrDefault().BuildingPlan.BuildingPlan_AreaDetail.FirstOrDefault().Area;
                    int fee = Convert.ToInt32(area * 2.80);

                    List<BuildingPlanRegistrationFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanRegistrationFeeSlabViewModel>()
                    {
                        new BuildingPlanRegistrationFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= fee,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = fee;
                    calculatedFeeInfo.Description = "Fee calculated as per building area : " + area.ToString() + " square feet";
                    calculatedFeeInfo.buildingPlanRegistrationFeeSlab = buildingPlanRegistrationFeeSlabs;
                }
            }
            else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
            {
                if (feeHeaderId == 3) // Fee as a factory
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Contractor_GeneralDetail.Contractor_PrincipalEmployer);

                    List<CommonLicenceRegistrationFeeSlabViewModel> commonLicenceRegistrationFeeSlab = new List<CommonLicenceRegistrationFeeSlabViewModel>()
                    {
                        // Fee slab as per the no of worker and without power.
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal=0
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (1-50)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (51-100)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (101-150)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2500,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                  MinVal=101,
                                  MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=101,
                               MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=101,
                               MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=101,
                               MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=101,
                               MaxVal=150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (151-250)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=151,
                               MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (251-500)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                               MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                               MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                               MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=251,
                               MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with no of worker & electricity load (501-1000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=501,
                               MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=501,
                               MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=501,
                               MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=501,
                               MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=350000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=501,
                               MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },

                         // Fee slab with no of worker & electricity load (1001-2000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                               MinVal=1001,
                               MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal=2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },

                         // Fee slab with no of worker & electricity load (2001-3000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=80000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal=3000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=80000,
                                IsSelected=false
                            }
                        },

                          // Fee slab with no of worker & electricity load (3001-4000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=80000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=90000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal=4000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=90000,
                                IsSelected=false
                            }
                        },

                          // Fee slab with no of worker & electricity load (4001-5000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=80000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=90000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=100000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal=5000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=110000,
                                IsSelected=false
                            }
                        },

                          // Fee slab with no of worker & electricity load (Above 5000)
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 19
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=20,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=70000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=80000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = 3000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=90000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=3001,
                                MaxVal = 4000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=100000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=4001,
                                MaxVal = 5000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=110000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=120000,
                                IsSelected=false
                            }
                        },
                    };

                    var CommonLicence = parentWithChildObjects.FirstOrDefault().CommonLicence_GeneralDetail;
                    var selectedSlabsAsPerNoOfWorkers = (commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab.Where(
                        x => CommonLicence.TotalNoWorkersToBeEmployedInLicence >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                        (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : CommonLicence.TotalNoWorkersToBeEmployedInLicence <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)
                        ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).ToList();

                    var selectedSlabAmount = (commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab.Where(
                        x => CommonLicence.ElectricLoadConnectedInKilowatts >= x.ElectricityKWSlab.MinVal &&
                        (x.ElectricityKWSlab.MaxVal == -1 ? true : CommonLicence.ElectricLoadConnectedInKilowatts <= x.ElectricityKWSlab.MaxVal)
                        ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    calculatedFeeInfo.Description = "Fee calculated as per details below:</br> 1. Total Number of workers to be employed in the period of license are " + CommonLicence.TotalNoWorkersToBeEmployedInLicence.ToString() + ".</br> 2. Electricity load connected or to be connected (in K.W.) is " + CommonLicence.ElectricLoadConnectedInKilowatts.ToString() + "</br>For more details, For more details,</br><b> Please refer to the fee slabs Schedule-1 below</b>"; //+ maxEmployees.ToString();
                    calculatedFeeInfo.commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab;
                }
                else if (feeHeaderId == 7) // Beedi & Cigar
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.CommonLicence_GeneralDetail.CommonLicence_ContractorDetail);

                    List<CommonLicenceRegistrationFeeSlabViewModel> commonLicenceRegistrationFeeSlab = new List<CommonLicenceRegistrationFeeSlabViewModel>()
                    {
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=35000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=75000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 750
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=100000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=751,
                                MaxVal = 1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=125000,
                                IsSelected=false
                            }
                        },

                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = -1
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=150000,
                                IsSelected=false
                            }
                        }
                    };

                    int maxEmployees = parentWithChildObjects.FirstOrDefault().CommonLicence_GeneralDetail.TotalNoWorkersToBeEmployedInLicence;
                    var selectedSlabAmount = (commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                    && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString() + "</br><b>Please refer to the fee slabs Schedule - II below for more details.</b> ";
                    calculatedFeeInfo.commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab;
                }
                else if (feeHeaderId == 11) // Engagement of Contractor
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Contractor_GeneralDetail.Contractor_PrincipalEmployer);

                    List<CommonLicenceRegistrationFeeSlabViewModel> commonLicenceRegistrationFeeSlab = new List<CommonLicenceRegistrationFeeSlabViewModel>()
                    {
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=5000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=20000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=25000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 750
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=751,
                                MaxVal = 1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=40000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=60000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2001,
                                MaxVal = -1
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=75000,
                                IsSelected=false
                            }
                        }
                    };

                    int maxEmployees = parentWithChildObjects.FirstOrDefault().CommonLicence_GeneralDetail.TotalNoWorkersToBeEmployedInLicence;
                    var selectedSlabAmount = (commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                    && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString() + "</br><b> Please refer to the fee slabs Schedule - III below for more details.</b>";
                    calculatedFeeInfo.commonLicenceRegistrationFeeSlab = commonLicenceRegistrationFeeSlab;
                }
                else if (feeHeaderId == 15) //Engagement of Contractor Security Fee
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.CommonLicence_GeneralDetail.CommonLicence_ContractorDetail);

                    List<CommonLicenceRegistrationFeeSlabViewModel> commonLicenceContractorRegistrationFeeSlabs = new List<CommonLicenceRegistrationFeeSlabViewModel>()
                    {
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=50000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 150
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=100000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=151,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=150000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=200000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=250000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 750
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=300000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=751,
                                MaxVal = 1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=400000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = 1500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=500000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1501,
                                MaxVal = 2000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=600000,
                                IsSelected=false
                            }
                        },
                        new CommonLicenceRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=2000,
                                MaxVal = -1
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=750000,
                                IsSelected=false
                            }
                        }
                    };

                    int maxEmployees = parentWithChildObjects.FirstOrDefault().CommonLicence_GeneralDetail.TotalNoWorkersToBeEmployedInLicence;
                    var selectedSlabAmount = (commonLicenceContractorRegistrationFeeSlabs = commonLicenceContractorRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                    && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString() + "</br><b>For more details, please refer to the fee slabs Schedule - III below.</b>";
                    calculatedFeeInfo.commonLicenceRegistrationFeeSlab = commonLicenceContractorRegistrationFeeSlabs;
                }
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
            {
                if (feeHeaderId == 18)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.BuildingPlanHUD_GeneralDetail);
                    int area = Convert.ToInt32(Math.Ceiling(parentWithChildObjects.FirstOrDefault().BuildingPlanHUD_GeneralDetail.PlotAreaAcres));

                    int fee = 0;
                    string subsequentAcresFeeDesc = "";
                    //if (area > 1)
                    //{
                    //    fee = (area - 1) * 1000;
                    //    subsequentAcresFeeDesc = " + Rs. " + fee.ToString() + "/- (Rs. 1000 for each subsequest " + (area - 1).ToString() + " acres)";
                    //}

                    //fee += 5000;

                    // Fee changed - Dated On : 14-Aug-2025 :: - First acre : 7500/- and for each subsequest acres 1500/-
                    if (area > 1)
                    {
                        fee = (area - 1) * 1500;
                        subsequentAcresFeeDesc = " + Rs. " + fee.ToString() + "/- (Rs. 1500 for each subsequest " + (area - 1).ToString() + " acres)";
                    }

                    fee += 7500;

                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= fee,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = fee;
                    //calculatedFeeInfo.Description = "For first acre Rs. 5000/-" + subsequentAcresFeeDesc;
                    calculatedFeeInfo.Description = "For first acre Rs. 7500/-" + subsequentAcresFeeDesc;
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                    calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                }
                else if (feeHeaderId == 19)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.BuildingPlanHUD_GeneralDetail);
                    int CessAmount = (Convert.ToInt32((parentWithChildObjects.FirstOrDefault().BuildingPlanHUD_GeneralDetail.BuildingCost) / 100) * 1);

                    int fee = CessAmount;

                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                //FeeAmount= fee,
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0; // fee;
                    calculatedFeeInfo.Description = "1 % of building cost as per CA certificate";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 20)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "Rs 60 per square meter of covered area.";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 21)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 22)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 23)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 24)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 25)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 26)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 27)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 28)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
                else if (feeHeaderId == 29)
                {
                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = 0;
                    calculatedFeeInfo.Description = "";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
            {
                if (feeHeaderId == 16)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.BuildingPlanFactory_GeneralDetail);
                    int CessAmount = (Convert.ToInt32((parentWithChildObjects.FirstOrDefault().BuildingPlanFactory_GeneralDetail.BuildingCost) / 100) * 1);

                    int fee = CessAmount;

                    List<BuildingPlanHUDFeeSlabViewModel> buildingPlanRegistrationFeeSlabs = new List<BuildingPlanHUDFeeSlabViewModel>()
                    {
                        new BuildingPlanHUDFeeSlabViewModel ()
                        {
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount= 0,
                                IsSelected=false
                            }
                        },
                    };

                    calculatedFeeInfo.CalculatedAmount = fee;
                    calculatedFeeInfo.Description = "1 % of building cost as per CA certificate";
                    calculatedFeeInfo.BuildingPlanHUDFeeSlabs = buildingPlanRegistrationFeeSlabs;
                }
            }
            else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
            {
                //var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_Factory_GeneralDetails);
                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_Factory_GeneralDetails, x => x.Licence_Factory_GeneralDetails.Licence_Factory_OccupierAndManagerDetail);
                if (feeHeaderId == 30) // Additional Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else
                {
                    List<FactoryLicenceFeeSlabViewModel> factoryLicenceRegistrationFeeSlab = new List<FactoryLicenceFeeSlabViewModel>()
                    {
                        // Fee slab with worker & electricity upto-20
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=450,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=750,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=7500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal=20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },

                //paymentGatewayClientFormInputDetails = new PaymentGatewayClientFormInputDetailsViewModel();
                //paymentGatewayClientFormInputDetails.PaymentGatewayTargetUrl = paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl;
                        // Fee slab with worker & electricity 21-50
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=675,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2250,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=7500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal=50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with worker & electricity 51-100
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2250,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=7500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal=100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=18000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with worker & electricity 101-250
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2250,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=18000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal=250
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=22500,
                                IsSelected=false
                            }
                        },

                        // Fee slab with worker & electricity 251-500
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=7500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=12000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=18000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=22500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal=500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },

                        // Fee slab with worker & electricity 501-1000
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=22500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal=1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=37500,
                                IsSelected=false
                            }
                        },

                        // Fee slab with worker & electricity above-1000
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 20
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 250
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=22500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=251,
                                MaxVal = 500
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=30000,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=37500,
                                IsSelected=false
                            }
                        },
                        new FactoryLicenceFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1001,
                                MaxVal = int.MaxValue
                            },
                            ElectricityKWSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal=int.MaxValue
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=45000,
                                IsSelected=false
                            }
                        },
                    };

                    var factoryLicence = parentWithChildObjects.FirstOrDefault().Licence_Factory_GeneralDetails;
                    decimal highestPower = factoryLicence.PowerKW_Installed > factoryLicence.PowerKW_MaxProposed ? factoryLicence.PowerKW_Installed : factoryLicence.PowerKW_MaxProposed;
                    Int64 highestWorkers = factoryLicence.Workers_MaxDuringYear > factoryLicence.Workers_OrdinarilyEmployed ? factoryLicence.Workers_MaxDuringYear : factoryLicence.Workers_OrdinarilyEmployed;

                    var selectedSlabAmount = factoryLicenceRegistrationFeeSlab.Where(x =>
                        highestPower >= x.ElectricityKWSlab.MinVal &&
                        highestPower <= x.ElectricityKWSlab.MaxVal &&
                        highestWorkers >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                        highestWorkers <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    //var application = await _iGR_Application.GetAsync(x => x.AppId == appRefId).ConfigureAwait(false);

                    if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                            "<thead>" +
                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + Convert.ToDateTime(factoryLicence.RegistrationDate).ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of years</td><td class=\"text-center\">" + factoryLicence.NoOfYears.ToString() + "</td></tr>" +
                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Total power Installed(KW)</td><td class=\"text-center\">" + highestPower.ToString() + "</td><td class=\"text-right font-weight-bold\">Total manpower</td><td class=\"text-center\">" + highestWorkers.ToString() + "</td></tr>" +
                                            "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee(if any)<br>(25 % of the annual fee)</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                            "TBRS" +
                                            "</tbody>" +
                                            "<tfoot>" +
                                            "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                            "</tfoot>" +
                                            "</table>");

                        StringBuilder rows = new StringBuilder();

                        decimal totAmount = 0;
                        int srNo = 1;
                        decimal lateFee = 0;
                        for (int i = factoryLicence.RegistrationDate.Value.Year; i <= factoryLicence.RegistrationDate.Value.Year + (factoryLicence.NoOfYears - 1); i++)
                        {
                            lateFee = 0;
                            if (i > factoryLicence.RegistrationDate.Value.Year && i <= DateTime.Now.Year)
                            {
                                lateFee = Math.Ceiling((selectedSlabAmount * 25) / 100);

                            }
                            totAmount += (selectedSlabAmount + lateFee);
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-center\">" + i.ToString() + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                            srNo++;
                        }

                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", totAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = totAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        //calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                    }
                    else if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                    {
                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                            "<thead>" +
                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + Convert.ToDateTime(factoryLicence.RegistrationDate).ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of years</td><td class=\"text-center\">" + factoryLicence.NoOfYears.ToString() + "</td></tr>" +
                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Total power Installed(KW)</td><td class=\"text-center\">" + highestPower.ToString() + "</td><td class=\"text-right font-weight-bold\">Total manpower</td><td class=\"text-center\">" + highestWorkers.ToString() + "</td></tr>" +
                                            "<tr class=\"text-center\"><th>Sr No.</th><th>Particulars</th><th>Amount</th><th>Late fee(if any)<br>(25 % of the annual fee)</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                            "TBRS" +
                                            "</tbody>" +
                                            "<tfoot>" +
                                            "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                            "</tfoot>" +
                                            "</table>");

                        StringBuilder rows = new StringBuilder();
                        decimal totAmount = 0;
                        int srNo = 1;
                        decimal lateFee = 0;
                        var lateFeeStartDate = new DateTime(DateTime.Now.Year, 12, 2);
                        for (int i = factoryLicence.RenewalFromDate.Value.Year; i <= factoryLicence.RenewalFromDate.Value.Year + (factoryLicence.NoOfYears - 1); i++)
                        {
                            lateFee = 0;
                            if (i <= DateTime.Now.Year || (i == DateTime.Now.Year + 1 && DateTime.Now >= lateFeeStartDate))
                            {
                                lateFee = Math.Ceiling((selectedSlabAmount * 25) / 100);

                            }
                            totAmount += (selectedSlabAmount + lateFee);
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-center\">Fee for the year " + i.ToString() + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                            srNo++;
                        }

                        var amendmentDataHistories = _context.Licence_Factory_AmendmentDataHistories.Where(x => x.AppRefId == appRefId && x.ModifiedCounter == factoryLicence.ModifiedCounter).ToList();
                        if (amendmentDataHistories.Count() > 0)
                        {
                            foreach (var item in amendmentDataHistories)
                            {

                                totAmount += 100;
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-center\">Amendment fee of column " + item.FieldName.ToString() + "</td><td class=\"text-right\">100</td><td class=\"text-right\">0</td><td class=\"text-right\">0</td></tr>");
                                srNo++;
                            }
                        }

                        if (amendmentDataHistories.Any(x => x.FieldName == "Workers_MaxDuringYear" || x.FieldName == "PowerKW_Installed"))
                        {
                            Int64 previous_Workers_MaxDuringYear = factoryLicence.Workers_MaxDuringYear;
                            if (amendmentDataHistories.Any(x => x.FieldName == "Workers_MaxDuringYear"))
                            {
                                previous_Workers_MaxDuringYear = Convert.ToInt64(amendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").Select(x => x.PreviousValue).FirstOrDefault());
                            }

                            decimal previous_PowerKW_Installed = factoryLicence.PowerKW_Installed;
                            if (amendmentDataHistories.Any(x => x.FieldName == "PowerKW_Installed"))
                            {
                                previous_PowerKW_Installed = Convert.ToDecimal(amendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").Select(x => x.PreviousValue).FirstOrDefault());
                            }

                            var feeAsPerOldValues = factoryLicenceRegistrationFeeSlab.Where(x =>
                               previous_PowerKW_Installed >= x.ElectricityKWSlab.MinVal &&
                               previous_PowerKW_Installed <= x.ElectricityKWSlab.MaxVal &&
                               previous_Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                               previous_Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                            var feeAsPerCurrentValues = factoryLicenceRegistrationFeeSlab.Where(x =>
                                factoryLicence.PowerKW_Installed >= x.ElectricityKWSlab.MinVal &&
                                factoryLicence.PowerKW_Installed <= x.ElectricityKWSlab.MaxVal &&
                                factoryLicence.Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                                factoryLicence.Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            //if (feeAsPerCurrentValues > feeAsPerOldValues)
                            //{
                            //    totAmount += (feeAsPerCurrentValues - feeAsPerOldValues);
                            //    rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">Changes in values in Power KW Installed/Workers Max During Year <br>Previously fees paid: " + feeAsPerOldValues.ToString() + "<br>Fees calculated as per new slab: " + feeAsPerCurrentValues.ToString() + "</td><td class=\"text-left\">Power KW Installed: " + previous_PowerKW_Installed.ToString() + "<br><br>Workers Max During Year: " + previous_Workers_MaxDuringYear.ToString() + "</td><td class=\"text-left\">Power KW Installed: " + factoryLicence.PowerKW_Installed.ToString() + "<br><br>Workers Max During Year: " + factoryLicence.Workers_MaxDuringYear.ToString() + "</td><th class=\"text-right\">" + (feeAsPerCurrentValues - feeAsPerOldValues).ToString() + "</th></tr>");
                            //    srNo++;
                            //}
                        }

                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", totAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = totAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }
                    else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                            "<tr class=\"text-center\"><th>Sr No.</th><th class=\"text-left\">Amendment in section</th><th class=\"text-left\">Detail</th><th>Amendment charges</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "<tfoot>" +
                                           "<tr><td colspan = \"3\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                           "</tfoot>" +
                                           "</table>");

                        StringBuilder rows = new StringBuilder();
                        decimal totAmount = 0;
                        int srNo = 1;
                        //&& x.ModifiedCounter == factoryLicence.ModifiedCounter

                        var amendmentDataHistories = _context.Licence_Factory_AmendmentDataHistories
                            .Where(x => x.AppRefId == appRefId &&
                            (x.ModifiedCounter == factoryLicence.ModifiedCounter || x.ModifiedCounter == factoryLicence.Licence_Factory_OccupierAndManagerDetail.ModifiedCounter)).ToList();

                        //var amendmentDataHistories = _context.Licence_Factory_AmendmentDataHistories.Where(x => x.AppRefId == appRefId && x.ModifiedCounter == factoryLicence.ModifiedCounter).ToList();
                        if (amendmentDataHistories.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            var sections = amendmentDataHistories.Select(x => x.SectionCode).Distinct();
                            foreach (var sec in sections)
                            {
                                totAmount += 100;
                                sb.Clear();
                                sb.Append("<div class=\"row\">" +
                                    "<div class=\"col-sm-4 border bg-info\">Column Name</div><div class=\"col-sm-4 border bg-info\">Previous Value</div><div class=\"col-sm-4 border bg-info\">Modified Value</div>");

                                foreach (var item in amendmentDataHistories.Where(x => x.SectionCode == sec).ToList())
                                {

                                    //rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">Amendment fee of column : " + item.FieldName.ToString() + "</td><td class=\"text-left\">" + item.PreviousValue.ToString() + "</td><td class=\"text-left\">" + item.ModifiedValue.ToString() + "</td><th class=\"text-right\">100</th></tr>");
                                    //sb.Append("<td class=\"text-left\">" + item.FieldName.ToString() +
                                    //    "</td><td class=\"text-left\">" + item.PreviousValue.ToString() +
                                    //    "</td><td class=\"text-left\">" + item.ModifiedValue.ToString() +
                                    //    "</td></tr>");
                                    sb.Append("<div class=\"col-sm-4 border\">" + item.FieldName.ToString() + "</div><div class=\"col-sm-4 border\">" + item.PreviousValue.ToString() + "</div><div class=\"col-sm-4 border\">" + item.ModifiedValue.ToString() + "</div>");

                                }
                                sb.Append("</div>");
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                        "</td><td class=\"text-left\">" + (sec == "MD" ? "Manager Details" : sec == "OD" ? "Occupier Details" : sec == "KW" ? "Installed Power(KW)" : "MP") +
                                        "</td><td class=\"text-left\" style=\"padding: 25px;\">" + sb.ToString() +
                                        "</td><th class=\"text-right\" style=\"vertical-align: middle;\">100</th></tr>");
                                srNo++;
                            }
                        }

                        if (amendmentDataHistories.Any(x => x.FieldName == "Workers_MaxDuringYear" || x.FieldName == "PowerKW_Installed"))
                        {
                            Int64 previous_Workers_MaxDuringYear = factoryLicence.Workers_MaxDuringYear;
                            if (amendmentDataHistories.Any(x => x.FieldName == "Workers_MaxDuringYear"))
                            {
                                previous_Workers_MaxDuringYear = Convert.ToInt64(amendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").Select(x => x.PreviousValue).FirstOrDefault());
                            }

                            decimal previous_PowerKW_Installed = factoryLicence.PowerKW_Installed;
                            if (amendmentDataHistories.Any(x => x.FieldName == "PowerKW_Installed"))
                            {
                                previous_PowerKW_Installed = Convert.ToDecimal(amendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").Select(x => x.PreviousValue).FirstOrDefault());
                            }

                            var feeAsPerOldValues = factoryLicenceRegistrationFeeSlab.Where(x =>
                               previous_PowerKW_Installed >= x.ElectricityKWSlab.MinVal &&
                               previous_PowerKW_Installed <= x.ElectricityKWSlab.MaxVal &&
                               previous_Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                               previous_Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                            var feeAsPerCurrentValues = factoryLicenceRegistrationFeeSlab.Where(x =>
                                factoryLicence.PowerKW_Installed >= x.ElectricityKWSlab.MinVal &&
                                factoryLicence.PowerKW_Installed <= x.ElectricityKWSlab.MaxVal &&
                                factoryLicence.Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                                factoryLicence.Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            if (feeAsPerCurrentValues > feeAsPerOldValues)
                            {

                                int remainingYears = (factoryLicence.OldLicenceValidUpTo.Value.Year) - (factoryLicence.AmmendmentDate.Value.Year - 1);
                                totAmount += (feeAsPerCurrentValues - feeAsPerOldValues) * remainingYears;
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">Changes in values in Power KW Installed/Workers Max During Year <br>Previously fees paid: " + feeAsPerOldValues.ToString() + "<br>Fees calculated as per new slab: " + feeAsPerCurrentValues.ToString() + "</td><td class=\"text-left\">Power KW Installed: " + previous_PowerKW_Installed.ToString() + "<br><br>Workers Max During Year: " + previous_Workers_MaxDuringYear.ToString() + "</td><td class=\"text-left\">Power KW Installed: " + factoryLicence.PowerKW_Installed.ToString() + "<br><br>Workers Max During Year: " + factoryLicence.Workers_MaxDuringYear.ToString() + "</td><th class=\"text-right\">" + (feeAsPerCurrentValues - feeAsPerOldValues).ToString() + "</th></tr>");
                                srNo++;
                            }
                        }

                        if (totAmount == 0)
                        {
                            totAmount += 100;
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">Basic Ammendment Fee </td> <td class=\"text-left\"> " + "N/A" + " </td> <td class=\"text-right\">" + totAmount.ToString() + "</td> </th></tr>");
                        }


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", totAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = totAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }

                    calculatedFeeInfo.factoryLicenceFeeSlab = factoryLicenceRegistrationFeeSlab;
                }

            }
            else if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
            {

                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_BocwAct_GeneralDetail);

                List<BocwRegistrationFeeSlabViewModel> bocwRegistrationFeeSlabs = new List<BocwRegistrationFeeSlabViewModel>()
                    {
                        new BocwRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1000,
                                IsSelected=false
                            }
                        },
                        new BocwRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 500
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4000,
                                IsSelected=false
                            }
                        },
                        new BocwRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=501,
                                MaxVal = 1000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=8000,
                                IsSelected=false
                            }
                        },
                        new BocwRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=1000,
                                MaxVal = 10000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=10000,
                                IsSelected=false
                            }
                        },
                    };
                var applications = parentWithChildObjects.FirstOrDefault();
                int maxEmployees = parentWithChildObjects.FirstOrDefault().Licence_BocwAct_GeneralDetail.MaximumNoOfWorkers;
                long oldmaxEmployees = 0;
                if (applications.Legacy_AppId > 0)
                {
                    var OldHistory = _context.Licence_Bocw_AmendmentDataHistories.Where(x => x.AppRefId == applications.AppId).ToList();
                    oldmaxEmployees = Convert.ToInt64(OldHistory.Where(x => x.FieldName == "No_of_Worker").Select(x => x.PreviousValue).FirstOrDefault());
                }


                var selectedSlabAmount = bocwRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                        "<thead>" +
                                        "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + Convert.ToDateTime(applications.Licence_BocwAct_GeneralDetail.Work_CommencementDate).ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                        "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee(if any)<br>(50% Registration before 60 days from current date)</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                        "</thead>" +
                                        "<tbody>" +
                                        "TBRS" +
                                        "</tbody>" +
                                        "<tfoot>" +
                                        "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                        "</tfoot>" +
                                        "</table>");

                    StringBuilder rows = new StringBuilder();

                    decimal totAmount = 0;
                    int srNo = 1;
                    decimal lateFee = 0;
                    DateTime date60DaysAgo = DateTime.Now.AddDays(-60);
                    if (applications.Licence_BocwAct_GeneralDetail.Work_CommencementDate < date60DaysAgo)
                    {

                        lateFee = Math.Ceiling((selectedSlabAmount * 50) / 100);
                        totAmount += (selectedSlabAmount + lateFee);
                        rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-center\">" + "Fee for the BOCW Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                    }
                    else
                    {
                        totAmount = selectedSlabAmount;
                        rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-center\">" + "Fee for the BOCW Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");
                    }

                    descStr.Replace("TBRS", rows.ToString());
                    descStr.Replace("GRANDTOTAL", totAmount.ToString());
                    calculatedFeeInfo.Description = descStr.ToString();
                    calculatedFeeInfo.CalculatedAmount = totAmount;
                    calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    //calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                }
                else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    var oldselectedSlabAmount = bocwRegistrationFeeSlabs.Where(x => oldmaxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                    && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : oldmaxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                    StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                                        "<thead>" +
                                                        "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + Convert.ToDateTime(applications.Licence_BocwAct_GeneralDetail.Work_CommencementDate).ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                                        "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee(if any)<br>(50% Registration before 60 days from current date)</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                                        "</thead>" +
                                                        "<tbody>" +
                                                        "TBRS" +
                                                        "</tbody>" +
                                                        "<tfoot>" +
                                                        "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                                        "</tfoot>" +
                                                        "</table>");

                    StringBuilder rows = new StringBuilder();
                    decimal totAmount = 0;
                    decimal oldlatefees = 0;
                    decimal oldtotalfees = 0;
                    decimal amendmentFee = 50;
                    int srNo = 1;
                    //&& x.ModifiedCounter == factoryLicence.ModifiedCounter

                    var amendmentDataHistories = _context.Licence_Bocw_AmendmentDataHistories
                        .Where(x => x.AppRefId == appRefId).ToList();

                    if (amendmentDataHistories.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        var sections = amendmentDataHistories.Where(x => x.FieldName == "No_of_Worker").FirstOrDefault();
                        if (sections != null)
                        {
                            totAmount += 0;
                            sb.Clear();
                            sb.Append("<div class=\"row\">" +
                            "<div class=\"col-sm-4 border bg-info\">Column Name</div><div class=\"col-sm-4 border bg-info\">Previous Value</div><div class=\"col-sm-4 border bg-info\">Modified Value</div>");
                            sb.Append("<div class=\"col-sm-4 border\">" + "No O   f Worker" + "</div><div class=\"col-sm-4 border\">" + sections.PreviousValue.ToString() + "</div><div class=\"col-sm-4 border\">" + sections.ModifiedValue.ToString() + "</div>");


                            sb.Append("</div>");
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                "</td><td class=\"text-left\">" + "No of Worker" +
                                "</td><td class=\"text-left\" style=\"padding: 25px;\">" + sb.ToString() +
                                "</td><th class=\"text-right\" style=\"vertical-align: middle;\"></th></tr>");
                            srNo++;
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                "</td><td class=\"text-left\">" + "Amendment Fees" +
                                "</td><td class=\"text-right\">" + amendmentFee +
                                "</td><td class=\"text-right\">" + "0" +
                                "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                            srNo++;
                            totAmount += 50;
                        }
                        else
                        {
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                            "</td><td class=\"text-left\">" + "Amendment Fees" +
                            "</td><td class=\"text-right\">" + amendmentFee +
                            "</td><td class=\"text-right\">" + "0" +
                            "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                            srNo++;
                            totAmount += 50;
                        }


                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                            "</td><td class=\"text-left\">" + "Amendment Fees" +
                            "</td><td class=\"text-right\">" + amendmentFee +
                            "</td><td class=\"text-right\">" + "0" +
                            "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                        srNo++;
                        totAmount += 50;

                    }

                    if (amendmentDataHistories.Any(x => x.FieldName == "No_of_Worker"))
                    {
                        Int64 previous_Workers_MaxDuringYear = 0;
                        if (amendmentDataHistories.Any(x => x.FieldName == "No_of_Worker"))
                        {
                            previous_Workers_MaxDuringYear = Convert.ToInt64(amendmentDataHistories.Where(x => x.FieldName == "No_of_Worker").Select(x => x.PreviousValue).FirstOrDefault());
                        }


                        var feeAsPerOldValues = bocwRegistrationFeeSlabs.Where(x =>
                           previous_Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                           previous_Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                        var feeAsPerCurrentValues = bocwRegistrationFeeSlabs.Where(x =>
                            applications.Licence_BocwAct_GeneralDetail.MaximumNoOfWorkers >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                            applications.Licence_BocwAct_GeneralDetail.MaximumNoOfWorkers <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        //var OldFeeDetail = _context.AppFeeDetails.Where(x => x.AppRefId == applications.Legacy_AppId).FirstOrDefault();

                        if (feeAsPerCurrentValues > feeAsPerOldValues)
                        {
                            decimal lateFee = 0;
                            DateTime date60DaysAgo = DateTime.Now.AddDays(-60);
                            if (applications.Licence_BocwAct_GeneralDetail.Work_CommencementDate < date60DaysAgo)
                            {

                                lateFee = Math.Ceiling((selectedSlabAmount * 50) / 100);
                                totAmount += (selectedSlabAmount + lateFee);
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the BOCW Registration New Worker Slab" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                            }
                            else
                            {
                                lateFee = 0;
                                totAmount += (selectedSlabAmount + lateFee);
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the BOCW Registration New Worker Slab" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + (selectedSlabAmount + lateFee).ToString() + "</td></tr>");
                            }

                            oldlatefees = (oldselectedSlabAmount - oldselectedSlabAmount);
                            oldtotalfees += (oldselectedSlabAmount);
                            //rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">Changes in values in Workers <br>Previously fees paid: " + feeAsPerOldValues.ToString() + "<br>Fees calculated as per new slab: " + feeAsPerCurrentValues.ToString() +  "<br><br>Workers Max During Year: " + previous_Workers_MaxDuringYear.ToString() + "</td><th class=\"text-right\">" + (feeAsPerCurrentValues - feeAsPerOldValues).ToString() + "</th></tr>");
                            rows.Append("<tr style=\"background-color: #ff00003d;\"><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the BOCW Registration Old Worker Slab(Already Paid Fees)" + "</td><td class=\"text-right\">" + oldselectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + oldlatefees.ToString() + "</td><td class=\"text-right\">" + "- " + oldtotalfees.ToString("F0") + "</td></tr>");
                            srNo++;
                            totAmount -= oldtotalfees;
                        }
                    }


                    descStr.Replace("TBRS", rows.ToString());
                    descStr.Replace("GRANDTOTAL", totAmount.ToString());
                    calculatedFeeInfo.Description = descStr.ToString();
                    calculatedFeeInfo.CalculatedAmount = totAmount;
                }
                calculatedFeeInfo.bocwRegistrationFeeSlabs = bocwRegistrationFeeSlabs;

            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm() { ParmName = "ApplicationType", ParmValue = ((int)applicationType).ToString(), isNumber = true },
                    new StoreProcedureParm() { ParmName = "ApplicationPurposeType", ParmValue = ((int)applicationPurposeType).ToString(), isNumber = true }
                };
                var applicableRaiseFeeHeadsList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicableRaiseFeeHeadsViewModel>("dbo.sp_getApplicableRaiseFeeHeads", storeProcedureParms);
                calculatedFeeInfo.ApplicableRaiseFeeHeads = applicableRaiseFeeHeadsList;
            }
            else if (applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
            {
                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_CL_PE_GeneralDetail, x => x.Licence_CL_PE_GeneralDetail.Licence_CL_PE_Contrators);
                if (feeHeaderId == 30) // Additional Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else
                {

                    List<ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel> prinicpalemployerLabourRegistrationFeeSlabs = new List<ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel>()
                    {
                        new ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=6000,
                                IsSelected=false
                            }
                        },
                        new ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 400
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=12000,
                                IsSelected=false
                            }
                        },
                        new ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=400,
                                MaxVal = 40000
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=15000,
                                IsSelected=false
                            }
                        }
                    };
                    int maxWorkers = 0;
                    int Id = 0;
                    Int64 oldmaxWorkers = 0;
                    var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_CL_PE_GeneralDetail;
                    var applications = parentWithChildObjects.FirstOrDefault();
                    if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        Id = (int)parentWithChildObjects.FirstOrDefault().Licence_CL_PE_GeneralDetail.Id;
                        maxWorkers = _context.Licence_CL_PE_Contrator.Where(x => x.Licence_CL_PE_GeneralDetailRefId == Id).Sum(x => x.MaxLabourWorkerEmployed);
                        var selectedSlabAmount = prinicpalemployerLabourRegistrationFeeSlabs.Where(x => maxWorkers >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxWorkers <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                           "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxWorkers.ToString() + "</td></tr>" +
                                           "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "<tfoot>" +
                                           "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                           "</tfoot>" +
                                           "</table>");

                        StringBuilder rows = new StringBuilder();
                        rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Principal Employer Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", selectedSlabAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                    }
                    else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                                            "<thead>" +
                                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of worker</td><td class=\"text-center\">" + applications.Licence_CL_PE_GeneralDetail.TotalWorker.ToString() + "</td></tr>" +
                                                            "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th colspan = \"2\">Annual Fee<br>(as per fee slab)</th><th>Total fee<br>(Annual fee)</th></tr>" +
                                                            "</thead>" +
                                                            "<tbody>" +
                                                            "TBRS" +
                                                            "</tbody>" +
                                                            "<tfoot>" +
                                                            "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                                            "</tfoot>" +
                                                            "</table>");

                        StringBuilder rows = new StringBuilder();
                        decimal totAmount = 0;
                        decimal oldlatefees = 0;
                        decimal oldtotalfees = 0;
                        decimal amendmentFee = 150;
                        int srNo = 1;

                        var amendmentDataHistories = _context.Licence_PE_AmendmentDataHistories
                            .Where(x => x.AppRefId == appRefId &&
                            (x.ModifiedCounter == applications.Licence_CL_PE_GeneralDetail.ModifiedCounter)).ToList();

                        if (amendmentDataHistories.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            var sections = amendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();

                            if (sections != null)
                            {

                                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="Legacy_LicenceNo", ParmValue=parentWithChildObjects.FirstOrDefault().Legacy_LicenceNo.ToString(), isNumber=false},
                                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=parentWithChildObjects.FirstOrDefault().AppId.ToString(), isNumber=true},
                                };
                                var oldmaxworker = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_GetMaxLabourWorkerEmployedPrincipalEmp", storeProcedureParms);


                                totAmount += 0;
                                sb.Clear();
                                sb.Append("<div class=\"row\">" +
                                "<div class=\"col-sm-4 border bg-info\">Column Name</div><div class=\"col-sm-4 border bg-info\">Previous Value</div><div class=\"col-sm-4 border bg-info\">Modified Value</div>");
                                sb.Append("<div class=\"col-sm-4 border\">" + "Total Worker" + "</div><div class=\"col-sm-4 border\">" + oldmaxworker.Value.ToString() + "</div><div class=\"col-sm-4 border\">" + sections.ModifiedValue.ToString() + "</div>");


                                sb.Append("</div>");
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "No of Worker" +
                                    "</td><td class=\"text-left\" style=\"padding: 25px;\" colspan = \"2\">" + sb.ToString() +
                                    "</td><th class=\"text-right\" style=\"vertical-align: middle;\"></th></tr>");
                                srNo++;
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "Amendment Fees" +
                                    "</td><td class=\"text-center\" colspan = \"2\">" + amendmentFee +
                                    "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                                srNo++;
                                totAmount += 150;
                            }
                            else
                            {
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "Amendment Fees" +
                                    "</td><td class=\"text-right\">" + amendmentFee +
                                    "</td><td class=\"text-right\">" + "0" +
                                    "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                                srNo++;
                                totAmount += 150;
                            }


                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                "</td><td class=\"text-left\">" + "Amendment Fees" +
                                "</td><td class=\"text-right\">" + amendmentFee +
                                "</td><td class=\"text-right\">" + "0" +
                                "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                            srNo++;
                            totAmount += 150;

                        }

                        if (amendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                        {

                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="Legacy_LicenceNo", ParmValue=parentWithChildObjects.FirstOrDefault().Legacy_LicenceNo.ToString(), isNumber=false},
                                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=parentWithChildObjects.FirstOrDefault().AppId.ToString(), isNumber=true},
                                };
                            var previous_Workers_MaxDuringYear = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_GetMaxLabourWorkerEmployedPrincipalEmp", storeProcedureParms);



                            var feeAsPerOldValues = prinicpalemployerLabourRegistrationFeeSlabs.Where(x =>
                               previous_Workers_MaxDuringYear.Value >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                               previous_Workers_MaxDuringYear.Value <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                            var feeAsPerCurrentValues = prinicpalemployerLabourRegistrationFeeSlabs.Where(x =>
                                applications.Licence_CL_PE_GeneralDetail.TotalWorker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                                applications.Licence_CL_PE_GeneralDetail.TotalWorker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            if (feeAsPerCurrentValues > feeAsPerOldValues)
                            {

                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the Principal Employer Registration New Worker Slab" + "</td><td class=\"text-center\" colspan = \"2\">" + feeAsPerCurrentValues.ToString() + "</td><td class=\"text-right\">" + feeAsPerCurrentValues.ToString() + "</td></tr>");
                                oldtotalfees = (feeAsPerCurrentValues - feeAsPerOldValues);
                                srNo++;
                                rows.Append("<tr  style=\"color: red;\"><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the Principal Employer Old Worker Slab" + "</td><td class=\"text-center\" colspan = \"2\">" + feeAsPerOldValues.ToString() + "</td><td class=\"text-right\">" + "- " + oldtotalfees.ToString("F0") + "</td></tr>");
                                srNo++;
                                totAmount += oldtotalfees;

                            }
                        }


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", totAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = totAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }
                    calculatedFeeInfo.ContractLabourPrincipalEmployerRegistrationFeeSlab = prinicpalemployerLabourRegistrationFeeSlabs;


                }

            }
            else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
            {
                if (feeHeaderId == 36)
                {
                    var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_MotorTransport);
                    int maxEmployees = 0;
                    int finalamount = 0;
                    int slabchangeamount = 0;
                    int amendmentfee = 0;
                    var motertransport = parentWithChildObjects.FirstOrDefault().Licence_MotorTransport;
                    List<MotorTransportRegistrationFeeSlabViewModel> motorTransportRegistrationFeeSlabs = new List<MotorTransportRegistrationFeeSlabViewModel>()
                     {
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=1,
                                 MaxVal = 5
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=600,
                                 IsSelected=false
                             }

                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=6,
                                 MaxVal = 25
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=1500,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=26,
                                 MaxVal = 50
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=3000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=51,
                                 MaxVal = 100
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=6000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=101,
                                 MaxVal = 250
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=15000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=251,
                                 MaxVal = 500
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=30000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=501,
                                 MaxVal = 750
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=45000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=751,
                                 MaxVal = 1000
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=60000,
                                 IsSelected=false
                             }
                         },
                         new MotorTransportRegistrationFeeSlabViewModel ()
                         {
                             NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                             {
                                 MinVal=1000,
                                 MaxVal = -1
                             },
                             AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                             {
                                 FeeAmount=90000,
                                 IsSelected=false
                             }
                         }
                     };

                    if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        var latestAmendmentData = _context.Licence_Motor_Transport_AmendmentDataHistories
                        .Where(x => x.AppRefId == appRefId).ToList();
                        var sections = latestAmendmentData.Where(x => x.FieldName == "MaxTransportWorkers").FirstOrDefault();

                        if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                        {

                            var newworker = parentWithChildObjects.FirstOrDefault().Licence_MotorTransport.MaxTransportWorkers;

                            StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                              "<thead>" +
                              "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + newworker.ToString() + "</td></tr>" +
                              "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Renewel Fee<br>(as per fee slab)</th><th>Late fee(if any)<br>(25 % of the annual fee)</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                              "</thead>" +
                              "<tbody>" +
                              "TBRS" +
                              "</tbody>" +
                              "</table>");

                            StringBuilder rows = new StringBuilder();
                            if (sections != null)
                            {
                                amendmentfee = 150;
                                var newworkerlabamount = motorTransportRegistrationFeeSlabs.Where(x => newworker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                                && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : newworker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                decimal totAmount = 0;
                                int srNo = 1;
                                decimal lateFee = 0;
                                int currentYear = DateTime.Now.Year; ;
                                int currentMonth = DateTime.Now.Month;
                                var registrationYear = motertransport.LicenceForYear;
                                if (currentMonth >= 11 && currentYear != registrationYear)
                                {

                                    lateFee = Math.Ceiling((newworkerlabamount * 25) / 100);
                                    totAmount += (newworkerlabamount + lateFee + 150);
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Renewel" + "</td><td class=\"text-right\">" + newworkerlabamount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + totAmount.ToString() + "</td></tr>");
                                    rows.Append("<tr><td class=\"text-center\">" + 2 + "</td><td class=\"text-center\">" + "Fee for the Amendment" + "</td><td class=\"text-right\">" + amendmentfee.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + amendmentfee.ToString() + "</td></tr>");
                                }
                                else
                                {
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Renewel" + "</td><td class=\"text-right\">" + newworkerlabamount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + totAmount.ToString() + "</td></tr>");
                                    rows.Append("<tr><td class=\"text-center\">" + 2 + "</td><td class=\"text-center\">" + "Fee for the Amendment" + "</td><td class=\"text-right\">" + amendmentfee.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + amendmentfee.ToString() + "</td></tr>");
                                    totAmount += (newworkerlabamount + lateFee + 150);
                                }
                                calculatedFeeInfo.CalculatedAmount = totAmount;
                            }
                            else
                            {
                                var newworkerlabamount = motorTransportRegistrationFeeSlabs.Where(x => newworker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                                && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : newworker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                decimal totAmount = 0;
                                int srNo = 1;
                                decimal lateFee = 0;
                                int currentYear = DateTime.Now.Year;
                                int currentMonth = DateTime.Now.Month;
                                var registrationYear = motertransport.LicenceForYear;
                                if (currentMonth >= 11 && currentYear != registrationYear)
                                {

                                    lateFee = Math.Ceiling((newworkerlabamount * 25) / 100);
                                    totAmount += (newworkerlabamount + lateFee);
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Renewel" + "</td><td class=\"text-right\">" + newworkerlabamount.ToString() + "</td><td class=\"text-right\">" + lateFee.ToString() + "</td><td class=\"text-right\">" + totAmount.ToString() + "</td></tr>");

                                }
                                else
                                {
                                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Renewel" + "</td><td class=\"text-right\">" + newworkerlabamount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + newworkerlabamount.ToString() + "</td></tr>");
                                    totAmount += (newworkerlabamount);
                                }
                                calculatedFeeInfo.CalculatedAmount = totAmount;
                            }
                            descStr.Replace("TBRS", rows.ToString());
                            calculatedFeeInfo.Description = descStr.ToString();
                            calculatedFeeInfo.motorTransportRegistrationFeeSlabs = motorTransportRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                        }
                        else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            var newworker = parentWithChildObjects.FirstOrDefault().Licence_MotorTransport.MaxTransportWorkers;

                            StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                              "<thead>" +
                              "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                              "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                              "</thead>" +
                              "<tbody>" +
                              "TBRS" +
                              "</tbody>" +
                              "</table>");

                            StringBuilder rows = new StringBuilder();
                            if (sections != null)
                            {
                                var oldWorker = Convert.ToInt32(sections.PreviousValue);
                                var newworkerlabamount = motorTransportRegistrationFeeSlabs.Where(x => newworker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                               && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : newworker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                var oldworkerlabamount = motorTransportRegistrationFeeSlabs.Where(x => oldWorker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                               && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : oldWorker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                                finalamount = (Convert.ToInt32(newworkerlabamount) - Convert.ToInt32(oldworkerlabamount));

                                rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Amendment" + "</td><td class=\"text-right\">" + 150 + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + 150 + "</td></tr>");
                                rows.Append("<tr><td class=\"text-center\">" + 2 + "</td><td class=\"text-center\">" + "Fee for the Change Worker Slab" + "</td><td class=\"text-right\">" + finalamount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + finalamount.ToString() + "</td></tr>");
                                calculatedFeeInfo.CalculatedAmount = (Convert.ToInt32(newworkerlabamount) - Convert.ToInt32(oldworkerlabamount)) + 150;
                            }
                            else
                            {
                                rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Amendment" + "</td><td class=\"text-right\">" + 150 + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + 150 + "</td></tr>");
                                calculatedFeeInfo.CalculatedAmount = 150;
                            }
                            descStr.Replace("TBRS", rows.ToString());
                            calculatedFeeInfo.Description = descStr.ToString();
                            calculatedFeeInfo.CalculatedAmount = finalamount + 150;
                            calculatedFeeInfo.motorTransportRegistrationFeeSlabs = motorTransportRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }
                    }
                    else
                    {
                        maxEmployees = parentWithChildObjects.FirstOrDefault().Licence_MotorTransport.MaxTransportWorkers;
                        var selectedSlabAmount = (motorTransportRegistrationFeeSlabs = motorTransportRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                               "<thead>" +
                                               "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                               "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                               "</thead>" +
                                               "<tbody>" +
                                               "TBRS" +
                                               "</tbody>" +
                                               "<tfoot>" +
                                               "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                               "</tfoot>" +
                                               "</table>");

                        StringBuilder rows = new StringBuilder();
                        rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Motor Transport Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", selectedSlabAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }

                }
            }
            else if (applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
            {
                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_PE_ISM_GeneralDetails, x => x.Licence_PE_ISM_GeneralDetails.Licence_PE_ISM_Contrators);
                if (feeHeaderId == 30) // Additional Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else
                {

                    List<ISMPrincipalEmployerRegistrationFeeSlabViewModel> ISMprinicpalemployerLabourRegistrationFeeSlabs = new List<ISMPrincipalEmployerRegistrationFeeSlabViewModel>()
                    {
                        new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=5,
                                MaxVal = 20
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=120,
                                IsSelected=false
                            }
                        },
                        new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=21,
                                MaxVal = 50
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=300,
                                IsSelected=false
                            }
                        },
                        new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=51,
                                MaxVal = 100
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=600,
                                IsSelected=false
                            }
                        },
                        new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=101,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1200,
                                IsSelected=false
                            }
                        },
                         new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                         {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 400
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=2400,
                                IsSelected=false
                            }
                         },
                         new ISMPrincipalEmployerRegistrationFeeSlabViewModel ()
                         {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal =400,
                                MaxVal = 40000

                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                         }
                    };
                    int maxWorkers = 0;
                    Int64 oldmaxWorkers = 0;
                    var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_PE_ISM_GeneralDetails;
                    var applications = parentWithChildObjects.FirstOrDefault();
                    var contractorlist = contractLabour.Licence_PE_ISM_Contrators.ToList();
                    var totalMaxLabourWorkerEmployed = contractorlist.Sum(c => c.MaxLabourWorkerEmployed);
                    if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        maxWorkers = (int)parentWithChildObjects.FirstOrDefault().Licence_PE_ISM_GeneralDetails.TotalWorker;
                        var selectedSlabAmount = ISMprinicpalemployerLabourRegistrationFeeSlabs.Where(x => totalMaxLabourWorkerEmployed >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : totalMaxLabourWorkerEmployed <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList().Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                           "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + totalMaxLabourWorkerEmployed.ToString() + "</td></tr>" +
                                           "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "<tfoot>" +
                                           "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                           "</tfoot>" +
                                           "</table>");

                        StringBuilder rows = new StringBuilder();
                        rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Principal Employer Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", selectedSlabAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                    }
                    else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                                            "<thead>" +
                                                            "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of worker</td><td class=\"text-center\">" + totalMaxLabourWorkerEmployed.ToString() + "</td></tr>" +
                                                            "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th colspan = \"2\">Annual Fee<br>(as per fee slab)</th><th>Total fee<br>(Annual fee)</th></tr>" +
                                                            "</thead>" +
                                                            "<tbody>" +
                                                            "TBRS" +
                                                            "</tbody>" +
                                                            "<tfoot>" +
                                                            "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                                            "</tfoot>" +
                                                            "</table>");

                        StringBuilder rows = new StringBuilder();
                        decimal totAmount = 0;
                        decimal oldlatefees = 0;
                        decimal oldtotalfees = 0;
                        decimal amendmentFee = 150;
                        int srNo = 1;

                        var amendmentDataHistories = _context.Licence_PE_ISM_AmendmentDataHistories
                            .Where(x => x.AppRefId == appRefId &&
                            (x.ModifiedCounter == applications.Licence_PE_ISM_GeneralDetails.ModifiedCounter)).ToList();

                        if (amendmentDataHistories.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            var sections = amendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();

                            if (sections != null)
                            {
                                totAmount += 0;
                                sb.Clear();
                                sb.Append("<div class=\"row\">" +
                                "<div class=\"col-sm-4 border bg-info\">Column Name</div><div class=\"col-sm-4 border bg-info\">Previous Value</div><div class=\"col-sm-4 border bg-info\">Modified Value</div>");
                                sb.Append("<div class=\"col-sm-4 border\">" + "Total Worker" + "</div><div class=\"col-sm-4 border\">" + sections.PreviousValue.ToString() + "</div><div class=\"col-sm-4 border\">" + sections.ModifiedValue.ToString() + "</div>");


                                sb.Append("</div>");
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "No of Worker" +
                                    "</td><td class=\"text-left\" style=\"padding: 25px;\" colspan = \"2\">" + sb.ToString() +
                                    "</td><th class=\"text-right\" style=\"vertical-align: middle;\"></th></tr>");
                                srNo++;
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "Amendment Fees" +
                                    "</td><td class=\"text-center\" colspan = \"2\">" + amendmentFee +
                                    "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                                srNo++;
                                totAmount += 150;
                            }
                            else
                            {
                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                    "</td><td class=\"text-left\">" + "Amendment Fees" +
                                    "</td><td class=\"text-right\">" + amendmentFee +
                                    "</td><td class=\"text-right\">" + "0" +
                                    "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                                srNo++;
                                totAmount += 150;
                            }


                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() +
                                "</td><td class=\"text-left\">" + "Amendment Fees" +
                                "</td><td class=\"text-right\">" + amendmentFee +
                                "</td><td class=\"text-right\">" + "0" +
                                "</td><td class=\"text-right\">" + amendmentFee + "</td></tr>");
                            srNo++;
                            totAmount += 150;

                        }

                        if (amendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                        {
                            Int64 previous_Workers_MaxDuringYear = 0;
                            if (amendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                            {
                                previous_Workers_MaxDuringYear = Convert.ToInt64(amendmentDataHistories.Where(x => x.FieldName == "TotalWorker").Select(x => x.PreviousValue).FirstOrDefault());
                            }


                            var feeAsPerOldValues = ISMprinicpalemployerLabourRegistrationFeeSlabs.Where(x =>
                               previous_Workers_MaxDuringYear >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                               previous_Workers_MaxDuringYear <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();


                            var feeAsPerCurrentValues = ISMprinicpalemployerLabourRegistrationFeeSlabs.Where(x =>
                                applications.Licence_PE_ISM_GeneralDetails.TotalWorker >= x.NumberOfEmployeesToBeEmployedSlab.MinVal &&
                                applications.Licence_PE_ISM_GeneralDetails.TotalWorker <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            if (feeAsPerCurrentValues > feeAsPerOldValues)
                            {

                                rows.Append("<tr><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the Principal Employer Registration New Worker Slab" + "</td><td class=\"text-center\" colspan = \"2\">" + feeAsPerCurrentValues.ToString() + "</td><td class=\"text-right\">" + feeAsPerCurrentValues.ToString() + "</td></tr>");
                                oldtotalfees = (feeAsPerCurrentValues - feeAsPerOldValues);
                                srNo++;
                                rows.Append("<tr  style=\"color: red;\"><td class=\"text-center\">" + srNo.ToString() + "</td><td class=\"text-left\">" + "Fee for the Principal Employer Old Worker Slab" + "</td><td class=\"text-center\" colspan = \"2\">" + feeAsPerOldValues.ToString() + "</td><td class=\"text-right\">" + "- " + oldtotalfees.ToString("F0") + "</td></tr>");
                                srNo++;
                                totAmount += oldtotalfees;

                            }
                        }


                        descStr.Replace("TBRS", rows.ToString());
                        descStr.Replace("GRANDTOTAL", totAmount.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = totAmount;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }
                    calculatedFeeInfo.ISMEmployerRegistrationFeeSlab = ISMprinicpalemployerLabourRegistrationFeeSlabs;


                }

            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                if (feeHeaderId == 19) // BOCW-Cess
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                    calculatedFeeInfo.PaymentBatchCounter = feeDetails.PaymentBatchCounter;
                }
                else if (feeHeaderId == 39) // Scrutiny Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                    calculatedFeeInfo.PaymentBatchCounter = feeDetails.PaymentBatchCounter;
                }
            }
            else if (applicationType == ApplicationTypeEnum.TRADE_UNION)
            {

                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_TradeUnion);
                var tradeUnion = parentWithChildObjects.FirstOrDefault().Licence_TradeUnion;

                if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                           "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + 0 + "</td></tr>" +
                                           "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "<tfoot>" +
                                           "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                           "</tfoot>" +
                                           "</table>");

                    StringBuilder rows = new StringBuilder();
                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Trade Union Amendment" + "</td><td class=\"text-right\">" + "100" + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + "100" + "</td></tr>");


                    descStr.Replace("TBRS", rows.ToString());
                    descStr.Replace("GRANDTOTAL", "100");
                    calculatedFeeInfo.Description = descStr.ToString();
                    calculatedFeeInfo.CalculatedAmount = 100;
                    calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                }
                else
                {
                    StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                           "<thead>" +
                                           "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + 0 + "</td></tr>" +
                                           "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                           "</thead>" +
                                           "<tbody>" +
                                           "TBRS" +
                                           "</tbody>" +
                                           "<tfoot>" +
                                           "<tr><td colspan = \"4\" class=\"text-right font-weight-bold\">Grand total:</td><td class=\"text-right font-weight-bold text-danger\">GRANDTOTAL</td></tr>" +
                                           "</tfoot>" +
                                           "</table>");

                    StringBuilder rows = new StringBuilder();
                    rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Trade Union Registration" + "</td><td class=\"text-right\">" + "100" + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + "100" + "</td></tr>");


                    descStr.Replace("TBRS", rows.ToString());
                    descStr.Replace("GRANDTOTAL", "100");
                    calculatedFeeInfo.Description = descStr.ToString();
                    calculatedFeeInfo.CalculatedAmount = 100;
                    calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                }


            }
            else if (applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
            {
                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Licence_ISM_ContractLabour_GeneralDetails);
                if (feeHeaderId == 30) // Additional Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else if (feeHeaderId == 47) // Additional Security Balance Fee
                {
                    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.FeeHeaderRefId == feeHeaderId && x.PaymentBatchCounter == parentWithChildObjects.FirstOrDefault().PaymentBatchCounter).FirstOrDefault();
                    calculatedFeeInfo.CalculatedAmount = feeDetails.Amount;
                    calculatedFeeInfo.Description = feeDetails.Description;
                }
                else
                {
                    if (feeHeaderId == 44 || feeHeaderId == 45 || feeHeaderId == 46)
                    {
                    List<ContractLabourRegistrationFeeSlabViewModel> contractLabourRegistrationFeeSlabs = new List<ContractLabourRegistrationFeeSlabViewModel>()
                    {
                        new ContractLabourRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                                MaxVal = 200
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=1500,
                                IsSelected=false
                            }
                        },
                        new ContractLabourRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=201,
                                MaxVal = 400
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=3000,
                                IsSelected=false
                            }
                        },
                        new ContractLabourRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=400,
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=4500,
                                IsSelected=false
                            }
                        }
                    };


                    int maxEmployees = 0;
                    var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails;


                    if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        var latestAmendmentData = _context.Licence_ISM_ContractLabour_AmendmentDataHistories
                                             .Where(x => x.AppRefId == appRefId)
                                             .OrderByDescending(x => x.ModifiedOn)
                                             .FirstOrDefault();

                        if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                        {
                            maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                            var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                            && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                  "<thead>" +
                                  "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                  "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                  "</thead>" +
                                  "<tbody>" +
                                  "TBRS" +
                                  "</tbody>" +
                                  "</table>");

                            StringBuilder rows = new StringBuilder();
                            if (latestAmendmentData != null)
                            {
                                rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Renewel" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");
                                rows.Append("<tr><td class=\"text-center\">" + 2 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Amendment" + "</td><td class=\"text-right\">" + "150" + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + "150" + "</td></tr>");
                                calculatedFeeInfo.CalculatedAmount = selectedSlabAmount + 150;
                            }
                            else
                            {
                                rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Renewel" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");
                                calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                            }
                            descStr.Replace("TBRS", rows.ToString());
                            calculatedFeeInfo.Description = descStr.ToString();
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

                        }
                        else if (applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                        {
                            StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                              "<thead>" +
                              "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                              "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                              "</thead>" +
                              "<tbody>" +
                              "TBRS" +
                              "</tbody>" +
                              "</table>");

                            StringBuilder rows = new StringBuilder();
                            rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Amendment" + "</td><td class=\"text-right\">" + "150" + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + "150" + "</td></tr>");
                            descStr.Replace("TBRS", rows.ToString());
                            calculatedFeeInfo.Description = descStr.ToString();
                            calculatedFeeInfo.CalculatedAmount = 150;
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }

                    }
                    else
                    {
                        maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                        var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        && (x.NumberOfEmployeesToBeEmployedSlab.MaxVal == -1 ? true : maxEmployees <= x.NumberOfEmployeesToBeEmployedSlab.MaxVal)).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        StringBuilder descStr = new StringBuilder("<table class=\"table table-bordered\">" +
                                       "<thead>" +
                                       "<tr><td></td><td class=\"text-right font-weight-bold\">Registration from date:</td><td>" + DateTime.Now.ToLongDateString() + "</td><td class=\"text-right font-weight-bold\">Total no. of Worker</td><td class=\"text-center\">" + maxEmployees.ToString() + "</td></tr>" +
                                       "<tr class=\"text-center\"><th>Sr No.</th><th>Fee for the year</th><th>Annual Fee<br>(as per fee slab)</th><th>Late fee</th><th>Total fee<br>(Annual fee + late fee)</th></tr>" +
                                       "</thead>" +
                                       "<tbody>" +
                                       "TBRS" +
                                       "</tbody>" +
                                       "</table>");

                        StringBuilder rows = new StringBuilder();
                        rows.Append("<tr><td class=\"text-center\">" + 1 + "</td><td class=\"text-center\">" + "Fee for the Contract Labour Registration" + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td><td class=\"text-right\">" + 0 + "</td><td class=\"text-right\">" + selectedSlabAmount.ToString() + "</td></tr>");
                        descStr.Replace("TBRS", rows.ToString());
                        calculatedFeeInfo.Description = descStr.ToString();
                        calculatedFeeInfo.CalculatedAmount = selectedSlabAmount;
                        calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }


                }
                else if (feeHeaderId == 15)
                {
                    int maxEmployees = 0;

                    //var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId, null, x => x.Contractor_GeneralDetail.Contractor_PrincipalEmployer);
                    var contractLabour = parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails;
                    List<ContractLabourRegistrationFeeSlabViewModel> contractLabourRegistrationFeeSlabs = new List<ContractLabourRegistrationFeeSlabViewModel>()
                    {
                        new ContractLabourRegistrationFeeSlabViewModel ()
                        {
                            NumberOfEmployeesToBeEmployedSlab=new AppFeeSlabRangeParmsViewModel()
                            {
                                MinVal=0,
                            },
                            AppFeeSlabAmountInfo =new AppFeeSlabAmountInfoViewModel ()
                            {
                                FeeAmount=270,
                                IsSelected=false
                            }
                        },
                    };

                    var latestAmendmentData = _context.Licence_ISM_ContractLabour_AmendmentDataHistories
                        .Where(x => x.AppRefId == appRefId).ToList();

                    if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;

                        var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        calculatedFeeInfo.CalculatedAmount = selectedSlabAmount * maxEmployees;
                        calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString() + " X 270";
                        calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }
                    else if (latestAmendmentData != null && (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE))
                    {
                        maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                        var sections = latestAmendmentData.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                        if (sections == null)
                        {

                            calculatedFeeInfo.CalculatedAmount = 0;
                            calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString();
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }
                        else
                        {
                            int updatedEmployee = maxEmployees - (int)Convert.ToInt64(sections.PreviousValue);
                            var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => updatedEmployee >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                            ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                            calculatedFeeInfo.CalculatedAmount = (selectedSlabAmount * updatedEmployee);
                            calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + updatedEmployee.ToString();
                            calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                            calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                        }

                    }
                    else
                    {
                        maxEmployees = (int)parentWithChildObjects.FirstOrDefault().Licence_ISM_ContractLabour_GeneralDetails.MaximumNumberOfEmployee;
                        var selectedSlabAmount = (contractLabourRegistrationFeeSlabs = contractLabourRegistrationFeeSlabs.Where(x => maxEmployees >= x.NumberOfEmployeesToBeEmployedSlab.MinVal
                        ).Select(x => { x.AppFeeSlabAmountInfo.IsSelected = true; return x; }).ToList()).Where(x => x.AppFeeSlabAmountInfo.IsSelected).Select(x => x.AppFeeSlabAmountInfo.FeeAmount).FirstOrDefault();

                        calculatedFeeInfo.CalculatedAmount = 0;
                        calculatedFeeInfo.Description = "Fee calculated as per maximum employees to be employed any day: " + maxEmployees.ToString();
                        calculatedFeeInfo.contractLabourRegistrationFeeSlab = contractLabourRegistrationFeeSlabs;
                        calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;
                    }
                }
                }
            }
            else if (applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
            {

                var parentWithChildObjects = await _iGR_Application.GetAsync(x => x.AppId == appRefId);
                var fundMaster = _context.LWB_FundMaster.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                var amount = _context.LWB_LIdAndUnpaidWages.Where(x => x.FundMasterRefId == fundMaster.Id).Sum(x => x.Amount);

                calculatedFeeInfo.Description = "Amount calculated as per excel sheet uploaded";
                calculatedFeeInfo.CalculatedAmount = amount;
                calculatedFeeInfo.PaymentBatchCounter = parentWithChildObjects.FirstOrDefault().PaymentBatchCounter;

            }
            return calculatedFeeInfo;
        }

        public async Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId)
        {
            return await _iPaymentManagerRepository.GetFeeHeader(feeHeaderId);
        }

        public async Task<GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel>> PrepareAppFeePaymentInitiateTerminalInfo(Int64 appRefId, ApplicationTypeEnum applicationType, decimal netFeeCalculated, int paymentPartCounter, int paymentBatchCounter)
        {
            GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel> genericFormModel = new GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel>();
            try
            {
                genericFormModel.FormModel = new AppFeePaymentInitiateTerminalInfoViewModel();

                //1. Attech Payment Gateway Type
                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.SBI;
                }
                else if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }

                else if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT || applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER ||
                    applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || applicationType == ApplicationTypeEnum.TRADE_UNION || applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
                }

                else if (applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
                {
                    genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.HDFC;
                }

                //2. Set Payment Gateway specific information
                if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
                {
                    genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
                    genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY;
                    genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
                    //genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("PaymentGatewayTargetUrl").Value;
                }
                else if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
                {
                    genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
                    genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY;
                    genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
                    //genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("PaymentGatewayTargetUrl").Value;
                }
                else if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
                {
                    genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
                    genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY;
                    genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
                    //genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("PaymentGatewayTargetUrl").Value;
                }

                //3. set common information 
                genericFormModel.FormModel.AppRefId = appRefId;
                genericFormModel.FormModel.AmountCalculated = netFeeCalculated;
                genericFormModel.FormModel.UniquePaymentGatewayTransactionRefId = await GenerateUniquePaymentGatewayTransactionId(appRefId);
                if (applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
                {
                    string salt = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("HDFCKey").Value;
                    string key = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("HDFCSecret").Value;
                    Dictionary<string, object> input = new Dictionary<string, object>();
                    input.Add("amount", netFeeCalculated * 100);
                    input.Add("currency", "INR");
                    input.Add("receipt", Guid.NewGuid().ToString());
                    Razorpay.Api.Order order = new Razorpay.Api.Order();
                    RazorpayClient client = new RazorpayClient(salt, key);
                    order = client.Order.Create(input);
                    genericFormModel.FormModel.UniquePaymentGatewayTransactionRefId = order["id"].ToString();
                }
                else
                {
                    genericFormModel.FormModel.UniquePaymentGatewayTransactionRefId = await GenerateUniquePaymentGatewayTransactionId(appRefId);
                }
                genericFormModel.FormModel.ApplicationTypeName = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), applicationType));
                genericFormModel.FormModel.ApplicationPurposeTypeName = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), 1));

                //4. set client form inputs
                genericFormModel.FormModel.PaymentGatewayClientFormInputDetail = await PreparePaymentGatewayClientFormInputs(applicationType, appRefId, genericFormModel.FormModel, paymentPartCounter, paymentBatchCounter);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<PaymentGatewayClientFormInputDetailsViewModel> PreparePaymentGatewayClientFormInputs(ApplicationTypeEnum applicationType, Int64 appRefId, AppFeePaymentInitiateTerminalInfoViewModel AppFeePaymentInitiateTerminalInfo, int paymentPartCounter, int paymentBatchCounter)
        {
            PaymentGatewayClientFormInputDetailsViewModel paymentGatewayClientFormInputDetails = null;
            if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
            {
                PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = await GetPaymentGatewaySpecificInputFieldsData(applicationType, appRefId, AppFeePaymentInitiateTerminalInfo, paymentPartCounter, paymentBatchCounter);
                paymentGatewayClientFormInputDetails = new PaymentGatewayClientFormInputDetailsViewModel();
                paymentGatewayClientFormInputDetails.PaymentGatewayTargetUrl = paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl;

                paymentGatewayClientFormInputDetails.paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
                {
                    new PaymentGatewayClientFormInputViewModel()
                    {
                        FormInputName= "EncryptTrans",
                        FormInputValue=paymentGatewaySpecificInputProps.FormInputs_SBI.EncryptTrans,
                        IsHidden=true
                    },
                    new PaymentGatewayClientFormInputViewModel()
                    {
                        FormInputName = "merchIdVal",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_SBI.MerchIdVal,
                        IsHidden = true
                    }
                };
            }
            else if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
            {

                paymentGatewayClientFormInputDetails = new PaymentGatewayClientFormInputDetailsViewModel();
                PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = await GetPaymentGatewaySpecificInputFieldsData(applicationType, appRefId, AppFeePaymentInitiateTerminalInfo, paymentPartCounter, paymentBatchCounter);
                paymentGatewayClientFormInputDetails.PaymentGatewayTargetUrl = paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl;

                paymentGatewayClientFormInputDetails.paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
                {
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "key_id",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.HDFCKey,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "amount",
                        FormInputValue =  (Convert.ToDecimal(paymentGatewaySpecificInputProps.FormInputs_HDFC.totalAmt) * 100).ToString(),
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "order_id",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.deptRefNo,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "name",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.name,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "description",
                        FormInputValue = "",
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "image",
                        FormInputValue = "",
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "prefill[name]",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.name,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "prefill[contact]",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.mobNumber,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "prefill[email]",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.emailId,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "notes[shipping address]",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.addLine1,
                        IsHidden = true
                    },
                    // Uncomment if required
                    // new PaymentGatewayClientFormInputViewModel
                    // {
                    //     FormInputName = "notes[transaction_id]",
                    //     FormInputValue = model.TxnId,
                    //     IsHidden = true
                    // },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "callback_url",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.sURL,
                        IsHidden = true
                    },
                    new PaymentGatewayClientFormInputViewModel
                    {
                        FormInputName = "cancel_url",
                        FormInputValue = paymentGatewaySpecificInputProps.FormInputs_HDFC.fURL,
                        IsHidden = true
                    }
                };
            }
            else if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
            {
                PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = await GetPaymentGatewaySpecificInputFieldsData(applicationType, appRefId, AppFeePaymentInitiateTerminalInfo, paymentPartCounter, paymentBatchCounter);

                paymentGatewayClientFormInputDetails = new PaymentGatewayClientFormInputDetailsViewModel();
                paymentGatewayClientFormInputDetails.PaymentGatewayTargetUrl = paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl;

                if (paymentGatewaySpecificInputProps.FormInputs_IFMS != null)
                {
                    paymentGatewayClientFormInputDetails.paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
                    {
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName= "encData",
                            FormInputValue=paymentGatewaySpecificInputProps.FormInputs_IFMS.encData,
                            IsHidden=true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "clientId",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.clientId,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "clientSecret",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.clientSecret,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "integratingAgency",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.integratingAgency,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "ipAddress",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.ipAddress,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "transactionID",
                            FormInputValue = new Random().Next(100000, 999999).ToString(),
                            IsHidden = true
                        }

                    };
                }
                else if (paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury != null)
                {
                    paymentGatewayClientFormInputDetails.paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
                    {
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName= "encData",
                            FormInputValue=paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.encData,
                            IsHidden=true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "clientId",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.clientId,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "clientSecret",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.clientSecret,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "Service",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.Service,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "ipAddress",
                            FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.ipAddress,
                            IsHidden = true
                        },
                        new PaymentGatewayClientFormInputViewModel()
                        {
                            FormInputName = "transactionID",
                            FormInputValue = new Random().Next(100000, 999999).ToString(),
                            IsHidden = true
                        }
                    };
                }
            }
            return paymentGatewayClientFormInputDetails;
        }

        //public async Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId)
        //{
        //    return await _iPaymentManagerRepository.GetFeeHeader(feeHeaderId);
        //}

        //public async Task<GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel>> PrepareAppFeePaymentInitiateTerminalInfo(Int64 appRefId, ApplicationTypeEnum applicationType, decimal netFeeCalculated, int paymentPartCounter, int paymentBatchCounter)
        //{
        //    GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel> genericFormModel = new GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel>();
        //    try
        //    {
        //        genericFormModel.FormModel = new AppFeePaymentInitiateTerminalInfoViewModel();

        //        //1. Attech Payment Gateway Type
        //        if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.SBI;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.REG_ESTB)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.BP_PROPOSED_PLAN)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }
        //        else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
        //        {
        //            genericFormModel.FormModel.PaymentGatewayType = PaymentGatewayTypeEnum.IFMS;
        //        }

        //        //2. Set Payment Gateway specific information
        //        if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
        //        {
        //            genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
        //            genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY;
        //            genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
        //            genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("PaymentGatewayTargetUrl").Value;
        //        }
        //        else if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
        //        {
        //            genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
        //            genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY;
        //            genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
        //            genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("PaymentGatewayTargetUrl").Value;
        //        }
        //        else if (genericFormModel.FormModel.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
        //        {
        //            genericFormModel.FormModel.PaymentModeType = PaymentModeTypeEnum.ONLINE;
        //            genericFormModel.FormModel.PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY;
        //            genericFormModel.FormModel.PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST;
        //            genericFormModel.FormModel.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("PaymentGatewayTargetUrl").Value;
        //        }

        //        //3. set common information 
        //        genericFormModel.FormModel.AppRefId = appRefId;
        //        genericFormModel.FormModel.AmountCalculated = netFeeCalculated;
        //        genericFormModel.FormModel.UniquePaymentGatewayTransactionRefId = await GenerateUniquePaymentGatewayTransactionId(appRefId);

        //        genericFormModel.FormModel.ApplicationTypeName = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), applicationType));
        //        genericFormModel.FormModel.ApplicationPurposeTypeName = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), 1));

        //        //4. set client form inputs
        //        genericFormModel.FormModel.PaymentGatewayClientFormInputs = await PreparePaymentGatewayClientFormInputs(applicationType, appRefId, genericFormModel.FormModel, paymentPartCounter, paymentBatchCounter);
        //    }
        //    catch (Exception ex)
        //    {
        //        genericFormModel.HasError = true;
        //        genericFormModel.ErrorDesc = ex.Message;
        //    }
        //    return genericFormModel;
        //}

        //public async Task<List<PaymentGatewayClientFormInputViewModel>> PreparePaymentGatewayClientFormInputs(ApplicationTypeEnum applicationType, Int64 appRefId, AppFeePaymentInitiateTerminalInfoViewModel AppFeePaymentInitiateTerminalInfo, int paymentPartCounter, int paymentBatchCounter)
        //{
        //    List<PaymentGatewayClientFormInputViewModel> paymentGatewayClientFormInputList = null;
        //    if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
        //    {
        //        PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = await GetPaymentGatewaySpecificInputFieldsData(applicationType, appRefId, AppFeePaymentInitiateTerminalInfo, paymentPartCounter, paymentBatchCounter);
        //        paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
        //        {
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName= "EncryptTrans",
        //                FormInputValue=paymentGatewaySpecificInputProps.FormInputs_SBI.EncryptTrans,
        //                IsHidden=true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "merchIdVal",
        //                FormInputValue = paymentGatewaySpecificInputProps.FormInputs_SBI.MerchIdVal,
        //                IsHidden = true
        //            }
        //        };
        //    }
        //    else if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
        //    {

        //        paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
        //        {
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "key",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "txnid",
        //                FormInputValue = AppFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "amount",
        //                FormInputValue = AppFeePaymentInitiateTerminalInfo.AmountCalculated.ToString(),
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "productinfo",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "firstname",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "email",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "phone",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "lastname",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "address1",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "address2",
        //                FormInputValue = "",
        //                IsHidden = true
        //            },

        //        };
        //    }
        //    else if (AppFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
        //    {
        //        PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = await GetPaymentGatewaySpecificInputFieldsData(applicationType, appRefId, AppFeePaymentInitiateTerminalInfo, paymentPartCounter, paymentBatchCounter);

        //        paymentGatewayClientFormInputDetails = new PaymentGatewayClientFormInputDetailsViewModel();
        //        paymentGatewayClientFormInputDetails.PaymentGatewayTargetUrl = paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl;

        //        paymentGatewayClientFormInputDetails.paymentGatewayClientFormInputList = new List<PaymentGatewayClientFormInputViewModel>()
        //        {
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName= "encData",
        //                FormInputValue=paymentGatewaySpecificInputProps.FormInputs_IFMS.encData,
        //                IsHidden=true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "clientId",
        //                FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.clientId,
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "clientSecret",
        //                FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.clientSecret,
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "integratingAgency",
        //                FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.integratingAgency,
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "ipAddress",
        //                FormInputValue = paymentGatewaySpecificInputProps.FormInputs_IFMS.ipAddress,
        //                IsHidden = true
        //            },
        //            new PaymentGatewayClientFormInputViewModel()
        //            {
        //                FormInputName = "transactionID",
        //                FormInputValue = new Random().Next(100000, 999999).ToString(),
        //                IsHidden = true
        //            }

        //        };
        //    }
        //    return paymentGatewayClientFormInputDetails;
        //}

        public async Task<PaymentGatewaySpecificInputPropsViewModel> GetPaymentGatewaySpecificInputFieldsData(ApplicationTypeEnum applicationType, Int64 appRefId, AppFeePaymentInitiateTerminalInfoViewModel appFeePaymentInitiateTerminalInfo, int paymentPartCounter, int paymentBatchCounter)
        {
            PaymentGatewaySpecificInputPropsViewModel paymentGatewaySpecificInputProps = new PaymentGatewaySpecificInputPropsViewModel();
            if (appFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
            {
                paymentGatewaySpecificInputProps.FormInputs_SBI = new PaymentGatewayInputProps_SBI();
                paymentGatewaySpecificInputProps.FormInputs_SBI.MerchIdVal = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("PbLabourMerchantId").Value;

                StringBuilder sb = new StringBuilder();
                Random random = new Random();
                var SBI_Configs = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI");
                sb.Append(SBI_Configs.GetSection("PbLabourMerchantId").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("OperatingMode").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("MerchantCountry").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("MerchantCurrency").Value);
                sb.Append("|");
                //sb.Append(SBI_Configs.GetSection("parameter.PostingAmount").Value);
                sb.Append("1000");
                sb.Append("|");
                //sb.Append(SBI_Configs.GetSection("parameter.OtherDetails").Value);
                sb.Append("Bhupinder Singh");
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("SuccessURL").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("FailURL").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("AggregatorId").Value);
                sb.Append("|");
                //sb.Append(SBI_Configs.GetSection("MerchantOrderNo").Value);
                sb.Append(random.Next(99999999).ToString());
                sb.Append("|");
                //sb.Append(SBI_Configs.GetSection("parameter.MerchantCustomerID").Value);
                sb.Append(random.Next(99999999).ToString());
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("Paymode").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("Accesmedium").Value);
                sb.Append("|");
                sb.Append(SBI_Configs.GetSection("TransactionSource").Value);

                paymentGatewaySpecificInputProps.FormInputs_SBI.EncryptTrans = AES256.Encrypt(sb.ToString(), SBI_Configs.GetSection("key_Array").Value);
                paymentGatewaySpecificInputProps.FormInputs_SBI.EncryptTrans = paymentGatewaySpecificInputProps.FormInputs_SBI.EncryptTrans.Replace("\n", "");

            }
            else if (appFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
            {
                var application = _iGR_Application.GetById(appRefId);

                var projectSite = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(application.ProjectSiteRefId, appRefId, application.ProjectSiteVersion);
                IFMS_NonTreasuryChallanDataTemplateViewModel nonTrsyChallanData = null;
                var feeHeadersWithTreasuryCodes = await _iPaymentManagerRepository.GetAppFeeHeadersAndTreasuryHeadsByAppId(appRefId, paymentPartCounter, paymentBatchCounter);
               
                PaymentGatewayInputProps_HDFC hdfcChallanData = null;
                var userProfile = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(appRefId);
                hdfcChallanData = new PaymentGatewayInputProps_HDFC()
                {
                    Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("Service").Value,
                    deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                    ddoCode = 0.ToString(),
                    hodMstId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("HodMstId").Value,
                    challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                    totalAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)),
                    noOfTrans = 1.ToString(),
                    add1 = appRefId.ToString(),
                    add2 = paymentBatchCounter.ToString(),
                    add3 = paymentPartCounter.ToString(),
                    HDFCKey = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("HDFCKey").Value,
                    HDFCSalt = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("HDFCSalt").Value,
                    sURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("MarchantSiteResponseUrl_Success").Value,
                    fURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("MarchantSiteResponseUrl_Failure").Value,
                    name = userProfile.FirstName + " " + userProfile.LastName,
                    mobNumber = userProfile.MobileNo,
                    emailId = userProfile.Email,
                    addLine1 = projectSite.ResponseDataModel.Address,
                    addLine2 = projectSite.ResponseDataModel.Address,
                    addPincode = projectSite.ResponseDataModel.PinCode
                   
                };

                paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("PaymentGatewayTargetUrl").Value;
                paymentGatewaySpecificInputProps.FormInputs_HDFC = hdfcChallanData;


                //  identity = "1" --> PAN Number
                //  identity = "2" --> Aadhar Number
                //  identity = "3" --> Voter Card Number


            }
            else if (appFeePaymentInitiateTerminalInfo.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
            {
                List<IFMS_TresuryPaymentViewModel> tresuryHeads = new List<IFMS_TresuryPaymentViewModel>();
                var feeHeadersWithTreasuryCodes = await _iPaymentManagerRepository.GetAppFeeHeadersAndTreasuryHeadsByAppId(appRefId, paymentPartCounter, paymentBatchCounter);
                bool isNonTreasuryPaymentBatch = false;
                
                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    List<Int64> nonTreasuryFeeHeaders = new List<Int64>();
                    nonTreasuryFeeHeaders.Add(19);
                    nonTreasuryFeeHeaders.Add(22);
                    nonTreasuryFeeHeaders.Add(27);
                    isNonTreasuryPaymentBatch = feeHeadersWithTreasuryCodes.Any(x => nonTreasuryFeeHeaders.Contains(x.FeeHeaderRefId));
                }
                else if(applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    List<Int64> nonTreasuryFeeHeaders = new List<Int64>();
                    nonTreasuryFeeHeaders.Add(19);
                    isNonTreasuryPaymentBatch = feeHeadersWithTreasuryCodes.Any(x => nonTreasuryFeeHeaders.Contains(x.FeeHeaderRefId));
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    List<Int64> nonTreasuryFeeHeaders = new List<Int64>();
                    nonTreasuryFeeHeaders.Add(39);
                    nonTreasuryFeeHeaders.Add(48);
                    isNonTreasuryPaymentBatch = feeHeadersWithTreasuryCodes.Any(x => nonTreasuryFeeHeaders.Contains(x.FeeHeaderRefId));
                }

                List<Int64> feeHeads = new List<Int64>();
                foreach (var item in feeHeadersWithTreasuryCodes)
                {
                    if (item.Amount > 0)
                    {
                        tresuryHeads.Add(new IFMS_TresuryPaymentViewModel() { Head = item.MajorHead_MajorSubHead_MinorHead_MinorSubHead_00, amt = decimal.Ceiling(item.Amount).ToString() });
                        feeHeads.Add(item.FeeHeaderRefId);
                    }
                }
                AppDDOCodesInfoViewModel ddoCodeInfo = null;
   
                if (feeHeadersWithTreasuryCodes.Any(x=>x.HasDedicatedTreasuryCode))
                {
                    ddoCodeInfo = new AppDDOCodesInfoViewModel()
                    {
                        ALC_Treasury_Code = feeHeadersWithTreasuryCodes.Where(x => x.DedicatedTreasurCode != null).FirstOrDefault().DedicatedTreasurCode,
                        ALC_Treasury_DDOCode = feeHeadersWithTreasuryCodes.Where(x => x.DedicatedDDOCode != null).FirstOrDefault().DedicatedDDOCode
                    };
                }
                else if (feeHeads.Count == 1 && feeHeads.Contains(18))
                {
                    ddoCodeInfo = new AppDDOCodesInfoViewModel()
                    {
                        //ALC_Treasury_Code = "CHD00",
                        //ALC_Treasury_DDOCode = "0037"

                        ALC_Treasury_Code = "SAS00",
                        ALC_Treasury_DDOCode = "0604"
                    };
                }
                else
                {
                    ddoCodeInfo = await _iPaymentManagerRepository.GetAppDDOCodesByAppId(appRefId, applicationType);
                }
                var application = _iGR_Application.GetById(appRefId);

                var projectSite = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(application.ProjectSiteRefId, appRefId, application.ProjectSiteVersion);
                string challanDataJson = "";
                string requestdataJson = "";

                IFMS_ChallanDataTemplateViewModel challanData = null;
                IFMS_NonTreasuryChallanDataTemplateViewModel nonTrsyChallanData = null;
                if (isNonTreasuryPaymentBatch)
                {
                    ddoCodeInfo = await _iPaymentManagerRepository.GetAppDDOCodesByAppId(appRefId, applicationType);
                    //nonTrsyChallanData = new IFMS_NonTreasuryChallanDataTemplateViewModel()
                    //{
                    //    AgencyCode = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("AgencyCode").Value,
                    //    deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                    //    challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    //    expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                    //    hodMstId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("HodMstId").Value,
                    //    ddoCode = ddoCodeInfo.ALC_Treasury_Code + ddoCodeInfo.ALC_Treasury_DDOCode,
                    //    totalAmt  = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                    //    noOfTrans  = tresuryHeads.Count().ToString(),

                    //    //deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                    //    //clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value,
                    //    //deptCode = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("DeptCode").Value,
                    //    //challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    //    //expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                    //    //companyName = projectSite.FormModel.EstablishmentName,
                    //    //totalAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                    //    //trsyAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                    //    //nonTrsyAmt = "0",
                    //    //noOfTrans = tresuryHeads.Count().ToString(),
                    //    //ddoCode = (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : "0000"),// "0551",// + ddoCodeInfo.FC_Treasury_DDOCode.ToString(), //.PadLeft(4, '0'),
                    //    //payLocCode = applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_Code.ToString() : "0000",// "HPR00",// ddoCodeInfo.FC_Treasury_Code,
                    //    add1 = appRefId.ToString(),
                    //    add2 = paymentBatchCounter.ToString(),
                    //    add3 = paymentPartCounter.ToString(),
                    //    add4 = "",
                    //    add5 = "",
                    //    sURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("MarchantSiteResponseUrl_Success").Value,
                    //    fURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("MarchantSiteResponseUrl_Failure").Value,

                    //};

                    //IFMS_ChallanDataTemplateViewModel challanData = new IFMS_ChallanDataTemplateViewModel()
                    //{
                    //    deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                    //    receiptNo = "",
                    //    clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value,
                    //    deptCode = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("DeptCode").Value,
                    //    challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    //    expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                    //    companyName = projectSite.FormModel.EstablishmentName,
                    //    totalAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                    //    trsyAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                    //    nonTrsyAmt = "0",
                    //    noOfTrans = tresuryHeads.Count().ToString(),
                    //    ddoCode = (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.FACTORY_LICENCE ? ddoCodeInfo.FC_Treasury_DDOCode.ToString() : "0000"),// "0551",// + ddoCodeInfo.FC_Treasury_DDOCode.ToString(), //.PadLeft(4, '0'),
                    //    payLocCode = applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.FACTORY_LICENCE ? ddoCodeInfo.FC_Treasury_Code.ToString() : "0000",// "HPR00",// ddoCodeInfo.FC_Treasury_Code,
                    //    add1 = appRefId.ToString(),
                    //    add2 = "",
                    //    add3 = "",
                    //    add4 = "",
                    //    add5 = "",
                    //    sURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("MarchantSiteResponseUrl_Success").Value,
                    //    fURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("MarchantSiteResponseUrl_Failure").Value,
                    //};



                    nonTrsyChallanData = new IFMS_NonTreasuryChallanDataTemplateViewModel()
                    {
                        Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("Service").Value,
                        deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                        ddoCode = ddoCodeInfo.ALC_Treasury_Abbre + ddoCodeInfo.ALC_Treasury_DDOCode.ToString(),
                        hodMstId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("HodMstId").Value,
                        challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                        totalAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                        noOfTrans = tresuryHeads.Count().ToString(),
                        add1 = appRefId.ToString(),
                        add2 = paymentBatchCounter.ToString(),
                        add3 = paymentPartCounter.ToString(),
                        add4 = "",
                        add5 = "",
                        sURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("MarchantSiteResponseUrl_Success").Value,
                        fURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("MarchantSiteResponseUrl_Failure").Value,
                    };

                    var userProfile = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(appRefId);


                    //  identity = "1" --> PAN Number
                    //  identity = "2" --> Aadhar Number
                    //  identity = "3" --> Voter Card Number

                    nonTrsyChallanData.depositor_info = new IFMS_NonTreasuryPayeeInfo()
                    {
                        name = userProfile.FirstName + " " + userProfile.LastName,
                        mobNo = userProfile.MobileNo,
                        email = userProfile.Email,
                        addLine1 = projectSite.ResponseDataModel.Address,
                        addLine2 = projectSite.ResponseDataModel.Address,
                        addPincode = projectSite.ResponseDataModel.PinCode,
                        identity = "1",
                        identityNumber = projectSite.ResponseDataModel.ApplicantPanNumber == null ? "ABCDE1234F" : projectSite.ResponseDataModel.ApplicantPanNumber,
                        lgdDistCode = projectSite.ResponseDataModel.DistrictRefId,
                        lgdTehCode = projectSite.ResponseDataModel.TehsilRefId
                    };


                    //nonTrsyChallanData.payee_info = new IFMS_NonTreasuryPayeeInfo()
                    //nonTrsyChallanData.depositor_info = new IFMS_NonTreasuryPayeeInfo()
                    //{
                    //    payerName = userProfile.FirstName + " " + userProfile.LastName,
                    //    mobNo = userProfile.MobileNo,
                    //    email = userProfile.Email,
                    //    addLine1 = projectSite.FormModel.Address,
                    //    addLine2 = projectSite.FormModel.Address,
                    //    addPincode = projectSite.FormModel.PinCode,
                    //    lgdDistID = projectSite.FormModel.DistrictRefId.ToString(),
                    //    lgdTehID = projectSite.FormModel.TehsilRefId.ToString()
                    //};


                    //nonTrsyChallanData.trsyPayments = tresuryHeads;
                    //nonTrsyChallanData.nonTrsyPayments = null;

                    nonTrsyChallanData.ntPayments = new List<IFMS_NonTrsyPaymentsViewModel>();
                    string IFMS_Non_Treasury_ConfigSections = "IFMS_Non_Treasury";

                    if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                    {
                        //List<Int64> nonTreasuryFeeHeaders = new List<Int64>();
                        //nonTreasuryFeeHeaders.Add(19);
                        //nonTreasuryFeeHeaders.Add(22);
                        //nonTreasuryFeeHeaders.Add(27);
                        //var paymentDetailList = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && nonTreasuryFeeHeaders.Contains(x.FeeHeaderRefId)).ToList();

                        var appFeeDetail = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && (x.FeeHeaderRefId == 19 || x.FeeHeaderRefId == 27)).ToList();
                        var buildingPlanHUDPaymentDetail = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && x.FeeHeaderRefId == 22).FirstOrDefault();

                        foreach (var item in feeHeadersWithTreasuryCodes.Where(x=>x.Amount>0).ToList())
                        {
                            nonTrsyChallanData.ntPayments.Add(new IFMS_NonTrsyPaymentsViewModel()
                            {
                                Amt = Convert.ToInt32(item.Amount).ToString(),
                                desc = "FeeHeaderId_" + item.FeeHeaderRefId.ToString(),
                                //serviceID = (item.FeeHeaderRefId == 19 || item.FeeHeaderRefId == 27) ? appFeeDetail.Where(x => x.FeeHeaderRefId == item.FeeHeaderRefId).Select(x => x.DedicatedTreasurCode).FirstOrDefault() : buildingPlanHUDPaymentDetail.NonTreasuryCode // paymentDetailList.Where(x => x.FeeHeaderRefId == item.FeeHeaderRefId).Select(x => x.NonTreasuryCode).FirstOrDefault()
                                subServiceID = (item.FeeHeaderRefId == 19 || item.FeeHeaderRefId == 27) ? appFeeDetail.Where(x => x.FeeHeaderRefId == item.FeeHeaderRefId).Select(x => x.DedicatedTreasurCode).FirstOrDefault() : buildingPlanHUDPaymentDetail.NonTreasuryCode // paymentDetailList.Where(x => x.FeeHeaderRefId == item.FeeHeaderRefId).Select(x => x.NonTreasuryCode).FirstOrDefault()
                            });
                        }
                    }
                    else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                    {
                        var appFeeDetail = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && (x.FeeHeaderRefId == 19 || x.FeeHeaderRefId == 27)).ToList();
                        var payments_RaisedFee = _context.Payments_RaisedFee.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && x.FeeHeaderRefId == 22).FirstOrDefault();

                        foreach (var item in feeHeadersWithTreasuryCodes.Where(x => x.Amount > 0).ToList())
                        {
                            nonTrsyChallanData.ntPayments.Add(new IFMS_NonTrsyPaymentsViewModel()
                            {
                                Amt = Convert.ToInt32(item.Amount).ToString(),
                                desc = "FeeHeaderId_" + item.FeeHeaderRefId.ToString(),
                                subServiceID = (item.FeeHeaderRefId == 19) ? appFeeDetail.Where(x => x.FeeHeaderRefId == item.FeeHeaderRefId).Select(x => x.DedicatedTreasurCode).FirstOrDefault() : payments_RaisedFee.NonTreasuryCode
                            });
                        }
                    }
                    else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                    {
                        IFMS_Non_Treasury_ConfigSections = "IFMS_Non_Treasury_PSIEC";
                        nonTrsyChallanData.Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("Service").Value;
                        var appFeeDetail = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter && (x.FeeHeaderRefId == 39 || x.FeeHeaderRefId == 48)).ToList();
                        var feeHeader = _context.FeesHeaders.Where(x => x.FeeHeaderId == 39 || x.FeeHeaderId == 48).ToList();
                        foreach (var item in feeHeadersWithTreasuryCodes.Where(x => x.Amount > 0).ToList())
                        {
                            nonTrsyChallanData.ntPayments.Add(new IFMS_NonTrsyPaymentsViewModel()
                            {
                                Amt = Convert.ToInt32(item.Amount).ToString(),
                                desc = "FeeHeaderId_" + item.FeeHeaderRefId.ToString(),
                                subServiceID = (item.FeeHeaderRefId == 39 || item.FeeHeaderRefId == 48) ? feeHeader.Where(x => x.FeeHeaderId == item.FeeHeaderRefId).Select(x => x.DedicatedTreasurCode).FirstOrDefault() : ""
                            });
                        }
                    }

                    challanDataJson = JsonConvert.SerializeObject(nonTrsyChallanData);
                    //AgencyCode|deptRefNo|hodMstId|ddoCode|challanDate|totalAmt|noOfTrans|payerName
                    //string pipeline = nonTrsyChallanData.AgencyCode + "|" + nonTrsyChallanData.deptRefNo + "|" + nonTrsyChallanData.hodMstId + "|" + nonTrsyChallanData.ddoCode + "|" + nonTrsyChallanData.challanDate + "|" + nonTrsyChallanData.totalAmt + "|" + nonTrsyChallanData.noOfTrans + "|" + nonTrsyChallanData.payee_info.payerName;

                    string pipeline = nonTrsyChallanData.Service + "|" + nonTrsyChallanData.deptRefNo + "|" + nonTrsyChallanData.challanDate + "|" + nonTrsyChallanData.totalAmt + "|" + nonTrsyChallanData.noOfTrans + "|" + nonTrsyChallanData.depositor_info.name;

                    string checksumData = PaymentGatewayEncDecryOps.GenerateChecksum_IFMS(pipeline, Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("ChecksumKey").Value);
                    IFMS_RequestDataNonTreasuryViewModel iFMS_RequestData = new IFMS_RequestDataNonTreasuryViewModel()
                    {
                        chcksum = checksumData,
                        challandata = nonTrsyChallanData
                    };

                    requestdataJson = JsonConvert.SerializeObject(iFMS_RequestData);
                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury = new PaymentGatewayInputProps_IFMS_NonTreasury();

                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.encData = PaymentGatewayEncDecryOps.Encrypt_IFMS(requestdataJson, Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("SecretIV").Value);
                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("ClientId").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.clientSecret = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("ClientSecret").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("Service").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS_NonTreasury.ipAddress = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("IPAllow").Value;
                    paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection(IFMS_Non_Treasury_ConfigSections).GetSection("PaymentGatewayTargetUrl").Value;
                }
                else
                {
                    paymentGatewaySpecificInputProps.FormInputs_IFMS = new PaymentGatewayInputProps_IFMS();
                    challanData = new IFMS_ChallanDataTemplateViewModel()
                    {
                        deptRefNo = appFeePaymentInitiateTerminalInfo.UniquePaymentGatewayTransactionRefId,
                        receiptNo = "",
                        clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value,
                        deptCode = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("DeptCode").Value,
                        challanDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        expiryDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                        companyName = projectSite.ResponseDataModel.EstablishmentName,
                        totalAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                        trsyAmt = Convert.ToInt32(feeHeadersWithTreasuryCodes.Sum(x => x.Amount)).ToString(),
                        nonTrsyAmt = "0",
                        noOfTrans = tresuryHeads.Count().ToString(),
                        ddoCode = (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.FACTORY_LICENCE ? ddoCodeInfo.FC_Treasury_DDOCode.ToString() :applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.CONTRACT_LABOUR ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString(): applicationType == ApplicationTypeEnum.TRADE_UNION ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR ? ddoCodeInfo.ALC_Treasury_DDOCode.ToString() : "0000"),// "0551",// + ddoCodeInfo.FC_Treasury_DDOCode.ToString(), //.PadLeft(4, '0'),
                        payLocCode = applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.FACTORY_LICENCE ? ddoCodeInfo.FC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.CONTRACT_LABOUR ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER ? ddoCodeInfo.ALC_Treasury_Code.ToString(): applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.TRADE_UNION ? ddoCodeInfo.ALC_Treasury_Code.ToString() : applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR ? ddoCodeInfo.ALC_Treasury_Code.ToString() : "0000",// "HPR00",// ddoCodeInfo.FC_Treasury_Code,
                        add1 = appRefId.ToString(),
                        add2 = "",
                        add3 = "",
                        add4 = "",
                        add5 = "",
                        sURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("MarchantSiteResponseUrl_Success").Value,
                        fURL = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("MarchantSiteResponseUrl_Failure").Value,
                    };

                    var userProfile = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(appRefId);

                    challanData.payee_info = new IFMS_PayeeInfo()
                    {
                        payerName = userProfile.FirstName + " " + userProfile.LastName,
                        teleNumber = userProfile.MobileNo,
                        mobNumber = userProfile.MobileNo,
                        emailId = userProfile.Email,
                        addLine1 = projectSite.ResponseDataModel.Address,
                        addLine2 = projectSite.ResponseDataModel.Address,
                        //addPincode = Convert.ToString(app.PinCode) ?? Convert.ToString(app.SiteAddPinCode) ?? "123456",
                        addPincode = projectSite.ResponseDataModel.PinCode,
                        district = projectSite.ResponseDataModel.DistrictRefId.ToString(),
                        tehsil = projectSite.ResponseDataModel.TehsilRefId.ToString()
                    };
                    challanData.trsyPayments = tresuryHeads;
                    challanData.nonTrsyPayments = null;
                    challanDataJson = JsonConvert.SerializeObject(challanData);

                    string checksumData = PaymentGatewayEncDecryOps.GenerateChecksum_IFMS(challanDataJson, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ChecksumKey").Value);
                    IFMS_RequestDataViewModel iFMS_RequestData = new IFMS_RequestDataViewModel()
                    {
                        chcksum = checksumData,
                        challandata = challanData
                    };
                    requestdataJson = JsonConvert.SerializeObject(iFMS_RequestData);
                    paymentGatewaySpecificInputProps.FormInputs_IFMS.encData = PaymentGatewayEncDecryOps.Encrypt_IFMS(requestdataJson, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);
                    paymentGatewaySpecificInputProps.FormInputs_IFMS.clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientId").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS.clientSecret = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientSecret").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS.integratingAgency = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value;
                    paymentGatewaySpecificInputProps.FormInputs_IFMS.ipAddress = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IPAllow").Value;

                    paymentGatewaySpecificInputProps.PaymentGatewayTargetUrl = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("PaymentGatewayTargetUrl").Value;
                }
                //paymentGatewaySpecificInputProps.FormInputs_IFMS.encData = PaymentGatewayEncDecryOps.Encrypt_IFMS(requestdataJson, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);
            }
            return paymentGatewaySpecificInputProps;
        }

        public async Task<GenericServiceResultTemplate> LogAppFeeTransaction(AppFeePaymentInitiateTerminalInfoViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<AppFeePaymentInitiateTerminalInfoViewModel>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    //var test = formModel.PaymentGatewayClientFormInputDetail.PaymentGatewayTargetUrl;
                    //var jsonTestr = JsonConvert.SerializeObject(formModel.PaymentGatewayClientFormInputDetail.paymentGatewayClientFormInputList);
                    //formModel = GenericModelOps<Establishment_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                    //genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.REG_ESTB, formModel, formModel.ProjectSiteRefId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    var application = _iGR_Application.GetById(formModel.AppRefId);
                    AppFeeTransaction appFeeTransaction = new AppFeeTransaction()
                    {
                        AmountCalculated = formModel.AmountCalculated,
                        AppRefId = formModel.AppRefId,
                        PaymentGatewayApiMethodType = formModel.PaymentGatewayApiMethodType,
                        PaymentGatewayTargetUrl = formModel.PaymentGatewayClientFormInputDetail.PaymentGatewayTargetUrl,
                        PaymentGatewayType = formModel.PaymentGatewayType,
                        PaymentModeType = formModel.PaymentModeType,
                        PaymentTreasuryType = formModel.PaymentTreasuryType,
                        UniquePaymentGatewayTransactionId = formModel.UniquePaymentGatewayTransactionRefId,
                        TransactionInitializationDate = DateTime.Now,
                        IsWebRequestCycleCompleted = false,
                        RequestBodyData = JsonConvert.SerializeObject(formModel.PaymentGatewayClientFormInputDetail.paymentGatewayClientFormInputList),
                        TransactionFinalStatusType = TransactionFinalStatusTypeEnum.NEW,
                        PaymentBatchCounter = application.PaymentBatchCounter,
                        PaymentPartCounter = formModel.PaymentPartCounter
                    };
                    _iGR_AppFeeTransaction.Insert(appFeeTransaction);
                    await _iGR_AppFeeTransaction.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<List<dynamic>>> GetAllAppFeeTransactions(Int64 appRefId)
        {
            GenericFormModel<List<dynamic>> genericFormModel = new GenericFormModel<List<dynamic>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true}
                };
                List<AppFeeTransaction> allTransactions = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeeTransaction>("dbo.sp_Payments_Get_All_App_Fee_Transactions", storeProcedureParms);
                List<dynamic> transactions = new List<dynamic>();
                foreach (var item in allTransactions)
                {
                    dynamic transaction = new
                    {
                        TransactionInitializationDate = item.TransactionInitializationDate,
                        PaymentModeType = EnumOps.GetEnumAsSelectList<PaymentModeTypeEnum>().ToList().Where(x => x.Value == Convert.ToInt32(item.PaymentModeType).ToString()).Select(x => x.Text).FirstOrDefault(),
                        PaymentGatewayType = EnumOps.GetEnumAsSelectList<PaymentGatewayTypeEnum>().ToList().Where(x => x.Value == Convert.ToInt32(item.PaymentGatewayType).ToString()).Select(x => x.Text).FirstOrDefault(),
                        PaymentTreasuryType = EnumOps.GetEnumAsSelectList<PaymentTreasuryTypeEnum>().ToList().Where(x => x.Value == Convert.ToInt32(item.PaymentTreasuryType).ToString()).Select(x => x.Text).FirstOrDefault(),
                        UniquePaymentGatewayTransactionId = item.UniquePaymentGatewayTransactionId,
                        AmountCalculated = item.AmountCalculated,

                        IsWebRequestCycleCompleted = item.IsWebRequestCycleCompleted,
                        TransactionFinalStatusType = item.TransactionFinalStatusType,
                        TransactionFinalStatusTypeDesc = EnumOps.GetEnumAsSelectList<TransactionFinalStatusTypeEnum>().ToList().Where(x => x.Value == Convert.ToInt32(item.TransactionFinalStatusType).ToString()).Select(x => x.Text).FirstOrDefault(),
                        ResponseMessage = item.ResponseMessage,
                        AppFeeTransactionId = item.AppFeeTransactionId
                    };
                    transactions.Add(transaction);
                }
                genericFormModel.FormModel = transactions;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<bool>> VerifyAlreadyMadePayments(Int64[] appFeeTransactionIds)
        {
            GenericFormModel<bool> genericFormModel = new GenericFormModel<bool>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                if (appFeeTransactionIds.Count() > 0)
                {
                    StringBuilder commaSeparatedTranIds = new StringBuilder();
                    foreach (var item in appFeeTransactionIds)
                    {
                        commaSeparatedTranIds.Append(item.ToString() + ",");
                    }
                    storeProcedureParms.Add(new StoreProcedureParm() { ParmName = "appFeeTransactionIds", ParmValue = commaSeparatedTranIds.ToString(), isNumber = false });
                    List<AppFeeTransaction> allTransactions = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeeTransaction>("dbo.sp_Payments_Get_All_Verification_Transactions", storeProcedureParms);


                    foreach (var transaction in allTransactions)
                    {
                        if (transaction.PaymentGatewayType == PaymentGatewayTypeEnum.HDFC)
                        {

                            string Url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("PaymentVerificationUrl").Value;
                            string method = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("PaymentVerificationApiMethod").Value;
                            string salt = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("EncryptionConfigs").GetSection("Salt").Value;
                            string key = Configuration.GetSection("PaymentGatewayConfigs").GetSection("HDFC").GetSection("EncryptionConfigs").GetSection("Key").Value;

                            string plainText = key + "|" + method + "|" + transaction.UniquePaymentGatewayTransactionId + "|" + salt;
                            string hashedText = PaymentGatewayEncDecryOps.Generatehash512_HDFC(plainText);
                            string postString = "key=" + key +
                                                "&command=" + method +
                                                "&hash=" + hashedText +
                                                "&var1=" + transaction.UniquePaymentGatewayTransactionId;
                            //ViewBag.PostString = postString;
                            //ViewBag.Url = Url;

                            WebRequest myWebRequest = WebRequest.Create(Url);
                            myWebRequest.Method = "POST";
                            myWebRequest.ContentType = "application/x-www-form-urlencoded";
                            myWebRequest.Timeout = 180000;

                            StreamWriter requestWriter = new StreamWriter(myWebRequest.GetRequestStream());
                            requestWriter.Write(postString);
                            requestWriter.Close();

                            StreamReader responseReader = new StreamReader(myWebRequest.GetResponse().GetResponseStream());
                            WebResponse myWebResponse = myWebRequest.GetResponse();
                            Stream ReceiveStream = myWebResponse.GetResponseStream();
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                            StreamReader readStream = new StreamReader(ReceiveStream, encode);

                            string response = readStream.ReadToEnd();
                            JObject account = JObject.Parse(response);
                            string status = (string)account.SelectToken("transaction_details." + transaction.UniquePaymentGatewayTransactionId + ".status");
                        }
                        else if (transaction.PaymentGatewayType == PaymentGatewayTypeEnum.IFMS)
                        {
                            List<string> resData = new List<string>();
                            var resUrl = await PrepareChallanVerificationData_IFMS(transaction.UniquePaymentGatewayTransactionId);
                            resData.Add(resUrl);

                            ResponseDataViewModel_IFMS PaymentResp = new ResponseDataViewModel_IFMS();
                            var client = new RestSharp.RestClient(Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ChallanVerificationUrl").Value);
                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                            var request = new RestRequest();
                            request.Method = RestSharp.Method.Post;
                            request.AddHeader("Content-Type", "application/json");
                            Array responseArray = resData.ToArray();
                            int currIndex = 0;
                            while (responseArray.Length != currIndex)
                            {
                                request.AddParameter("application/json; charset=utf-8", responseArray.GetValue(currIndex), ParameterType.RequestBody);
                                //IRestResponse response = await client.ExecuteAsync(request);
                                var response = await client.ExecuteAsync(request);
                                PaymentResp = JsonConvert.DeserializeObject<ResponseDataViewModel_IFMS>(response.Content);
                                currIndex++;
                            }
                        }
                        else if (transaction.PaymentGatewayType == PaymentGatewayTypeEnum.SBI)
                        {
                            string mid = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("PbLabourMerchantId").Value;
                            string trxid = transaction.UniquePaymentGatewayTransactionId;// our unique cleint trx id/ order no
                            string URI = Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("SBIVerifyPayment").Value;

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            var request = (HttpWebRequest)WebRequest.Create(URI);
                            var postData = "queryRequest=|" + mid + "|" + trxid;
                            postData += "&aggregatorId=SBIEPAY";
                            postData += "&merchantId=" + mid + "";

                            var data = Encoding.ASCII.GetBytes(postData);
                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.ContentLength = data.Length;
                            using (var stream = request.GetRequestStream())
                            {
                                stream.Write(data, 0, data.Length);
                            }
                            var response = (HttpWebResponse)request.GetResponse();
                            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                            //if (verifyPaymentSBI.Type == "MobileApp")
                            //{
                            //    ViewBag.apiUrl = WebConfigurationManager.AppSettings["MobileAppResponse"];
                            //    ViewBag.inputJson = new JavaScriptSerializer().Serialize(responseClass);
                            //    return View();
                            //}
                            //else
                            //{
                            //    ViewBag.apiUrl = WebConfigurationManager.AppSettings["CessResponse"];
                            //    ViewBag.inputJson = new JavaScriptSerializer().Serialize(responseClass);
                            //    return View();
                            //}
                        }
                    }



                }
                genericFormModel.FormModel = true;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<string> PrepareChallanVerificationData_IFMS(string _deptRefno)
        {
            VerifyChallanInfo_IFMS challanDataInfo = new VerifyChallanInfo_IFMS();
            //IFMS_EncrDecr obj = new IFMS_EncrDecr("lBrWhKntarPx", "rGsWhKntvrPxn4tN", "rGsWhKntvrPxn4tN");
            challanDataInfo.challandata = new ChallanData_IFMS { deptRefNo = _deptRefno, clientId = "LAB01", deptCode = "LAB", challanDate = DateTime.Now };
            string serializedChallanData = JsonConvert.SerializeObject(challanDataInfo.challandata);
            challanDataInfo.chcksum = PaymentGatewayEncDecryOps.GenerateChecksum_IFMS(serializedChallanData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ChecksumKey").Value); //obj.CheckSum(json);
            string serializedChallanDataInfo = JsonConvert.SerializeObject(challanDataInfo);
            string encData = PaymentGatewayEncDecryOps.Encrypt_IFMS(serializedChallanDataInfo, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);

            var challanDataWithConfigSettings = new
            {
                encData = encData,
                clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientId").Value,
                clientSecret = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientSecret").Value,
                transactionID = new Random().Next(100000, 999999).ToString(),
                ipAddress = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IPAllow").Value,
                integratingAgency = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value
            };
            var reqData = JsonConvert.SerializeObject(challanDataWithConfigSettings);
            return reqData;
        }
        public async Task<string> GenerateUniquePaymentGatewayTransactionId(Int64 appRefId)
        {
            StringBuilder UniquePaymentGatewayTransactionId = new StringBuilder();
            string[] alphaCodes = { "0", "Z", "A", "X", "H", "V", "K", "T", "C", "R", "Q", "L" };
            var appRefIdAsChars = (appRefId.ToString()).ToCharArray();
            foreach (var item in appRefIdAsChars)
            {
                UniquePaymentGatewayTransactionId.Append(alphaCodes[Convert.ToInt32(item.ToString())]);
            }
            UniquePaymentGatewayTransactionId.Append("_");
            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true}
                };
            List<AppFeeTransaction> allTransactions = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeeTransaction>("dbo.sp_Payments_Get_All_App_Fee_Transactions", storeProcedureParms);
            UniquePaymentGatewayTransactionId.Append("T" + (allTransactions.Count() + 1).ToString());
            int lengthRequiredDiff = 15 - (UniquePaymentGatewayTransactionId.ToString().Length);
            if (lengthRequiredDiff > 0)
            {
                string zeros = "QTXYFBAMKDWHLPI";
                UniquePaymentGatewayTransactionId = UniquePaymentGatewayTransactionId.Replace("_", zeros.Substring(0, lengthRequiredDiff + 1));
            }
            else
            {
                UniquePaymentGatewayTransactionId = UniquePaymentGatewayTransactionId.Replace("_", "0");
            }


            return UniquePaymentGatewayTransactionId.ToString();
        }

        public async Task<decimal> HandlePaymentGatewayResponse_SBI(ResponseData_SBI_ViewModel requestData)
        {
            var tt = AES256.Decrypt(requestData.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("key_Array").Value);
            return 0;
        }

        public async Task<GenericResponseTemplateModel<int>> LogApplicationFeeHeaders(List<FeeCalculatorInfoParmsViewModel> requestData)
        {
            GenericResponseTemplateModel<int> genericServiceResultTemplate = new GenericResponseTemplateModel<int>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                var CustomeValidatorResp = CustomeValidator<List<FeeCalculatorInfoParmsViewModel>>.ValidateModel_AllProperties(requestData);
                if (CustomeValidatorResp.IsValid)
                {
                    var alreadyFeeDetails = _iGR_AppFeeDetail.GetAsync(x => x.AppRefId == requestData.Select(x => x.AppRefId).FirstOrDefault() && x.PaymentBatchCounter == requestData.FirstOrDefault().PaymentBatchCounter).Result.ToList();

                    if (alreadyFeeDetails.Count>0)
                    {
                        _iGR_AppFeeDetail.DeleteRange(alreadyFeeDetails);
                        await _iGR_AppFeeDetail.SavechangeAsync();
                    }

                    var alreadyPaymentParts = _iGR_AppPaymentPart.GetAsync(x => x.AppRefId == requestData.Select(x => x.AppRefId).FirstOrDefault() && x.PaymentBatchCounter == requestData.FirstOrDefault().PaymentBatchCounter).Result.ToList();

                    if (alreadyPaymentParts.Count>0)
                    {
                        _iGR_AppPaymentPart.DeleteRange(alreadyPaymentParts);
                        await _iGR_AppPaymentPart.SavechangeAsync();
                    }

                    if (requestData.Count>0)
                    {

                        requestData = requestData.Select(x => { x.IsTreasuryPayment = (x.FeeHeaderId == 19 || x.FeeHeaderId == 22 || x.FeeHeaderId == 27 || x.FeeHeaderId == 39 || x.FeeHeaderId == 48); return x; }).ToList();
                        
                        //var application = _iGR_Application.GetById(requestData.Select(x => x.AppRefId).FirstOrDefault());
                        genericServiceResultTemplate.ResponseDataModel = requestData.FirstOrDefault().PaymentBatchCounter;
                        List<string> distinctTreasuryCodes = new List<string>();

                        if (requestData.Any(x => x.HasDedicatedTreasuryCode))
                        {
                            distinctTreasuryCodes = requestData.Where(x=>!x.IsTreasuryPayment).Select(x => x.DedicatedTreasurCode).Distinct().ToList();
                        }
                        if (distinctTreasuryCodes.Count>0)
                        {
                            

                            int i = 1;
                            foreach (var treasuryCode in distinctTreasuryCodes)
                            {
                                //if (treasuryCode != "XXX00")
                                //{
                                bool isAppFeeDetailEntered = false;
                                    foreach (var item in requestData.Where(x => x.DedicatedTreasurCode == treasuryCode && x.FeeHeaderId != 22))
                                    {
                                        if (item.AmountCalculated>0)
                                        {
                                            AppFeeDetail appFeeTransaction = new AppFeeDetail()
                                            {
                                                Amount = item.AmountCalculated,
                                                AppRefId = item.AppRefId,
                                                CalculatedOn = DateTime.Now,
                                                FeeHeaderRefId = item.FeeHeaderId,
                                                IsDeduductible = item.IsDeduductible,
                                                PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                                PaymentPartCounter = i,
                                                HasDedicatedTreasuryCode = item.HasDedicatedTreasuryCode,
                                                DedicatedTreasurCode = item.DedicatedTreasurCode,
                                                DedicatedDDOCode = item.DedicatedDDOCode,
                                                Description = item.Description

                                            };
                                            _iGR_AppFeeDetail.Insert(appFeeTransaction);
                                            await _iGR_AppFeeDetail.SavechangeAsync();
                                            isAppFeeDetailEntered = true;
                                        }
                                    }

                                    if (requestData.Where(x => x.DedicatedTreasurCode == treasuryCode).Sum(x => x.AmountCalculated)>0 && isAppFeeDetailEntered)
                                    {

                                        AppPaymentPart paymentPart = new AppPaymentPart()
                                        {
                                            AppRefId = requestData.Select(x => x.AppRefId).FirstOrDefault(),
                                            LastUpdatedOn = DateTime.Now,
                                            PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                            PaymentPartCounter = i,
                                            TransactionFinalStatusType = TransactionFinalStatusTypeEnum.NEW
                                        };
                                        _iGR_AppPaymentPart.Insert(paymentPart);
                                        await _iGR_AppPaymentPart.SavechangeAsync();
                                        i = i + 1;
                                    }
                                //}
                            }

                            List<string> nonDistinctTreasuryCodes = new List<string>();

                            nonDistinctTreasuryCodes = requestData.Where(x => x.IsTreasuryPayment).Select(x => x.DedicatedTreasurCode).Distinct().ToList();
                            decimal totNonTreasuryAmount = 0;
                            foreach (var nonTreasuryCode in nonDistinctTreasuryCodes)
                            {
                                //if (treasuryCode != "XXX00")
                                //{
                                foreach (var item in requestData.Where(x => x.DedicatedTreasurCode == nonTreasuryCode && x.FeeHeaderId != 29))
                                {
                                    if (item.AmountCalculated > 0)
                                    {
                                        totNonTreasuryAmount += item.AmountCalculated;
                                        AppFeeDetail appFeeTransaction = new AppFeeDetail()
                                        {
                                            Amount = item.AmountCalculated,
                                            AppRefId = item.AppRefId,
                                            CalculatedOn = DateTime.Now,
                                            FeeHeaderRefId = item.FeeHeaderId,
                                            IsDeduductible = item.IsDeduductible,
                                            PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                            PaymentPartCounter = i,
                                            HasDedicatedTreasuryCode = item.HasDedicatedTreasuryCode,
                                            DedicatedTreasurCode = item.DedicatedTreasurCode,
                                            DedicatedDDOCode = item.DedicatedDDOCode,
                                            Description = item.Description
                                        };
                                        _iGR_AppFeeDetail.Insert(appFeeTransaction);
                                        await _iGR_AppFeeDetail.SavechangeAsync();
                                    }
                                }
                            }

                            if (nonDistinctTreasuryCodes.Count() > 0)
                            {
                                if (totNonTreasuryAmount > 0)
                                {

                                    AppPaymentPart paymentPart = new AppPaymentPart()
                                    {
                                        AppRefId = requestData.Select(x => x.AppRefId).FirstOrDefault(),
                                        LastUpdatedOn = DateTime.Now,
                                        PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                        PaymentPartCounter = i,
                                        TransactionFinalStatusType = TransactionFinalStatusTypeEnum.NEW
                                    };
                                    _iGR_AppPaymentPart.Insert(paymentPart);
                                    await _iGR_AppPaymentPart.SavechangeAsync();
                                    //i = i + 1;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in requestData)
                            {
                                AppFeeDetail appFeeTransaction = new AppFeeDetail()
                                {
                                    Amount = item.AmountCalculated,
                                    AppRefId = item.AppRefId,
                                    CalculatedOn = DateTime.Now,
                                    FeeHeaderRefId = item.FeeHeaderId,
                                    IsDeduductible = item.IsDeduductible,
                                    PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                    HasDedicatedTreasuryCode = item.HasDedicatedTreasuryCode,
                                    DedicatedTreasurCode = item.DedicatedTreasurCode,
                                    PaymentPartCounter = 1,
                                    DedicatedDDOCode = item.DedicatedDDOCode,
                                    Description = item.Description
                                };
                                _iGR_AppFeeDetail.Insert(appFeeTransaction);
                                await _iGR_AppFeeDetail.SavechangeAsync();


                                AppPaymentPart paymentPart = new AppPaymentPart()
                                {
                                    AppRefId = requestData.Select(x => x.AppRefId).FirstOrDefault(),
                                    LastUpdatedOn = DateTime.Now,
                                    PaymentBatchCounter = requestData.FirstOrDefault().PaymentBatchCounter,
                                    PaymentPartCounter = 1,
                                    TransactionFinalStatusType = TransactionFinalStatusTypeEnum.NEW
                                };
                                _iGR_AppPaymentPart.Insert(paymentPart);
                                await _iGR_AppPaymentPart.SavechangeAsync();

                            }
                        }
                    }
                }
                else
                {
                    genericServiceResultTemplate.HasError = true;
                    genericServiceResultTemplate.ErrorDesc = "Custome validation errors..!";
                    genericServiceResultTemplate.ResponseDataModel = 0;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                genericServiceResultTemplate.ResponseDataModel = 0;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<PaymentGatewayResponseToUiViewModel>> HandlePaymentGatewayResponse(dynamic responseObject, PaymentGatewayTypeEnum paymentGatewayType)
        {
            GenericFormModel<PaymentGatewayResponseToUiViewModel> genericFormModel = new GenericFormModel<PaymentGatewayResponseToUiViewModel>();
            PaymentGatewayResponseViewModel paymentGatewayResponse = null;
            string gatewayResponseType = string.Empty;
            if (paymentGatewayType == PaymentGatewayTypeEnum.HDFC) {
                ResponseDataViewModel_HDFC responseData = responseObject;
                string encData = "";
                Int64 appRefId = 0;
                string bankRefNumber = "";
                decimal transactionAmount = 0;
                int paymentBatchCounter = 0;
                int paymentPartCounter = 0;
                RazorpayClient client = new RazorpayClient(Configuration.GetSection("PaymentGatewayConfigs").GetSection("hdfc").GetSection("HDFCKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("hdfc").GetSection("HDFCSecret").Value);

                if (responseData.error != null) {
                  var  meta = JsonConvert.DeserializeObject<ErrorMetadata>(responseData.error.metadata);
                    responseData.razorpay_order_id = meta?.order_id;
                    responseData.razorpay_payment_id = meta?.payment_id;
                }
                    Razorpay.Api.Order order = client.Order.Fetch(responseData.razorpay_order_id);
                List<Razorpay.Api.Payment> payments = order.Payments();
                var transation = await _iGR_AppFeeTransaction.GetAsync(x => x.UniquePaymentGatewayTransactionId == responseData.razorpay_order_id).ConfigureAwait(false);
                appRefId = transation.FirstOrDefault().AppRefId;
                paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;
                bankRefNumber = "";
                Razorpay.Api.Payment payment = payments[0];
                transactionAmount = Convert.ToDecimal(payment["amount"]) / 100m;
                string paymentResponse = payment.Attributes.ToString();
                paymentGatewayResponse = new PaymentGatewayResponseViewModel()
                {
                    IsWebRequestCycleCompleted = true,
                    ResponseBodyData = paymentResponse,
                    ResponseMessage = order["status"].ToString(),
                    ResponseReceivedOn = DateTime.Now,
                    UniquePaymentGatewayTransactionId = responseData.razorpay_order_id, //resData.challandata.deptRefNo,
                    AppRefId = appRefId,
                    PaymentBatchCounter = paymentBatchCounter,
                    PaymentPartCounter = paymentPartCounter,
                    PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY,

                    TransactionFinalStatusType = order["status"].ToString() == "paid" ? TransactionFinalStatusTypeEnum.SUCCEED :
                      (order["status"].ToString() == "failed") ? TransactionFinalStatusTypeEnum.FAILED :
                      TransactionFinalStatusTypeEnum.UNKNOWN
                };
                var userProfile = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(appRefId);
                var projectSites = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(Convert.ToInt64(userProfile.FormModel.ProjectSiteId), appRefId, userProfile.FormModel.ProjectSiteVersion);
                genericFormModel.FormModel = new PaymentGatewayResponseToUiViewModel()
                {
                    PaymentGatewatResponseText = paymentResponse,
                    TransactionFinalStatus = paymentGatewayResponse.TransactionFinalStatusType,
                    TransactionDate = DateTime.Now,
                    AppRefId = appRefId,
                    EstablishmentName = projectSites.ResponseDataModel.EstablishmentName,
                    EstablishmentAddress = projectSites.ResponseDataModel.Address,
                    PublicAppRefNum = projectSites.ResponseDataModel.PublicAppRefNum,
                    ApplicationType = projectSites.ResponseDataModel.ApplicationType,
                    ApplicationPurposeType = projectSites.ResponseDataModel.ApplicationPurposeType,
                    BankRefNumber = bankRefNumber,
                    BankTransactionRefNumberType = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                    TransactionAmount = Convert.ToInt32(transactionAmount),
                    PaymentGatewayType = paymentGatewayType
                };

                
            }
            else if (paymentGatewayType == PaymentGatewayTypeEnum.SBI)
            {
                //var tt= AES256.Decrypt(requestData.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("SBI").GetSection("key_Array").Value);

            }
            else if (paymentGatewayType == PaymentGatewayTypeEnum.IFMS)
            {
                ResponseDataViewModel_IFMS responseData = responseObject;
                string encData = "";
                Int64 appRefId = 0;
                string bankRefNumber = "";
                decimal transactionAmount = 0;
                int paymentBatchCounter = 0;
                int paymentPartCounter = 0;
                //responseData.statusCode = "SC300";
                if (responseData.statusCode != "SC300")
                {
                    var transation = await _iGR_AppFeeTransaction.GetAsync(x => x.UniquePaymentGatewayTransactionId == responseData.deptRefNo).ConfigureAwait(false);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;
                    bankRefNumber = "";
                    transactionAmount = 0;
                }
                else
                {
                    encData = PaymentGatewayEncDecryOps.Decrypt_IFMS(responseData.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);
                    PaymentGatewayResponseTemplateViewModel_IFMS resData = JsonConvert.DeserializeObject<PaymentGatewayResponseTemplateViewModel_IFMS>(encData);
                    //appRefId = Convert.ToUInt32((resData.challandata.add1));

                    var transation = await _iGR_AppFeeTransaction.GetAsync(x => x.UniquePaymentGatewayTransactionId == responseData.deptRefNo).ConfigureAwait(false);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;

                    bankRefNumber = resData.challandata.bank_Res.BankRefNo;
                    transactionAmount = Convert.ToInt32(resData.challandata.totalAmt);
                }

                gatewayResponseType = responseData.statusCode;
                paymentGatewayResponse = new PaymentGatewayResponseViewModel()
                {
                    IsWebRequestCycleCompleted = true,
                    ResponseBodyData = encData,
                    ResponseMessage = responseData.msg,
                    ResponseReceivedOn = DateTime.Now,
                    UniquePaymentGatewayTransactionId = responseObject.deptRefNo, //resData.challandata.deptRefNo,
                    AppRefId = appRefId,
                    PaymentBatchCounter = paymentBatchCounter,
                    PaymentPartCounter = paymentPartCounter,
                    PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY,

                    TransactionFinalStatusType = gatewayResponseType == "SC300" ? TransactionFinalStatusTypeEnum.SUCCEED :
                        gatewayResponseType == "SC310" ? TransactionFinalStatusTypeEnum.PENDING :
                        (gatewayResponseType == "EC301" || gatewayResponseType == "EC302" || gatewayResponseType == "EC303" || gatewayResponseType == "EC304"
                        || gatewayResponseType == "EC305" || gatewayResponseType == "EC306" || gatewayResponseType == "EC307") ? TransactionFinalStatusTypeEnum.FAILED :
                        TransactionFinalStatusTypeEnum.UNKNOWN
                };

                var userProfile = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(appRefId);
                var projectSites = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(Convert.ToInt64(userProfile.FormModel.ProjectSiteId), appRefId, userProfile.FormModel.ProjectSiteVersion);

                //Update gateway response to view - model
                genericFormModel.FormModel = new PaymentGatewayResponseToUiViewModel()
                {
                    PaymentGatewatResponseText = responseObject.msg,
                    TransactionFinalStatus = paymentGatewayResponse.TransactionFinalStatusType,
                    TransactionDate = DateTime.Now,
                    AppRefId = appRefId,
                    EstablishmentName = projectSites.ResponseDataModel.EstablishmentName,
                    EstablishmentAddress = projectSites.ResponseDataModel.Address,
                    PublicAppRefNum = projectSites.ResponseDataModel.PublicAppRefNum,
                    ApplicationType = projectSites.ResponseDataModel.ApplicationType,
                    ApplicationPurposeType = projectSites.ResponseDataModel.ApplicationPurposeType,
                    BankRefNumber = bankRefNumber,
                    BankTransactionRefNumberType = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                    TransactionAmount = Convert.ToInt32(transactionAmount),
                    PaymentGatewayType = paymentGatewayType
                };

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BankTransactionReferenceTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BankTransactionReferenceTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "TransactionFinalStatusTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<TransactionFinalStatusTypeEnum>()
                });

                var transactionUpdateResp = await UpdateTransactionResponse(paymentGatewayResponse);

                if (transactionUpdateResp.ResponseDataModel == 1 && paymentGatewayResponse.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED) // Payment Success
                {
                    // Share status to Invest Punjab Portal

                    //if (Convert.ToBoolean(Configuration.GetSection("AppProcessSetting").GetSection("TimeLineFileProcessingEnabled").Value))
                    //{
                    //    await _iAppTimeLineManagerService.SeedTimeLineData(paymentGatewayResponse.AppRefId);
                    //}
                    var appdetails = _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefault();

                    if(appdetails.IsTimeLineFlow == true && (genericFormModel.FormModel.ApplicationType != ApplicationTypeEnum.TRADE_UNION || genericFormModel.FormModel.ApplicationType != ApplicationTypeEnum.MOTOR_TRANSPORT))
                    {
                        await _iAppTimeLineManagerService.SeedTimeLineData(paymentGatewayResponse.AppRefId, 0);
                    }

                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, (ApplicationTypeEnum)genericFormModel.FormModel.ApplicationType, AppActionTypeEnum.APP_SUBMITTED);
                }



            }
            else if (paymentGatewayType == PaymentGatewayTypeEnum.IFMS_NON_TREASURY)
            {
                ResponseDataViewModel_IFMS responseData = responseObject;
                string encData = "";
                Int64 appRefId = 0;
                string bankRefNumber = "";
                decimal transactionAmount = 0;
                int paymentBatchCounter = 0;
                int paymentPartCounter = 0;
                //responseData.statusCode = "SC300";
                if (responseData.statusCode != "SC300")
                {
                    var transation = await _iGR_AppFeeTransaction.GetAsync(x => x.UniquePaymentGatewayTransactionId == responseData.deptRefNo).ConfigureAwait(false);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;
                    bankRefNumber = "";
                    transactionAmount = 0;
                }
                else
                {
                    encData = PaymentGatewayEncDecryOps.Decrypt_IFMS(responseData.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretIV").Value);
                    PaymentGatewayResponseTemplateViewModel_IFMS resData = JsonConvert.DeserializeObject<PaymentGatewayResponseTemplateViewModel_IFMS>(encData);
                    //appRefId = Convert.ToUInt32((resData.challandata.add1));

                    var transation = await _iGR_AppFeeTransaction.GetAsync(x => x.UniquePaymentGatewayTransactionId == responseData.deptRefNo).ConfigureAwait(false);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;

                    bankRefNumber = resData.challandata.bank_Res.BankRefNo;
                    transactionAmount = Convert.ToInt32(resData.challandata.totalAmt);
                }

                gatewayResponseType = responseData.statusCode;
                paymentGatewayResponse = new PaymentGatewayResponseViewModel()
                {
                    IsWebRequestCycleCompleted = true,
                    ResponseBodyData = encData,
                    ResponseMessage = responseData.msg,
                    ResponseReceivedOn = DateTime.Now,
                    UniquePaymentGatewayTransactionId = responseObject.deptRefNo, //resData.challandata.deptRefNo,
                    AppRefId = appRefId,
                    PaymentBatchCounter = paymentBatchCounter,
                    PaymentPartCounter = paymentPartCounter,
                    PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY,

                    TransactionFinalStatusType = gatewayResponseType == "SC300" ? TransactionFinalStatusTypeEnum.SUCCEED :
                        gatewayResponseType == "SC310" ? TransactionFinalStatusTypeEnum.PENDING :
                        (gatewayResponseType == "EC301" || gatewayResponseType == "EC302" || gatewayResponseType == "EC303" || gatewayResponseType == "EC304"
                        || gatewayResponseType == "EC305" || gatewayResponseType == "EC306" || gatewayResponseType == "EC307") ? TransactionFinalStatusTypeEnum.FAILED :
                        TransactionFinalStatusTypeEnum.UNKNOWN
                };

                var userProfile = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(appRefId);
                var projectSites = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(Convert.ToInt64(userProfile.FormModel.ProjectSiteId), appRefId, userProfile.FormModel.ProjectSiteVersion);

                //Update gateway response to view - model
                genericFormModel.FormModel = new PaymentGatewayResponseToUiViewModel()
                {
                    PaymentGatewatResponseText = responseObject.msg,
                    TransactionFinalStatus = paymentGatewayResponse.TransactionFinalStatusType,
                    TransactionDate = DateTime.Now,
                    AppRefId = appRefId,
                    EstablishmentName = projectSites.ResponseDataModel.EstablishmentName,
                    EstablishmentAddress = projectSites.ResponseDataModel.Address,
                    PublicAppRefNum = projectSites.ResponseDataModel.PublicAppRefNum,
                    ApplicationType = projectSites.ResponseDataModel.ApplicationType,
                    ApplicationPurposeType = projectSites.ResponseDataModel.ApplicationPurposeType,
                    BankRefNumber = bankRefNumber,
                    BankTransactionRefNumberType = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                    TransactionAmount = Convert.ToInt32(transactionAmount),
                    PaymentGatewayType = paymentGatewayType
                };

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BankTransactionReferenceTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BankTransactionReferenceTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "TransactionFinalStatusTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<TransactionFinalStatusTypeEnum>()
                });

                var transactionUpdateResp = await UpdateTransactionResponse(paymentGatewayResponse);

                if (transactionUpdateResp.ResponseDataModel == 1 && paymentGatewayResponse.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED) // Payment Success
                {
                    // Share status to Invest Punjab Portal
                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, (ApplicationTypeEnum)genericFormModel.FormModel.ApplicationType, AppActionTypeEnum.APP_SUBMITTED);
                }

                

            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<int>> UpdateTransactionResponse(PaymentGatewayResponseViewModel paymentGatewayResponse)
        {

            GenericResponseTemplateModel<int> genericServiceResultTemplate = new GenericResponseTemplateModel<int>() { ResponseDataModel = 0 };
            try
            {
                genericServiceResultTemplate.HasError = false;
                if (paymentGatewayResponse != null)
                {

                    AppFeeTransaction appFeeTransaction = _iGR_AppFeeTransaction.GetAsync(x =>
                                                        x.UniquePaymentGatewayTransactionId == paymentGatewayResponse.UniquePaymentGatewayTransactionId
                                                        && x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter
                                                        && x.PaymentBatchCounter == paymentGatewayResponse.PaymentBatchCounter).Result.FirstOrDefault();
                    if (appFeeTransaction != null)
                    {
                        appFeeTransaction.IsWebRequestCycleCompleted = paymentGatewayResponse.IsWebRequestCycleCompleted;
                        appFeeTransaction.ResponseBodyData = paymentGatewayResponse.ResponseBodyData;
                        appFeeTransaction.ResponseReceivedOn = paymentGatewayResponse.ResponseReceivedOn;
                        appFeeTransaction.ResponseMessage = paymentGatewayResponse.ResponseMessage;
                        appFeeTransaction.TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                        appFeeTransaction.BankTransactionRefNumber1 = paymentGatewayResponse.UniquePaymentGatewayTransactionId;
                        appFeeTransaction.BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.CIN;
                        appFeeTransaction.BankSettlementOn = DateTime.Now;
                        appFeeTransaction.PaymentBatchCounter = paymentGatewayResponse.PaymentBatchCounter;
                        appFeeTransaction.PaymentTreasuryType = paymentGatewayResponse.PaymentTreasuryType;

                        _iGR_AppFeeTransaction.Update(appFeeTransaction);
                        await _iGR_AppFeeTransaction.SavechangeAsync();



                        //AppPaymentPart appPaymentPart = _iGR_AppPaymentPart.GetAsync(x=>x.AppRefId == paymentGatewayResponse.AppRefId 
                        //                                && x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter
                        //                                && x.PaymentBatchCounter == paymentGatewayResponse.PaymentBatchCounter).Result.FirstOrDefault();
                        var allPaymentParts = _iGR_AppPaymentPart.GetAsync(x => x.AppRefId == paymentGatewayResponse.AppRefId
                                                                           && x.PaymentBatchCounter == paymentGatewayResponse.PaymentBatchCounter).Result.ToList();
                        List<AppPaymentPart> paymentParts = new List<AppPaymentPart>();

                        foreach (var item in allPaymentParts.Select(x => new { x.PaymentBatchCounter, x.PaymentPartCounter }).Distinct().ToList())
                        {
                            paymentParts.Add(allPaymentParts.Where(x => x.PaymentPartCounter == item.PaymentPartCounter && x.PaymentBatchCounter == item.PaymentBatchCounter).FirstOrDefault());
                        }

                        if (paymentParts.Where(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter).Count() > 0)
                        {
                            var paymentPart = paymentParts.Where(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter).FirstOrDefault();
                            paymentPart.TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                            _iGR_AppPaymentPart.Update(paymentPart);
                            await _iGR_AppPaymentPart.SavechangeAsync();
                            paymentParts[paymentParts.ToList().FindIndex(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter)].TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                        }

                        GenericServiceResultTemplate appServiceResponse = new GenericServiceResultTemplate();
                        if (appFeeTransaction.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED)
                        {
                            int hasAllPartsPaid = 0;
                            hasAllPartsPaid = (paymentParts.Count() == 0 || (paymentParts.Count() > 0 && paymentParts.Count() == paymentParts.Count(x => x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED))) ? 1 : 0;
                            genericServiceResultTemplate.ResponseDataModel = hasAllPartsPaid;
                            appServiceResponse = await _iApplicationMamnagementService.RecordAppFeeSuccessTransactionEntry(appFeeTransaction.AppRefId, appFeeTransaction.AppFeeTransactionId, paymentGatewayResponse.PaymentPartCounter, paymentGatewayResponse.PaymentBatchCounter, hasAllPartsPaid);

                        }

                        if (appServiceResponse.HasException)
                        {
                            genericServiceResultTemplate.HasError = true;
                            genericServiceResultTemplate.ErrorDesc = appServiceResponse.Exceptions.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        #region Application Payment Details
        public async Task<GenericResponseTemplateModel<PaymentDetailViewModel>> GetApplicationPaymentDetails(Int64 appRefId)
        {
            GenericResponseTemplateModel<PaymentDetailViewModel> genericFormModel = new GenericResponseTemplateModel<PaymentDetailViewModel>();
            try
            {
                var successfullPayments = await _iGR_AppPaymentSuccessTransactionMapping.GetAsync(x => x.AppRefId == appRefId, null, x => x.AppFeeTransaction).ConfigureAwait(false);
                var availableBatchs = successfullPayments.Select(x => x.AppFeeTransaction.PaymentBatchCounter).Distinct().ToArray();
                var appFeeDetailList = await _iGR_AppFeeDetail.GetAsync(x => x.AppRefId == appRefId && availableBatchs.Contains(x.PaymentBatchCounter) && !(x.FeeHeaderRefId >= 19 && x.FeeHeaderRefId <= 28), null, x => x.FeesHeader).ConfigureAwait(false);
                var buildingPlanRaisedFees = await _iGR_BuildingPlanHUDPaymentDetail.GetAsync(x => x.AppRefId == appRefId && availableBatchs.Contains(x.PaymentBatchCounter), null, x => x.FeesHeader).ConfigureAwait(false);
                genericFormModel.ResponseDataModel = new PaymentDetailViewModel();
                genericFormModel.ResponseDataModel.AppFeeTransactionDetailList = new List<AppFeeTransactionDetailsViewModal>();

                foreach (var item in availableBatchs)
                {
                    genericFormModel.ResponseDataModel.AppFeeTransactionDetailList.Add(new AppFeeTransactionDetailsViewModal()
                    {
                        PaymentBatchCounter = item,
                        BankSettlementOn = successfullPayments.Where(x => x.AppFeeTransaction.PaymentBatchCounter == item).OrderByDescending(x => x.AppFeeTransaction.BankSettlementOn).Select(x => x.AppFeeTransaction.BankSettlementOn).FirstOrDefault(),
                        AppFeeDetails = appFeeDetailList.Where(x => x.PaymentBatchCounter == item).ToList(),
                        AppFeeSuccessTransactions = successfullPayments.Where(x => x.AppFeeTransaction.PaymentBatchCounter == item).Select(x => x.AppFeeTransaction).ToList(),
                        BuildingPlanHUDPaymentDetailList = buildingPlanRaisedFees.Where(x => x.PaymentBatchCounter == item).ToList()
                    });
                }
                genericFormModel.ResponseDataModel.PaymentProofList = new List<PaymentProofViewModal>();

                var parentObject = await _iGR_ApplicationDocument.GetAsync(x => x.AppRefId == appRefId && x.DocumentRefId == 10002 && x.IsLocked, null, x => x.Document).ConfigureAwait(false);
                if (parentObject.ToList().Count() > 0)
                {
                    foreach (var item in parentObject.ToList())
                    {
                        genericFormModel.ResponseDataModel.PaymentProofList.Add(new PaymentProofViewModal() { AttachmentName = item.AttachmentName, AttachmentTitle = item.Document.DocumentName, UploadedOn = item.LastModifiedDate });
                    }
                }


                if (buildingPlanRaisedFees.Count() > 0)
                {
                    buildingPlanRaisedFees = buildingPlanRaisedFees.Select(x => { x.AmountPayable = x.AmountRaised - x.AmountAlreadyPaid; return x; }).ToList();

                    //buildingPlanRaisedFees = buildingPlanRaisedFees.Where(x => x.FeeHeaderRefId == 19).Select(x => { x.Description = "1 % of building cost as per CA certificate"; return x; }).ToList();
                    //buildingPlanRaisedFees = buildingPlanRaisedFees.Where(x => x.FeeHeaderRefId == 20).Select(x => { x.Description = "Rs 60 per square meter of covered area."; return x; }).ToList();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Application Payment Details

        #region Building Plan HUD Fee Raised
        public async Task<GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>> BuildingPlanHUDFeeRaised(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId)
        {
            GenericFormModel<List<FeeCalculatorInfoParmsViewModel>> genericFormModel = new GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>();
            try
            {
                genericFormModel.FormModel = new List<FeeCalculatorInfoParmsViewModel>();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 20;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 21;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 22;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 23;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 24;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 25;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 26;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 27;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });

                    feeHeaderId = 28;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        FeeHeaderId = feeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        Description = calculatedFeeDetail.Description,
                        IsDeduductible = false,
                        AppRefId = appRefId
                    });
                }

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, applicationType, entityKeyId, "PAYMENT");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

        public async Task<GenericResponseTemplateModel<AppPaymentPartsDetailViewModel>> GetPaymentPart(Int64 appRefId, int paymentBatchCounter)
        {
            GenericResponseTemplateModel<AppPaymentPartsDetailViewModel> genericFormModel = new GenericResponseTemplateModel<AppPaymentPartsDetailViewModel>()
            {
                ResponseDataModel = new AppPaymentPartsDetailViewModel()
            };
            try
            {
                genericFormModel.ResponseDataModel.AppPaymentParts = new List<AppPaymentPart>();
                genericFormModel.ResponseDataModel.AppPaymentParts = _iGR_AppPaymentPart.GetAsync(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter).Result.ToList();

                genericFormModel.ResponseDataModel.AppFeeDetails = new List<AppFeeDetail>();
                genericFormModel.ResponseDataModel.AppFeeDetails = _iGR_AppFeeDetail.GetAsync(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter, null, x => x.FeesHeader).Result.ToList();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<AppPaymentEDCAuthority>>> GetPaymentEDCAuthorities()
        {
            GenericResponseTemplateModel<List<AppPaymentEDCAuthority>> genericFormModel = new GenericResponseTemplateModel<List<AppPaymentEDCAuthority>>();
            try
            {
                genericFormModel.ResponseDataModel = _context.AppPaymentEDCAuthorities.ToList();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<AppFeePaymentReceiptViewModel>> GenerateFeeReceipt(Int64 investPunjabIPin, Int64 investPunjabAppId,Int64 appRefId)
        {
            GenericResponseTemplateModel<AppFeePaymentReceiptViewModel> genericFormModel = new GenericResponseTemplateModel<AppFeePaymentReceiptViewModel>();
            try
            {
                genericFormModel.ResponseDataModel = new AppFeePaymentReceiptViewModel();
                genericFormModel.ResponseDataModel.AppFeePaymentReceiptData = new List<AppFeePaymentReceiptDataViewModel>(); 
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="InvestPunjab_Ipin", ParmValue= investPunjabIPin.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="InvestPunjab_AppId", ParmValue= investPunjabAppId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true}
                };
                genericFormModel.ResponseDataModel.AppFeePaymentReceiptData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeePaymentReceiptDataViewModel>("dbo.sp_Payments_AppFeeReceiptDetail", storeProcedureParms);

                genericFormModel.ResponseDataModel.AppRefId = genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Select(x => x.AppRefId).FirstOrDefault();
                genericFormModel.ResponseDataModel.EstablishmentName = genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Select(x => x.EstablishmentName).FirstOrDefault();
                genericFormModel.ResponseDataModel.PublicAppRefNum = genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Select(x => x.PublicAppRefNum).FirstOrDefault();
                genericFormModel.ResponseDataModel.GrandTotal = genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Sum(x=>x.Amount);

                if (genericFormModel.ResponseDataModel.AppRefId > 0)
                {
                    genericFormModel.ResponseDataModel.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Select(x => x.ApplicationType.ToString()).FirstOrDefault());
                    genericFormModel.ResponseDataModel.ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(genericFormModel.ResponseDataModel.AppFeePaymentReceiptData.Select(x => x.ApplicationPurposeType.ToString()).FirstOrDefault());
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<ApplicationRaiseFeeParmsViewModel>> GetApplicationRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<ApplicationRaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<ApplicationRaiseFeeParmsViewModel>();
            try
            {
                genericFormModel.FormModel = new ApplicationRaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms = new List<ApplicationRaiseFeeCalculatorInfoParmsViewModel>();
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new ApplicationRaiseFeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.ApplicableRaiseFeeHeads[0]?.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = Configuration.GetSection("PaymentGatewayConfigs").GetSection("NonTreasuryCodes").GetSection("BOCWCess").Value
                    });
                }

                genericFormModel.FormModel.FeeCalculatorInfoParms = genericFormModel.FormModel.FeeCalculatorInfoParms.OrderBy(x => x.IsTreasuryPayment).ToList();

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Payments_RaisedFee>>> GetRaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<Payments_RaisedFee>> genericResponse = new GenericFormModel<List<Payments_RaisedFee>>();
            try
            {
                if (paymentBatchCounter > -1)
                {
                    var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();

                }
                var parentObject = _context.Payments_RaisedFee.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == (paymentBatchCounter > 0 ? paymentBatchCounter : x.PaymentBatchCounter)).Include(x => x.FeesHeader).ToList();
                genericResponse.FormModel = parentObject.ToList();
                genericResponse.FormModel = genericResponse.FormModel.Select(x => { x.HasDedicatedTreasuryCode = x.FeesHeader.HasDedicatedTreasuryCode; x.DedicatedTreasurCode = x.FeesHeader.DedicatedTreasurCode.Trim(); x.DedicatedDDOCode = x.FeesHeader.DedicatedDDOCode; return x; }).ToList();
                genericResponse.AppFormStepsList = null;
                if (!genericResponse.HasError && genericResponse.FormModel.Count() > 0)
                {
                    genericResponse.FormModel = genericResponse.FormModel.Select(x =>
                    {
                        x.AmountPayable = x.AmountRaised - x.AmountAlreadyPaid;
                        x.Description = x.Description = _context.ApplicationAndFeeRaiseAllowedMappings.Where(x => x.FeeHeaderRefId == parentObject.FirstOrDefault().FeeHeaderRefId).Select(x => x.Description).FirstOrDefault();
                        return x;
                    }).ToList();
                }
                genericResponse.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericResponse.HasError = true;
                genericResponse.ErrorDesc = ex.Message;
            }
            return genericResponse;
        }

        public async Task<GenericResponseTemplateModel<List<AppFeeDetail>>> GetAppFeeDetails(Int64 appRefId)
        {
            GenericResponseTemplateModel<List<AppFeeDetail>> genericResponse = new GenericResponseTemplateModel<List<AppFeeDetail>>();
            try
            {
                genericResponse.ResponseDataModel = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId).ToList();
            }
            catch (Exception ex)
            {
                genericResponse.HasError = true;
                genericResponse.ErrorDesc = ex.Message;
            }
            return genericResponse;
        }

        public async Task<GenericResponseTemplateModel<List<EstablishmentWisePaymentDetailsViewModel>>> GetEstablishmentWisePaymentDetails(EstablishmentWisePaymentDetailsRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<EstablishmentWisePaymentDetailsViewModel>> genericResponseTemplate = new GenericResponseTemplateModel<List<EstablishmentWisePaymentDetailsViewModel>>();
            try
            {
                if(requestData.ServiceType == null)
                {
                    requestData.ServiceType = "0";
                }
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="FromDate", ParmValue=requestData.FromDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDate", ParmValue=requestData.ToDate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="ServiceType", ParmValue=requestData.ServiceType.ToString(), isNumber=true}
                };
                genericResponseTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EstablishmentWisePaymentDetailsViewModel>("dbo.sp_Dashboard_GetEstablishmentWisePaymentDetails", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc = ex.Message;
            }
            return genericResponseTemplate;
        }
    }

    public class PaymentDetailViewModel
    {
        public List<AppFeeTransactionDetailsViewModal> AppFeeTransactionDetailList { get; set; }
        public List<PaymentProofViewModal> PaymentProofList { get; set; }
    }

    public class AppFeeTransactionDetailsViewModal
    {
        public int PaymentBatchCounter { get; set; }
        public DateTime BankSettlementOn { get; set; }
        public List<AppFeeDetail> AppFeeDetails { get; set; }
        public List<AppFeeTransaction> AppFeeSuccessTransactions { get; set; }
        public List<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetailList { get; set; }

    }

    public class PaymentProofViewModal
    {
        public string AttachmentName { get; set; }
        public string AttachmentTitle { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}