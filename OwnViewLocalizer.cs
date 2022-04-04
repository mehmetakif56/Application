using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace DBE.ENERGY.Web.Localization
{
    public class OwnViewLocalizer : IViewLocalizer, IViewContextAware
    {
        private readonly IHtmlLocalizerFactory _localizerFactory;
        private readonly string _applicationName;
        private IHtmlLocalizer _localizer;

        public OwnViewLocalizer(IHtmlLocalizerFactory localizerFactory)
        {
            _applicationName = "DBE.ENERGY.Resources";
            _localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));
        }

        public LocalizedHtmlString this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                return _localizer[key];
            }
        }

        public LocalizedHtmlString this[string key, params object[] arguments]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                return _localizer[key, arguments];
            }
        }

        public void Contextualize(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            // Given a view path "/Views/Home/Index.cshtml" we want a baseName like "MyApplication.Views.Home.Index"
            var path = viewContext.ExecutingFilePath;

            if (string.IsNullOrEmpty(path))
            {
                path = viewContext.View.Path;
            }

            Debug.Assert(!string.IsNullOrEmpty(path), "Couldn't determine a path for the view");

            _localizer = _localizerFactory.Create(BuildBaseName(path), _applicationName);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _localizer.GetAllStrings(includeParentCultures);

        public LocalizedString GetString(string name) => _localizer.GetString(name);

        public LocalizedString GetString(string name, params object[] values) => _localizer.GetString(name, values);

        public IHtmlLocalizer WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);

        private string BuildBaseName(string path)
        {
            var extension = Path.GetExtension(path);
            var startIndex = path[0] == '/' || path[0] == '\\' ? 1 : 0;
            var length = path.Length - startIndex - extension.Length;
            var capacity = length + _applicationName.Length + 1;
            var builder = new StringBuilder(path, startIndex, length, capacity);

            builder.Replace('/', '.').Replace('\\', '.');

            // Prepend the application name
            builder.Insert(0, '.');
            builder.Insert(0, _applicationName);

            return builder.ToString();
        }
    }
}