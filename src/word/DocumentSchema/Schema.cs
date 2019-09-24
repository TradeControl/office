using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace TradeControl.Documents.Word
{
    public sealed class Schema
    {
        const string regKeyTradeControl = @"Trade Control\Documents";
        const string regValXmlFolder = "XmlFolder";
        const string xmlFileExtension = ".xml";

        #region XML File Details
        /// <summary>
        /// Gets the folder location of .dot associated Xml documents
        /// </summary>
        public static string XmlFolder
        {
            get
            {
                try
                {
                    string xmlFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + '\\' + regKeyTradeControl;

                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    if (rootKey == null)
                        rootKey = Registry.CurrentUser.CreateSubKey(@"Software\" + regKeyTradeControl, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    if (rootKey.GetValue(regValXmlFolder, null) != null)
                        xmlFolder = rootKey.GetValue(regValXmlFolder).ToString();
                    else
                    {
                        rootKey.SetValue(regValXmlFolder, xmlFolder, RegistryValueKind.String);
                        if (!Directory.Exists(xmlFolder))
                            Directory.CreateDirectory(xmlFolder);
                    }

                    return xmlFolder;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }


        public static string XmlFileExtension
        {
            get
            {
                return xmlFileExtension;
            }
        }

        public static string InvoiceTaskXmlFileName
        {
            get
            {
                return XmlFolder + '\\' + Properties.Settings.Default.InvoiceTaskXml + XmlFileExtension;
            }
        }

        public static string InvoiceActivityXmlFileName
        {
            get
            {
                return XmlFolder + '\\' + Properties.Settings.Default.InvoiceActivityXml + XmlFileExtension;
            }
        }

        public static string TaskSingleXmlFileName
        {
            get
            {
                return XmlFolder + '\\' + Properties.Settings.Default.TaskSingleXml + XmlFileExtension;
            }
        }

        public static string TaskMultipleXmlFileName
        {
            get
            {
                return XmlFolder + '\\' + Properties.Settings.Default.TaskMultipleXml + XmlFileExtension;
            }
        }
        #endregion

    }
}
