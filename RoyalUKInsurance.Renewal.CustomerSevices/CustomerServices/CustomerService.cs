using CsvHelper;
using Microsoft.Extensions.Logging;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly CustomerValidator _customerValidator;
        private readonly PaymentsCalculator _paymentsCalculator;
        private readonly RenewalMessageGenerator _renewalMessageGenerator;

        public CustomerService(ILogger<CustomerService> logger) : this(new CustomerValidator(), new PaymentsCalculator(), new RenewalMessageGenerator()) { _logger = logger; }
        internal CustomerService(CustomerValidator customerValidator, PaymentsCalculator paymentsCalculator, RenewalMessageGenerator renewalMessageGenerator)
        {
            _customerValidator = customerValidator;
            _paymentsCalculator = paymentsCalculator;
            _renewalMessageGenerator = renewalMessageGenerator;
        }

        public Task<string> GenerateRenewalMessage(string inputPath, string outputPath, string templatePath)
        {
            IDictionary<int, string> results = new Dictionary<int, string>();
            int success = 0;
            using (var reader = new StreamReader(inputPath))
            using (var csvReader = new CsvReader(reader))
            {
                var customers = csvReader.GetRecords<Customer>();
               /* Parallel.ForEach(customers, (customer) =>
                 {
                     try
                     {
                         _customerValidator.ValidateCustomerForNull(customer);
                         var customerModel = _paymentsCalculator.CalculatePayments(customer).Result;
                         if (customerModel == null)
                             return;
                         var _outputPath = $"{outputPath}\\{customerModel.Customer.ID}_{customerModel.Customer.FirstName}.txt";
                         if (File.Exists(_outputPath))
                             return;
                         if (_renewalMessageGenerator.CreateRenewalMessage(customerModel: customerModel, outputPath: _outputPath, templatePath: templatePath).Result)
                             success++;
                     }
                     catch (Exception e)
                     {
                         _logger.LogError($"Errorn in customer service: {e.Message}");
                     }
                 });
                */
                foreach (var customer in customers)
                {
                    try
                    {
                        _customerValidator.ValidateCustomerForNull(customer);
                        var customerModel = _paymentsCalculator.CalculatePayments(customer).Result;
                        if (customerModel == null)
                            continue;
                        var _outputPath = $"{outputPath}\\{customerModel.Customer.ID}_{customerModel.Customer.FirstName}.txt";
                        if (File.Exists(_outputPath))
                            continue;
                        if (_renewalMessageGenerator.CreateRenewalMessage(customerModel: customerModel, outputPath: _outputPath, templatePath: templatePath))
                            success++;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Errorn in customer service: {e.Message}");
                    }
                }
            }
            return Task.FromResult($"{success} renewal letter generated...");
        }

    }
}
