using Microsoft.Extensions.Logging;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.RenewalServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.RenewalServices.Services
{
    /// <summary>
    /// Main service which can be used from any from end.
    /// </summary>
    public class RenewalMessageService: IRenewalMessageService
    {
        #region Readonlymembers
        private readonly ICustomerService _customerService;
        private readonly ILogger<RenewalMessageService> _logger;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerService">Application service</param>
        /// <param name="logger">Logger</param>
        public RenewalMessageService(ICustomerService customerService, ILogger<RenewalMessageService> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }
        #endregion
        /// <summary>
        /// Implementation
        /// </summary>
        /// <param name="inputPath">Inputpath</param>
        /// <param name="outputPath">putputpath</param>
        /// <param name="templatePath">template path</param>
        /// <returns></returns>
        public string GenerateRenewalMessage(string inputPath, string outputPath, string templatePath)
        {
            try
            {
                return _customerService.BuildRenewalMessage(inputPath, outputPath, templatePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occured in process, {e.Message}");
                return $"Error occured in process, please see errors in logs.";
            }

        }
    }
}
