using CsvHelper;
using Microsoft.Extensions.Logging;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using RoyalUKInsurance.Renewal.Data;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerValidator _customerValidator;
        private readonly MessageBuilder _renewalMessageGenerator;
        #endregion
        #region Constructors
        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger) : this(new CustomerValidator(), new MessageBuilder())
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Internal Constructor (We can use factory pattern here )
        /// </summary>
        /// <param name="customerValidator"> Customer Validator</param>
        /// <param name="paymentsCalculator">Payments Calculator</param>
        /// <param name="renewalMessageGenerator">RenewalMessageGenerator</param>
        internal CustomerService(CustomerValidator customerValidator, MessageBuilder renewalMessageGenerator)
        {
            _customerValidator = customerValidator;
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
        public string BuildRenewalMessage(string inputPath, string outputPath, string templatePath)
        {

            int success = 0;
            int unsuccess = 0;
            //Fetching customer records
            var customers = _customerRepository.GetCustomers(inputPath);
            //Generating letter for each customer
            Parallel.ForEach(customers, (customer) =>
            {
                try
                {
                    if (_customerValidator.IsValidated(customer))
                    {
                        _logger.LogInformation($"{customer.FirstName} {customer.Surname} Customer validated.");

                        var customerModel = new CustomerModel(customer);
                        var _outputPath = $"{outputPath}\\{customerModel.Customer.ID}_{customerModel.Customer.FirstName}.txt";
                        if (!File.Exists(_outputPath))
                        {
                            //Creating message using template and storing. Return true if succeeded else false
                            if (_renewalMessageGenerator.BuildMessage(customerModel: customerModel, outputPath: _outputPath, templatePath: templatePath))
                            {
                                success++;
                                _logger.LogInformation($"{customer.FirstName} {customer.Surname} Customer message generated.");
                            }
                        }
                        else
                        {
                            unsuccess++;
                            _logger.LogInformation($"{customer.FirstName} {customer.Surname} Customer Files already exists.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Customer validation failed.");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error in customer service: {e.Message}");
                }
            });
            if (unsuccess == 0)
                return $"{success} renewal letter generated.";
            else
                return $"{success} renewal letter generated. {unsuccess} files already exists.";
        }
        #endregion
    }
}
