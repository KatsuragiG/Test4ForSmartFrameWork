using System;
using System.Linq;
using System.Reflection;

namespace TradeStops.Contracts.Mappers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a contract")]
    internal static class PropertiesMapper
    {
        /// <summary>
        /// Fill all properties of destination object with values from matching properties of source object.
        /// ! Note: All properties must be simple types, like int, decimal, enum, string.
        /// </summary>
        /// <param name="from">Source object with data</param>
        /// <param name="to">Destination object that you want to patch</param>
        /// <param name="throwForMissingPropertiesInSourceObject">Throw exception if matching property from destination object is not found in source object</param>
        /// <param name="useDefaultValueWhenMappingNullToNonNullable">
        /// Allow to map null value from source object to default value when destination property is not nullable. 
        /// Example: source property "bool? UseIntraday = null", destination property "bool UseIntraday" will be set to default(bool) if parameter set to true, or exception will be thrown if parameter set to false
        /// </param>
        public static void Map(object from, object to, bool throwForMissingPropertiesInSourceObject, bool useDefaultValueWhenMappingNullToNonNullable)
        {
            var toProperties = to.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite);
            var fromPropertiesDictionary = from.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead).ToDictionary(x => x.Name);

            foreach (var toProperty in toProperties)
            {
                if (!fromPropertiesDictionary.ContainsKey(toProperty.Name))
                {
                    if (throwForMissingPropertiesInSourceObject)
                    {
                        throw new InvalidCastException($"Property {to.GetType().Name}.{toProperty.Name} doesn't have matching property in type {from.GetType().Name}");
                    }

                    continue;
                }

                var fromProperty = fromPropertiesDictionary[toProperty.Name];
                var fromValue = fromProperty.GetValue(from);

                // Convert.ChangeType here for case when we map decimal to int (trigger ThresholdValue property).
                // Ideally we must have a contract with more specific name: for example, int NumberOfPeriods instead of int ThresholdValue
                try
                {
                    toProperty.SetValue(to, ChangeType(fromValue, toProperty.PropertyType, useDefaultValueWhenMappingNullToNonNullable));
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException($"{toProperty.Name} property can't contain {fromValue} value", ex);
                }
            }
        }

        // we need this method to correctly convert nullable types to non-nullable
        // https://stackoverflow.com/questions/18015425/invalid-cast-from-system-int32-to-system-nullable1system-int32-mscorlib
        private static object ChangeType(object value, Type conversion, bool useDefaultValueWhenMappingNullToNonNullable)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }
            else // if destination type is non-nullable
            {
                if (value == null && useDefaultValueWhenMappingNullToNonNullable)
                {
                    return GetDefault(t);
                }
            }

            return Convert.ChangeType(value, t);
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        //// Commented code may be useful in future.
        //// Method uses BaseTriggerMapper and BaseTriggerStateMapper to create instance of trigger
        //// and works with complex contracts (recursive) and with lists.
        //// /// <summary>
        //// /// Fill all properties of destination object with values from matching properties of source object.
        //// /// If property type is class (except string type), then it will be mapped using the same strategy
        //// /// </summary>
        //// /// <param name="from">source object with data</param>
        //// /// <param name="to">destination object that you want to patch</param>
        //// /// <param name="throwForMissingPropertiesInSourceObject">throw exception if matching property from destination object is not found in source object</param>
        //// public static void AutoMapForTriggers(object from, object to, bool throwForMissingPropertiesInSourceObject)
        //// {
        ////     if (from == null)
        ////     {
        ////         return;
        ////     }

        ////     var fromType = from.GetType();
        ////     var toType = to.GetType();

        ////     if (IsList(fromType))
        ////     {
        ////         var resultListGenericType = toType.GetGenericArguments().Single();

        ////         // code is commented because created list already passed as parameter
        ////         //var resultListType = typeof(List<>).MakeGenericType(resultListGenericType);
        ////         //var resultList = (IList)Activator.CreateInstance(resultListType);
        ////         var resultList = (IList)to;

        ////         var sourceList = (from as IEnumerable<object>).Cast<object>().ToList();
        ////         foreach (var sourceListItem in sourceList)
        ////         {
        ////             var resultListItem = CreateInstance(resultListGenericType, sourceListItem);
        ////             AutoMapForTriggers(sourceListItem, resultListItem, throwForMissingPropertiesInSourceObject);
        ////             resultList.Add(resultListItem);
        ////         }

        ////         return;
        ////     }

        ////     var toProperties = toType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite);
        ////     var fromPropertiesDictionary = fromType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead).ToDictionary(x => x.Name);

        ////     foreach (var toProperty in toProperties)
        ////     {
        ////         if (!fromPropertiesDictionary.ContainsKey(toProperty.Name))
        ////         {
        ////             if (throwForMissingPropertiesInSourceObject)
        ////             {
        ////                 throw new InvalidCastException($"Property {toType.Name}.{toProperty.Name} doesn't have matching property in type {fromType.Name}");
        ////             }

        ////             continue;
        ////         }

        ////         var fromProperty = fromPropertiesDictionary[toProperty.Name];
        ////         var fromValue = fromProperty.GetValue(from);

        ////         object toValue;

        ////         if (toProperty.PropertyType.IsClass && toProperty.PropertyType != typeof(string))
        ////         {
        ////             try
        ////             {
        ////                 toValue = CreateInstance(toProperty.PropertyType, fromValue);
        ////             }
        ////             catch (Exception ex)
        ////             {
        ////                 throw new InvalidCastException($"Can't create instance of {toProperty.PropertyType.Name}. See inner exception for details", ex);
        ////             }

        ////             AutoMapForTriggers(fromValue, toValue, throwForMissingPropertiesInSourceObject);
        ////         }
        ////         else
        ////         {
        ////             toValue = ChangeType(fromValue, toProperty.PropertyType);
        ////         }

        ////         // Convert.ChangeType here for case when we map decimal to int (trigger ThresholdValue property).
        ////         // Ideally we must have a contract with more specific name: for example, int NumberOfPeriods instead of int ThresholdValue
        ////         try
        ////         {
        ////             toProperty.SetValue(to, toValue);
        ////         }
        ////         catch (InvalidCastException ex)
        ////         {
        ////             throw new InvalidCastException($"{toProperty.Name} property can't contain {fromValue} value", ex);
        ////         }
        ////     }
        //// }

        //// private static object CreateInstance(Type destinationType, object sourceObject)
        //// {
        ////     object toValue;
        ////     if (destinationType.Name == nameof(BaseTriggerContract))
        ////     {
        ////         var triggerTypeValue = GetPropertyValue<TriggerTypes>(sourceObject, nameof(TriggerFieldsContract.TriggerType));

        ////         toValue = BaseTriggersMapper.CreateInstanceByTriggerType(triggerTypeValue);
        ////     }
        ////     else if (destinationType.Name == nameof(BaseTriggerStateContract))
        ////     {
        ////         var triggerTypeValue = GetPropertyValue<TriggerTypes>(
        ////             sourceObject,
        ////             nameof(TriggerStateFieldsContract.TriggerType));

        ////         toValue = BaseTriggersStateMapper.CreateInstanceByTriggerType(triggerTypeValue);
        ////     }
        ////     else
        ////     {
        ////         toValue = Activator.CreateInstance(destinationType);
        ////     }
        ////     return toValue;
        //// }
    }
}
