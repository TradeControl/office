using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace TradeControl.Tax.Office
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("TradeControl.Tax.Office")]
    [ComVisible(true)]
    public class OfficeMTD
    {
        const string regKeyTradeControl = @"Trade Control\Documents";
        const string regValCSVFolder = "CSVFolder";

        /// <summary>
        /// Sql Server connection string or datasource
        /// </summary>
        [ComVisible(true)]
        public string ConnectionString { get; set; }

        [ComVisible(true)]
        public bool ExportVatToCSV(DateTime startOn)
        {
            try
            {
                if (ConnectionString.Length == 0)
                    throw new Exception(Properties.Resources.ConnectionStringUnspecified);

                string csvFolder = CSVFolder;

                SaveFileDialog fileDialog = new SaveFileDialog()
                {
                    Title = Properties.Resources.OpenFileDialogTitle,
                    InitialDirectory = csvFolder,
                    Filter = "CSV Files|*.csv",
                    CheckFileExists = false,
                    CheckPathExists = true,
                    DefaultExt = "csv"
                };

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                    CSVFolder = fileInfo.DirectoryName;
                    return GenerateCSV(fileDialog.FileName, startOn);
                }
                else
                    return false;
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool GenerateCSV(string fileName, DateTime startOn)
        {
            try
            {
                string FormatString(string s) =>  s.IndexOf(',') > 0 ? '"' +  s + '"' : s;

                dbTradeControlDataContext dbTradeControl = new dbTradeControlDataContext(ConnectionString);

                vwTaxVatTotal vatPeriod = dbTradeControl.vwTaxVatTotals.Where(period => startOn == period.StartOn).FirstOrDefault();

                if (vatPeriod == null)
                    throw new Exception(Properties.Resources.VatPeriodNotFound);

                using (StreamWriter stream = new StreamWriter(fileName, false, Encoding.UTF8, 512))
                {
                    const string comma = ",";
                    string line = string.Empty;
                    
                    line = FormatString(Properties.Resources.HomeSalesVat) + comma + FormatString(vatPeriod.HomeSalesVat.Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.ExportSalesVat) + comma + FormatString(vatPeriod.ExportSalesVat.Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.SalesVatDue) + comma +
                        FormatString((vatPeriod.HomeSalesVat + vatPeriod.ExportSalesVat).Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.HomePurchasesVat) + comma + FormatString(vatPeriod.HomePurchasesVat.Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.VatDue) + comma + FormatString(vatPeriod.VatDue.Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.TotalSales) + comma + 
                        FormatString((vatPeriod.HomeSales + vatPeriod.ExportSales).Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.TotalPurchases) + comma + 
                        FormatString((vatPeriod.HomePurchases + vatPeriod.ExportPurchases).Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.ExportSales) + comma + FormatString(vatPeriod.ExportSales.Value.ToString("C0"));
                    stream.WriteLine(line);
                    line = FormatString(Properties.Resources.ExportPurchases) + comma + FormatString(vatPeriod.ExportPurchases.Value.ToString("C0"));
                    stream.WriteLine(line);
                }

                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

}

        private string CSVFolder
        {
            get
            {
                try
                {
                    string csvFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Trade Control";

                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    if (rootKey == null)
                        rootKey = Registry.CurrentUser.CreateSubKey(@"Software\" + regKeyTradeControl, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    if (rootKey.GetValue(regValCSVFolder, null) != null)
                        csvFolder = rootKey.GetValue(regValCSVFolder).ToString();
                    else
                    {
                        rootKey.SetValue(regValCSVFolder, csvFolder, RegistryValueKind.String);
                        if (!Directory.Exists(csvFolder))
                            Directory.CreateDirectory(csvFolder);
                    }

                    return csvFolder;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName;
                }
            }
            set
            {
                RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                rootKey.SetValue(regValCSVFolder, value, RegistryValueKind.String);
            }
        }
    }
}
