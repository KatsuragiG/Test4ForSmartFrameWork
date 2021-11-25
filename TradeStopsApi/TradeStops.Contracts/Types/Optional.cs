using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts.Types
{
    /// <summary>
    /// Class is used in cases when you want to wrap any other class to indicate it's optional field in contract.
    /// It works almost like Nullable(T) type but for all classes, not only for value-types.
    /// Example of usage: when user sends data to TS API to edit (patch) some entity,
    /// we need to understand if a value was set to null explicitly or it was just not initialized.
    /// Check OptionalWrapperExtensions.WasSet() and ObjectExtensions.Wrap() methods also.
    /// </summary>
    /// <typeparam name="T">Any reference type, nullable type, or value type</typeparam>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Documentation is unnecessary here")]
    public class Optional<T> // : IComparable, IEquatable<Optional<T>>
    {
        public Optional()
        {
        }

        public Optional(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Wrapped value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Creates a new Wrapper from generic T type.
        /// Note: if you need to assign null value to a property, cast this null to the nullable type.
        /// Example: editPositionContract.PurchaseDate = (DateTime?)null;
        /// </summary>
        /// <param name="value">value of T type to convert to Wrapper(T) type</param>
        [SuppressMessage("Microsoft.Usage", "CA2225: Operator overloads have named alternates", Justification = "Haven't found any use-cases for required alternate method.")]
        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        ////public bool Equals(Optional<T> other)
        ////{
        ////    if (other == null)
        ////    {
        ////        return false;
        ////    }

        ////    return EqualityComparer<T>.Default.Equals(Value, other.Value);
        ////}

        ////public override bool Equals(object obj)
        ////{
        ////    var other = obj as Optional<T>;

        ////    return Equals(other);
        ////}

        ////public override int GetHashCode()
        ////{
        ////    return Value.GetHashCode();
        ////}
    }
}