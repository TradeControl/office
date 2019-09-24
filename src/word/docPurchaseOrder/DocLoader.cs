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

namespace docPurchaseOrder
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
        public bool LoadPurchaseOrder()
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
                WriteControl("NickName", taskRow.NickName);
                WriteControl("TaskTitle", taskRow.IsTaskTitleNull() ? string.Empty : taskRow.TaskTitle);
                WriteControl("DeliveryAddress", taskRow.IsDeliveryAddressNull() ? string.Empty : taskRow.DeliveryAddress);
                WriteControl("Quantity", taskRow.Quantity.ToQuantity());
                WriteControl("UnitOfMeasure", taskRow.UnitOfMeasure);
                WriteControl("ActionOn", taskRow.ActionOn.ToString());
                WriteControl("TotalCharge", taskRow.TotalCharge.ToCurrency());
                WriteControl("TaxRate", taskRow.TaxRate.ToPercentage());
                WriteControl("PaymentTerms", taskRow.PaymentTerms);
                WriteControl("TaskNotes", taskRow.IsTaskNotesNull() ? string.Empty : taskRow.TaskNotes);
                WriteControl("UserName", taskRow.UserName);
                WriteControl("CompanyName", taskRow.CompanyName);
                WriteControl("PaymentTerms", taskRow.PaymentTerms);
                WriteControl("DeliveryAddress", taskRow.DeliveryAddress);
                #endregion

                #region Attributes
                Word.Table attributeTable = null;
                Word.Row attributeRow = null;
                object attribObj = null;
                int[] idxAttrib = new int[] {1, 2};

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
                                    case "a1":
                                        attributeTable = table;
                                        attribObj = (object)row;
                                        attributeRow = row;
                                        idxAttrib[0] = cell.ColumnIndex;
                                        break;
                                    case "a2":
                                        idxAttrib[1] = cell.ColumnIndex;
                                        break;
                                }

                            }
                        }
                        if (attributeRow != null)
                            break;
                    }
                    if (attributeRow != null)
                        break;
                }

                if (attributeRow != null)
                {
                    var attributes = from tb in xmlTask.Attributes
                                     select tb;

                    foreach (xsTaskCode.AttributesRow row in attributes)
                    {
                        if (((AttributeType)row.AttributeTypeCode == AttributeType.Order)
                           || ((AttributeType)row.AttributeTypeCode == AttributeType.Quote && (TaskStatus)taskRow.TaskStatusCode == TaskStatus.Pending))

                        {
                            Word.Row newRow = attributeTable.Rows.Add(ref attribObj);
                            newRow.Cells[idxAttrib[0]].Range.Text = row.Attribute;
                            newRow.Cells[idxAttrib[1]].Range.Text = row.AttributeDescription;
                        }

                    }

                    attributeRow.Delete();
                }
                #endregion

                #region Schedule
                Word.Table scheduleTable = null;
                Word.Row scheduleRow = null;
                object scheduleObj = null;
                int[] idxSched = new int[] { 1, 2, 3 };

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
                                    case "s1":
                                        scheduleTable = table;
                                        scheduleObj = (object)row;
                                        scheduleRow = row;
                                        idxSched[0] = cell.ColumnIndex;
                                        break;
                                    case "s2":
                                        idxSched[1] = cell.ColumnIndex;
                                        break;
                                    case "s3":
                                        idxSched[2] = cell.ColumnIndex;
                                        break;
                                }

                            }
                        }
                        if (scheduleRow != null)
                            break;
                    }
                    if (scheduleRow != null)
                        break;
                }

                if (scheduleRow != null)
                {
                    var schedule = from tb in xmlTask.Schedule
                                     select tb;

                    foreach (xsTaskCode.ScheduleRow row in schedule)
                    {
                        Word.Row newRow = scheduleTable.Rows.Add(ref scheduleObj);
                        newRow.Cells[idxSched[0]].Range.Text = row.Operation;
                        newRow.Cells[idxSched[1]].Range.Text = row.EndOn.ToShortDateString();
                        newRow.Cells[idxSched[2]].Range.Text = row.IsNoteNull() ? string.Empty : row.Note;
                    }

                    scheduleRow.Delete();
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
