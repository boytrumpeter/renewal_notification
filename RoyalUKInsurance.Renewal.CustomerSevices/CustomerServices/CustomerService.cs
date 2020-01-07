using CsvHelper;
using Microsoft.Extensions.Logging;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        #region ReadonlyMembers
        private readonly ILogger<CustomerService> _logger;
        private readonly CustomerValidator _customerValidator;
        private readonly PaymentsCalculator _paymentsCalculator;
        private readonly RenewalMessageGenerator _renewalMessageGenerator;
        #endregion
        #region Constructors
        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public CustomerService(ILogger<CustomerService> logger) : this(new CustomerValidator(), new PaymentsCalculator(), new RenewalMessageGenerator()) { _logger = logger; }
        /// <summary>
        /// Internal Constructor (We can use factory pattern here )
        /// </summary>
        /// <param name="customerValidator"> Customer Validator</param>
        /// <param name="paymentsCalculator">Payments Calculator</param>
        /// <param name="renewalMessageGenerator">RenewalMessageGenerator</param>
        internal CustomerService(CustomerValidator customerValidator, PaymentsCalculator paymentsCalculator, RenewalMessageGenerator renewalMessageGenerator)
        {
            _customerValidator = customerValidator;
            _paymentsCalculator = paymentsCalculator;
            _renewalMessageGenerator = renewalMessageGenerator;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Message to generate message and store file
        /// </summary>
        /// <param name="inputPath">Input path</param>
        /// <param name="outputPath">output path</param>
        /// <param name="templatePath">template file path</param>
        /// <returns></returns>
     

        #endregion

        public string GenerateRenewalMessage(string inputPath, string outputPath, string templatePath)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                int success = 0;

                Object obj = new Object();
                IDictionary<int, string> results = new Dictionary<int, string>();

                using (var reader = new StreamReader(inputPath))
                using (var csvReader = new CsvReader(reader))
                {
                    var customers = csvReader.GetRecords<Customer>();

                    Parallel.ForEach(customers, (customer) =>
                    {
                        try
                        {
                            _customerValidator.ValidateCustomer(customer);
                            var customerModel = _paymentsCalculator.CalculatePayments(customer).Result;
                            if (customerModel != null)
                            {
                                var _outputPath = $"{outputPath}\\{customerModel.Customer.ID}_{customerModel.Customer.FirstName}.txt";
                                if (!File.Exists(_outputPath))
                                {
                                        if (_renewalMessageGenerator.CreateRenewalMessage(customerModel: customerModel, outputPath: _outputPath, templatePath: templatePath))
                                            success++;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Errorn in customer service: {e.Message}");
                        }
                    });
                }

                return $"{success} renewal letter generated...";
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return $"{e.Message}";
            }
        }
    }
}
