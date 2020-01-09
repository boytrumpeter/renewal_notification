using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace RoyalUKInsurance.Renewal.CustomerSevices.Models
{
    /// <summary>
    /// Customer Model enables to carry required data to generate renewal letter
    /// </summary>
    public class CustomerModel
    {
        #region Properties
        public decimal CreditCharge { get; private set; }
        public decimal InitialPayment { get { return GetMonthlypayments().Item1; } }
        public decimal OtherMonthlyPayment { get { return GetMonthlypayments().Item2; } }
        public decimal TotalPremium { get { return GetTotalPremium(); } }
        public int CreditChargeRate { get; private set; }
        public Customer Customer { get; }
        #endregion
        #region Constructors
        public CustomerModel(Customer customer)
        {
            Customer = customer;
            //Default Credit card charge
            SetCreditChargeRate(5);
        }
        public CustomerModel(Customer customer, int creditCardRate)
        {
            Customer = customer;
            //Default Credit card charge
            SetCreditChargeRate(creditCardRate);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Method to set the Credit charge rate, if other than defaul value which is 5
        /// </summary>
        /// <param name="rate">Credit charge rate (default is 5)</param>
        public void SetCreditChargeRate(int rate)
        {
            CreditChargeRate = rate;
            SetCreditCharge(rate);
        }
        /// <summary>
        /// Calculating Credit charge and setting CreditCharge.
        /// </summary>
        /// <param name="creditCharge">decimal</param>
        void SetCreditCharge(decimal creditCharge)
        {
            CreditCharge = Math.Round(Customer.AnnualPremium * CreditChargeRate / 100, 2); ;
        }
        /// <summary>
        /// Gets Total Premium
        /// </summary>
        /// <returns></returns>
        decimal GetTotalPremium()
        {
            return Math.Round(Customer.AnnualPremium + CreditCharge, 2);
        }
        /// <summary>
        /// Gets Monthly installments
        /// </summary>
        /// <returns>Tuple, First item in Tuple is Initial payment and the Second ine is all other payments</returns>
        Tuple<decimal, decimal> GetMonthlypayments()
        {
            var avgMonthlyPayment = Math.Round((TotalPremium / 12), 2);
            var monthlyPayment = TotalPremium - (11 * avgMonthlyPayment);
            if (monthlyPayment >= avgMonthlyPayment)
                return new Tuple<decimal, decimal>(monthlyPayment, avgMonthlyPayment);
            else
                return new Tuple<decimal, decimal>(avgMonthlyPayment, monthlyPayment);
        }
        #endregion
    }
}
