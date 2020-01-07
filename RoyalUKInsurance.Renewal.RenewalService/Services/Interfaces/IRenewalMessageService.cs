using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.RenewalServices.Services.Interfaces
{
    public interface IRenewalMessageService
    {
        string GenerateRenewalMessage(string inputPath, string outputPath, string templatePath);
    }
}
