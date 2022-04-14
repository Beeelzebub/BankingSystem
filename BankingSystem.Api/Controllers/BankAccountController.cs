using BankingSystem.Api.Services.Contracts;
using BankingSystem.Common.DTO;
using BankingSystem.Common.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BankAccountController : BankControllerBase
    {
        private readonly IBankAccountService _service;

        public BankAccountController(IBankAccountService service)
        {
            _service = service;
        }

        [HttpPost("MoneyTransfer")]
        public async Task<IActionResult> MoneyTransfer(string senderAccountNumber, string receiverAccountNumber, float transferAmount)
        {
            var result = await _service.MakeTransaction(senderAccountNumber, receiverAccountNumber, transferAmount);

            return ApiResponse(result);
        }

        [HttpPost("CreateLegalPesonAccount")]
        public async Task<IActionResult> CreateLegalPesonAccount(CreateLegalPersonAccountDto payload)
        {
            var result = await _service.CreateBankAccountAsync(payload);

            return ApiResponse(result);
        }

        [HttpPost("CreateNaturalPersonAccount")]
        public async Task<IActionResult> CreateNaturalPersonAccount(CreateNaturalPersonAccountDto payload)
        {
            var result = await _service.CreateBankAccountAsync(payload);

            return ApiResponse(result);
        }

        [HttpPost("CreateUtilityServiceAccount")]
        public async Task<IActionResult> CreateLegalPesonAccount(CreateUtilityServiceAccountDto payload)
        {
            var result = await _service.CreateBankAccountAsync(payload);

            return ApiResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBankAccounts()
        {
            var result = await _service.GetBankAccountsAsync();

            return ApiResponse(result);
        }

        [HttpGet("BankAccount/{owner}")]
        public async Task<IActionResult> GetBankAccounts(string owner)
        {
            var result = await _service.GetBankAccountsByUserAsync(owner);

            return ApiResponse(result);
        }

        [HttpGet("Transacions/{bankAccountNumber}")]
        public async Task<IActionResult> GetTransacions(string bankAccountNumber)
        {
            var result = await _service.GetBankAccountTransacionAsync(bankAccountNumber);

            return ApiResponse(result);
        }
    }
}
