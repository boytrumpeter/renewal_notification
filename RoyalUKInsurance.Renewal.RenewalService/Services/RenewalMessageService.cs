using Microsoft.Extensions.Logging;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.RenewalServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.RenewalServices.Services
{
    public class RenewalMessageService: IRenewalMessageService
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<RenewalMessageService> _logger;

        public RenewalMessageService(ICustomerService customerService, ILogger<RenewalMessageService> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }
        public async Task<string> GenerateRenewalMessage(string inputPath, string outputPath, string templatePath)
        {
            try
            {
                return await _customerService.GenerateRenewalMessage(inputPath, outputPath, templatePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

        }
    }
}
