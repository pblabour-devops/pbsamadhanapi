using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class GenericModelOps<T> where T : class
    {
        public static T SetNullAllNevigationProperties(T obj)
        {
            var allNavigationProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType.Namespace == typeof(T).Namespace && p.PropertyType.IsEnum == typeof(T).IsEnum)
                    .Select(p => p.Name)
                    .ToArray();
            if (allNavigationProperties.Length > 0)
            {
                Type type = typeof(T);
                foreach (var item in allNavigationProperties)
                {
                    PropertyInfo p = type.GetProperty(item.ToString());
                    p.SetValue(obj, null);
                }
            }
            return obj;
        }

        public static bool TrySetProperty(T obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
    }
    public static class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
    {
        public static void Copy(TParent parent, TChild child)
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(child, parentProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
    }


    public static class ReflectionOps
    {
        public static PropertyFinderRespViewModel GeValueByPropName(object obj, string propName)
        {
            PropertyFinderRespViewModel propertyFinderResp = new PropertyFinderRespViewModel();
            Type myType = obj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            if (props.Any(x => x.Name.ToLower() == propName.ToLower()))
            {
                propertyFinderResp.HasPropName = true;
                propertyFinderResp.PropValue = props.Where(x => x.Name.ToLower() == propName.ToLower()).FirstOrDefault().GetValue(obj, null).ToString();
            }
            else
            {
                propertyFinderResp.HasPropName = false;
                propertyFinderResp.PropValue = "";
            }
            return propertyFinderResp;
        }
        public static class ModelOps
        {
            public static List<PropertyTitleValuePair> GetTitleAndValues<T>(T obj) where T : class
            {
                List<PropertyTitleValuePair> propertyTitleValuePairs = new List<PropertyTitleValuePair>();
                var excludedProperties = new[] { "Id", "InspectionRefId" };

                PropertyInfo[] properties = obj.GetType().GetProperties().Where(p => !p.GetAccessors()[0].IsVirtual && !excludedProperties.Contains(p.Name)).ToArray();


                foreach (var item in properties)
                {
                    try
                    {
                        propertyTitleValuePairs.Add(new PropertyTitleValuePair()
                        {
                            Title = ((DisplayAttribute)(obj.GetType().GetProperty(item.Name).GetCustomAttributes(typeof(DisplayAttribute), true)[0])).Name,
                            Value = item.GetValue(obj).ToString(),
                            PropertyName = item.Name
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return propertyTitleValuePairs;

            }
        }
    }
}