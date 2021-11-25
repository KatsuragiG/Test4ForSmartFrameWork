using System;

namespace TradeStops.Common.Extensions
{
    // https://stackoverflow.com/questions/609501/generating-a-random-decimal-in-c-sharp
    public static class RandomExtensions
    {
        /// <summary>
        /// Generate random decimal value in range [0..1]
        /// </summary>
        public static decimal NextDecimal(this Random rng)
        {
            return new decimal(rng.NextDouble());

            ////return new decimal(
            ////    rng.NextInt32(),
            ////    rng.NextInt32(),
            ////    rng.Next(0x204FCE5E),
            ////    false,
            ////    0);
        }

        /// <summary>
        /// Generate random decimal value in range [min..max]
        /// </summary>
        public static decimal NextDecimal(this Random rng, decimal minValue, decimal maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException($"minValue={minValue} can't be bigger than maxValue={maxValue}.");
            }

            var randomValue = NextDecimal(rng);

            var rangeDiff = maxValue - minValue;

            return randomValue * rangeDiff + minValue;

            ////return new decimal(
            ////    rng.NextInt32(),
            ////    rng.NextInt32(),
            ////    rng.Next(0x204FCE5E),
            ////    false,
            ////    0);
        }

        public static T NextEnum<T>(this Random rng)
            where T : struct
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type.Name} is not Enum.");
            }

            Array values = Enum.GetValues(type);
            var index = rng.Next(values.Length);
            return (T)values.GetValue(index);
        }

        public static bool NextBool(this Random rng)
        {
            var value = rng.Next(2);

            return value == 1;
        }

        public static DateTime NextDate(this Random rng, int minYear, int maxYear)
        {
            var year = rng.Next(minYear, maxYear);
            var month = rng.Next(1, 12);
            var day = rng.Next(1, 28);

            return new DateTime(year, month, day);
        }
    }
}
