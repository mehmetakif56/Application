using DBE.ENERGY.Core.Interfaces;
using DBE.ENERGY.Core.Services;
using DBE.ENERGY.Infrastructure.Data;
using DBE.ENERGY.Web.Helper;
using DBE.ENERGY.Web.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace DBE.ENERGY.Web
{
    public static class StartupDependencies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(IntegrationRepository<>));
            services.AddTransient<IHistoricalValueService, HistoricalValueService>();
            services.AddScoped<IFacilityFormService, FacilityFormService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISessionHelper, SessionHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddTransient<IViewLocalizer, OwnViewLocalizer>();
            services.AddSingleton(typeof(GenericSharedResourceService));
            services.AddSingleton(typeof(GenericSharedResourceService<>));
            services.AddScoped<CurrencyService, CurrencyService>();
            services.AddScoped<IIntegrationService, IntegrationService>();
            services.AddScoped<ISettingsParameterService, SettingsParameterService>();
            services.AddScoped<ICashFlowService, CashFlowService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAdviceService, AdviceService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFacilityDocumentService, FacilityDocumentService>();
            services.AddScoped<ITaskVerifyService, TaskVerifyService>();
            services.AddScoped<IAutomatedMeterReadingService, AutomatedMeterReadingService>();
            services.AddScoped<ITaskManDayService, TaskManDayService>();
            services.AddScoped<IFacilityCommissioningService, FacilityCommissioningService>();
            services.AddScoped<IReportCheckListService, ReportCheckListService>();
            services.AddScoped<IFacilityScoreService, FacilityScoreService>();
            services.AddScoped<IMasterParameterService, MasterParameterService>();
            services.AddScoped<IReportService, ReportService>();
            return services;
        }
    }
}
