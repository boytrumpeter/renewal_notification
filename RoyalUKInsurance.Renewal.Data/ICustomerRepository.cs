using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoyalUKInsurance.Renewal.Data
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers(string path);
    }
}
