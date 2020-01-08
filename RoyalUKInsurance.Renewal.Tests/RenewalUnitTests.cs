using CsvHelper;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServices;
using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using RoyalUKInsurance.Renewal.Data;
using RoyalUKInsurance.Renewal.Data.Models;
using RoyalUKInsurance.Renewal.RenewalServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoyalUKInsurance.Renewal.Tests
{
    [TestClass]
    public class RenewalUnitTests
    {
        string TemplatePath;
        string InputPath;
        string OutputPath;
        IList<Customer> Customers;

        public Mock<ICustomerRepository> MockCustomerRepository { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            InputPath = $"{GetApplicationRoot()}\\TestData\\CustomerTestData.csv";
            OutputPath = $"{GetApplicationRoot()}\\Imports";
            TemplatePath = $"{GetApplicationRoot()}\\TestData\\tt.txt";
            Customers = GetCustomerList();
            MockCustomerRepository = new Mock<ICustomerRepository>();
            MockCustomerRepository.Setup(a => a.GetCustomers(It.IsAny<string>()))
                .Returns(GetCustomers(InputPath));
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
            var result = new MessageBuilder().BuildMessage(customerModel, outputPath, TemplatePath);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CustomerService_MessageGenerator_Test()
        {
            
            var customerService = new CustomerService(MockCustomerRepository.Object,new NullLogger<CustomerService>());
            var result = customerService.BuildRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void RenewalService_MessageGenerator_Test()
        {
            var renewalService = new RenewalMessageService(new CustomerService(MockCustomerRepository.Object, new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void FetchWrongCSVTest()
        {
            InputPath = $"{GetApplicationRoot()}\\TestData\\wrong.csv";
            MockCustomerRepository.Setup(a => a.GetCustomers(InputPath))
                .Returns(GetCustomers(InputPath));
            var renewalService = new RenewalMessageService(new CustomerService(MockCustomerRepository.Object, new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("0 renewal letter generated...", result);
        }
        [TestMethod]
        public void FetchCSVWithMissingData_Test()
        {
            InputPath = $"{GetApplicationRoot()}\\TestData\\CustomerWrongTestData.csv";
            MockCustomerRepository.Setup(a => a.GetCustomers(It.IsAny<string>()))
                .Returns(GetCustomers(InputPath));
            var renewalService = new RenewalMessageService(new CustomerService(MockCustomerRepository.Object, new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void BulkData_Test()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\BulkData.csv";
            MockCustomerRepository.Setup(a => a.GetCustomers(It.IsAny<string>()))
                .Returns(GetCustomers(InputPath));
            var renewalService = new RenewalMessageService(new CustomerService(MockCustomerRepository.Object,new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestMethod]
        public void CustomerNameNullTest()
        {
            InputPath = $"{Directory.GetCurrentDirectory()}\\TestData\\CustomerNameNull.csv";
            MockCustomerRepository.Setup(a => a.GetCustomers(It.IsAny<string>()))
                .Returns(GetCustomers(InputPath));
            var renewalService = new RenewalMessageService(new CustomerService(MockCustomerRepository.Object,new NullLogger<CustomerService>()), new NullLogger<RenewalMessageService>());
            var result = renewalService.GenerateRenewalMessage(InputPath, OutputPath, TemplatePath);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        /// <summary>
        /// Get application root path
        /// </summary>
        /// <returns></returns>
        string GetApplicationRoot()
        {
            var exePath = System.IO.Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }

        /// <summary>
        /// Get customers
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IList<Customer> GetCustomers(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csvReader = new CsvReader(reader))
            {
                //Fetching customer records
                var customers = csvReader.GetRecords<Customer>();
                return customers.ToList();
            }
        }
    }
}
