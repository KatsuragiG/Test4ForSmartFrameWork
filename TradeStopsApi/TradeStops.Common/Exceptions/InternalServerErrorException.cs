using System;

namespace TradeStops.Common.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
