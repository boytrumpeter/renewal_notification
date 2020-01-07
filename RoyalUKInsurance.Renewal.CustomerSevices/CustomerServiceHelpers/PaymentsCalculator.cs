using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using RoyalUKInsurance.Renewal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers
{
    internal class PaymentsCalculator
    {
        /// <summary>
        /// Methos to calculate customer payments
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>CustomerModel</returns>
        public async Task<CustomerModel> CalculatePayments(Customer customer)
        {
            var creditCharge =  CreditCharge(customer);
            var totalPremium =  TotalPremium(customer);
            var monthlyPayments =  MonthlyPayments(customer, totalPremium);
            return new CustomerModel(customer, CreditCharge(customer), monthlyPayments.Item1, monthlyPayments.Item2, totalPremium);
        }
        #region PrivateMethods
        /// <summary>
        /// Calculate Credit Charge
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>CreditCharge</returns>
        decimal CreditCharge(Customer customer)
        {
            return Math.Round(customer.AnnualPremium * customer.CreditChargeRate / 100, 2);
        }

        /// <summary>
        /// Calculate initial and other monthly payments
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="totalPremium">TotalPremium</param>
        /// <returns>Tuple first item is initial payment, second item is other monthly payments</returns>
        Tuple<decimal, decimal> MonthlyPayments(Customer customer, decimal totalPremium)
        {
            var avgMonthlyPayment = Math.Round((totalPremium / 12), 2);
            var monthlyPayment = totalPremium - (11 * avgMonthlyPayment);
            if (monthlyPayment >= avgMonthlyPayment)
                return new Tuple<decimal, decimal>(monthlyPayment, avgMonthlyPayment);
            else
                return new Tuple<decimal, decimal>(avgMonthlyPayment, monthlyPayment);

        }
        /// <summary>
        /// Calculate Annual total premium
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Total annual premium as Task</returns>
        decimal TotalPremium(Customer customer)
        {
            return Math.Round(customer.AnnualPremium + (customer.AnnualPremium * customer.CreditChargeRate / 100), 2);
        }
        #endregion

    }
}
