using System;
using GFS.Core.Enums;

namespace GFS.Core
{
    public class GfsException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public GfsException(ErrorCode code)
        {
            ErrorCode = code;
        }

        public GfsException(ErrorCode code, string message) : base(message)
        {
            ErrorCode = code;
        }

        public GfsException(ErrorCode code, string message, Exception innerException) : base(message,
            innerException)
        {
            ErrorCode = code;
        }
    }
}