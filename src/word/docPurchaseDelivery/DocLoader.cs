using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

using TradeControl.Documents.Word;

namespace docPurchaseDelivery
{
    class DocLoader
    {
        ThisDocument document;

        public DocLoader()
        {
            document = Globals.ThisDocument;
        }

        /// <summary>
        /// Load Purchase Order controls from xml file data
        /// </summary>
        /// <returns>Successfully loaded Word document</returns>
        public bool LoadPurchaseDelivery()
        {
            try
            {
    
                xsTaskCode xmlTask = new xsTaskCode();
                xmlTask.ReadXml(Schema.TaskSingleXmlFileName);
                xsTaskCode.TaskRow taskRow = (xsTaskCode.TaskRow)xmlTask.Task.Rows[0];

                #region header
                WriteControl("ContactName", taskRow.ContactName);
                WriteControl("AccountName", taskRow.AccountName);
                WriteControl("InvoiceAddress", taskRow.IsInvoiceAddressNull() ? string.Empty : taskRow.InvoiceAddress);
                WriteControl("TaskCode", taskRow.TaskCode);
                WriteControl("SecondReference", taskRow.IsSecondReferenceNull() ? string.Empty : taskRow.SecondReference);
                WriteControl("TaskTitle", taskRow.IsTaskTitleNull() ? string.Empty : taskRow.TaskTitle);
                WriteControl("TaskNotes", taskRow.IsTaskNotesNull() ? string.Empty : taskRow.TaskNotes);
                WriteControl("CollectionAccountName", taskRow.IsCollectionAccountNameNull() ? string.Empty : taskRow.CollectionAccountName);
                WriteControl("CollectionAddress", taskRow.IsCollectionAddressNull() ? string.Empty : taskRow.CollectionAddress);
                WriteControl("DeliveryAccountName", taskRow.IsDeliveryAccountNameNull() ? string.Empty : taskRow.DeliveryAccountName);
                WriteControl("DeliveryAddress", taskRow.IsDeliveryAddressNull() ? string.Empty : taskRow.DeliveryAddress);
                WriteControl("Quantity", taskRow.Quantity.ToQuantity());
                WriteControl("UnitOfMeasure", taskRow.UnitOfMeasure);
                WriteControl("ActionOn", taskRow.ActionOn.ToString());
                WriteControl("TotalCharge", taskRow.TotalCharge.ToCurrency());
                WriteControl("TaxRate", taskRow.TaxRate.ToPercentage());
                WriteControl("PaymentTerms", taskRow.PaymentTerms);
                WriteControl("UserName", taskRow.UserName);
                WriteControl("CompanyName", taskRow.CompanyName);
                WriteControl("CompanyWebsite", taskRow.CompanyWebsite);
                #endregion

                return true;


            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        /// <summary>
        /// Check if control exists, and according to type, write string value to the content 
        /// </summary>
        /// <param name="_controlName"></param>
        /// <param name="_value"></param>
        private void WriteControl(string _controlName, string _value)
        {
            try
            {
                if (document.Controls.IndexOf(_controlName) >= 0)
                {
                    Type type = document.Controls[_controlName].GetType();

                    //{Name = "PlainTextContentControlImpl" FullName = "Microsoft.Office.Tools.Word.PlainTextContentControlImpl"}
                    //if (type == typeof(PlainTextContentControl))
                    if (type.Name == "PlainTextContentControlImpl")
                    {
                        PlainTextContentControl ptcc = (PlainTextContentControl)document.Controls[_controlName];
                        ptcc.Text = _value;
                    }
                    //else if (type == typeof(DatePickerContentControl))
                    else if (type.Name == "DatePickerContentControlImpl")
                    {
                        DatePickerContentControl dpcc = (DatePickerContentControl)document.Controls[_controlName];
                        dpcc.Text = _value;
                    }
                }
            }
            catch //(Exception err)
            {
                //tag deleted
                //MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteControl(string _controlName, byte[] _value)
        {
            try
            {
                if (document.Controls.IndexOf(_controlName) >= 0)
                {
                    Type type = document.Controls[_controlName].GetType();
                    //if (type == typeof(PictureContentControl))
                    if (type.Name == "PictureContentControlImpl")
                    {

                        PictureContentControl pcc = (PictureContentControl)document.Controls[_controlName];
                        Image img = Image.FromStream(new MemoryStream(_value));
                        pcc.Image = new Bitmap(img);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
