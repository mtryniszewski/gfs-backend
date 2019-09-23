using System;
using System.Collections.Generic;
using System.Text;
using GFS.Core.Enums;

namespace GFS.Transfer.Shared
{
    public class ErrorDto
    {
        public ErrorDto(ErrorCode errorCode, string errorName,string message)
        {
            ErrorCode = errorCode;
            ErrorName = errorName;
            Message = message;
        }
        public ErrorDto(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }
        public ErrorDto()
        {

        }
        public ErrorCode ErrorCode { get; set; }
        public string ErrorName { get; set; }
        public string Message { get; set; }
    }
}
