using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("RoyalUKInsurance.Renewal.Tests")]
namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers
{
    internal class CustomerValidator
    {
        //private readonly Customer _customer;

        //#region Constructor
        //public CustomerValidator(Customer customer)
        //{
        //    _customer = customer;
        //}
        //#endregion
        #region Methods
        /// <summary>
        /// Verify the customer object is not null.
        /// </summary>
        /// <param name="customer">Customer data object</param>
        public void ValidateCustomerForNull(Customer customer)
        {
            if (customer == null)
                throw new NullReferenceException("Customer reference is null");
        }
        #endregion
    }
}
