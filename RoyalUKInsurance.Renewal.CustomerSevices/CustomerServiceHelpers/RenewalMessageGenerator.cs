using RoyalUKInsurance.Renewal.CustomerSevices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers
{
    internal class RenewalMessageGenerator
    {
        /// <summary>
        /// Internal method to Create a message from template
        /// </summary>
        /// <param name="customerModel">CustomerModel</param>
        /// <param name="outputPath">outputPath</param>
        /// <param name="templatePath">templatePath</param>
        /// <returns></returns>
        internal bool CreateRenewalMessage(CustomerModel customerModel, string outputPath, string templatePath)
        {
            try
            {
                    var templateData = ReadTemplate(templatePath);
                    var completedTemplate = SearchAndReplace(customerModel, templateData);
                    var IsWritten = WriteCompletedTemplate(completedTemplate, outputPath);
                return IsWritten;
                
            }
            catch (Exception)
            {
                return false;

            }

        }
        #region PrivateMethods
        /// <summary>
        /// Read the contents in letter template
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns>Task<string> of contents in template</returns>
        string ReadTemplate(string templatePath)
        {
            using (StreamReader reader = new StreamReader(templatePath, Encoding.GetEncoding("iso-8859-1")))
            {
                var task = Task.Run(async () =>
                {
                    return await reader.ReadToEndAsync();
                });
                task.Wait();
                return task.Result;
            }
        }
        /// <summary>
        /// Searech and replaces the values in the temlate for individual customer
        /// </summary>
        /// <param name="customerModel"></param>
        /// <param name="content"></param>
        /// <returns>Task<string> contents are replaced with data</returns>
        string SearchAndReplace(CustomerModel customerModel, string content)
        {
            try
            {
                var task = Task.Run(async () =>
                {
                    Regex re = new Regex(@"\$(\w+)\$", RegexOptions.Compiled);
                    var replacements = new Dictionary<string, string>()
                    {
                        ["date"] = $"{DateTime.Now.ToString("dd/MM/yyyy")}",
                        ["title"] = customerModel.Customer.Title,
                        ["name"] = customerModel.Customer.FirstName,
                        ["surname"] = customerModel.Customer.Surname,
                        ["ProductName"] = customerModel.Customer.ProductName,
                        ["PayoutAmount"] = customerModel.Customer.PayoutAmount.ToString(),
                        ["AnnualPremium"] = customerModel.Customer.AnnualPremium.ToString(),
                        ["CreditCharge"] = customerModel.CreditCharge.ToString(),
                        ["TotalAnnualPremium"] = customerModel.TotalPremium.ToString(),
                        ["InitialMonthlyPaymentAmount"] = customerModel.InitialPayment.ToString(),
                        ["OtherMonthlyPaymentsAmounteach"] = customerModel.OtherMonthlyPayment.ToString(),
                    };
                    return re.Replace(content, match => { return replacements.ContainsKey(match.Groups[1].Value) ? replacements[match.Groups[1].Value] : match.Value; });
                });
                task.Wait();
                return task.Result;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }
        /// <summary>
        /// Method to write the completed file to destination path.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="outputPath"></param>
        /// <returns>Bool</returns>
        bool WriteCompletedTemplate(string content, string outputPath)
        {
            try
            {
                using (StreamWriter wr = new StreamWriter(outputPath, true, Encoding.GetEncoding("iso-8859-1")))
                {
                    var task = Task.Run(async () => { await wr.WriteAsync(content); });
                    task.Wait();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
           
            
        }

        #endregion
    }
}
