using System;
using TradeStops.Common.Extensions;

namespace TradeStops.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(int errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
        }

        public BadRequestException(Error error)
            : base(error.Message)
        {
            ErrorCode = error.Code;
        }

        /// <summary>
        /// Constructor that allows to append additional information to error message.
        /// </summary>
        /// <param name="error">Object with predefined Error values.</param>
        /// <param name="additionalInfo">Additional information to append to error message.</param>
        public BadRequestException(Error error, string additionalInfo)
            : base(GetMessage(error, additionalInfo))
        {
            ErrorCode = error.Code;
        }

        public int ErrorCode { get; set; }

        private static string GetMessage(Error error, string additionalInfo)
        {
            if (additionalInfo.IsNullOrEmpty())
            {
                return error.Message;
            }

            return $"{error.Message} {additionalInfo}";
        }
    }
}
