using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Common.DTO
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ApiResult CreateSuccessfulResult(string message = "") =>
            new ApiResult { Success = true, Message = message};
        public static ApiResult CreateFailedResult(string message) =>
            new ApiResult { Success = false, Message = message };
    }

    public class ApiResult<T> : ApiResult
    {
        public T Result { get; set; }

        public static ApiResult<T> CreateSuccessfulResult(T result, string message = "") =>
            new ApiResult<T> { Success = true, Message = message, Result = result };

        public static new ApiResult<T> CreateFailedResult(string message) =>
            new ApiResult<T> { Success = false, Message = message, Result = default };
    }
}
