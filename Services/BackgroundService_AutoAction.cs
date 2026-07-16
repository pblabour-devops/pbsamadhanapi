using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class BackgroundService_AutoAction : BackgroundService
    {
        private readonly ILogger<BackgroundService_AutoAction> _logger;
        private Timer? _timer;
        private Timer? _timer2;

        private IServiceScopeFactory _iServiceScopeFactory;
        public static List<ApplicationToBeElapsedViewModel> elapsedApplications = new List<ApplicationToBeElapsedViewModel>();
        public static List<ElapsedPhaseApplicationViewModel> elapsedPhaseApplications = new List<ElapsedPhaseApplicationViewModel>();
        public static List<ApplicationToBeElapsedViewModel> elapsedFeeRaisedToBeApplications = new List<ApplicationToBeElapsedViewModel>();



        public BackgroundService_AutoAction(ILogger<BackgroundService_AutoAction> logger, IServiceScopeFactory iServiceScopeFactory)
        {
            _iServiceScopeFactory = iServiceScopeFactory;
            _logger = logger;
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Create a timer that triggers every minute
            if (!Debugger.IsAttached)
            {
                _logger.LogInformation("MinuteSchedulerService is starting.");
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
                //_timer2 = new Timer(DoWorkFeeRaised, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }
            return Task.CompletedTask;
        }
        private async void ProcessElapsedApplications()
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

                        var elapsedApplications2 = await _iGenericRepo.CallStoreProcedureReaderAsyncWithoutParams<ApplicationToBeElapsedViewModel>("dbo.sp_TimeLine_GetLapsedApplications_ParallelProcess");

                        elapsedApplications.AddRange(elapsedApplications2);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        string path2 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception1" + ".txt";
                        if (!File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
                        else if (File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
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
                        if (elapsedApp.ApplicationType == 81)
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            var roleInfo = _context.Roles.Include(r => r.UserRoles).FirstOrDefault(r => r.UserRoles.Any(ur => ur.UserId == elapsedApp.UserRefId));
                            var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                            var storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="ActionCode", ParmValue=elapsedApp.AllowedAppActionType.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=elapsedApp.AppRefId.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=EnumOps.GetEnumValue<ApplicationTypeEnum>(elapsedApp.ApplicationType.ToString()), isNumber=true},
                                new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleInfo.Name, isNumber=false},
                            };
                            allowedReceiverDetails = await _iGenericRepo.CallStoreProcedureReaderAsync<AllowedReceiverDetailsViewModel>("sp_TimeLine_GetNextUserByAction_BuildingPlan_PSIEC", storeProcedureParms);
                        }
                        else
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

                    }


                    ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                    {
                        AppActionType = elapsedApp.AllowedAppActionType,
                        AppDocumentRefId = 0,
                        AppRefId = elapsedApp.AppRefId,
                        CheckListFormJson = null,
                        IsDocumentUploaded = false,
                        PdfNameGUID = "NA",
                        ApplicationActionLogId = elapsedApp.ApplicationActionLogId,
                        PreviousActionType = (AppActionTypeEnum)elapsedApp.AppActionType,
                        PublicAppRefNum = "NA",
                        Receiver_ProfileRefId = 0,
                        Receiver_UserRefId = allowedReceiverDetails.FirstOrDefault().UserId,
                        IpAddress = "Manual",
                        Latitude = "Manual",
                        Longitude = "Manual",

                        //Remarks = "This action is performed by system due to escalation of timeframe of user..!",
                        Remarks = elapsedApp.ACtionCanTakenUpto > DateTime.Now ? "This action is performed by system due to fee not raised in the given time period..!" : "The file has been automatically escalated by the system due to the expiry of the user’s allowed timeframe..!",
                        UserId = elapsedApp.UserRefId,
                        ActionTakenModeType = ActionTakenModeTypeEnum.SYSTEM
                    };

                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        var _iAppMgmtSer = scope.ServiceProvider.GetRequiredService<IApplicationManagementService<ApplicationAction>>();
                        var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, elapsedApp.UserRefId);

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
                    elapsedApplications.RemoveAt(0);
                    string error = ex.ToString();
                    string path3 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception2" + ".txt";
                    if (!File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                    else if (File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                }

            }
        }

        private async void ProcessTimelinePhaseApplications()
        {
            if (elapsedPhaseApplications.Count == 0)
            {
                using (var scope = _iServiceScopeFactory.CreateScope())
                {
                    try
                    {
                        var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                        //var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, "f1ba27b3-27ca-46a6-8bad-609951bf17c4");
                        elapsedPhaseApplications = await _iGenericRepo.CallStoreProcedureReaderAsyncWithoutParams<ElapsedPhaseApplicationViewModel>("dbo.sp_Timeline_GetPhaseLapsedApplications");

                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        string path2 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception1" + ".txt";
                        if (!File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
                        else if (File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
                    }

                    //do your stuff....
                }

            }
            else
            {
                try
                {
                    ElapsedPhaseApplicationViewModel elapsedPhaseApp = elapsedPhaseApplications[0];
                    System.Diagnostics.Debug.WriteLine("Processing AppId : " + elapsedPhaseApp.AppRefId.ToString());

                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        
                        ApplicationProcessPhaseLog phaseLog = new ApplicationProcessPhaseLog()
                        {
                            ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.DEEMED_PHASE,
                            AppRefId = elapsedPhaseApp.AppRefId,
                            PhaseCounter = 1,
                            LastModifiedOn = DateTime.Now
                        };
                        await _context.AddAsync(phaseLog);
                        await _context.SaveChangesAsync();

                        if (elapsedPhaseApp.AppTimelineId > 0)
                    {
                        var actionTimeLine = _context.AppActionTimeLines.Where(x => x.Id == elapsedPhaseApp.AppTimelineId).FirstOrDefault();
                        actionTimeLine.IsExpiredByAutoProcess = true;
                        _context.Update<AppActionTimeLine>(actionTimeLine);
                        await _context.SaveChangesAsync();
                    }

                        
                    }


                    elapsedPhaseApplications.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    elapsedPhaseApplications.RemoveAt(0);
                    string error = ex.ToString();
                    string path3 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception2" + ".txt";
                    if (!File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                    else if (File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                }

            }
        }

        private async void ProcessElapsedFeeRaisedToBeApplications()
        {
            if (elapsedFeeRaisedToBeApplications.Count == 0)
            {
                using (var scope = _iServiceScopeFactory.CreateScope())
                {
                    try
                    {
                        var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                        //var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, "f1ba27b3-27ca-46a6-8bad-609951bf17c4");
                        elapsedFeeRaisedToBeApplications = await _iGenericRepo.CallStoreProcedureReaderAsyncWithoutParams<ApplicationToBeElapsedViewModel>("dbo.sp_GetFeeRaisedToBeLapsedApplications");

                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        string path2 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception1" + ".txt";
                        if (!File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
                        else if (File.Exists(path2))
                        {
                            using (var txtFile = File.AppendText(path2))
                            {
                                txtFile.WriteLine($"{ex.InnerException} : {DateTime.Now}");
                            }
                        }
                    }

                    //do your stuff....
                }

            }
            else
            {
                try
                {
                    ApplicationToBeElapsedViewModel elapsedApp = elapsedFeeRaisedToBeApplications[0];

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
                        ApplicationActionLogId = elapsedApp.ApplicationActionLogId,
                        PreviousActionType = (AppActionTypeEnum)elapsedApp.AppActionType,
                        PublicAppRefNum = "NA",
                        Receiver_ProfileRefId = 0,
                        Receiver_UserRefId = allowedReceiverDetails.FirstOrDefault().UserId,
                        IpAddress = "Manual",
                        Latitude = "Manual",
                        Longitude = "Manual",

                        Remarks = "This action is performed by system due to escalation of timeframe of user..!",
                        UserId = elapsedApp.UserRefId,
                        ActionTakenModeType = ActionTakenModeTypeEnum.SYSTEM
                    };

                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        var _iAppMgmtSer = scope.ServiceProvider.GetRequiredService<IApplicationManagementService<ApplicationAction>>();
                        var resp = await _iAppMgmtSer.RecordApplicationAction(applicationAction, elapsedApp.UserRefId);

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
                    elapsedFeeRaisedToBeApplications.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    elapsedFeeRaisedToBeApplications.RemoveAt(0);
                    string error = ex.ToString();
                    string path3 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "exception2" + ".txt";
                    if (!File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                    else if (File.Exists(path3))
                    {
                        using (var txtFile = File.AppendText(path3))
                        {
                            txtFile.WriteLine($"{ex} : {DateTime.Now}");
                        }
                    }
                }

            }
        }
        private async void DoWork(object? state)
        {
            _logger.LogInformation($"MinuteSchedulerService is doing work at: {DateTime.Now}");

            string path = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "run" + ".txt";
            if (!File.Exists(path))
            {
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine($"Code Runs : {DateTime.Now}");
                }
            }
            else if (File.Exists(path))
            {
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine("The next line!");
                }
            }
             ProcessElapsedApplications();
             ProcessTimelinePhaseApplications();

            Console.WriteLine($"BackSertvice : {DateTime.Now}");

           
            System.Diagnostics.Debug.WriteLine("Hello ******************");
        }

        private async void DoWorkFeeRaised(object? state)
        {
            _logger.LogInformation($"MinuteSchedulerService is doing work at: {DateTime.Now}");

            string path = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BackgroundServiceLogs")).Root + "run" + ".txt";
            if (!File.Exists(path))
            {
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine($"Code Runs : {DateTime.Now}");
                }
            }
            else if (File.Exists(path))
            {
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine("The next line!");
                }
            }
            ProcessElapsedFeeRaisedToBeApplications();
            Console.WriteLine($"BackSertvice : {DateTime.Now}");


            System.Diagnostics.Debug.WriteLine("Hello ******************");
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MinuteSchedulerService is stopping.");

            _timer?.Change(Timeout.Infinite, 0); // Stop the timer
            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}

