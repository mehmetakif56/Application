using AutoMapper;
using DBE.ENERGY.Infrastructure.Data;
using DBE.ENERGY.Web.Extensions;
using DBE.ENERGY.Web.Helper;
using DBE.ENERGY.Web.Localization;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DBE.ENERGY.Web
{
    public class Startup
    {
        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
                                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                        });
        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();
            services.AddMiniProfiler().AddEntityFramework();
            services.AddDbContext<DBEEnergyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                            .UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();

                

            });
            //services.AddDbContext<IntegrationContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("IntegrationConnection"))
            //                .UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();

            //});
            //services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            //services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            //services.AddSingleton<IMongoDBContext, MongoDBContext>();

            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            services.AddDependencies();
            services.AddAuth();
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddMemoryCache();
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile(provider.GetService<GenericSharedResourceService>(),
                    HostingEnvironment));
            }).CreateMapper());
            #region Hangfire
            services.AddHangfire(conf => conf
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseActivator(new AspNetCoreJobActivator(services.BuildServiceProvider().GetService<IServiceScopeFactory>()))
            .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions()
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(10),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true
            }));

            services.AddHangfireServer();
            #endregion
            services.AddEnhancedMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //if (!env.IsProd())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //if (env.IsProd())
            //{
            //    // Only Production config
            //}

            //if (env.IsDevelopment())
            //{
            //    //app.UseAddFakeUserMail(configuration.GetValue<string>("EMail"));
            //    //app.UseMiniProfiler();
            //}

            //app.UseExceptionHandler("/error");

            //if (env.IsProd() || env.IsTest() || env.IsDev)
            //{
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<DBEEnergyContext>();
                    context.Database.Migrate();
                }
            //}

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            //app.UseSetUserSession();

            app.UseRouting();
            app.UseAuthorization();

            ///Hangfire kaldýrýldý,Ayrý bir proje haline getirelecek.
            //app.UseHangfireDashboard(options: new DashboardOptions { Authorization = new[] { new HangfireDashboardAuthFilter() } });
            //HangfireJobSchedular.ScheduleRecurringJobs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

            });

            // Set Asponse Licence
            //var licence = configuration.GetSection("Licences").GetValue<string>("Aspose");
            //new Aspose.Pdf.License().SetLicense(new MemoryStream(Convert.FromBase64String(licence)));
            //new Aspose.Words.License().SetLicense(new MemoryStream(Convert.FromBase64String(licence)));
            //new Aspose.Html.License().SetLicense(new MemoryStream(Convert.FromBase64String(licence)));


        }
    }
}
