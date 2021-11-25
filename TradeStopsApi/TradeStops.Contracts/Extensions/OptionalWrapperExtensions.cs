using TradeStops.Contracts.Types;

namespace TradeStops.Contracts.Extensions
{
    /// <summary>
    /// Extension methods for 'Optional' wrapper class to simplify code and maintainability
    /// </summary>
    public static class OptionalWrapperExtensions
    {
        // todo: add OptionalWrapperUtils.UpdateIfWasSet(ref T target, Optional<T> source) { if (source.WasSet) target = source.Value }

        /// <summary>
        /// This extension method is added as alternative to explicit null check in code.
        /// We always easily can inline all usages of this method using Resharper (if necessary).
        /// Consider to use better name like 'IsAssigned', 'HasValue', etc. 'WasSet' name was chosen because we already used this name in previous 'BasePatchContract' approach
        /// </summary>
        /// <typeparam name="T">generic type of wrapped value</typeparam>
        /// <param name="wrapper">instance of wrapper</param>
        /// <returns>True if instance of wrapper is not null, meaning that some explicit value (either null or not null) was wrapped</returns>
        public static bool WasSet<T>(this Optional<T> wrapper)
        {
            return wrapper != null;
        }

        /// <summary>
        /// Get wrapped value or default value for wrapped object type. NOTE: This method is not recommended to use. Always use .Value directly if possible
        /// </summary>
        /// <typeparam name="T">generic type of wrapped value</typeparam>
        /// <param name="wrapper">instance of wrapper</param>
        /// <returns>wrapped value or default value</returns>
        public static T GetValueOrDefault<T>(this Optional<T> wrapper)
        {
            return wrapper == null ? default(T) : wrapper.Value;
        }
    }
}
