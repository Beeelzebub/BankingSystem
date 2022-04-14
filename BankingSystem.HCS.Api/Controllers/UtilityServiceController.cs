using BankingSystem.Common.DTO;
using BankingSystem.Common.Infrastructure.Controllers;
using BankingSystem.UtilityService.Api.Data;
using BankingSystem.UtilityService.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.UtilityService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UtilityServiceController : BankControllerBase
    {
        private readonly UtilityServiceContext _dbContext;

        public UtilityServiceController(UtilityServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyAccrual()
        {
            var payments = await _dbContext.UtilityServicePayment.ToListAsync();

            Random random = new Random();

            foreach (var payment in payments)
            {
                payment.Amount += random.Next(30, 80);
            }

            await _dbContext.SaveChangesAsync();

            return ApiResponse(ApiResult.CreateSuccessfulResult());
        }

        [HttpPost]
        public async Task<IActionResult> PayUtilityService(PayUtilityServiceDto payload)
        {
            var payment = await _dbContext.UtilityServicePayment.FirstOrDefaultAsync(p => p.BankAccountNumber == payload.BankAccountNumber);

            if (payment == null)
            {
                return ApiResponse(ApiResult<float>.CreateFailedResult("Incorrect bank account number"));
            }

            var overPay = payment.Amount < payload.Amount
                ? payload.Amount - payment.Amount
                : 0;

            payment.Amount -= payload.Amount - overPay;

            await _dbContext.SaveChangesAsync();

            return ApiResponse(ApiResult<float>.CreateSuccessfulResult(overPay));
        }

        [HttpPost("{bankAccountNumber}")]
        public async Task<IActionResult> CreateUtilityServicePayment([FromRoute]string bankAccountNumber)
        {
            var payment = await _dbContext.UtilityServicePayment.FirstOrDefaultAsync(p => p.BankAccountNumber == bankAccountNumber);

            if (payment != null)
            {
                return ApiResponse(ApiResult.CreateFailedResult("Bank account number already exists"));
            }

            _dbContext.UtilityServicePayment.Add(new UtilityServicePayment { Amount = 0, BankAccountNumber = bankAccountNumber });

            await _dbContext.SaveChangesAsync();

            return ApiResponse(ApiResult.CreateSuccessfulResult());
        }
    }
}
