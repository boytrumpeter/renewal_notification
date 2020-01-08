using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.Interfaces
{
    public interface ICustomerService
    {
        string BuildRenewalMessage(string inputPath, string outputPath, string templatePath);
    }
}
