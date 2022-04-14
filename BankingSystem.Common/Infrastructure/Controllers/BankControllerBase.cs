using BankingSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Common.Infrastructure.Controllers
{
    public class BankControllerBase : ControllerBase
    {
        protected IActionResult ApiResponse(ApiResult result) => 
            result.Success ? Ok(result) : BadRequest(result);
    }
}
