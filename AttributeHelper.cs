using DBE.ENERGY.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DBE.ENERGY.Core.Extensions
{
    public static class AttributeHelper
    {
        private const string nameSpace = "DBE.ENERGY.Core.Entities";
        private static Type objType;
        private static object CallObjectInstance(string className)
        {
            objType = Type.GetType(nameSpace + "." + className);
            return Activator.CreateInstance(objType);
        }

        public static Dictionary<string, string> GetAllEntityAttribute()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var entitylist = asm.GetTypes().Where(x => x.Namespace == nameSpace && x.CustomAttributes.Where(x => x.AttributeType.Name == "NotificationAttribute").Count() > 0).Select(x => x.Name).ToList();
            var _dict = new Dictionary<string, string>();

            foreach ( string classname in entitylist)
            {
                var customAttributes = CallObjectInstance(classname).GetType().GetCustomAttributes(typeof(NotificationAttribute), true);
                if (customAttributes.Length > 0)
                {
                    _dict.Add(classname, ((NotificationAttribute)customAttributes[0]).NotifyProperty);
                }
            }
            return _dict;
         }

        private static object CreateInstance(string strName)
        {
            Type type = Type.GetType(strName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        public static Dictionary<string, string> GetPropertyAttributeByEntity(string classname,string attrname= "NotificationAttribute")
        {
            var prop = CallObjectInstance(classname).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(x => x.CustomAttributes.Where(x => x.AttributeType.Name == attrname).Count() > 0);
            var _dict = new Dictionary<string, string>();
            foreach (var item in prop)
            {
                if (prop != null)
                {
                    _dict.Add(item.Name, item.CustomAttributes.FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString());
                }
            }
            return _dict;
        }
    }
 }
