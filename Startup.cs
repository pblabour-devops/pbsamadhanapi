using Hangfire.Common;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using pbsamadhannetcoreapi.CommonUtiliteis.Hubs;
using pbsamadhannetcoreapi.Middlewares;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.CustomRepositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using System.Threading;
using pbsamadhannetcoreapi.CustomFillters;
using pbsamadhannetcoreapi.CommonUtiliteis.RSA;

namespace pbsamadhannetcoreapi
{
    public class Startup
    {
        readonly string CorsPolicy = "_corsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CustomAppConfigsModel>(Configuration.GetSection("CustomAppConfigs"));
            services.AddCors(c =>
            {
                c.AddPolicy(CorsPolicy, options =>
                {
                    options.WithOrigins(
                        "http://localhost:4200",  // the Angular app url
                        "http://103.118.160.157",
                        "https://103.118.160.157",
                        "https://esewadev.punjab.gov.in",
                        "https://esewatst.punjab.gov.in",
                        "https://esewastg.punjab.gov.in",
                        "https://esewauat.punjab.gov.in",
                        "https://esewa.punjab.gov.in",
                        "https://bocw.punjab.gov.in",
                        "http://4.187.138.206"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            //services.AddCors(c =>
            //{
            //    c.AddPolicy(CorsPolicy, options => 
            //    {
            //        options.WithOrigins("http://localhost:4200") // the Angular app url
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials();
            //    });
            //});

            services.AddIdentity<User, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<AppDbContext>()
                            //.AddCustomStores()
                            .AddDefaultTokenProviders();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration.GetSection("CustomAppConfigs").GetSection("JwtConfigs").GetSection("Issuer").Value,
                    ValidAudience = Configuration.GetSection("CustomAppConfigs").GetSection("JwtConfigs").GetSection("Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("CustomAppConfigs").GetSection("JwtConfigs").GetSection("SecurityKey").Value))
                };
            });
            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetSection("ConnectionStrings").GetSection("SQLServerConnection").Value));
            //services.AddHangfireServer();

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("SQLServerConnection"), sqlOptions => sqlOptions.CommandTimeout(90))
            );

            #region Service Classes DI
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IEstablishmentService, EstablishmentService>();
            services.AddScoped(typeof(IApplicationManagementService<>), typeof(ApplicationManagementService<>));
            //services.AddScoped<IBusinessEntityService, BusinessEntityService>();
            services.AddScoped<IProjectSiteService, ProjectSiteService>();
            services.AddScoped<IContractLabourService, ContractLabourService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDigitalSignatureService, DigitalSignatureService>();
            services.AddScoped<ICommonApisService, CommonApisService>();
            services.AddScoped<IBuildingPlanService, BuildingPlanService>();
            services.AddScoped<IPaymentManagerService, PaymentManagerService>();
            services.AddScoped<IUserMobileAppDeviceManagerService, UserMobileAppDeviceManagerService>();
            services.AddScoped<ICircleManager, CircleManagerService>();
            services.AddScoped<INotificationManagerService, NotificationManagerService>();
            services.AddScoped<IPdfOprationsService, PdfOprationsService>();
            services.AddScoped<ICommonLicenceService, CommonLicenceService>();
            services.AddScoped<IBuildingPlanHUDService, BuildingPlanHUDService>();
            services.AddScoped<IThirdPartyInegrationsService, ThirdPartyInegrationsService>();
            services.AddScoped<IInvestPunjabShareStatusService, InvestPunjabShareStatusService>();
            services.AddScoped<IShopLicenceService, ShopLicenceService>();
            services.AddScoped<ISystem_O_CommunicationService, System_O_CommunicationService>();
            services.AddScoped<IBuildingPlanFactoryService, BuildingPlanFactoryService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDeemedProcessService, DeemedProcessService>();
            services.AddScoped<IFactoryLicenceService, FactoryLicenceService>();
            services.AddScoped<ILicence_Shop_NightShiftService, Licence_Shop_NightShiftService>();
            services.AddScoped<ILicence_ContractLabourService, Licence_ContractLabourService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IInspectionService, InspectionService>();
            services.AddScoped<ILicence_Factory_NightShiftService, Licence_Factory_NightShiftService>();
            services.AddScoped<IMPRService, MPRService>();
            services.AddScoped<IBocwLicenceService, BocwLicenceService>();
            services.AddScoped<IAutoApproveProcessService, AutoApproveProcessService>();
            services.AddScoped<IMiscellaneousService, MiscellaneousService>();
            services.AddScoped<ILWBCommunicationService, LWBCommunicationService>();
            services.AddScoped<ILicence_MotorTransportService, Licence_MotorTransportService>();
            services.AddScoped<ILicence_BuildingPlanService, Licence_BuildingPlanService>();
            services.AddScoped<IToDoManagerService, ToDoManagerService>();
            services.AddScoped<ICrudService, CrudService>();
            services.AddScoped<ILicence_CL_PEService, Licence_CL_PEService>();
            services.AddScoped<ILicence_TradeUnionService, Licence_TradeUnionService>();
            services.AddScoped<ILicence_PE_ISMService, Licence_PE_ISMService>();
            services.AddScoped<IAppTimeLineManagerService, AppTimeLineManagerService>();
            services.AddScoped<ILicene_BuildingPlan_PSIECService, Licene_BuildingPlan_PSIECService>();
            //services.AddTransient<IHangfire_AutoAppProcessManagerService, Hangfire_AutoAppProcessManagerService>();
            services.AddScoped<ILicence_ISM_ContractLabourService, Licence_ISM_ContractLabourService>();
            services.AddScoped<IGolferRegistrationService, GolferRegistrationService>();
            //services.AddHostedService<BackgroundService_AutoAction>();
            services.AddScoped<IAnnualReturnService, AnnualReturnService>();
            services.AddScoped<IComplaintService, ComplaintService>();


            #endregion

            #region Repository Classes DI
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IZoneRepository), typeof(ZoneRepository));
            services.AddScoped(typeof(IEstablishmentRepository), typeof(EstablishmentRepository));
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddScoped(typeof(IApplicationManagementRepository<>), typeof(ApplicationManagementRepository<>));
            services.AddScoped(typeof(IGeneric_SP_Repository), typeof(Generic_SP_Repository));
            services.AddScoped(typeof(IProjectSiteRepository), typeof(ProjectSiteRepository));
            services.AddScoped(typeof(IPaymentManagerRepository), typeof(PaymentManagerRepository));
            services.AddScoped(typeof(IBuildingPlanHUDRepository), typeof(BuildingPlanHUDRepository));
            services.AddScoped(typeof(IBuildingPlanFactoryRepository), typeof(BuildingPlanFactoryRepository));
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<ApplicationManagementService<Licence_Existing_BuildingPlan_GeneralDetail>>();
            services.AddScoped<ApplicationManagementService<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>>();
            #endregion
            


            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(CorsPolicy);
            app.UseRouting();
            app.UseAuthentication();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            // Remove Headers for security audit
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("Vary");
                context.Response.Headers.Remove("Access-Control-Allow-Origin");
                context.Response.Headers.Remove("Access-Control-Allow-Headers");
                context.Response.Headers.Remove("Access-Control-Allow-Methods");
                context.Response.Headers.Remove("Access-Control-Allow-Credentials");
                context.Response.Headers.Remove("X-Content-Type-Options");
                context.Response.Headers.Remove("X-Frame-Options");
                context.Response.Headers.Remove("X-AspNet-Version");
                context.Response.Headers.Remove("Access-Control-Allow-Credentials");
                var customHeaders = Configuration.GetSection("CustomHeaders");
                foreach (var header in customHeaders.GetChildren())
                {
                    context.Response.Headers[header.Key] = header.Value;
                }
                await next();
            });
            app.UseMiddleware<GlobalExceptionManager>();
            app.UseAuthorization();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"TempFiles")),
            //    RequestPath = new PathString("/TempFiles")
            //});
            app.UseHttpsRedirection();
            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHubs>("/signalrhub");
            });

        }
    }
}
