using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RoyalUKInsurance.Renewal.RenewalServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoyalUKInsurance.Renewal.AppUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Readonly members

        private readonly string templatePath;
        private readonly ILogger<MainWindow> _logger;
        private readonly IRenewalMessageService _renewalMessageService;
        #endregion
        #region Readonly Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="renewalMessageService">Main service</param>
        /// <param name="logger">Logger</param>
        public MainWindow(IRenewalMessageService renewalMessageService, ILogger<MainWindow> logger)
        {
            InitializeComponent();
            txtOutputPath.Text = $"{GetApplicationRoot()}\\ImportFiles"; ;
            txtInputPath.Text = $"{GetApplicationRoot()}\\Data"; ;
            templatePath = $"{GetApplicationRoot()}\\Data\\tt.txt";
            _logger = logger;
            _renewalMessageService = renewalMessageService;
        }
        #endregion
        #region Readonly Events
        /// <summary>
        /// Fetch file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblResult.Content = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = $"{GetApplicationRoot()}\\Data";
                openFileDialog.FileName = "Customer.csv";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                _logger.LogInformation(txtInputPath.Text);
                if (openFileDialog.ShowDialog() == true)
                {
                    txtInputPath.Text = openFileDialog.FileName;
                    _logger.LogInformation(txtInputPath.Text);
                    btnSubmit.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Submit button
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e"> Exception</param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result =  _renewalMessageService.GenerateRenewalMessage(txtInputPath.Text, txtOutputPath.Text, templatePath);
                lblResult.Content = result;
                _logger.LogInformation($"Files created : {result}");
                btnSubmit.IsEnabled = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                lblResult.Content = "Unexpected error occurened";
            }
        }
        #endregion
        #region PrivateMethods

        /// <summary>
        /// Get Application root folder path
        /// </summary>
        /// <returns>path in a form of string</returns>
        public string GetApplicationRoot()
        {
            var exePath = System.IO.Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
        #endregion
    }
}
