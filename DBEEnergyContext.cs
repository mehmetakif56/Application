using Microsoft.EntityFrameworkCore;
using DBE.ENERGY.Core.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace DBE.ENERGY.Infrastructure.Data
{
    public partial class DBEEnergyContext : DbContext
    {
        public DBEEnergyContext() { }
        public DBEEnergyContext(DbContextOptions<DBEEnergyContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            this.Database.SetCommandTimeout(300);
        }

        public DbSet<ConditionEntity> Conditions { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<TaskMasterFacilityEntity> TaskMasterFacilitys { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<TaskDetailEntity> TaskDetails { get; set; }
        public DbSet<TaskMasterEntity> TaskMasters { get; set; }
        public DbSet<FacilityEntity> Facilities { get; set; }
        public DbSet<TaskRoleEntity> TaskRoles { get; set; }
        public DbSet<CustomClaimEntity> CustomClaims { get; set; }
        public DbSet<RoleClaimEntity> RoleClaims { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
        public DbSet<TaskUserAuthRoleEntity> TaskUserAuthRoles { get; set; }
        public DbSet<ADPEntity> ADPs { get; set; }
        public DbSet<BackupMaterialEntity> BackupMaterials { get; set; }
        public DbSet<CameraSystemEntity> CameraSystems { get; set; }
        public DbSet<CBoxEntity> CBoxes { get; set; }
        public DbSet<ConstructionEntity> Constructions { get; set; }
        public DbSet<GroundEntity> Grounds { get; set; }
        public DbSet<InverterEntity> Inverters { get; set; }
        public DbSet<OGNotifyEntity> OGNotifies { get; set; }
        public DbSet<PanelEntity> Panels { get; set; }
        public DbSet<RemoteMonitoringEntity> RemoteMonitorings { get; set; }
        public DbSet<SecurityAdministrativeEntity> SecurityAdministratives { get; set; }
        public DbSet<SoilStructureEntity> SoilStructures { get; set; }
        public DbSet<TDPEntity> TDPs { get; set; }
        public DbSet<TransformerKioskEntity> TransformerKiosks { get; set; }
        public DbSet<WiringEntity> Wirings { get; set; }
        public DbSet<TaskRuleEntity> TaskRules { get; set; }
        public DbSet<TaskManDayEntity> TaskManHours { get; set; }
        public DbSet<TaskUserOwnerRoleEntity> TaskUserOwnerRoles { get; set; }
        public DbSet<FacilityMeasuresMatchEntity> FacilityMeasuresMatches { get; set; }
        public DbSet<MailTemplateEntity> MailTemplates { get; set; }
        public DbSet<InverterDetailEntity> InverterDetails { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<TownEntity> Towns { get; set; }
        public DbSet<FacilityTrendDailyEntity> FacilityTrendDailies { get; set; }
        public DbSet<SettingsParameterEntity> SettingsParameters { get; set; }
        public DbSet<CashFlowCategoryMaster> CashFlowCategoryMasters { get; set; }
        public DbSet<CashFlowCategoryDetail> CashFlowCategoryDetails { get; set; }
        public DbSet<CashFlowFacilityEntity> CashFlowFacilities { get; set; }
        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<FacilityTablesMatchEntity> FacilityTablesMatches { get; set; }
        public DbSet<PeriodicValuesEntity> PeriodicValues { get; set; }
        public DbSet<AdviceEnitity> Advices { get; set; }
        public DbSet<FacilityResponsibleEntity> FacilityResponsibles { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<NotificationEventEntity> NotificationEvents { get; set; }
        public DbSet<NotificationRuleEntity> NotificationRules { get; set; }
        public DbSet<FacilityDocumentEntity> FacilityDocuments { get; set; }
        public DbSet<FacilityTotalEnergyValuesEntity> FacilityTotalEnergyValues { get; set; }
        public DbSet<InverterTotalEnergyValuesEntity> InverterTotalEnergyValues { get; set; }
        public DbSet<VariablesUnits> VariablesUnits { get; set; }
        public DbSet<DailyEnergyValuesEntity> DailyEnergies { get; set; }
        public DbSet<FacilityPasswordEntity> FacilityPasswords { get; set; }
        public DbSet<FacilityContactEntity> FacilityContacts { get; set; }
        public DbSet<NotificationValuesEntity> NotificationValues { get; set; }
        public DbSet<AutomatedMeterReadingEntity> AutomatedMeterReadings { get; set; }
        public DbSet<CashFlowMasterEntity> CashFlowMasters { get; set; }
        public DbSet<CashFlowDetailEntity> CashFlowDetails { get; set; }
        public DbSet<FacilityCommissioningEntity> FacilityCommissionings { get; set; }
        public DbSet<ReportCheckListEntity> ReportCheckLists { get; set; }
        public DbSet<MasterParameterEntity> MasterParameters { get; set; }
        public DbSet<MessengerMasterEntity> MessengerMasters { get; set; }

        public DbSet<FacilityCreatingValuesEntity> FacilityCreatingValues { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyBaseEntityConfiguration();

            builder.Entity<ConditionEntity>(ConfigureCondition);
            builder.Entity<CustomerEntity>(ConfigureCustomer);
            builder.Entity<DocumentEntity>(ConfigureFile);
            builder.Entity<EmployeeEntity>(ConfigurePersonnel);
            builder.Entity<TaskMasterFacilityEntity>(ConfigureRequestSolarPowerPlant);
            builder.Entity<RoleEntity>(ConfigureRole);
            builder.Entity<UserEntity>(ConfigureUser);
            builder.Entity<UserRoleEntity>(ConfigureUserRole);
            builder.Entity<TaskUserAuthRoleEntity>(ConfigureTaskUserRole);
            builder.Entity<ClaimEntity>(ConfigureClaim);
            builder.Entity<RoleClaimEntity>(ConfigureRoleClaim);
            builder.Entity<CustomClaimEntity>(ConfigureCustomClaim);
            builder.Entity<TaskUserOwnerRoleEntity>(ConfigureTaskUserOwnerRole);
            builder.Entity<TaskRoleEntity>(ConfigureTaskRole);
            builder.Entity<FacilityEntity>(ConfigureFacility);
            builder.Entity<TaskMasterEntity>(ConfigureTaskMaster);
            builder.Entity<TaskDetailEntity>(ConfigureTaskDetail);

            builder.Entity<ADPEntity>(ConfigureADP);
            builder.Entity<BackupMaterialEntity>(ConfigureBackupMaterial);
            builder.Entity<CameraSystemEntity>(ConfigureCameraSystem);
            builder.Entity<CBoxEntity>(ConfigureCBox);
            builder.Entity<ConstructionEntity>(ConfigureConstruction);
            builder.Entity<GroundEntity>(ConfigureGround);
            builder.Entity<InverterEntity>(ConfigureInverter);
            builder.Entity<OGNotifyEntity>(ConfigureOGNotify);
            builder.Entity<PanelEntity>(ConfigurePanel);
            builder.Entity<RemoteMonitoringEntity>(ConfigureRemoteMonitoring);
            builder.Entity<SecurityAdministrativeEntity>(ConfigureSecurityAdministrative);
            builder.Entity<SoilStructureEntity>(ConfigureSoilStructure);
            builder.Entity<TDPEntity>(ConfigureTDP);
            builder.Entity<TransformerKioskEntity>(ConfigureTransformerKiosk);
            builder.Entity<WiringEntity>(ConfigureWiring);
            builder.Entity<TaskRuleEntity>(ConfigureTaskRule);
            builder.Entity<TaskManDayEntity>(ConfigureTaskManDay);
            builder.Entity<FacilityMeasuresMatchEntity>(ConfigureFacilityMeasuresMatch);
            builder.Entity<MailTemplateEntity>(ConfigureMailTemplate);
            builder.Entity<InverterDetailEntity>(ConfigureInverterDetail);
            builder.Entity<CityEntity>(ConfigureCity);
            builder.Entity<TownEntity>(ConfigureTown);
            builder.Entity<FacilityTrendDailyEntity>(ConfigureFacilityTrendDaily);
            builder.Entity<SettingsParameterEntity>(ConfigureSettingsParameter);
            builder.Entity<CashFlowCategoryMaster>(ConfigureCashFlowCategoryMaster);
            builder.Entity<CashFlowCategoryDetail>(ConfigureCashFlowCategoryDetail);
            builder.Entity<CashFlowFacilityEntity>(ConfigureCashFlowFacility);
            builder.Entity<CurrencyEntity>(ConfigureCurrency);
            builder.Entity<FacilityTablesMatchEntity>(ConfigureFacilityTablesMatch);
            builder.Entity<PeriodicValuesEntity>(ConfigurePeriodicValues);
            builder.Entity<AdviceEnitity>(ConfigureAdvice);
            builder.Entity<FacilityResponsibleEntity>(ConfigureFacilityResponsible);
            builder.Entity<NotificationEntity>(ConfigureNotification);
            builder.Entity<NotificationEventEntity>(ConfigureNotificationEvent);
            builder.Entity<NotificationRuleEntity>(ConfigureNotificationRule);
            builder.Entity<FacilityDocumentEntity>(ConfigureFacilityDocument);
            builder.Entity<FacilityContactEntity>(ConfigureFacilityContact);
            builder.Entity<FacilityCommissioningEntity>(ConfigureFacilityCommissioning);
            builder.Entity<ReportCheckListEntity>(ConfigureReportCheckList);
            builder.Entity<MasterParameterEntity>(ConfigureMasterType);
            builder.Entity<FacilityCreatingValuesEntity>(ConfigureFacilityCreatingValues);
            builder.Entity<MessengerMasterEntity>(ConfigureMessengerMaster);
            builder.Entity<MessengerUserEntity>(ConfigureMessengerUser);
            #region Total Facility and Inverters Values
            builder.Entity<FacilityTotalEnergyValuesEntity>(ConfigureFacilityTotalEnergyValues);
            builder.Entity<InverterTotalEnergyValuesEntity>(ConfigureInverterTotalEnergyValues);
            builder.Entity<VariablesUnits>(ConfigureVariablesUnits);
            builder.Entity<DailyEnergyValuesEntity>(ConfigureDailyEnergy);
            builder.Entity<FacilityPasswordEntity>(ConfigureFacilityPassword);
            builder.Entity<AutomatedMeterReadingEntity>(ConfigureAutomatedMeterReadings);
            builder.Entity<CashFlowMasterEntity>(ConfigureCashFlowMaster);
            builder.Entity<CashFlowDetailEntity>(ConfigureCashFlowDetail);
            #endregion
        }
    }
}
