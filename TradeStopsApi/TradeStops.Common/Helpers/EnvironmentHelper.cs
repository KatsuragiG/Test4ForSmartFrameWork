using System;
using System.Collections;
using System.Reflection;
using TradeStops.Common.Attributes;

namespace TradeStops.Common.Helpers
{
    public static class EnvironmentHelper
    {
        public static void SetEnvironmentValues<T>(T obj, IDictionary envValues)
            where T : class
        {
            var properties = obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(EnvironmentPropertyAttribute)) as EnvironmentPropertyAttribute;

                if (attribute != null)
                {
                    if (!envValues.Contains(attribute.EnvPropertyName))
                    {
                        continue;
                    }

                    var envValue = envValues[attribute.EnvPropertyName];

                    property.SetValue(obj, Convert.ChangeType(envValue, property.PropertyType), index: null);
                }
            }
        }
    }
}
