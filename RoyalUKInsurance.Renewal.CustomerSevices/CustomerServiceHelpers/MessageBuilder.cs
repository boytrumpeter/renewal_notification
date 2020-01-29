using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers
{
    internal class MessageBuilder
    {
        const string err = "error";
        /// <summary>
        /// Internal method to Create a message/letter from template
        /// </summary>
        /// <param name="customerModel">CustomerModel</param>
        /// <param name="outputPath">outputPath</param>
        /// <param name="templatePath">templatePath</param>
        /// <returns></returns>
        internal bool BuildMessage(CustomerModel customerModel, string outputPath, string templateData)
        {
            try
            {
                //var templateData = Utilities.ReadFile(templatePath);
                var message = SearchAndReplace(customerModel, templateData).Result;
                if (message == err)
                    return false;
                File.WriteAllText(outputPath, message, Encoding.GetEncoding("iso-8859-1"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #region PrivateMethods
        /// <summary>
        /// Search and replace the values in the temlate for individual customer
        /// </summary>
        /// <param name="customerModel"></param>
        /// <param name="content"></param>
        /// <returns>Task<string> contents are replaced with data</returns>
        Task<string> SearchAndReplace(CustomerModel customerModel, string content)
        {
            try
            {
                var task = Task.Run(() =>
                {
                    Regex re = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
                    var replacements = new Dictionary<string, string>()
                    {
                        ["date"] = $"{DateTime.Now.ToString("dd/MM/yyyy")}",
                        ["title"] = customerModel.Customer.Title,
                        ["name"] = customerModel.Customer.FirstName,
                        ["surname"] = customerModel.Customer.Surname,
                        ["ProductName"] = customerModel.Customer.ProductName,
                        ["PayoutAmount"] = $"{customerModel.Customer.PayoutAmount:N}",
                        ["AnnualPremium"] = $"{customerModel.Customer.AnnualPremium:N}",
                        ["CreditCharge"] = $"{customerModel.CreditCharge:N}",
                        ["TotalAnnualPremium"] = $"{customerModel.TotalPremium:N}",
                        ["InitialMonthlyPaymentAmount"] = $"{customerModel.InitialPayment:N}",
                        ["OtherMonthlyPaymentsAmounteach"] = $"{customerModel.OtherMonthlyPayment:N}",
                    };
                    return re.Replace(content, match => { return replacements.ContainsKey(match.Groups[1].Value) ? replacements[match.Groups[1].Value] : match.Value; });
                });
                task.Wait();
                return task;
            }
            catch (Exception e)
            {
                return Task.Run(()=>err);
            }

        }
        #endregion
    }
}
