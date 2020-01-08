using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RoyalUKInsurance.Renewal.CustomerSevices.CustomerServiceHelpers
{
    public class Utilities
    {
        /// <summary>
        /// Stream read file 
        /// </summary>
        /// <param name="path">string</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1")))
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
        /// write contents to file
        /// </summary>
        /// <param name="content">message as string </param>
        /// <param name="path">file location path to store message</param>
        public static void WriteToFile(string content, string path)
        {
                using (StreamWriter wr = new StreamWriter(path, true, Encoding.GetEncoding("iso-8859-1")))
                {
                    var task = Task.Run(async () => { await wr.WriteAsync(content); });
                    task.Wait();
                }
        }
    }
}
