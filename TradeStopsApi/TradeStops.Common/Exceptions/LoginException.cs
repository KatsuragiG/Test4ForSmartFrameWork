using System;

namespace TradeStops.Common.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(Error error)
            : base(error.Message)
        {
            ErrorCode = error.Code;
        }

        public int ErrorCode { get; set; }
    }
}
