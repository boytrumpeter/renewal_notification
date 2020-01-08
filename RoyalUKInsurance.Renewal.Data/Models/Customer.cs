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
        #region Properties
        public int ID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ProductName { get; set; }
        public decimal PayoutAmount { get; set; }
        public decimal AnnualPremium { get; set; }
       
        #endregion
        
    }
}
