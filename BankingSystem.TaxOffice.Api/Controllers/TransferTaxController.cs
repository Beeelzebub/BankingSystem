using BankingSystem.Common.Infrastructure.Controllers;
using BankingSystem.TaxOffice.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.TaxOffice.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class TransferTaxController : BankControllerBase
    {
        private ITransferTaxService _service;

        public TransferTaxController(ITransferTaxService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransferTaxes()
        {
            var result = await _service.GetTransferTaxes();

            return ApiResponse(result);
        }

        [HttpGet("{transferAmount}")]
        public async Task<IActionResult> GetTransferTaxes(float transferAmount)
        {
            var result = await _service.GetTransferTax(transferAmount);

            return ApiResponse(result);
        }
    }
}
