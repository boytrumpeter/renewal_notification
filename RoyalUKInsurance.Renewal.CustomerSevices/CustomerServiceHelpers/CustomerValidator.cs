﻿using RoyalUKInsurance.Renewal.Data.Models;
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
        public bool ValidateCustomerForNull(Customer customer)
        {
            if (customer == null)
                return false;
            return true;

        }
        /// <summary>
        /// Validate customer name
        /// </summary>
        /// <param name="customer">Customer</param>
        public bool IsValidated(Customer customer)
        {
            if(ValidateCustomerForNull(customer))
                return !(string.IsNullOrEmpty(customer.FirstName) && string.IsNullOrEmpty(customer.Surname));
            else
                return false;
        }
        #endregion
    }
}
