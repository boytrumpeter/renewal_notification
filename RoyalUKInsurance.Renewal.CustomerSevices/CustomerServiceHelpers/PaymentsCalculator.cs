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
            var creditCharge = await CreditCharge(customer);
            var totalPremium = await TotalPremium(customer);
            var monthlyPayments = await MonthlyPayments(customer, totalPremium);
            return new CustomerModel(customer, CreditCharge(customer).Result, monthlyPayments.Item1, monthlyPayments.Item2, totalPremium);
        }
        #region PrivateMethods
        /// <summary>
        /// Calculate Credit Charge
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>CreditCharge</returns>
        Task<decimal> CreditCharge(Customer customer)
        {
            return Task.FromResult<decimal>(Math.Round(customer.AnnualPremium * customer.CreditChargeRate / 100, 2));
        }

        /// <summary>
        /// Calculate initial and other monthly payments
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="totalPremium">TotalPremium</param>
        /// <returns>Tuple first item is initial payment, second item is other monthly payments</returns>
        Task<Tuple<decimal, decimal>> MonthlyPayments(Customer customer, decimal totalPremium)
        {
            var avgMonthlyPayment = Math.Round((totalPremium / 12), 2);
            var monthlyPayment = totalPremium - (11 * avgMonthlyPayment);
            if (monthlyPayment >= avgMonthlyPayment)
                return Task.FromResult(new Tuple<decimal, decimal>(monthlyPayment, avgMonthlyPayment));
            else
                return Task.FromResult<Tuple<decimal, decimal>>(new Tuple<decimal, decimal>(avgMonthlyPayment, monthlyPayment));

        }
        /// <summary>
        /// Calculate Annual total premium
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Total annual premium as Task</returns>
        Task<decimal> TotalPremium(Customer customer)
        {
            return Task.FromResult<decimal>(Math.Round(customer.AnnualPremium + (customer.AnnualPremium * customer.CreditChargeRate / 100), 2));
        }
        #endregion

    }
}
