using DBE.ENERGY.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBE.ENERGY.Infrastructure.Data
{
    public partial class DBEEnergyContext
    {
        private void ConfigureFile(EntityTypeBuilder<DocumentEntity> builder)
        {
            builder.ToTable("Document");
        }
        private void ConfigureCustomer(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.ToTable("Customer");
        }
        private void ConfigureTaskMaster(EntityTypeBuilder<TaskMasterEntity> builder)
        {
            builder.ToTable("TaskMaster");
        }
        private void ConfigureTaskDetail(EntityTypeBuilder<TaskDetailEntity> builder)
        {
            builder.ToTable("TaskDetail");
        }
        private void ConfigureRequestSolarPowerPlant(EntityTypeBuilder<TaskMasterFacilityEntity> builder)
        {
            builder.ToTable("TaskMasterFacility");
            builder.HasKey(x => new { x.TaskMasterId, x.FacilityId });
        }
        private void ConfigureUser(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");
        }
        private void ConfigureFacility(EntityTypeBuilder<FacilityEntity> builder)
        {
            builder.ToTable("Facility");
        }
        private void ConfigureTaskRole(EntityTypeBuilder<TaskRoleEntity> builder)
        {
            builder.ToTable("TaskRole");
        }
        private void ConfigureTaskUserOwnerRole(EntityTypeBuilder<TaskUserOwnerRoleEntity> builder)
        {
            builder.ToTable("TaskUserOwnerRole");

            builder.Ignore("Id");

            builder.HasKey(ur => new { ur.TaskRoleId, ur.UserId });

            builder.HasOne(ur => ur.TaskRole)
                            .WithMany(r => r.TaskUserOwnerRoles)
                            .HasForeignKey(ur => ur.TaskRoleId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                            .WithMany(u => u.TaskUserOwnerRoles)
                            .HasForeignKey(ur => ur.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureCondition(EntityTypeBuilder<ConditionEntity> builder)
        {
            builder.ToTable("Condition");
        }
        private void ConfigurePersonnel(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.ToTable("Employee");
        }

      
        private void ConfigureUserRole(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ToTable("UserRole");

            //there is no need for a surrogate key on many-to-many mapping table
            builder.Ignore("Id");

            builder.HasKey(ur => new { ur.RoleId, ur.UserId });

            builder.HasOne(ur => ur.Role)
                            .WithMany(r => r.UserRoles)
                            .HasForeignKey(ur => ur.RoleId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                            .WithMany(u => u.UserRoles)
                            .HasForeignKey(ur => ur.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureCustomClaim(EntityTypeBuilder<CustomClaimEntity> builder)
        {
            builder.ToTable("CustomClaim");

            builder.HasOne(cc => cc.User)
                            .WithMany(u => u.CustomClaims)
                            .HasForeignKey(cc => cc.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureRoleClaim(EntityTypeBuilder<RoleClaimEntity> builder)
        {
            //there is no need for a surrogate key on many-to-many mapping table
            builder.Ignore("Id");
            builder.ToTable("RoleClaim");

            builder.HasKey(rc => new { rc.ClaimId, rc.RoleId });

            builder.HasOne(rc => rc.Role)
                            .WithMany(r => r.RoleClaims)
                            .HasForeignKey(rc => rc.RoleId).OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(rc => rc.Claim)
                            .WithMany(c => c.RoleClaims)
                            .HasForeignKey(c => c.ClaimId).OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureClaim(EntityTypeBuilder<ClaimEntity> builder)
        {
            builder.ToTable("Claim");
        }
        private void ConfigureRole(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Role");
        }
        private void ConfigureTaskUserRole(EntityTypeBuilder<TaskUserAuthRoleEntity> builder)
        {
            builder.ToTable("TaskUserAuthRole");

            //there is no need for a surrogate key on many-to-many mapping table
            builder.Ignore("Id");

            builder.HasKey(ur => new { ur.TaskRoleId, ur.UserId });

            builder.HasOne(ur => ur.TaskRole)
                            .WithMany(r => r.TaskUserAuthRoles)
                            .HasForeignKey(ur => ur.TaskRoleId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                            .WithMany(u => u.TaskUserAuthRoles)
                            .HasForeignKey(ur => ur.UserId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.TaskRule)
                        .WithMany(u => u.TaskUserAuthRoles)
                        .HasForeignKey(ur => ur.TaskRuleId)
                        .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureADP(EntityTypeBuilder<ADPEntity> builder)
        {
            builder.ToTable("ADP");
        }

        private void ConfigureBackupMaterial(EntityTypeBuilder<BackupMaterialEntity> builder)
        {
            builder.ToTable("BackupMaterial");
        }

        private void ConfigureCameraSystem(EntityTypeBuilder<CameraSystemEntity> builder)
        {
            builder.ToTable("CameraSystem");
        }

        private void ConfigureCBox(EntityTypeBuilder<CBoxEntity> builder)
        {
            builder.ToTable("CBox");
        }

        private void ConfigureConstruction(EntityTypeBuilder<ConstructionEntity> builder)
        {
            builder.ToTable("Construction");
        }
        private void ConfigureGround(EntityTypeBuilder<GroundEntity> builder)
        {
            builder.ToTable("Ground");
        }
        private void ConfigureInverter(EntityTypeBuilder<InverterEntity> builder)
        {
            builder.ToTable("Inverter");
        }

        private void ConfigureOGNotify(EntityTypeBuilder<OGNotifyEntity> builder)
        {
            builder.ToTable("OGNotify");
        }

        private void ConfigurePanel(EntityTypeBuilder<PanelEntity> builder)
        {
            builder.ToTable("Panel");
        }

        private void ConfigureRemoteMonitoring(EntityTypeBuilder<RemoteMonitoringEntity> builder)
        {
            builder.ToTable("RemoteMonitoring");
        }

        private void ConfigureSecurityAdministrative(EntityTypeBuilder<SecurityAdministrativeEntity> builder)
        {
            builder.ToTable("SecurityAdministrative");
        }

        private void ConfigureSoilStructure(EntityTypeBuilder<SoilStructureEntity> builder)
        {
            builder.ToTable("SoilStructure");
        }

        private void ConfigureTDP(EntityTypeBuilder<TDPEntity> builder)
        {
            builder.ToTable("TDP");
        }

        private void ConfigureTransformerKiosk(EntityTypeBuilder<TransformerKioskEntity> builder)
        {
            builder.ToTable("TransformerKiosk");
        }

        private void ConfigureWiring(EntityTypeBuilder<WiringEntity> builder)
        {
            builder.ToTable("Wiring");
        }

        private void ConfigureTaskRule(EntityTypeBuilder<TaskRuleEntity> builder)
        {
            builder.ToTable("TaskRule");
        }

        private void ConfigureTaskManDay(EntityTypeBuilder<TaskManDayEntity> builder)
        {
            builder.ToTable("TaskManDay");
        }

        private void ConfigureFacilityMeasuresMatch(EntityTypeBuilder<FacilityMeasuresMatchEntity> builder)
        {
            builder.ToTable("FacilityMeasuresMatch");
        }

        private void ConfigureMailTemplate(EntityTypeBuilder<MailTemplateEntity> builder)
        {
            builder.ToTable("MailTemplate");
        }

        private void ConfigureInverterDetail(EntityTypeBuilder<InverterDetailEntity> builder)
        {
            builder.ToTable("InverterDetail");
        }

        private void ConfigureCity(EntityTypeBuilder<CityEntity> builder)
        {
            builder.ToTable("City");
        }

        private void ConfigureTown(EntityTypeBuilder<TownEntity> builder)
        {
            builder.ToTable("Town");
        }

        private void ConfigureFacilityTrendDaily(EntityTypeBuilder<FacilityTrendDailyEntity> builder)
        {
            builder.ToTable("FacilityTrendDaily");
        }
        private void ConfigureSettingsParameter(EntityTypeBuilder<SettingsParameterEntity> builder)
        {
            builder.ToTable("SettingsParameter");
        }

        private void ConfigureCashFlowCategoryMaster(EntityTypeBuilder<CashFlowCategoryMaster> builder)
        {
            builder.ToTable("CashFlowCategoryMaster");
        }
        private void ConfigureCashFlowCategoryDetail(EntityTypeBuilder<CashFlowCategoryDetail> builder)
        {
            builder.ToTable("CashFlowCategoryDetail");
        }

        private void ConfigureCashFlowFacility(EntityTypeBuilder<CashFlowFacilityEntity> builder)
        {
            builder.ToTable("CashFlowFacility");
            builder.HasKey(x => new { x.CashFlowMasterId, x.FacilityId });
        }
        private void ConfigureCurrency(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.ToTable("Currency");
        }
        private void ConfigureFacilityTablesMatch(EntityTypeBuilder<FacilityTablesMatchEntity> builder)
        {
            builder.ToTable("FacilityTablesMatch");
        }
        private void ConfigurePeriodicValues(EntityTypeBuilder<PeriodicValuesEntity> builder)
        {
            builder.ToTable("PeriodicValues");
        }
        private void ConfigureAdvice(EntityTypeBuilder<AdviceEnitity> builder)
        {
            builder.ToTable("Advice");
        }

        private void ConfigureFacilityResponsible(EntityTypeBuilder<FacilityResponsibleEntity> builder)
        {
            builder.ToTable("FacilityResponsible");
            builder.Ignore("Id");

            builder.HasKey(ur => new { ur.FacilityId, ur.UserId });

            builder.HasOne(ur => ur.Facility)
                            .WithMany(r => r.FacilityResponsibles)
                            .HasForeignKey(ur => ur.FacilityId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                            .WithMany(u => u.FacilityResponsibles)
                            .HasForeignKey(ur => ur.UserId)
                            .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureNotification(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notification");
        }

        private void ConfigureNotificationEvent(EntityTypeBuilder<NotificationEventEntity> builder)
        {
            builder.ToTable("NotificationEvent");
        }

        private void ConfigureNotificationRule(EntityTypeBuilder<NotificationRuleEntity> builder)
        {
            builder.ToTable("NotificationRule");
        }

        private void ConfigureFacilityDocument(EntityTypeBuilder<FacilityDocumentEntity> builder)
        {
            builder.ToTable("FacilityDocument");
        }

        private void ConfigureFacilityTotalEnergyValues(EntityTypeBuilder<FacilityTotalEnergyValuesEntity> builder)
        {
            builder.ToTable("FacilityTotalEnergyValues");

        }

        private void ConfigureInverterTotalEnergyValues(EntityTypeBuilder<InverterTotalEnergyValuesEntity> builder)
        {
            builder.ToTable("InverterTotalEnergyValues");
        }

        private void ConfigureVariablesUnits(EntityTypeBuilder<VariablesUnits> builder)
        {
            builder.ToTable("VariablesUnits");
        }

        private void ConfigureDailyEnergy(EntityTypeBuilder<DailyEnergyValuesEntity> builder)
        {
            builder.ToTable("DailyEnergyValues");
            builder.HasNoKey();
            //builder.Property(v => v.FacilityId).HasColumnName("FacilityId");
            builder.Property(v => v.InverterDetailId).HasColumnName("InverterDetailId");
            //builder.HasIndex(x => x.FacilityId);
            builder.HasIndex(x => x.InverterDetailId);
        }

        private void ConfigureNotifyValuesEnergy(EntityTypeBuilder<NotificationValuesEntity> builder)
        {
            builder.ToTable("NotificationValues");
            builder.HasNoKey();
            builder.Property(v => v.Id).HasColumnName("Id");
        }

        private void ConfigureFacilityCreatingValues(EntityTypeBuilder<FacilityCreatingValuesEntity> builder)
        {
            builder.ToTable("FacilityCreatingValues");
            builder.HasNoKey();
            builder.Property(v => v.Id).HasColumnName("Id");
        }

        private void ConfigureFacilityPassword(EntityTypeBuilder<FacilityPasswordEntity> builder)
        {
            builder.ToTable("FacilityPassword");
        }

        private void ConfigureFacilityContact(EntityTypeBuilder<FacilityContactEntity> builder)
        {
            builder.ToTable("FacilityContact");
        }

        private void ConfigureAutomatedMeterReadings(EntityTypeBuilder<AutomatedMeterReadingEntity> builder)
        {
            builder.ToTable("AutomatedMeterReading");
        }

        private void ConfigureCashFlowMaster(EntityTypeBuilder<CashFlowMasterEntity> builder)
        {
            builder.ToTable("CashFlowMaster");
        }

        private void ConfigureCashFlowDetail(EntityTypeBuilder<CashFlowDetailEntity> builder)
        {
            builder.ToTable("CashFlowDetail");
        }

        private void ConfigureFacilityCommissioning(EntityTypeBuilder<FacilityCommissioningEntity> builder)
        {
            builder.ToTable("FacilityCommissioning");
        }

        private void ConfigureReportCheckList(EntityTypeBuilder<ReportCheckListEntity> builder)
        {
            builder.ToTable("ReportCheckList");
        }
        private void ConfigureMasterType(EntityTypeBuilder<MasterParameterEntity> builder)
        {
            builder.ToTable("MasterParameter");
        } 
        private void ConfigureMessengerMaster(EntityTypeBuilder<MessengerMasterEntity> builder)
        {
            builder.ToTable("MessengerMaster");
        }
        private void ConfigureMessengerUser(EntityTypeBuilder<MessengerUserEntity> builder)
        {
            builder.ToTable("MessengerUser");
        }
    }
}

