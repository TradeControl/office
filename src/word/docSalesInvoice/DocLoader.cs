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

namespace docSalesInvoice
{
    class DocLoader
    {
        ThisDocument document;

        public DocLoader()
        {
            document = Globals.ThisDocument;
        }

        /// <summary>
        /// Load Acknowledgement controls from xml file data
        /// </summary>
        /// <returns>Successfully loaded Word document</returns>
        public bool LoadInvoice()
        {
            try
            {
                xsInvoiceTask xmlInvoice = new xsInvoiceTask();
                xmlInvoice.ReadXml(Schema.InvoiceTaskXmlFileName);

                xsInvoiceTask.InvoiceRow headerRow = (xsInvoiceTask.InvoiceRow)xmlInvoice.Invoice.Rows[0];

                WriteControl("AccountName", headerRow.AccountName);
                WriteControl("InvoiceAddress", headerRow.IsInvoiceAddressNull() ? string.Empty : headerRow.InvoiceAddress);
                WriteControl("InvoicedOn", headerRow.InvoicedOn.ToString());
                WriteControl("DueOn", headerRow.DueOn.ToString());
                WriteControl("InvoiceNumber", headerRow.InvoiceNumber);
                WriteControl("InvoiceType", headerRow.InvoiceType);
                WriteControl("Notes", headerRow.IsNotesNull() ? string.Empty : headerRow.Notes);
                WriteControl("PaymentTerms", headerRow.IsPaymentTermsNull() ? string.Empty : headerRow.PaymentTerms);
                WriteControl("TotalInvoiceValue", headerRow.TotalInvoiceValue.ToCurrency());
                WriteControl("TotalTaxValue", headerRow.TotalTaxValue.ToCurrency());
                WriteControl("InvoiceCharge", headerRow.InvoiceCharge.ToCurrency());
                WriteControl("CompanyName", headerRow.CompanyName);
                WriteControl("CompanyNumber", headerRow.CompanyNumber);
                WriteControl("CompanyVatNumber", headerRow.CompanyVatNumber);
                WriteControl("BankAccount", headerRow.BankAccount);
                WriteControl("BankAccountNumber", headerRow.BankAccountNumber);
                WriteControl("BankSortCode", headerRow.BankSortCode);

                #region Invoice Items
                Word.Table itemTable = null;
                Word.Row itemRow = null;
                object itemObj = null;
                int[] idxItem = new int[10];

                for (int i = 0; i < idxItem.Length; i++) idxItem[i] = -1;

                foreach (Word.Table table in document.Tables)
                {
                    foreach (Word.Row row in table.Rows)
                    {
                        foreach (Word.Cell cell in row.Range.Cells)
                        {
                            foreach (Word.ContentControl ctl in cell.Range.ContentControls)
                            {
                                switch (ctl.Tag)
                                {
                                    case "ItemDescription":
                                        itemTable = table;
                                        itemObj = (object)row;
                                        itemRow = row;
                                        idxItem[0] = cell.ColumnIndex;
                                        break;
                                    case "TaskCode":
                                        idxItem[1] = cell.ColumnIndex;
                                        break;
                                    case "ActionedOn":
                                        idxItem[2] = cell.ColumnIndex;
                                        break;
                                    case "Quantity":
                                        idxItem[3] = cell.ColumnIndex;
                                        break;
                                    case "UnitOfMeasure":
                                        idxItem[4] = cell.ColumnIndex;
                                        break;
                                    case "InvoiceValue":
                                        idxItem[5] = cell.ColumnIndex;
                                        break;
                                    case "TaxCode":
                                        idxItem[6] = cell.ColumnIndex;
                                        break;
                                    case "TaxValue":
                                        idxItem[7] = cell.ColumnIndex;
                                        break;
                                    case "ItemCharge":
                                        idxItem[8] = cell.ColumnIndex;
                                        break;
                                    case "SecondReference":
                                        idxItem[9] = cell.ColumnIndex;
                                        break;
                                }

                            }
                        }
                        if (itemRow != null)
                            break;
                    }
                    if (itemRow != null)
                        break;
                }

                if (itemRow != null)
                {
                    var items = from tb in xmlInvoice.InvoiceItem
                                orderby tb.TaskCode
                                select tb;

                    foreach (xsInvoiceTask.InvoiceItemRow row in items)
                    {
                        Word.Row newRow = itemTable.Rows.Add(ref itemObj);
                        if (idxItem[0] >= 0) newRow.Cells[idxItem[0]].Range.Text = row.ItemDescription;
                        if (idxItem[1] >= 0) newRow.Cells[idxItem[1]].Range.Text = row.TaskCode;
                        if (idxItem[2] >= 0) newRow.Cells[idxItem[2]].Range.Text = row.ActionedOn.ToShortDateString();
                        if (idxItem[3] >= 0) newRow.Cells[idxItem[3]].Range.Text = row.Quantity.ToQuantity();
                        if (idxItem[4] >= 0) newRow.Cells[idxItem[4]].Range.Text = row.Quantity.ToQuantity() + ' ' + row.UnitOfMeasure;
                        if (idxItem[5] >= 0) newRow.Cells[idxItem[5]].Range.Text = row.InvoiceValue.ToCurrency();
                        if (idxItem[6] >= 0) newRow.Cells[idxItem[6]].Range.Text = row.TaxCode;
                        if (idxItem[7] >= 0) newRow.Cells[idxItem[7]].Range.Text = row.TaxValue.ToCurrency();
                        if (idxItem[8] >= 0) newRow.Cells[idxItem[8]].Range.Text = row.ItemCharge.ToCurrency();
                        if (idxItem[9] >= 0) newRow.Cells[idxItem[9]].Range.Text = row.SecondReference;
                    }

                    itemRow.Delete();
                }
                #endregion

                #region Vat Summary
                Word.Table vatTable = null;
                Word.Row vatRow = null;
                object vatObj = null;
                int[] idxVat = new int[] { 0, 1, 2, 3 };

                foreach (Word.Table table in document.Tables)
                {
                    foreach (Word.Row row in table.Rows)
                    {
                        foreach (Word.Cell cell in row.Range.Cells)
                        {
                            foreach (Word.ContentControl ctl in cell.Range.ContentControls)
                            {
                                switch (ctl.Tag)
                                {
                                    case "VatTaxCode":
                                        vatTable = table;
                                        vatObj = (object)row;
                                        vatRow = row;
                                        idxVat[0] = cell.ColumnIndex;
                                        break;
                                    case "VatTaxRate":
                                        idxVat[1] = cell.ColumnIndex;
                                        break;
                                    case "InvoiceValueTotal":
                                        idxVat[2] = cell.ColumnIndex;
                                        break;
                                    case "TaxValueTotal":
                                        idxVat[3] = cell.ColumnIndex;
                                        break;
                                }

                            }
                        }
                        if (vatRow != null)
                            break;
                    }
                    if (vatRow != null)
                        break;
                }

                if (vatRow != null)
                {
                    var vatCodes = from tb in xmlInvoice.InvoiceVat
                                   orderby tb.VatTaxCode
                                   select tb;

                    foreach (xsInvoiceTask.InvoiceVatRow row in vatCodes)
                    {
                        Word.Row newRow = vatTable.Rows.Add(ref vatObj);
                        newRow.Cells[idxVat[0]].Range.Text = row.VatTaxCode;
                        newRow.Cells[idxVat[1]].Range.Text = row.VatTaxRate.ToPercentage();
                        newRow.Cells[idxVat[2]].Range.Text = row.InvoiceValueTotal.ToCurrency();
                        newRow.Cells[idxVat[3]].Range.Text = row.TaxValueTotal.ToCurrency();
                    }

                    vatRow.Delete();
                }
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
