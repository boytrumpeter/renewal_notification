using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoyalUKInsurance.Renewal.CustomerSevices.Models
{
    /// <summary>
    /// Customer Model enables to carry required data to generate renewal letter
    /// </summary>
    public class CustomerModel
    {
        #region Readonly Members
        public readonly Customer Customer;
        public readonly decimal CreditCharge;
        public readonly decimal InitialPayment;
        public readonly decimal OtherMonthlyPayment;
        public readonly decimal TotalPremium;
        #endregion
        #region Constructor
        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="customer">Customer Domain object</param>
        /// <param name="creditCharge">Credit Charge Amount</param>
        /// <param name="initialPayment">First installment payment</param>
        /// <param name="otherMonthlyPayment">Installment payments for the rest of the months in an year</param>
        /// <param name="totalPremium">Total Annual Premium</param>
        public CustomerModel(Customer customer, decimal creditCharge, decimal initialPayment, decimal otherMonthlyPayment, decimal totalPremium)
        {
            Customer = customer;
            CreditCharge = creditCharge;
            InitialPayment = initialPayment;
            OtherMonthlyPayment = otherMonthlyPayment;
            TotalPremium = totalPremium;
        }
        #endregion
    }
}
