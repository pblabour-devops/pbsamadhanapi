using Castle.DynamicProxy.Generators;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    public class AppTimeLineManagerService : IAppTimeLineManagerService
    {
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        //private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        public AppTimeLineManagerService(AppDbContext context, IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _context = context;
            //_iApplicationMamnagementService = iapplicationManagementService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }
        public async Task<GenericResponseTemplateModel<bool>> SeedTimeLineData(Int64 appRefId, Int64 appActionLogRefId)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();

                var applicationActionLog = new ApplicationActionLog();
                if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    var parentWithChildObject = await _context.ApplicationAction_ParallelProcesses.Where(x => x.ApplicationRefId == appRefId && x.IsAlive == true && x.AppActionParallelProcessId == appActionLogRefId).FirstOrDefaultAsync();
                    applicationActionLog = new ApplicationActionLog()
                    {
                        ActionDate = parentWithChildObject.ActionDate,
                        ActionTakenDaysCount = parentWithChildObject.ActionTakenDaysCount,
                        ActionTakenHoursCount = parentWithChildObject.ActionTakenHoursCount,
                        AppActionType = parentWithChildObject.AppActionType,
                        ApplicationRefId = parentWithChildObject.ApplicationRefId,

                        Receiver_ProfileRefId = parentWithChildObject.Receiver_ProfileRefId,
                        Receiver_UserRefId = parentWithChildObject.Receiver_UserRefId,

                        Sender_ProfileRefId = parentWithChildObject.Sender_ProfileRefId,
                        Sender_UserRefId = parentWithChildObject.Sender_UserRefId,
                        Remarks = parentWithChildObject.Remarks,
                        ReceiverRoleId = parentWithChildObject.ReceiverRoleId,
                        SenderRoleId = parentWithChildObject.SenderRoleId,
                        AppDocumentRefId = 0,
                        IsDocumentUploaded = false,

                        IpAddress = parentWithChildObject.IpAddress,
                        Latitude = parentWithChildObject.Latitude,
                        Longitude = parentWithChildObject.Longitude,
                        ApplicationActionLogId = parentWithChildObject.AppActionParallelProcessId
                    };
                }
                else
                {
                    applicationActionLog = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == appRefId && x.IsDeleted == false).OrderByDescending(x => x.ApplicationActionLogId).FirstOrDefault();
                }

                //var applicationActionLog = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == appRefId && x.IsDeleted == false).OrderByDescending(x => x.ApplicationActionLogId).FirstOrDefault();
                var latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (latestProcessPhaseLog ==null || (applicationActionLog.AppActionType == (int)AppActionTypeEnum.APP_SUBMITTED || applicationActionLog.AppActionType == (int)AppActionTypeEnum.FACTORY_APP_SUBMITTED_FEE_NOT_APPLICABLE
                    || (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC
                    || application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR
                    || application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                    && (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)))
                {
                    ApplicationProcessPhaseLog applicationProcessPhaseLog = new ApplicationProcessPhaseLog()
                    {
                        AppRefId = appRefId,
                        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE,
                        LastModifiedOn = applicationActionLog.ActionDate,
                        PhaseCounter = 1
                    };

                    await _context.AddAsync(applicationProcessPhaseLog);
                    await _context.SaveChangesAsync();
                }

                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId).OrderByDescending(x => x.Id).FirstOrDefault();

                List<AllowedActionWithTimeLimitViewModel> allowedAppActionTypes = new List<AllowedActionWithTimeLimitViewModel>();
                //int maxHoursAllowed = 0;


                // SWITHING OF APP PHASE TYPE
                 if ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE &&
                    (applicationActionLog.SenderRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionLog.SenderRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa"))
                    ||

                    (applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE &&
                    (applicationActionLog.SenderRoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088")) //ALLC
                    ||
                    

                    ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE
                    && (applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")) //ADRF, DDRF
                    )

                    ||

                    ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE
                    && (applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce" && applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce")) //JDRF, JDRF
                    )

                    ||

                    ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE
                    && (applicationActionLog.SenderRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3" || applicationActionLog.SenderRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16")) //JDM || JDRF
                    )

                    ||

                    ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED &&
                    latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE
                    && (applicationActionLog.SenderRoleId == "2052EA93-333A-40A7-A6B7-0303FBF1C2E2" && applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce")) //JDRF, JDRF
                    )


                    )
                {
                    await AddAppProcessLogType(new ApplicationProcessPhaseLog
                    {
                        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE,
                        AppRefId = appRefId,
                        LastModifiedOn = applicationActionLog.ActionDate,
                        PhaseCounter = 1,
                    });
                }
                else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE &&
                    (
                        (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && (applicationActionLog.ReceiverRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416"
                                || applicationActionLog.ReceiverRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa") 
                                && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED) //ADRF, DDRF

                        ||

                        (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT && (_context.Licence_BocwAct_GeneralDetails.Where(x=>x.AppRefId== appRefId).Select(x=>x.BOCWActCircleType).FirstOrDefault()==BOCWActCircleTypeEnum.FACTORY_WING) && (applicationActionLog.ReceiverRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416"
                                || applicationActionLog.ReceiverRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa") 
                                && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED) //ADRF, DDRF

                        ||

                        (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT 
                            && (_context.Licence_BocwAct_GeneralDetails.Where(x => x.AppRefId == appRefId).Select(x => x.BOCWActCircleType).FirstOrDefault() == BOCWActCircleTypeEnum.LABOUR_WING) 
                            && applicationActionLog.ReceiverRoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088" && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED) //ALLC

                        ||

                        ((application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR ||
                        application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                            && applicationActionLog.ReceiverRoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088" && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED) //ALLC

                        ||

                        ((application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                            && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9"
                                && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)//ADDF

                        ||

                        ((application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                            && applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce"
                                && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)

                        ||

                        ((application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                            && applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3"
                                && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)
                   )

                )
                {
                    await AddAppProcessLogType(new ApplicationProcessPhaseLog
                    {
                        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE,
                        AppRefId = appRefId,
                        LastModifiedOn = applicationActionLog.ActionDate,
                        PhaseCounter = 1,
                    });
                }
                else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE
                               && applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)
                
                {
                    await AddAppProcessLogType(new ApplicationProcessPhaseLog
                    {
                        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE,
                        AppRefId = appRefId,
                        LastModifiedOn = applicationActionLog.ActionDate,
                        PhaseCounter = 1,
                    });
                }




                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId).OrderByDescending(x => x.Id).FirstOrDefault();
                string userId = applicationActionLog.ReceiverRoleId;
                if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.APP_OBJECTION || applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED_SCRUTINY_PHASE)
                {
                    //await UpdateAppProcessedTypePhaseCounter(latestProcessPhaseLog);
                    await AddPhaseAndObjectionLog(new ApplicationProcessPhaseAndObjectionLog()
                    {
                        AppActionLogRefId = applicationActionLog.ApplicationActionLogId,
                        AppRefId = applicationActionLog.ApplicationRefId,
                        ApplicationProcessPhaseLogType = latestProcessPhaseLog.ApplicationProcessPhaseLogType,
                        RoleRefId = applicationActionLog.SenderRoleId,

                    });
                    userId = applicationActionLog.SenderRoleId;
                }
                int totalObjectionRaisedByRoleId = _context.ApplicationProcessPhaseAndObjectionLogs.Count(x => x.AppRefId == appRefId && x.RoleRefId == userId && x.ApplicationProcessPhaseLogType == latestProcessPhaseLog.ApplicationProcessPhaseLogType);

                List<AppActionTime_MutualProcessFlag> mutualProcessFlags = new List<AppActionTime_MutualProcessFlag>();

                // ADD ALLOWED ACTION CODES
                if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    if (applicationActionLog.ReceiverRoleId == "692e93a4-9351-4d8a-9ed3-ecf9c3574178") // DH- FACTORY
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 24, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { 
                                AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, 
                                MaxHoursAllowed = 24, 
                                IsAutoAction = true, 
                                UserRefId = applicationActionLog.Receiver_UserRefId, 
                                PrecheckCode ="",
                                IsExpiredByAutoProcess = false
                            });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionLog.ReceiverRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa") // ADRF/DDRF- FACTORY
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "" , IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 168; // 7 Days
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "" });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 72; // 3 Days
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0 && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // ADDF  receives from LC - FACTORY
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF - FACTORY
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LC - FACTORY
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") // ADDF- BP  && INDL
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND, MaxHoursAllowed = 72 });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_BP || applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_OFFLINE) && (applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // INDL -ADDF
                        {
                            if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED
                            && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE
                            && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED
                            && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE
                            && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED
                            && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.SenderRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754") // ADDF  && DLBP
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        var remainingHours = 72 - consumedHours;
                        List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                        remainingHours = remainingHours + (dates.Count() * 24);
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND, MaxHoursAllowed = remainingHours });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754") // DLBP
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = (72 - consumedHours), IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF  && ADDF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_FIELD_OFFICER, MaxHoursAllowed = 0, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.ReceiverRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionLog.ReceiverRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa")
                        && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADRF-DDRF  || ADDF
                    {
                        //latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        //int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }

                    else if ((applicationActionLog.SenderRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionLog.SenderRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa")
                        && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADRF-DDRF  || ADDF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }

                    else if ((applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") && applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // ADDF-LBCR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1 , IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1 , IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });

                        }
                    }
                    else if ((applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // LBCR -ADDF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.REJECT_RECOMMENDATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.MARK_BACK && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.MARK_BACK && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.REJECT_RECOMMENDATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    if (applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce" && applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") // JDRF - INDL
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId });
                        }
                        else if ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED || applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED_SCRUTINY_PHASE) && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID || applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_OFFLINE) && (applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") && applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") // INDL -JDRF
                        {
                            if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsDocumentUploadOption=true, DocRefId= 30013, IsOptional= false, IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false, IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED
                            && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE
                            && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED
                            && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE
                            && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false, IsDocumentUploadOption = true, DocRefId= 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754" && applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") // DLBP - JDRF
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        var remainingHours = 72 - consumedHours;

                        List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                        remainingHours = remainingHours + (dates.Count() * 24);

                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce" && applicationActionLog.SenderRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754") // JDRF  && DLBP
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        var remainingHours = 72 - consumedHours;
                        List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                        remainingHours = remainingHours + (dates.Count() * 24);
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce" && applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") // JDRF  && JDRF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_FIELD_OFFICER, MaxHoursAllowed = 240, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 240, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.ReceiverRoleId == "5D4484CC-7BBF-4996-B035-139B5EBBC07E") && applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") // DTP  || JDRF
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 51, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 51, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.ReceiverRoleId == "9F45F1B3-E8B7-4CC9-BDFD-4CC9DCBD3755") && applicationActionLog.SenderRoleId == "5D4484CC-7BBF-4996-B035-139B5EBBC07E") // ARHQ  || DTP
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.SenderRoleId == "9F45F1B3-E8B7-4CC9-BDFD-4CC9DCBD3755") && applicationActionLog.ReceiverRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") // ARHQ  || JDRF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_FIELD_OFFICER, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72 - consumedHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_FIELD_OFFICER, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_FIELD_OFFICER, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }

                    else if ((applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") && applicationActionLog.ReceiverRoleId == "9F45F1B3-E8B7-4CC9-BDFD-4CC9DCBD3755") // ARHQ  || JDRF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE || latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE || latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // JDRF-ADDF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if ((applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" || applicationActionLog.SenderRoleId == "4b4cbe33-e4cb-4307-ab16-7fc546a4c7ce") && applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // ADDF-LBCR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }

                    else if ((applicationActionLog.SenderRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754" || applicationActionLog.SenderRoleId == "5D4484CC-7BBF-4996-B035-139B5EBBC07E" || applicationActionLog.SenderRoleId == "9F45F1B3-E8B7-4CC9-BDFD-4CC9DCBD3755" 
                        || applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        && applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // ADDF-LBCR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }



                    else if ((applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // LBCR -ADDF
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.REJECT_RECOMMENDATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.MARK_BACK && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.REJECT_RECOMMENDATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.MARK_BACK && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                   /*why such action is coming in deemed phase */        //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (((applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID || applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_OFFLINE) && applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") && applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // INDL -ADDF  -- Raise Fee Paid
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754" && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // DLBP - ADDF
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_TO_DEALING_HAND_FOR_FEE_RECONCILE && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }


                        //latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        //int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        //var remainingHours = 72 - consumedHours;
                        //List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                        //remainingHours = remainingHours + (dates.Count() * 24);
                        //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9" && applicationActionLog.SenderRoleId == "0689829f-dd5d-459e-aae6-9dd6243b7754") // ADDF - DLBP
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 72 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = 72 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FEE_RECONCILIATION && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsDocumentUploadOption = true, DocRefId = 30013, IsOptional = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_SENT_TO_APPLICANT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }


                        //latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        //int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 240 - consumedHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }

                    // Case Sent to concerned DTP Directely from JDRF Account-
                    else if (applicationActionLog.ReceiverRoleId == "5D4484CC-7BBF-4996-B035-139B5EBBC07E") // DTP
                    {
                        latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                        int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 51, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 51, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9" && applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL && applicationActionLog.Remarks == "This action is performed by system due to escalation of timeframe of user..!") // Esclation To LBCR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {

                    //List<ReceiverBackupViewModel> receiverBackups = new List<ReceiverBackupViewModel>();
                    //receiverBackups.Add(new ReceiverBackupViewModel()
                    //{
                    //    ReceiverRoleId = applicationActionLog.ReceiverRoleId,
                    //    SenderRoleId = applicationActionLog.SenderRoleId,
                    //    AppActionType = (AppActionTypeEnum)applicationActionLog.AppActionType,
                    //    Receiver_UserRefId = applicationActionLog.Receiver_UserRefId,
                    //    Receiver_ActionOn = applicationActionLog.ActionDate,
                    //    Sender_UserRefId = applicationActionLog.Sender_UserRefId,
                    //    Receiver_ProfileRefId = applicationActionLog.Receiver_ProfileRefId
                    //});

                    //var appActionExtLog = _context.ApplicationActionExtensionLogs.Where(x => x.CurrentAppActionLogRefId == applicationActionLog.ApplicationActionLogId).FirstOrDefault();
                    //if (appActionExtLog != null)
                    //{
                    //    int i = 1;
                    //    foreach (var item in typeof(ApplicationActionExtension).GetProperties().Where(x => x.Name.Contains("Receiver_UserRefId_")))
                    //    {
                    //        receiverBackups.Add(new ReceiverBackupViewModel()
                    //        {
                    //            ReceiverRoleId = Convert.ToString(appActionExtLog.GetType().GetProperty("ReceiverRoleId_" + i.ToString()).GetValue(appActionExtLog)),
                    //            SenderRoleId = Convert.ToString(appActionExtLog.GetType().GetProperty("SenderRoleId_" + i.ToString()).GetValue(appActionExtLog)),
                    //            AppActionType = (AppActionTypeEnum)appActionExtLog.GetType().GetProperty("AppActionType_" + i.ToString()).GetValue(appActionExtLog),
                    //            Receiver_UserRefId = Convert.ToString(appActionExtLog.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExtLog)),
                    //            Receiver_ActionOn = Convert.ToDateTime(appActionExtLog.GetType().GetProperty("Receiver_ActionOn_" + i.ToString()).GetValue(appActionExtLog)),
                    //            Sender_UserRefId = Convert.ToString(appActionExtLog.GetType().GetProperty("Sender_UserRefId_" + i.ToString()).GetValue(appActionExtLog)),
                    //            Receiver_ProfileRefId = Convert.ToInt64(appActionExtLog.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).GetValue(appActionExtLog))
                    //        });
                    //        i++;
                    //    }
                    //}
                    //receiverBackups = receiverBackups.Where(x => x.ReceiverRoleId != String.Empty).ToList();
                    //if (appActionExtLog != null)
                    //{
                    //    // ********** IN CASE PARELLEL OFFICER DON'T GET MENU OPTION -
                    //    //receiverBackups = receiverBackups.Where(x => x.Sender_UserRefId != appActionExtLog.CurrentActionTakenUserRefId && x.AppActionType != appActionExtLog.CurrentActionTakenCode).ToList(); // FOR PAST SEEDING
                    //    receiverBackups = receiverBackups.Where(x => x.Sender_UserRefId == appActionExtLog.CurrentActionTakenUserRefId && x.AppActionType == appActionExtLog.CurrentActionTakenCode).ToList(); // ORIGINAL
                    //}


                    //foreach (var item in receiverBackups)
                    //{
                    if (applicationActionLog.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0") // INDL
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FACTORY_APP_SUBMITTED_FEE_NOT_APPLICABLE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 72 - consumedHours;
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = true });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED_SCRUTINY_PHASE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT, MaxHoursAllowed = 72, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_BP)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_APPROVAL_LABOUR_DEPT, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 72 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.RAISE_OBJECTION_SENT_TO_JDM_SCRUTINY_PHASE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_SCRUTINY_COMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = true });
                            }
                        }

                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER_LABOUR_DEPT && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE || latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE || latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 72 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 60034, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 240, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_DRAFTSMAN, MaxHoursAllowed = 216, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 240 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);

                            if (applicationActionLog.ReceiverRoleId == "B32CC21B-06C8-4512-9976-A883FC8CF3A0") // SDO
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 96, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 151, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 96, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 151, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });

                                mutualProcessFlags.Add(new AppActionTime_MutualProcessFlag()
                                {
                                    AppActionLogRefId = applicationActionLog.ApplicationActionLogId,
                                    HasClosed = false,
                                    ReceiverProfileRefId = applicationActionLog.Receiver_ProfileRefId,
                                    ReceiverRoleId = applicationActionLog.ReceiverRoleId,
                                    ReceiverUserRefId = applicationActionLog.Receiver_UserRefId,
                                    SenderUserRefId = applicationActionLog.Sender_UserRefId,
                                    AppRefId = appRefId,
                                });
                            }
                            else if (applicationActionLog.ReceiverRoleId == "F807797B-3225-486D-B897-426053DBDF34") // EO
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 96, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 152, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 96, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 152, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                mutualProcessFlags.Add(new AppActionTime_MutualProcessFlag()
                                {
                                    AppActionLogRefId = applicationActionLog.ApplicationActionLogId,
                                    HasClosed = false,
                                    ReceiverProfileRefId = applicationActionLog.Receiver_ProfileRefId,
                                    ReceiverRoleId = applicationActionLog.ReceiverRoleId,
                                    ReceiverUserRefId = applicationActionLog.Receiver_UserRefId,
                                    SenderUserRefId = applicationActionLog.Sender_UserRefId,
                                    AppRefId = appRefId
                                });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_ATP)
                        {
                            if (applicationActionLog.ReceiverRoleId == "F40DA663-3447-43CF-B6BD-EF9D71FD29FF") // ATP
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 240 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_CGM, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_APPROVAL_LABOUR_DEPT)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 60034, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                    {
                        if ((applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_OBJECTION || applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_SCRUTINY_COMPLETE) && latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 72 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_DRAFTSMAN)
                        {
                            if (applicationActionLog.ReceiverRoleId == "E1E9E251-71DA-4D38-9CA4-77295F1F2195") // PSIEC.Draftsman
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.RAISE_OBJECTION_SENT_TO_JDM_SCRUTINY_PHASE)
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(applicationActionLog.ActionDate - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 72 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL || applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_OBJECTION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "81_1" });
                            }
                            else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                //IsAutoAction = true set false As per Mr. Aman Ambala Wale 
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = 240, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                            else if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                            {
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_DRAFTSMAN, MaxHoursAllowed = 216, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "E1E9E251-71DA-4D38-9CA4-77295F1F2195") // PSIEC.Draftsman
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_TO_NODAL_OFFICER)
                        {
                            if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }
                    else if (applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                    {
                        if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LBCR
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 216 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_AS_FACTORY_ACT_NOT_APPLICABLE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (applicationActionLog.ReceiverRoleId == "38D54781-39A2-4AF6-87EE-3102431EAB16") // JDRF_PSIEC
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 216 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }

                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION && applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM_PSIEC
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 240 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 60035, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }

                        else if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // JDM_PSIEC
                        {
                            latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                            int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                            var remainingHours = 240 - consumedHours;
                            List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                            remainingHours = remainingHours + (dates.Count() * 24);
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_IN_PSIEC, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LBCR
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.APPROVE_RECOMMENDATION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 60034, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.REJECT_RECOMMENDATION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.MARK_BACK)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REPLIED_TO_QUERY, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVED_AND_FORWARDED_FOR_FURTHER_ACTION, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 60034, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                        else if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_AS_FACTORY_ACT_NOT_APPLICABLE)
                        {
                            if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 216 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_AS_FACTORY_ACT_NOT_APPLICABLE, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "B32CC21B-06C8-4512-9976-A883FC8CF3A0") // SDO
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL || applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_OBJECTION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 240 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_ATP, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 153, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                await UpdateMutualFlags(appRefId, applicationActionLog.Receiver_UserRefId, applicationActionLog.Sender_UserRefId);

                            }
                        }
                    }
                    else if (applicationActionLog.SenderRoleId == "F807797B-3225-486D-B897-426053DBDF34") // EO
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_APPROVAL || applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARD_FOR_OBJECTION)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 240 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_ATP, MaxHoursAllowed = remainingHours, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, IsDocumentUploadOption = true, IsOptional = false, DocRefId = 153, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                await UpdateMutualFlags(appRefId, applicationActionLog.Receiver_UserRefId, applicationActionLog.Sender_UserRefId);

                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "F40DA663-3447-43CF-B6BD-EF9D71FD29FF") // JDRF_ATP
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_CGM)
                        {
                            if (applicationActionLog.ReceiverRoleId == "2FE87C8C-A2D6-4427-A176-836D2043BC28") // PSIEC.CGM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 240 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);
                                allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_JDM, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            }
                        }
                    }

                    else if (applicationActionLog.SenderRoleId == "2FE87C8C-A2D6-4427-A176-836D2043BC28") // JDRF_CGM
                    {
                        if (applicationActionLog.AppActionType == (int)AppActionTypeEnum.FORWARDED_FOR_FURTHER_ACTION_JDM)
                        {
                            if (applicationActionLog.ReceiverRoleId == "A9CE9875-D23D-4AA2-8271-6A8799EEF0E3") // PSIEC.JDM
                            {
                                latestProcessPhaseLog = _context.ApplicationProcessPhaseLogs.Where(x => x.AppRefId == appRefId && x.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE).OrderByDescending(x => x.Id).FirstOrDefault();
                                int consumedHours = (int)(Convert.ToDateTime(applicationActionLog.ActionDate) - latestProcessPhaseLog.LastModifiedOn).TotalHours;
                                var remainingHours = 240 - consumedHours;
                                List<DateTime> dates = await GetNonWorkingDaysBetweenDays(latestProcessPhaseLog.LastModifiedOn, applicationActionLog.ActionDate, appRefId);
                                remainingHours = remainingHours + (dates.Count() * 24);

                                var finalDeptAction = await _context.TimeLine_DepartmentWiseFinalActions.Where(x => x.AppRefId == appRefId).FirstOrDefaultAsync();
                                if (finalDeptAction != null)
                                {
                                    if (finalDeptAction.AppActionType == AppActionTypeEnum.FORWARD_FOR_OBJECTION)
                                    {
                                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                    }
                                    else if (finalDeptAction.AppActionType == AppActionTypeEnum.FEE_RAISED)
                                    {
                                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED_IN_PSIEC, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                    }
                                }
                                else
                                {
                                    allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FEE_RAISED, MaxHoursAllowed = remainingHours, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                                }
                            }
                        }
                    }
                }

                else if (application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR 
                    || application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                {
                    if (applicationActionLog.ReceiverRoleId == "e4df7019-17ca-4948-bb07-4bfeaea544c2") // DH- LABOUR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 24, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 24, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088") // ALC-LABOUR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 168; // 7 Days
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 72; // 3 Days
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0 && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "674EAAAB-1D01-4EA7-A18E-A64EB62AEEA4" && applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // DLC  receives from LC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "674EAAAB-1D01-4EA7-A18E-A64EB62AEEA4") // DLC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "B4615DF2-2D70-42EB-8970-015341775DA5") // DHL.Punjab
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "efc763ec-6e6d-476c-8763-d6f62fb89fgh") // adlc.punjab
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0 && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                {
                    if (applicationActionLog.ReceiverRoleId == "e4df7019-17ca-4948-bb07-4bfeaea544c2") // DH- LABOUR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 24, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 24, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088") // ALC-LABOUR
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 168; // 7 Days
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 72; // 3 Days
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0 && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE )
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "692e93a4-9351-4d8a-9ed3-ecf9c3574178") // DH- FACTORY
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION, MaxHoursAllowed = 24, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.FORWARD_FOR_APPROVAL, MaxHoursAllowed = 24, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionLog.ReceiverRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa") // ADRF/DDRF- FACTORY
                    {
                        if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_SCRUTINY_PHASE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.SCRUTINY_COMPLETED, MaxHoursAllowed = 48, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.DECLINED_DUE_TO_INCOMPLETE, MaxHoursAllowed = 48, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 168; // 7 Days
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            //allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = 168, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            //maxHoursAllowed = 72; // 3 Days
                        }

                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId == 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION, MaxHoursAllowed = 72, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0 && applicationActionLog.SenderRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9")
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.CLARIFICATION_PHASE && totalObjectionRaisedByRoleId > 0)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                        else if (latestProcessPhaseLog.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE)
                        {
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_OBJECTION_WITH_BALANCE_FEE, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                            allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_APPROVED, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        }
                    }
                    else if (applicationActionLog.ReceiverRoleId == "674EAAAB-1D01-4EA7-A18E-A64EB62AEEA4" && applicationActionLog.SenderRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // DLC  receives from LC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "674EAAAB-1D01-4EA7-A18E-A64EB62AEEA4") // DLC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LC - LABOUR
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }

                    else if (applicationActionLog.ReceiverRoleId == "51af8801-36d2-4a61-9a2d-85707534edd9") // ADDF - FACTORY
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.REJECT_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                    else if (applicationActionLog.ReceiverRoleId == "112cf6be-22b8-480d-af1a-1e1cb8f712b9") // LC - FACTORY
                    {
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APP_REJECT, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.MARK_BACK, MaxHoursAllowed = -1, IsAutoAction = false, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                        allowedAppActionTypes.Add(new AllowedActionWithTimeLimitViewModel() { AppActionType = AppActionTypeEnum.APPROVE_RECOMMENDATION, MaxHoursAllowed = -1, IsAutoAction = true, UserRefId = applicationActionLog.Receiver_UserRefId, PrecheckCode = "", IsExpiredByAutoProcess = false, IsHidden = false });
                    }
                }
                if (allowedAppActionTypes.Count() > 0)
                {
                    var holidayList = _context.Holidays.Where(x => x.HolidayDate.Year == applicationActionLog.ActionDate.Year).ToList();
                    List<WorkingHourSlabViewModel> workingHourSlabs = null; //CalcWorkingTimeSlabs(maxHoursAllowed, applicationActionLog.ActionDate, holidayList);

                    List<AppActionTimeLineDefination> appActionTimeLineDefinations = null;// new List<AppActionTimeLineDefination>();
                                                                                          //appActionTimeLineDefinations = JsonConvert.DeserializeObject<List<AppActionTimeLineDefination>>(JsonConvert.SerializeObject(workingHourSlabs));.

                    //appActionTimeLineDefinations = appActionTimeLineDefinations.Select(x => { x.AppRefId = appRefId; x.AppActionLogRefId = applicationActionLog.ApplicationActionLogId; return x; }).ToList();
                    //await _context.BulkInsertAsync<AppActionTimeLineDefination>(appActionTimeLineDefinations);

                    foreach (var item in allowedAppActionTypes)
                    {
                        if (item.MaxHoursAllowed > 0)
                        {
                            workingHourSlabs = await CalcWorkingTimeSlabs(item.MaxHoursAllowed, applicationActionLog.ActionDate, holidayList);
                            appActionTimeLineDefinations = new List<AppActionTimeLineDefination>();
                            appActionTimeLineDefinations = JsonConvert.DeserializeObject<List<AppActionTimeLineDefination>>(JsonConvert.SerializeObject(workingHourSlabs));
                            appActionTimeLineDefinations = appActionTimeLineDefinations.Select(x => { x.AppRefId = appRefId; x.AppActionLogRefId = applicationActionLog.ApplicationActionLogId; x.AllowedAppActionType = item.AppActionType; x.UserRefId = item.UserRefId; return x; }).ToList();
                            await _context.BulkInsertAsync<AppActionTimeLineDefination>(appActionTimeLineDefinations);
                        }
                        AppActionTimeLine appActionTimeLine = new AppActionTimeLine()
                        {
                            AppRefId = appRefId,
                            AppActionLogRefId = application.ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_PSIEC ? applicationActionLog.ApplicationActionLogId : 1,
                            AppActionParallelProcessRefId = application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC ? applicationActionLog.ApplicationActionLogId : 1,

                            AllowedAppActionType = item.AppActionType,
                            ActionCanTakenUpto = item.MaxHoursAllowed > 0 ? workingHourSlabs.LastOrDefault().WorkEndsTime : DateTime.MaxValue,
                            IsProcessed = false,
                            IsActionSuspended = false,
                            MaxHoursAllowed = item.MaxHoursAllowed,
                            IsAutoAction = item.IsAutoAction,
                            IsDocumentUploadOption = item.IsDocumentUploadOption,
                            DocRefId = item.DocRefId,
                            IsOptional = item.IsOptional,
                            UserRefId = item.UserRefId,
                            PrecheckCode = item.PrecheckCode,
                            IsExpiredByAutoProcess = item.IsExpiredByAutoProcess,
                            IsHidden = item.IsHidden
                        };
                        //if(item.AppActionType == AppActionTypeEnum.FORWARD_FOR_APPROVAL)
                        //{
                        //    appActionTimeLine.IsDocumentUploadOption = true;
                        //    appActionTimeLine.IsOptional = item.AppActionType == AppActionTypeEnum.FORWARD_FOR_APPROVAL ? true : false;
                        //    appActionTimeLine.DocRefId = 51;
                        //}

                        await _context.AppActionTimeLines.AddAsync(appActionTimeLine);
                        await _context.SaveChangesAsync();
                    }
                }

                if (mutualProcessFlags.Count() > 0)
                {
                    await _context.BulkInsertAsync<AppActionTime_MutualProcessFlag>(mutualProcessFlags);
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        // Code Commented on : 09-jan-2026 :- By Jashandeep singh
        //protected static List<WorkingHourSlabViewModel> CalcWorkingTimeSlabs(int maxHoursAllowed, DateTime submissionDateTime, List<Holiday> holidayList)
        //{
        //    submissionDateTime = new DateTime(submissionDateTime.Year, submissionDateTime.Month, submissionDateTime.Day, submissionDateTime.Hour, submissionDateTime.Minute, 0);
        //    DateTime startDateTime = submissionDateTime;
        //    DateTime nextActionDateTime;
        //    nextActionDateTime = submissionDateTime.AddHours(maxHoursAllowed);
        //    List<WorkingHourSlabViewModel> workingHourSlabs = new List<WorkingHourSlabViewModel>();

        //    if (startDateTime != nextActionDateTime)
        //    {
        //        bool isTimeReachedLimit = false;
        //        var actionWorkingDayMaxTime = submissionDateTime.TimeOfDay - (new TimeSpan(0, 0, 0));
        //        while (!isTimeReachedLimit)
        //        {
        //            if (startDateTime.DayOfWeek == DayOfWeek.Saturday) // is Sat
        //            {

        //                //var endDateTime = new DateTime(lastSlab.SlabDate.Year, lastSlab.SlabDate.Month, lastSlab.SlabDate.Day, timeSpan.Hours, timeSpan.Minutes, 0);

        //                workingHourSlabs.Add(new WorkingHourSlabViewModel
        //                {
        //                    SlabDate = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),// startDateTime,
        //                    SlabDayDesc = "Weekend (Saturday)",
        //                    SlabDayType = SlabDayTypeEnum.WEEKEND_SATURDAY,
        //                    TotalWorkingTime = 0,
        //                    WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),
        //                    WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0)
        //                });//
        //            }
        //            else if (startDateTime.DayOfWeek == DayOfWeek.Sunday) // is Sun
        //            {
        //                workingHourSlabs.Add(new WorkingHourSlabViewModel
        //                {
        //                    SlabDate = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),// startDateTime,
        //                    SlabDayDesc = "Weekend (Sunday)",
        //                    SlabDayType = SlabDayTypeEnum.WEEKEND_SUNDAY,
        //                    TotalWorkingTime = 0,
        //                    WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),
        //                    WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0)
        //                });
        //            }
        //            else if (holidayList.Any(x => x.HolidayDate.ToShortDateString() == startDateTime.ToShortDateString())) // is Holiday
        //            {
        //                workingHourSlabs.Add(new WorkingHourSlabViewModel
        //                {
        //                    SlabDate = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),// startDateTime,
        //                    SlabDayDesc = holidayList.Where(x => x.HolidayDate.ToShortDateString() == startDateTime.ToShortDateString()).Select(x => x.HolidayName).FirstOrDefault(),
        //                    SlabDayType = SlabDayTypeEnum.HOLIDAY,
        //                    TotalWorkingTime = 0,
        //                    WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0),
        //                    WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0)
        //                });
        //            }
        //            else
        //            {
        //                WorkingHourSlabViewModel workingHourSlab = new WorkingHourSlabViewModel
        //                {
        //                    SlabDate = startDateTime,
        //                    SlabDayDesc = "Working Day",
        //                    SlabDayType = SlabDayTypeEnum.WORKING,
        //                };

        //                if (startDateTime == submissionDateTime)
        //                {
        //                    workingHourSlab.TotalWorkingTime = ((new TimeSpan(24, 0, 0)) - startDateTime.TimeOfDay).TotalHours;
        //                    workingHourSlab.WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, startDateTime.Hour, startDateTime.Minute, 0);
        //                    workingHourSlab.WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);

        //                }
        //                //else if ((calcTotalWorkingHours(workingHourSlabs) + actionWorkingDayMaxTime.TotalHours) >= maxHoursAllowed)
        //                //{
        //                //    workingHourSlab.TotalWorkingTime = maxHoursAllowed - calcTotalWorkingHours(workingHourSlabs);
        //                //    workingHourSlab.WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
        //                //    workingHourSlab.WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, actionWorkingDayMaxTime.Hours, actionWorkingDayMaxTime.Minutes, 0);
        //                //}
        //                else if ((calcTotalWorkingHours(workingHourSlabs)) + 24 >= maxHoursAllowed)
        //                {
        //                    var remainingHrs = 24 - calcTotalWorkingHours(workingHourSlabs);
        //                    remainingHrs = remainingHrs - 0.01;

        //                    if (remainingHrs < 0)
        //                    {
        //                        remainingHrs = remainingHrs * (-1);
        //                    }
        //                    workingHourSlab.TotalWorkingTime = maxHoursAllowed - calcTotalWorkingHours(workingHourSlabs);
        //                    workingHourSlab.WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
        //                    workingHourSlab.WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, TimeSpan.FromHours(remainingHrs).Hours, TimeSpan.FromHours(remainingHrs).Minutes, 0);
        //                }
        //                else
        //                {
        //                    workingHourSlab.TotalWorkingTime = 24;
        //                    workingHourSlab.WorkStartsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
        //                    workingHourSlab.WorkEndsTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
        //                }
        //                workingHourSlabs.Add(workingHourSlab);
        //            }
        //            if (workingHourSlabs.Where(x => x.SlabDayType == SlabDayTypeEnum.WORKING).Sum(x => x.TotalWorkingTime) < maxHoursAllowed)
        //            {
        //                startDateTime = startDateTime.AddDays(1);
        //                startDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
        //            }
        //            else
        //            {
        //                isTimeReachedLimit = true;
        //            }

        //            //if ((calcTotalWorkingHours(workingHourSlabs)) >= maxHoursAllowed)
        //            //{
        //            //    WorkingHourSlabViewModel lastSlab = workingHourSlabs.LastOrDefault();
        //            //    TimeSpan timeSpan = TimeSpan.FromHours(lastSlab.TotalWorkingTime);
        //            //    //timeSpan.Add(TimeSpan.FromMinutes(1));
        //            //    var endDateTime  = new DateTime(lastSlab.SlabDate.Year, lastSlab.SlabDate.Month, lastSlab.SlabDate.Day, timeSpan.Hours, timeSpan.Minutes, 0);
        //            //    lastSlab.SlabDate = endDateTime.AddMinutes(1);
        //            //    lastSlab.WorkEndsTime = endDateTime.AddHours(calcTotalWorkingHours(workingHourSlabs));//.AddMinutes(1);
        //            //    lastSlab.TotalWorkingTime = (lastSlab.WorkEndsTime- lastSlab.WorkStartsTime).TotalHours;
        //            //    workingHourSlabs[workingHourSlabs.Count() - 1] = lastSlab;
        //            //    isTimeReachedLimit = true;
        //            //}
        //        }
        //    }
        //    else
        //    {

        //    }
        //    return workingHourSlabs;
        //}

        protected async Task<List<WorkingHourSlabViewModel>> CalcWorkingTimeSlabs(int maxHoursAllowed, DateTime submissionDateTime, List<Holiday> holidayList)
        {
            List<WorkingHourSlabViewModel> workingHourSlabs = new List<WorkingHourSlabViewModel>();
            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
            {
                 new StoreProcedureParm() { ParmName = "MaxHoursAllowed", ParmValue = maxHoursAllowed.ToString(), isNumber = true },
                 new StoreProcedureParm() { ParmName = "SubmissionDateTime", ParmValue = submissionDateTime.ToString("yyyy-MM-dd HH:mm:ss"), isNumber = false }
            };

            workingHourSlabs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WorkingHourSlabViewModel>("sp_GetActionTakenUptoDateTime", storeProcedureParms);
            return workingHourSlabs;
        }

        protected static double calcTotalWorkingHours(List<WorkingHourSlabViewModel> workingHourSlabs)
        {
            return workingHourSlabs.Sum(x => x.TotalWorkingTime);
        }

        public async Task<bool> ProcessTimeLineWise(ElapsedApplicationViewModel elapsedApplication)
        {
            bool isNeedToDeemed = false;
            ApplicationProcessPhaseLogTypeEnum appProcessPhaseLogType = elapsedApplication.ApplicationProcessPhaseLogType;
            ApplicationActionViewModel applicationAction = null;

            //*** W H A T   T O   D O

            if (elapsedApplication.ApplicationType == (int)ApplicationTypeEnum.FACTORY_LICENCE)
            {
                if (elapsedApplication.ApplicationProcessPhaseLogType == ApplicationProcessPhaseLogTypeEnum.SCRUTINY_PHASE && elapsedApplication.ReceiverRoleId == "692e93a4-9351-4d8a-9ed3-ecf9c3574178") // (Dhf)
                {
                    applicationAction = new ApplicationActionViewModel()
                    {
                        //AppActionType = (int)AppActionTypeEnum.DEEMED_COMPLETED,
                        //AppDocumentRefId = 0,
                        //AppRefId = elapsedApplication.AppRefId,
                        //CheckListFormJson = null,
                        //IsDocumentUploaded = false,
                        //PdfNameGUID = certResp.PdfNameGUID,

                        //PublicAppRefNum = item.Application.PublicAppRefNum,
                        //Receiver_ProfileRefId = 0,
                        //Receiver_UserRefId = item.Application.ProjectSites.UserRefId,

                        Remarks = "Application approved as deemed..",
                        UserId = "C79F57CB-40BA-48EA-8336-327DC1B78701"
                    };
                }
            }

            //*** R E C O R D   A P P L I C A T I O N   A C T I O N
            if (applicationAction != null)
            {
                //var resp = await _iApplicationMamnagementService.RecordApplicationAction(applicationAction, applicationAction.UserId);
            }



            //*** S E T   I S P R O C E S S E D   T R U E 




            //*** S E T   P H A S E

            if (appProcessPhaseLogType != elapsedApplication.ApplicationProcessPhaseLogType)
            {

            }


            //*** G E N E R A T E   C E R T I F I C A T E  (I F  D E E M E D)
            if (isNeedToDeemed)
            {

            }




            //*** S H A R E   S T A T U S
            return true;
        }

        public async Task<bool> UpdateAppProcessedTypePhaseCounter(Int64 id)
        {
            var processPhaseLog = await _context.ApplicationProcessPhaseLogs.Where(x => x.Id == id).FirstOrDefaultAsync();
            processPhaseLog.PhaseCounter = processPhaseLog.PhaseCounter + 1;
            _context.Update<ApplicationProcessPhaseLog>(processPhaseLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMutualFlags(Int64 appRefId, string receiverUserRefId, string senderUserRefId)
        {
            var openMutualFlags = _context.AppActionTime_MutualProcessFlags.Where(x => x.AppRefId == appRefId && x.SenderUserRefId == receiverUserRefId && x.ReceiverUserRefId == senderUserRefId).ToList();
            if (openMutualFlags.Count() > 0)
            {
                openMutualFlags = openMutualFlags.Select(x => { x.HasClosed = true; return x; }).ToList();
                _context.BulkUpdate<AppActionTime_MutualProcessFlag>(openMutualFlags);
            }
            return true;
        }

        public async Task<bool> AddPhaseAndObjectionLog(ApplicationProcessPhaseAndObjectionLog appProcessPhaseAndObjectionLog)
        {
            await _context.AddAsync<ApplicationProcessPhaseAndObjectionLog>(appProcessPhaseAndObjectionLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAppProcessLogType(ApplicationProcessPhaseLog appProcessPhaseLogs)
        {
            await _context.ApplicationProcessPhaseLogs.AddAsync(appProcessPhaseLogs);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DateTime>> GetNonWorkingDaysBetweenDays(DateTime fromDate, DateTime toDate, Int64 appRefId)
        {
            List<DateTime> dates = new List<DateTime>();
            dates = _context.AppActionTimeLineDefinations.Where(x => x.AppRefId == appRefId && (x.SlabDate.Date >= fromDate.Date && x.SlabDate.Date <= toDate.Date) && (x.SlabDayType == SlabDayTypeEnum.WEEKEND_SATURDAY || x.SlabDayType == SlabDayTypeEnum.WEEKEND_SUNDAY || x.SlabDayType == SlabDayTypeEnum.HOLIDAY)).Select(x=> x.SlabDate).ToList();
            return dates.Distinct().ToList();
        }
    }
}