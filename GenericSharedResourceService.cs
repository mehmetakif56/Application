using DBE.ENERGY.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DBE.ENERGY.Web.Localization
{
    /// <summary>
    /// Create a localizer object for all shared resources 
    /// </summary>
    public class GenericSharedResourceService
    {
        public List<IStringLocalizer> _sharedLocalizers { get; set; } = new List<IStringLocalizer>();

        public string this[string key]
        {
            get
            {
                foreach (IStringLocalizer localizer in _sharedLocalizers)
                {
                    if (localizer == null)
                        continue;

                    if (key == null || localizer.GetString(key).ResourceNotFound)
                        continue;

                    return localizer[key];
                }
                return key;
            }
        }

        public GenericSharedResourceService(IStringLocalizerFactory factory)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                 .Where(a => a.FullName.StartsWith("DBE.ENERGY.Resources")).FirstOrDefault();

            var assembly = Assembly.Load(assemblyName);

            var resources = assembly.GetTypes().Where(t => typeof(IShared).IsAssignableFrom(t) && !t.IsInterface);

            foreach (var resource in resources)
            {
                var resourceAssemblyName = new AssemblyName(resource.GetTypeInfo().Assembly.FullName);
                _sharedLocalizers.Add(factory.Create(resource.GetTypeInfo().Name, resourceAssemblyName.Name));
            }
        }
    }

    /// <summary>
    /// Create a localizer object for specified shared resource 
    /// </summary>
    /// <typeparam name="T">Type of shared resource</typeparam>
    public class GenericSharedResourceService<T> where T : class
    {
        private readonly IStringLocalizer _localizer;

        public string this[string key]
        {
            get
            {
                if (_localizer != null && _localizer.GetString(key).ResourceNotFound)
                {
                    return key;
                }
                else
                {
                    return _localizer[key];
                }
            }
        }

        public GenericSharedResourceService(IStringLocalizerFactory factory)
        {
            var type = typeof(T);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type.GetTypeInfo().Name, assemblyName.Name);
        }
    }
}