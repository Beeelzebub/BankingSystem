using BankingSystem.Common.DTO;
using BankingSystem.Common.Infrastructure.Controllers;
using BankingSystem.JuridicalRegister.Api.Dto;
using BankingSystem.JuridicalRegister.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.JuridicalRegister.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class JuridicalRegisterController : BankControllerBase
    {
        private readonly IJuridicalRegisterService _service;

        public JuridicalRegisterController(IJuridicalRegisterService service)
        {
            _service = service;
        }

        [HttpGet("{bankAccountNumber}")]
        public async Task<IActionResult> GetLegalPesonPaymentInfo([FromRoute]string bankAccountNumber)
        {
            var result = await _service.GetLegalPersonPaymentInfo(bankAccountNumber);

            return ApiResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBankAccountNumber(CreateLegalPersonAccountDto paylod)
        {
            var result = await _service.AddBankAccountNumber(paylod.Organisation, paylod.BankAccountNumber);

            return ApiResponse(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLegalPesron(CreateLegalPesronDto legalPerson)
        {
            var result = await _service.CreateLegalPesron(legalPerson);

            return ApiResponse(result);
        }
    }
}
