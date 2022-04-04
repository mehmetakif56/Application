using DBE.ENERGY.Core.Interfaces;
using DBE.ENERGY.Core.Services;
using DBE.ENERGY.Web.Extensions;
using DBE.ENERGY.Web.ViewModels.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBE.ENERGY.Web.Controllers
{
    public class LoginController : BaseController<LoginController>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISessionHelper _sessionHelper;
        private readonly ILogger<FacilityFormController> _logger;
        private readonly IIntegrationService _integrationService;

        public LoginController(IUserService userService, IHttpContextAccessor contextAccessor, ISessionHelper sessionHelper, ILogger<FacilityFormController> logger, IIntegrationService integrationService)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _sessionHelper = sessionHelper;
            _logger = logger;
            _integrationService = integrationService;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            ViewBag.Message = "Web Portala erişim için lütfen giriş yapınız";
            return View();
        }

        // POST: LoginController
        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserPassword))
            {
                ViewBag.Message = "Lütfen kullanıcı adı veya şifreyi boş bırakmayınız!";
                return View();
            }

            var user = _userService.GetUserByMailAndPassword(model.UserName, model.UserPassword);
            if (user != null && user.Count() > 0)
            {
                ClaimsExtensions.CreateCookies(user.FirstOrDefault(), _userService, _contextAccessor, _sessionHelper);
                //UserSessionMiddleware.firsLoading = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Lütfen geçerli bir kullanıcı adı veya şifre ile login olmayı deneyiniz!";
            }

            return View();
        }



        public ActionResult ForgetPassword()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordViewModel model)
        {

            return View();
        }

        public IActionResult GetAllFacilityRTUData()
        {
            var stackedCharts = new List<ChartModel>();

            try
            {
                var lst = _integrationService.GetFacilityRTUForLogin().ToList();
                foreach (var item in lst)
                {
                    var stackedChart = new ChartModel();
                    stackedChart.RTU = item.RTUEnergy;
                    stackedChart.Month = item.RegisteredDate.Value.Month;
                    stackedChart.City = item.CityName;
                    stackedChart.Facility = item.FacilityName;
                    stackedChart.FacilityId = item.FacilityId;
                    stackedChart.CityId = item.CityId;
                    stackedChart.Year = item.RegisteredDate.Value.Year;
                    stackedCharts.Add(stackedChart);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorPath: /{"Login"}/{"GetAllFacilityRTUData"}, ErrorMessage: { ex.Message }, ErrorDetail: {(ex.StackTrace?.Length > 3000 ? ex.StackTrace?.Substring(0, 3000) : ex.StackTrace)} ");
                return Redirect("~/");
                //return CurrentUser == null ? BadRequest("Kullanıcı bilgileri getirelemedi. Lütfen tekrar login olmayı deneyiniz." + ex.Message) : BadRequest(ex.Message);
            }
            var groupedList = stackedCharts.GroupBy(x => new { x.FacilityId, x.CityId, x.City, x.Facility, x.Month, x.Year })
                                                    .Select(x => new ChartModel { RTU = x.Sum(c => c.RTU), FacilityId = x.Key.FacilityId, Facility = x.Key.Facility, City = x.Key.City, Month = x.Key.Month, Year = x.Key.Year })
                                                    .OrderBy(x => x.Month);
            return Json(groupedList);

        }

        public class ChartModel
        {
            public decimal RTU { get; set; }
            public string Facility { get; set; }
            public Guid? FacilityId { get; set; }
            public string City { get; set; }
            public Guid? CityId { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
        }
    }
}

