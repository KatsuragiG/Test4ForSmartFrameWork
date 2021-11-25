using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Common.Helpers
{
    /// <summary>
    /// This helper can be used to validate parameters that passed as input to avoid processing parameters that are wrong.
    /// It has the same purpose as Debug.Assert, but works for any build configuration.
    /// It is also has similar purpose as Code Contracts tool.
    /// Use it when you want to fail the code when wrong parameter is passed, so issue can be investigated and fixed ASAP.
    /// Code must not rely on these validations, so they can be removed at any time without breaking any logic.
    /// </summary>
    public static class AssertHelper
    {
        public static void NotEqual(int first, int second)
        {
            if (first == second)
            {
                ThrowException($"Values must be different, but they are both equal {first}.");
            }
        }

        public static void True(bool conditionThatMustBeTrue, string additionalMessage = "")
        {
            if (!conditionThatMustBeTrue)
            {
                ThrowException("Provided argument is not valid. " + additionalMessage);
            }
        }

        public static void False(bool conditionThatMustBeFalse, string additionalMessage = "")
        {
            if (!conditionThatMustBeFalse)
            {
                ThrowException("Provided argument is not valid. " + additionalMessage);
            }
        }

        public static void NotNull(object obj, string parameterName, string additionalMessage = "")
        {
            if (obj == null)
            {
                ThrowException($"Null is not valid for {parameterName}. " + additionalMessage);
            }
        }

        [SuppressMessage("Performance", "CA1820:Test for empty strings using string length", Justification = "Code is used to avoid passing string.Empty parameters in method when it's not valid")]
        public static void NotStringEmpty(string stringThatCanBeNullButMustNotBeEmpty, string parameterName)
        {
            if (stringThatCanBeNullButMustNotBeEmpty == string.Empty)
            {
                ThrowException($"Empty string is not valid value for {parameterName}");
            }
        }

        private static void ThrowException(string message)
        {
            Debug.Assert(false, message);
            throw new ArgumentException(message);
        }
    }
}
