using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Enums;
using DBE.ENERGY.Core.Services;
using DBE.ENERGY.Web.Models;
using DBE.ENERGY.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DBE.ENERGY.Web.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _requestService;
        private readonly ITaskVerifyService _taskVerifyService;
        private readonly IIntegrationService _integrationService;
        private readonly ICashFlowService _cashFlowService;
        private readonly ISettingsParameterService _settingsParameter;

        public HomeController(ITaskVerifyService taskVerifyService,ILogger<HomeController> logger, ITaskService requestService, IIntegrationService integrationService, ICashFlowService cashFlowService, ISettingsParameterService settingsParameter)
        {
            _logger = logger;
            _requestService = requestService;
            _taskVerifyService = taskVerifyService;
            _integrationService = integrationService;
            _settingsParameter = settingsParameter;
            _cashFlowService = cashFlowService;
        }

        public IActionResult Index()
        {
            try
            {
                if (HttpContext.User.IsInRole(UserAppRole.Director.ToString("D")))
                    return RedirectToAction("Index", "HomeDirector");
                else if (HttpContext.User.IsInRole(UserAppRole.Personnel.ToString("D")))
                    return RedirectToAction("Index", "HomePersonal");
                else if (HttpContext.User.IsInRole(UserAppRole.Customer.ToString("D")))
                    return RedirectToAction("Index", "HomeCustomer");
                else
                    return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"Index"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return RedirectToAction("/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetSettings()
        {
            try
            {
                return Json(_settingsParameter.GetSettingByUserId(CurrentUser.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetSettings"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        public IActionResult GetFacilityProductionTimelineData()
        {
            try
            {
                var setting = _settingsParameter.GetSettingByGraficType(CurrentUser.Id, GraficCategory.InsantProduction);
                if (setting !=null)
                {
                    var chartList = new List<TimeChartModel>();
                    var lst = _integrationService
                        .GetDailyEnergyLacivert(setting.InverterDetailId, setting.Facility.FacilityMeasuresMatches.FirstOrDefault().MatchType)
                        .OrderBy(x => x.CreatedDate)
                        .ToList();
                    foreach (var item in lst)
                    {
                        var timeChart = new TimeChartModel();
                        timeChart.date = item.CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                        timeChart.production = Math.Round(item.TotalEnergyAmount, 2);
                        timeChart.radiation = Math.Round(item.IrraditonAmount, 2);
                        timeChart.kwp = Math.Round(item.KWPAmount, 2);
                        chartList.Add(timeChart);
                    }
                    return Json(new { isShow=true, list=chartList });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }          
            catch(Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetFacilityProductionTimelineData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        } 

        public IActionResult GetFacilityRTUData()
        {
            try
            {
                var stackedCharts = new List<StackedChartModel>();
                var lst = _integrationService.GetFacilityRTUByCustomerId(CurrentUser.CustomerId).ToList();

                if (lst.Count() > 0)
                {
                    foreach (var item in lst)
                    {
                        var stackedChart = new StackedChartModel();
                        stackedChart.RTU = item.RTUEnergy;
                        stackedChart.Month = item.RegisteredDate.Value.Month;
                        stackedChart.City = item.CityName;
                        stackedChart.Facility = item.FacilityName;
                        stackedChart.FacilityId = item.FacilityId;
                        stackedChart.CityId = item.CityId;
                        stackedChart.Year = item.RegisteredDate.Value.Year;
                        stackedCharts.Add(stackedChart);
                    }
                    var groupedList = stackedCharts.GroupBy(x => new { x.FacilityId, x.CityId, x.City, x.Facility, x.Month, x.Year })
                                                        .Select(x => new StackedChartModel { RTU = x.Sum(c => c.RTU), FacilityId = x.Key.FacilityId, Facility = x.Key.Facility, City = x.Key.City, Month = x.Key.Month, Year = x.Key.Year })
                                                        .OrderBy(x => x.Month);

                    return Json(new { isShow = true, list = groupedList });
                }
                else
                {
                    return Json(new { isShow = false }); 
                }  
            }
            catch(Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetFacilityRTUData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        public IActionResult GetInverterScoresData(PeriodType periodType)
        {
            var resultList = new EnergyTemperatureModel { TemperatureScores=  new List<TemperatureScoreModel>(), EnergyScores= new List<EnergyScoreModel>(), isShow= true };
            try
            {
                var setting = _settingsParameter.GetSettingByGraficType(CurrentUser.Id, GraficCategory.InverterScore);
                if (setting != null)
                {
                    var list = _integrationService
                        .GetInverterScoreValues(setting.FacilityId, periodType)
                        .GroupBy(x => new { x.InverterDetailId })
                        .Select(x => new ScoreTableModel {
                            TotalEnergy = x.Average(x => x.TotalEnergy),
                            TotalKwp = x.Average(x => x.TotalKwp),
                            TotalTemp = x.Average(x => x.TotalTemperature),
                            InverterName = x.FirstOrDefault().InverterName
                        });

                    foreach (var item in list.Count() > 10 ? list.OrderByDescending(x => x.TotalTemp).Where((v, i) => i < 5 || i > list.Count() - 6) : list.OrderByDescending(x => x.TotalTemp))
                    {
                        resultList.TemperatureScores.Add(new TemperatureScoreModel {
                            TotalTemp = Math.Round(item.TotalTemp, 2),
                            InverterName = item.InverterName
                        });
                    }

                    foreach (var item in list.Count() > 10 ? list.OrderByDescending(x => x.TotalEnergy).Where((v, i) => i < 5 || i > list.Count() - 6) : list.OrderByDescending(x => x.TotalEnergy))
                    {
                        resultList.EnergyScores.Add(new EnergyScoreModel {
                            TotalEnergy = Math.Round(item.TotalEnergy, 2),
                            InverterName = item.InverterName,
                            TotalKwp = Math.Round(item.TotalKwp, 2)
                        });
                    }
                }
                else
                {
                    resultList.isShow = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetInverterScoresData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
            return Json(resultList);
        }

        public IActionResult GetLuytobData()
        {
            try
            {
                var settingPrm = _settingsParameter.GetSettingByGraficType(CurrentUser.Id, GraficCategory.Luytob);
                if (settingPrm != null)
                {
                    var luytobCharts = new List<LuytobModel>();
                    var lst = _integrationService.GetLuytobValues(settingPrm.FacilityId).OrderBy(o => o.RegisteredDate).ToList();
                    foreach (var item in lst)
                    {
                        var LuytobChart = new LuytobModel();
                        LuytobChart.IncomeAmount = item.IncomeAmount;
                        LuytobChart.ExpenseAmount = item.ExpenseAmount;
                        LuytobChart.Day = item.RegisteredDate.Value.Day;
                        LuytobChart.Month = item.RegisteredDate.Value.ToString("MM");
                        LuytobChart.Facility = item.FacilityName;
                        LuytobChart.FacilityId = item.FacilityId;
                        LuytobChart.Year = item.RegisteredDate.Value.Year;
                        luytobCharts.Add(LuytobChart);
                    }
                    var groupedList = luytobCharts.GroupBy(g => g.Month);
                    return Json(new { isShow = true, list = groupedList });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetLuytobData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        public IActionResult GetCashFlowGraphicData()
        {
            var cashFlowProjectionCharts = new List<CashFlowProjectionModel>();
            try
            {
                var settingPrm = _settingsParameter.GetSettingByGraficType(CurrentUser.Id, GraficCategory.CashFlow);
                if (settingPrm !=null)
                {
                    List<CashFlowMasterEntity> cashFlowMasters = _cashFlowService.GetCashFlowGrahicData(settingPrm.FacilityId).ToList();
                    foreach(CashFlowMasterEntity cashFlowMaster in cashFlowMasters)
                    {


                        if (cashFlowMaster.CashFlowStatusType == CashFlowStatusType.Open)
                        {
                            cashFlowProjectionCharts.Add(new CashFlowProjectionModel
                            {
                                Type = "Planned",
                                IncomeAmount = cashFlowMaster.CashFlowType == CashFlowType.Income ? (cashFlowMaster.Amount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0,
                                ExpenseAmount = -1 * (cashFlowMaster.CashFlowType == CashFlowType.Expense ? (cashFlowMaster.Amount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0),
                                FacilityId = settingPrm.FacilityId,
                                Facility = cashFlowMaster.CashFlowFacilityEntities.Where(x => x.FacilityId == settingPrm.FacilityId).FirstOrDefault().Facility.Name,
                                RegisteredDate = cashFlowMaster.PaymentDate.Value.ToString("dd-MM-yyyy")
                            });
                        }
                        else if (cashFlowMaster.CashFlowStatusType == CashFlowStatusType.Closed) // All Payed
                        {
                            foreach (CashFlowDetailEntity cashFlowDetail in cashFlowMaster.CashFlowDetails)
                            {
                                cashFlowProjectionCharts.Add(new CashFlowProjectionModel
                                {
                                    Type = "Payed",
                                    SubType = "Payed",
                                    IncomeAmount = cashFlowMaster.CashFlowType == CashFlowType.Income ? (cashFlowDetail.ProccessAmount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0,
                                    ExpenseAmount = -1 * (cashFlowMaster.CashFlowType == CashFlowType.Expense ? (cashFlowDetail.ProccessAmount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0),
                                    FacilityId = settingPrm.FacilityId,
                                    Facility = cashFlowMaster.CashFlowFacilityEntities.Where(x => x.FacilityId == settingPrm.FacilityId).FirstOrDefault().Facility.Name,
                                    RegisteredDate = cashFlowDetail.CreatedDate.Value.ToString("dd-MM-yyyy")
                                });

                            }
                        }
                        else // Half Payed
                        {
                            foreach (CashFlowDetailEntity cashFlowDetail in cashFlowMaster.CashFlowDetails) // Add every detail to list and calc total payed 
                            {
                                cashFlowProjectionCharts.Add(new CashFlowProjectionModel
                                {
                                    Type = "HalfPayed",
                                    SubType = "Payed",
                                    IncomeAmount = cashFlowMaster.CashFlowType == CashFlowType.Income ? (cashFlowDetail.ProccessAmount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0,
                                    ExpenseAmount = -1 * (cashFlowMaster.CashFlowType == CashFlowType.Expense ? (cashFlowDetail.ProccessAmount / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0),
                                    FacilityId = settingPrm.FacilityId,
                                    Facility = cashFlowMaster.CashFlowFacilityEntities.Where(x => x.FacilityId == settingPrm.FacilityId).FirstOrDefault().Facility.Name,
                                    RegisteredDate = cashFlowDetail.CreatedDate.Value.ToString("dd-MM-yyyy")
                                });
                            }

                            cashFlowProjectionCharts.Add(new CashFlowProjectionModel
                            {
                                Type = "HalfPayed",
                                SubType = "Planned",
                                IncomeAmount = cashFlowMaster.CashFlowType == CashFlowType.Income ? ((cashFlowMaster.Amount - cashFlowMaster.PayedAmount) / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0,
                                ExpenseAmount = -1 * (cashFlowMaster.CashFlowType == CashFlowType.Expense ? ((cashFlowMaster.Amount - cashFlowMaster.PayedAmount) / cashFlowMaster.CashFlowFacilityEntities.Count()) : 0.0),
                                FacilityId = settingPrm.FacilityId,
                                Facility = cashFlowMaster.CashFlowFacilityEntities.Where(x => x.FacilityId == settingPrm.FacilityId).FirstOrDefault().Facility.Name,
                                RegisteredDate = cashFlowMaster.PaymentDate.Value.ToString("dd-MM-yyyy")
                            });
                        }



                    }
                    return Json(new { isShow = true, list = cashFlowProjectionCharts.OrderBy(x => x.RegisteredDate).GroupBy(x => x.RegisteredDate) });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetCashFlowProjectionData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Json(new { isShow = false });
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        public IActionResult GetCashFlowBoxChart()
        {
            try
            {
                if (CurrentUser.CustomerId != null)
                {
                    var dtList = new List<GetCashFlowBoxChartModel>();
                    var allCashFlows = _cashFlowService.GetCashFlowChartData().ToList();

                    foreach (var item in allCashFlows)
                    {
                        var cashItem = new GetCashFlowBoxChartModel();

                        cashItem.MasterCategoryId = (Guid)item.CashFlowCategoryMasterId;
                        cashItem.MasterCategoryName = item.Name;
                        cashItem.DetailCategoryName = item.Description;
                        cashItem.Amount = item.Amount;
                        cashItem.CashFlowType = item.CashFlowType;
                        dtList.Add(cashItem);
                    }

                    return Json(new { isShow = true, list = dtList });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetCashFlowBoxChart"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetDirectorData()
        {
            var chartData = new List<DirectorChartModel>();
            try
            {
                var list = _requestService.GetTaskDetailByRoleId(CurrentUser.TaskUserOwnerRoles.FirstOrDefault().TaskRoleId);

                if (list.Count() > 0)
                {
                    foreach (var item in list.GroupBy(x => x.TaskRoleId))
                    {
                        foreach (var item2 in item.GroupBy(x => x.AssignedUserName))
                        {
                            chartData.Add(new DirectorChartModel
                            {
                                role = item2.First().TaskRole.Name,
                                user = item2.First().AssignedUserName,
                                open = item2.Where(x => x.TaskDetailStatus == TaskDetailStatus.Open).Count(),
                                inprogress = item2.Where(x => x.TaskMaster.TaskMasterStatus != TaskMasterStatus.Closed && x.TaskDetailStatus == TaskDetailStatus.Closed && x.TaskMaster.CreatedBy==item2.FirstOrDefault().AssignedUserId).Count(),
                                closed = item2.Where(x=> x.TaskDetailStatus == TaskDetailStatus.Closed).Count()
                            });
                        }
                    }
                    return Json(new { isShow = true, list = chartData });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetDirectorData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }
        
        public IActionResult GetTaskGraphData()
        {
            var chartData = new HomePageModel();
            try
            {
                var allTasks = _requestService.GetTaskMasterEntities(CurrentUser.Id);
                var allDetails = HttpContext.User.IsInRole(UserAppRole.Customer.ToString("D")) 
                    ? _taskVerifyService.GetTaskDetailEntities() 
                    : _requestService.GetTaskDetailForHasAssignedById();

                chartData.CountAll = allTasks.Count() + allDetails.Count();
                if (chartData.CountAll > 0)
                {
                    chartData.CountMasterOpen = allTasks.Where(w => w.TaskMasterStatus == TaskMasterStatus.Open).ToList().Count;
                    chartData.CountMasterClosed = allTasks.Where(w => w.TaskMasterStatus == TaskMasterStatus.Closed).ToList().Count;
                    chartData.CountMasterInprogress = allTasks.Where(w => w.TaskMasterStatus == TaskMasterStatus.Inprogress).ToList().Count;
                    chartData.CountDetailOpen = allDetails.Where(s => s.TaskDetailStatus == TaskDetailStatus.Open).Count();
                    chartData.CountDetailClosed = allDetails.Where(s => s.TaskDetailStatus == TaskDetailStatus.Closed).Count();

                    return Json(new { isShow = true, list = chartData });
                }
                else
                {
                    return Json(new { isShow = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Home"}/{"GetTaskGraphData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
        }

        #region Models
        public class DirectorChartModel
        {
            public string role { get; set; }
            public string user { get; set; }
            public int open { get; set; }
            public int inprogress { get; set; }
            public int closed { get; set; }
        }
        public class TimeChartModel
        {
            public string date { get; set; }
            public double production { get; set; }
            public double radiation { get; set; }
            public double kwp { get; set; }
        }
        public class StackedChartModel
        {
            public decimal RTU { get; set; }
            public string Facility { get; set; }
            public Guid? FacilityId { get; set; }
            public string City { get; set; }
            public Guid? CityId { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
        }
        public class LuytobModel
        {
            public decimal ExpenseAmount { get; set; }
            public decimal IncomeAmount { get; set; }
            public string Facility { get; set; }
            public Guid? FacilityId { get; set; }
            public int Day { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
        }
        public class CashFlowProjectionModel
        {
            public string Type { get; set; }
            public string SubType { get; set; }
            public double ExpenseAmount { get; set; }
            public double IncomeAmount { get; set; }
            public string Facility { get; set; }
            public Guid? FacilityId { get; set; }
            public string RegisteredDate { get; set; }
        }
        public class TemperatureScoreModel
        {
            public decimal TotalTemp { get; set; }
            public string InverterName { get; set; }
            public int OrderNum { get; set; }
        }
        public class EnergyScoreModel
        {
            public decimal TotalEnergy { get; set; }
            public string InverterName { get; set; }
            public int OrderNum { get; set; }
            public decimal TotalKwp { get; set; }

        }
        public class ScoreTableModel
        {
            public decimal TotalEnergy { get; set; }
            public string InverterName { get; set; }
            public int OrderNum { get; set; }
            public decimal TotalKwp { get; set; }
            public decimal TotalTemp { get; set; }

        }
        public class EnergyTemperatureModel
        {
            public List<TemperatureScoreModel> TemperatureScores { get; set; }
            public List<EnergyScoreModel> EnergyScores { get; set; }
            public bool isShow { get; set; }
        }
        private class GetCashFlowBoxChartModel
        {
            public Guid MasterCategoryId { get; set; }
            public double Amount { get; set; }
            public string MasterCategoryName { get; set; }
            public string DetailCategoryName { get; set; }
            public CashFlowType CashFlowType { get; set; }
        }


        #endregion

    }
}
