using System;
using System.Collections.Generic;
using System.Text;

namespace RoyalUKInsurance.Renewal.Data.Models
{
    /// <summary>
    /// Customer Domain Class. We sould place this in another class library 
    /// </summary>
    public class Customer
    {
        #region Constructor
        public Customer()
        {
            SetCreditChargeRate(5);
        }
        #endregion
        #region Properties
        public int ID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ProductName { get; set; }
        public decimal PayoutAmount { get; set; }
        public decimal AnnualPremium { get; set; }
        public int CreditChargeRate { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Method to set the Credit charge rate, if other than defaul value which is 5
        /// </summary>
        /// <param name="rate">Credit charge rate (default is 5)</param>
        public void SetCreditChargeRate(int rate)
        {
            CreditChargeRate = rate;
        }
        #endregion
    }
}
