using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EnvironmentPropertyAttribute : Attribute
    {
        public EnvironmentPropertyAttribute(string envPropertyName)
        {
            EnvPropertyName = envPropertyName;
        }

        public string EnvPropertyName { get; private set; }
    }
}
