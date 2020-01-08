
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using RoyalUKInsurance.Renewal.Data.Models;

namespace RoyalUKInsurance.Renewal.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<Customer> GetCustomers(string path)
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
