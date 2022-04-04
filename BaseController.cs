using AutoMapper;
using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Interfaces;
using DBE.ENERGY.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DBE.ENERGY.Web.Controllers
{
    public class BaseController<T> : Controller where T : BaseController<T>
    {
        private IMapper _mapper;
        private ILogger<T> _logger;
        private IConfiguration _conf;
        private GenericSharedResourceService _localizer;
        private UserEntity _currentUser;

        protected IConfiguration Configuration => _conf ?? (_conf = HttpContext.RequestServices.GetService<IConfiguration>());
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
        protected ILogger<T> Logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<T>>());

        protected UserEntity CurrentUser => _currentUser ?? (_currentUser = HttpContext.RequestServices.GetService<ISessionHelper>().User);
        protected GenericSharedResourceService L => _localizer ??
                   (_localizer = HttpContext.RequestServices.GetService<GenericSharedResourceService>());
        protected string BasePath => Configuration.GetValue<string>("BasePath");

    }
}
