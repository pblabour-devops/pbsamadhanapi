using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Services
{
    public class Hangfire_AutoAppProcessManagerService //: IHangfire_AutoAppProcessManagerService
    {
        private IServiceScopeFactory _iServiceScopeFactory;
        public static List<ApplicationToBeElapsedViewModel> elapsedApplications = new List<ApplicationToBeElapsedViewModel>();
        public Hangfire_AutoAppProcessManagerService(IServiceScopeFactory iServiceScopeFactory

            )
        {
            _iServiceScopeFactory = iServiceScopeFactory;

        }
        public Hangfire_AutoAppProcessManagerService() { }
        public async Task ExecuteElapsedAction()
         {
           
            if (elapsedApplications.Count == 0)
                {
                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        try
                        {
                            var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                            //var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, "f1ba27b3-27ca-46a6-8bad-609951bf17c4");
                            elapsedApplications = await _iGenericRepo.CallStoreProcedureReaderAsyncWithoutParams<ApplicationToBeElapsedViewModel>("dbo.sp_TimeLine_GetLapsedApplications");
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();

                        }

                        //do your stuff....
                    }

                }
                else
                {
                    try
                    {
                        ApplicationToBeElapsedViewModel elapsedApp = elapsedApplications[0];

                        List<AllowedReceiverDetailsViewModel> allowedReceiverDetails = new List<AllowedReceiverDetailsViewModel>();
                        System.Diagnostics.Debug.WriteLine("Processing AppId : " + elapsedApp.AppRefId.ToString());
                            
                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                            var storeProcedureParms = new List<StoreProcedureParm>()
                            {
                            new StoreProcedureParm (){ ParmName="ActionCode", ParmValue=elapsedApp.AllowedAppActionType.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="UserId", ParmValue=elapsedApp.UserRefId.ToString(), isNumber=false},
                            new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=elapsedApp.AppRefId.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=EnumOps.GetEnumValue<ApplicationTypeEnum>(elapsedApp.ApplicationType.ToString()), isNumber=true},
                            new StoreProcedureParm (){ ParmName="DefaultRoleName", ParmValue=elapsedApp.DefaultRoleName.ToString(), isNumber=false},
                            };
                            allowedReceiverDetails = await _iGenericRepo.CallStoreProcedureReaderAsync<AllowedReceiverDetailsViewModel>("sp_TimeLine_GetNextUserByAction", storeProcedureParms);
                        }
                    
                    
                        ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                        {
                            AppActionType = elapsedApp.AllowedAppActionType,
                            AppDocumentRefId = 0,
                            AppRefId = elapsedApp.AppRefId,
                            CheckListFormJson = null,
                            IsDocumentUploaded = false,
                            PdfNameGUID = "NA",

                            PublicAppRefNum = "NA",
                            Receiver_ProfileRefId = 0,
                            Receiver_UserRefId = allowedReceiverDetails.FirstOrDefault().UserId,

                            Remarks = "This action is performed by system due to escalation of timeframe of user..!",
                            UserId = elapsedApp.UserRefId,
                            ActionTakenModeType = ActionTakenModeTypeEnum.SYSTEM
                        };

                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _iAppMgmtSer = scope.ServiceProvider.GetRequiredService<IApplicationManagementService<ApplicationAction>>();
                            //var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, elapsedApp.UserRefId);

                           

                        }
                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            if (elapsedApp.TimelineId > 0)
                            {
                                var actionTimeLine = _context.AppActionTimeLines.Where(x => x.Id == elapsedApp.TimelineId).FirstOrDefault();
                                actionTimeLine.IsProcessed = true;
                                _context.Update<AppActionTimeLine>(actionTimeLine);
                                await _context.SaveChangesAsync();
                            }

                        }

                    elapsedApplications.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {

                        string error = ex.ToString(); ;
                    }

                }
                System.Diagnostics.Debug.WriteLine("Hello ******************");
        }
    }
}
