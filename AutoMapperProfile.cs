using AutoMapper;
using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Web.Localization;
using DBE.ENERGY.Web.ViewModels;
using DBE.ENERGY.Web.ViewModels.Admin;
using DBE.ENERGY.Web.ViewModels.Advice;
using DBE.ENERGY.Web.ViewModels.AutomatedMeterReading;
using DBE.ENERGY.Web.ViewModels.CashFlow;
using DBE.ENERGY.Web.ViewModels.City;
using DBE.ENERGY.Web.ViewModels.Facility;
using DBE.ENERGY.Web.ViewModels.FacilityCommissioning;
using DBE.ENERGY.Web.ViewModels.FacilityDocument;
using DBE.ENERGY.Web.ViewModels.FacilityScore;
using DBE.ENERGY.Web.ViewModels.Notification;
using DBE.ENERGY.Web.ViewModels.Report;
using DBE.ENERGY.Web.ViewModels.TaskDetail;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace DBE.ENERGY.Web.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(GenericSharedResourceService localizer, IHostingEnvironment env)
        {

            CreateMap<TaskViewModel, TaskMasterEntity>()
                 // .ForMember(dest => dest.IncidentDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.IncidentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.PriorityType, opt => opt.MapFrom(src => (int)src.Priority));


            CreateMap<TaskMasterEntity, TaskViewModel>()
                // .ForMember(dest => dest.IncidentDate, opt => opt.MapFrom(src => src.IncidentDate.HasValue ? src.IncidentDate.Value.ToString("dd/MM/yyyy") : ""))
                .ForMember(dest => dest.Facilities, opt => opt.MapFrom(src => src.TaskMasterFacilities.Select(x => x.Facility.Name)))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (int)src.PriorityType));

            CreateMap<GraficSettingViewModel, SettingsParameterEntity>();
            CreateMap<SettingsParameterEntity, GraficSettingViewModel>();

            CreateMap<AdviceViewModel, AdviceEnitity>();
            CreateMap<AdviceEnitity, AdviceViewModel>();

            CreateMap<EmployeeViewModel, EmployeeEntity>();
            CreateMap<EmployeeEntity, EmployeeViewModel>();

            CreateMap<CustomerViewModel, CustomerEntity>();
            CreateMap<CustomerEntity, CustomerViewModel>();

            CreateMap<RoleViewModel, RoleEntity>();
            CreateMap<RoleEntity, RoleViewModel>();

            CreateMap<UserViewModel, UserEntity>();
            CreateMap<UserEntity, UserViewModel>();

            CreateMap<NotificationViewModel, NotificationEntity>();
            CreateMap<NotificationEntity, NotificationViewModel>();

            CreateMap<NotificationEventViewModel, NotificationEventEntity>();
            CreateMap<NotificationEventEntity, NotificationEventViewModel>();

            CreateMap<NotificationRuleViewModel, NotificationRuleEntity>()
                .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => (int)src.Operator))
                .ForMember(dest => dest.NotificationRepeater, opt => opt.MapFrom(src => (int)src.NotificationRepeater));
            CreateMap<NotificationRuleEntity, NotificationRuleViewModel>()
                .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => (int)src.Operator))
                .ForMember(dest => dest.NotificationRepeater, opt => opt.MapFrom(src => (int)src.NotificationRepeater)); ; 

            CreateMap<FacilityDocumentViewModel, FacilityDocumentEntity>();
            CreateMap<FacilityDocumentEntity, FacilityDocumentViewModel>();

            CreateMap<PlanViewModel, TaskManDayEntity>();
            CreateMap<TaskManDayEntity, PlanViewModel>();

            CreateMap<TaskUserAuthRoleViewModel, TaskUserAuthRoleEntity>();
            CreateMap<TaskUserAuthRoleEntity, TaskUserAuthRoleViewModel>();

            CreateMap<TaskUserOwnerRoleViewModel, TaskUserOwnerRoleEntity>();
            CreateMap<TaskUserOwnerRoleEntity, TaskUserOwnerRoleViewModel>();

            CreateMap<FacilityPasswordViewModel, FacilityPasswordEntity>();
            CreateMap<FacilityPasswordEntity, FacilityPasswordViewModel>();

            CreateMap<FacilityContactViewModel, FacilityContactEntity>();
            CreateMap<FacilityContactEntity, FacilityContactViewModel>();

            CreateMap<FacilityViewModel, FacilityEntity>();
            CreateMap<FacilityEntity, FacilityViewModel>();

            CreateMap<AutomatedMeterReadingViewModel,AutomatedMeterReadingEntity>();
            CreateMap<AutomatedMeterReadingEntity, AutomatedMeterReadingViewModel>();

            CreateMap<PanelViewModel, PanelEntity>();
            CreateMap<PanelEntity, PanelViewModel>();

            CreateMap<InverterViewModel, InverterEntity>();
            CreateMap<InverterEntity, InverterViewModel>();

            CreateMap<WiringViewModel, WiringEntity>();
            CreateMap<WiringEntity, WiringViewModel>();

            CreateMap<CBoxViewModel, CBoxEntity>();
            CreateMap<CBoxEntity, CBoxViewModel>();

            CreateMap<TdpViewModel, TDPEntity>();
            CreateMap<TDPEntity, TdpViewModel>();

            CreateMap<AdpViewModel, ADPEntity>();
            CreateMap<ADPEntity, AdpViewModel>();

            CreateMap<GroundViewModel, GroundEntity>();
            CreateMap<GroundEntity, GroundViewModel>();

            CreateMap<TransformerKioskViewModel, TransformerKioskEntity>();
            CreateMap<TransformerKioskEntity, TransformerKioskViewModel>();

            CreateMap<OGNotifyViewModel, OGNotifyEntity>();
            CreateMap<OGNotifyEntity, OGNotifyViewModel>();

            CreateMap<ConstructionViewModel, ConstructionEntity>();
            CreateMap<ConstructionEntity, ConstructionViewModel>();

            CreateMap<SoilStructureViewModel, SoilStructureEntity>();
            CreateMap<SoilStructureEntity, SoilStructureViewModel>();

            CreateMap<CashFlowMasterViewModel, CashFlowMasterEntity>();
            CreateMap<CashFlowMasterEntity, CashFlowMasterViewModel>()
                      .ForMember(dest => dest.Facilities, opt => opt.MapFrom(src => src.CashFlowFacilityEntities.Select(x => x.Facility.Name)));

            CreateMap<CashFlowDetailViewModel, CashFlowDetailEntity>();
            CreateMap<CashFlowDetailEntity, CashFlowDetailViewModel>();

            CreateMap<SoilStructureViewModel, SoilStructureEntity>();
            CreateMap<SoilStructureEntity, SoilStructureViewModel>();

            CreateMap<SecurityAdministrativeViewModel, SecurityAdministrativeEntity>();
            CreateMap<SecurityAdministrativeEntity, SecurityAdministrativeViewModel>();

            CreateMap<RemoteMonitoringViewModel, RemoteMonitoringEntity>();
            CreateMap<RemoteMonitoringEntity, RemoteMonitoringViewModel>();

            CreateMap<CameraSystemViewModel, CameraSystemEntity>();
            CreateMap<CameraSystemEntity, CameraSystemViewModel>();

            CreateMap<BackupMaterialViewModel, BackupMaterialEntity>();
            CreateMap<BackupMaterialEntity, BackupMaterialViewModel>();

            CreateMap<FacilityInformationViewModel, FacilityEntity>();
            CreateMap<FacilityEntity, FacilityInformationViewModel>();

            CreateMap<FacilityCommissioningViewModel, FacilityCommissioningEntity>();
            CreateMap<FacilityCommissioningEntity, FacilityCommissioningViewModel>();
            
            CreateMap<CheckListItemViewModel, ReportCheckListEntity>();
            CreateMap<ReportCheckListEntity, CheckListItemViewModel>();


            CreateMap<CityViewModel, CityEntity>();
            CreateMap<CityEntity, CityViewModel>();

            CreateMap<FacilityCommissioningViewModel, FacilityTablesMatchEntity>();
            CreateMap<FacilityTablesMatchEntity, FacilityCommissioningViewModel>();

            CreateMap<FacilityScoreParameterViewModel, MasterParameterEntity>();
            CreateMap<MasterParameterEntity, FacilityScoreParameterViewModel>();

            CreateMap<FacilityScoreViewModel, FacilityEntity>();
            CreateMap<FacilityEntity, FacilityScoreViewModel>();
        }
    }
}
