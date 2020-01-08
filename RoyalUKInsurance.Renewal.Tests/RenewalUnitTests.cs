using CsvHelper;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServices;
using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using RoyalUKInsurance.Renewal.Data.Models;
using RoyalUKInsurance.Renewal.RenewalServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoyalUKInsurance.Renewal.Tests
{
    [TestClass]
    public class RenewalUnitTests
    {
        string TemplatePath;
        string InputPath;
        string OutputPath;
        IList<Customer> Customers;
        [TestInitialize]
        public void Initialize()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\CustomerTestData.csv";
            OutputPath = $"{Directory.GetCurrentDirectory()}\\TestData";
            TemplatePath = $"{Directory.GetCurrentDirectory()}\\TestData\\tt.txt";
            Customers = GetCustomerList();
        }
        IList<Customer> GetCustomerList()
        {
            using (var reader = new StreamReader(InputPath))
            using (var csvReader = new CsvReader(reader))
            {
                return csvReader.GetRecords<Customer>().ToList();
            }
        }

        [TestMethod]
        public void CustomerValidator_NullReferenceException_Test()
        {
            var validator = new CustomerValidator();
            Assert.IsFalse(validator.IsValidated(null));
        }

        [TestMethod]
        public void RenewalMessageGenerator_Test()
        {
            var customer = Customers.FirstOrDefault();
            var customerModel = new CustomerModel(customer);
            var outputPath = $"{OutputPath}\\{customerModel.Customer.FirstName}_{customerModel.Customer.ID}.txt";
            var result = new RenewalMessageGenerator().CreateRenewalMessage(customerModel, outputPath, TemplatePath);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CustomerService_MessageGenerator_Test()
        {
            var customerService = new CustomerService(new NullLogger<CustomerService>());
            var result = customerService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void RenewalService_MessageGenerator_Test()
        {
            var renewalService = new RenewalMessageService(new CustomerService(new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void FetchWrongCSVTest()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\wrong.csv";
            var renewalService = new RenewalMessageService(new CustomerService(new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void FetchCSVWithMissingData_Test()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\CustomerWrongTestData.csv";
            var renewalService = new RenewalMessageService(new CustomerService(new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void BulkData_Test()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\BulkData.csv";
            var renewalService = new RenewalMessageService(new CustomerService(new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void CustomerNameNullTest()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\CustomerNameNull.csv";
            var renewalService = new RenewalMessageService(new CustomerService(new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
    }
}
