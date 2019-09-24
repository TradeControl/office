using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

using Outlook = Microsoft.Office.Interop.Outlook;
using MicrosoftWord = Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace TradeControl.Documents.Word
{

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("TradeControl.Documents.Word")]
    [ComVisible(true)]
    public class WordDoc
    {

        string documentFileName = string.Empty;
        string emailAddress = string.Empty;
        string emailSubject = string.Empty;

        const string regKeyTradeControl = @"Trade Control\Documents";
        const string regValTemplateFolder = "TemplateFolder";
        const string regValRepository = "RepositoryFolder";
        const string regValXmlFolder = "XmlFolder";
        const string regValEmailFooter = "EmailFooter";

        /// <summary>
        /// Sql Server connection string or datasource
        /// </summary>
        [ComVisible(true)]
        public string ConnectionString { get; set; }
        

        #region Open Document
        /// <summary>
        /// Open the template as a word document using the supplied parameters
        /// </summary>
        /// <param name="_docType">Type of document to open</param>
        /// <param name="_fileName">Template file name without the windows path</param>
        /// <param name="_documentReference">Task code or invoice number</param>
        [ComVisible(true)]
        public bool OpenDocument(short _docType, string _fileName, string _documentReference)
        {
            try
            {
                if (LoadDocument(_docType, _fileName, _documentReference))
                {
                    MicrosoftWord.Application appWord = new MicrosoftWord.Application();

                    object template = _fileName;
                    object newTemplate = false;
                    object docType = MicrosoftWord.WdNewDocumentType.wdNewBlankDocument;
                    object visible = true;

                    MicrosoftWord.Document doc = appWord.Documents.Add(ref template, ref newTemplate, ref docType, ref visible);
                    doc.PrintPreview();
                    appWord.Visible = true;
                    appWord.Activate();
                                        
                    return true;
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


        /// <summary>
        /// Open the template as a word document using the supplied parameters
        /// </summary>
        /// <param name="_docType">Type of document to open</param>
        /// <param name="_fileName">Template file name without the windows path</param>
        /// <param name="_documentReference">Task code or invoice number</param>
        /// <returns>Successfully emailed</returns>
        [ComVisible(true)]
        public bool EmailDocument(short _docType, string _fileName, string _documentReference)
        {
            try
            {
                if (LoadDocument(_docType, _fileName, _documentReference))
                {
                    MicrosoftWord.Application appWord = new MicrosoftWord.Application();

                    Object template = _fileName;
                    Object newTemplate = false;
                    Object docType = MicrosoftWord.WdNewDocumentType.wdNewBlankDocument;
                    Object visible = true;

                    MicrosoftWord.Document doc = appWord.Documents.Add(ref template, ref newTemplate, ref docType, ref visible);

                    Object fileName = documentFileName + ".docx";
                    Object fileFormat = MicrosoftWord.WdSaveFormat.wdFormatDocumentDefault; //.wdFormatPDF;
                    Object lockComments = Type.Missing;
                    Object password = Type.Missing;
                    Object addToRecentFiles = Type.Missing;
                    Object writePassword = Type.Missing;
                    Object readOnlyRecommended = false;
                    Object embedTrueTypeFonts = Type.Missing;
                    Object saveNativePictureFormat = Type.Missing;
                    Object saveFormsData = false;
                    Object saveAsAOCELetter = Type.Missing;
                    Object encoding = Type.Missing;
                    Object insertLineBreaks = Type.Missing;
                    Object allowSubstitutions = Type.Missing;
                    Object lineEnding = Type.Missing;
                    Object addBiDiMarks = Type.Missing;

                    doc.SaveAs(ref fileName, ref fileFormat, ref lockComments, 
                      ref password, ref addToRecentFiles, ref writePassword, 
                      ref readOnlyRecommended, ref embedTrueTypeFonts, 
                      ref saveNativePictureFormat, ref saveFormsData, 
                      ref saveAsAOCELetter, ref encoding, ref insertLineBreaks, 
                      ref allowSubstitutions, ref lineEnding, ref addBiDiMarks);

                    Object saveChanges = MicrosoftWord.WdSaveOptions.wdDoNotSaveChanges;
                    Object originalFormat = Type.Missing;
                    Object routeDocument = Type.Missing;

                    Outlook.Application outlook = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)outlook.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = emailAddress;
                    mail.Subject = emailSubject;
                    mail.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                    mail.HTMLBody = EmailBody;
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    mail.Attachments.Add(fileName, Outlook.OlAttachmentType.olByValue, 1, emailSubject);

                    mail.Display(false);

                    doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                    appWord.Quit(ref saveChanges, ref originalFormat, ref routeDocument);

                    return true;
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

        /// <summary>
        /// Read the html information from the specified file name for active document type
        /// </summary>
        private string EmailBody
        {
            get
            {
                try
                {
                    string htmlBody = string.Empty;

                    string fileName = EmailFooterHtmlFile;
                    if (fileName.Length > 0)
                    {
                        if (File.Exists(fileName))
                        {
                            StreamReader reader = new StreamReader(fileName);
                            htmlBody = reader.ReadToEnd();
                        }
                    }

                    return htmlBody;

                }
                catch
                {
                    return string.Empty;
                }
            }

        }

        private bool LoadDocument(short _docType, string _fileName, string _documentReference)
        {
            try
            {
                FileInfo info = new FileInfo(_fileName);
                string templateName = info.Name.Substring(0, info.Name.Length - info.Extension.Length);
                TemplateFolder = info.Directory;

                DocType docType = (DocType)_docType;
                bool xmlLoaded = false;
                
                dbTradeControlDataContext context = new dbTradeControlDataContext(ConnectionString);
                tbDocType docTypeRow = (from tb in context.tbDocTypes
                                              where tb.DocTypeCode == _docType
                                              select tb).First();

                emailSubject = docTypeRow.DocType;

                switch (docType)
                {
                    case DocType.CreditNote:
                        xmlLoaded = LoadInvoice(context, templateName, _documentReference);
                        break;
                    case DocType.DebitNote:
                        xmlLoaded = LoadInvoice(context, templateName, _documentReference);
                        break;
                    case DocType.SalesInvoice:
                        xmlLoaded = LoadInvoice(context, templateName, _documentReference);
                        break;
                    case DocType.PurchaseEnquiry:
                        xmlLoaded = LoadTask(context, templateName, _documentReference, true);
                        break;
                    case DocType.PurchaseOrder:
                        xmlLoaded = LoadTask(context, templateName, _documentReference, false);
                        break;
                    case DocType.Quotation:
                        xmlLoaded = LoadTask(context, templateName, _documentReference, true);
                        break;
                    case DocType.SalesOrder:
                        xmlLoaded = LoadTask(context, templateName, _documentReference, false);
                        break;
                }

                return xmlLoaded;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        #endregion

        #region Open Template
        [ComVisible(true)]
        public bool OpenTemplate(string _fileName)
        {
            try
            {
                MicrosoftWord.Application appWord = new MicrosoftWord.Application();

                object template = _fileName;
                object confirmConversions = Type.Missing;
                object readOnly = Type.Missing;
                object addToRecentFiles = Type.Missing;
                object passwordDocument = Type.Missing;
                object passwordTemplate = Type.Missing;
                object revert = Type.Missing;
                object writePasswordDocument = Type.Missing;
                object writePasswordTemplate = Type.Missing;
                object format = Type.Missing;
                object encoding = Type.Missing;
                object visible = true;
                object openAndRepair = Type.Missing;
                object documentDirection = Type.Missing;
                object noEncodingDialog = Type.Missing;
                object xmlTransform = Type.Missing;


                MicrosoftWord.Document doc = appWord.Documents.Open(ref template,
                    ref confirmConversions,
                    ref readOnly,
                    ref addToRecentFiles,
                    ref passwordDocument,
                    ref passwordTemplate,
                    ref revert,
                    ref writePasswordDocument,
                    ref writePasswordTemplate,
                    ref format,
                    ref encoding,
                    ref visible,
                    ref openAndRepair,
                    ref documentDirection,
                    ref noEncodingDialog,
                    ref xmlTransform);

                appWord.Visible = true;

                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region File Locations

        [ComVisible(true)]
        public string GetTemplateFileName()
        {
            try
            {
                string filename = string.Empty;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = Properties.Resources.OpenTemplateFileDialogTitle,
                    InitialDirectory = TemplateFolder.ToString(),
                    Multiselect = false,
                    Filter = @"Word Template|*.dotx",
                    CheckFileExists = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    filename = openFileDialog.FileName;
                    FileInfo info = new FileInfo(filename);
                    TemplateFolder = info.Directory;
                }

                return filename;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }


        /// <summary>
        /// Gets or sets the default folder location of the Word .dot templates
        /// </summary>
        private DirectoryInfo TemplateFolder
        {
            get
            {
                try
                {
                    string templateFolder = Environment.GetFolderPath(Environment.SpecialFolder.Templates) + @"\Trade Control";

                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    if (rootKey == null)
                        rootKey = Registry.CurrentUser.CreateSubKey(@"Software\" + regKeyTradeControl, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    if (rootKey.GetValue(regValTemplateFolder, null) != null)
                        templateFolder = rootKey.GetValue(regValTemplateFolder).ToString();
                    else
                    {
                        rootKey.SetValue(regValTemplateFolder, templateFolder, RegistryValueKind.String);
                        if (!Directory.Exists(templateFolder))
                            Directory.CreateDirectory(templateFolder);
                    }

                    return new DirectoryInfo(templateFolder);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Templates));
                }
            }
            set
            {
                try
                {
                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    rootKey.SetValue(regValTemplateFolder, value.ToString(), RegistryValueKind.String);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        [ComVisible(true)]
        public string Repository
        {
            get
            {
                try
                {
                    string repositoryFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Trade Control";

                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    if (rootKey == null)
                        rootKey = Registry.CurrentUser.CreateSubKey(@"Software\" + regKeyTradeControl, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    if (rootKey.GetValue(regValRepository, null) != null)
                        repositoryFolder = rootKey.GetValue(regValRepository).ToString();
                    else
                    {
                        rootKey.SetValue(regValRepository, repositoryFolder, RegistryValueKind.String);
                        if (!Directory.Exists(repositoryFolder))
                            Directory.CreateDirectory(repositoryFolder);
                    }

                    return repositoryFolder;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).FullName;
                }
            }
            set
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog { SelectedPath = value };
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string repository = folderBrowser.SelectedPath;
                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    rootKey.SetValue(regValRepository, repository, RegistryValueKind.String);
                }
            }
        }

        public string EmailFooterHtmlFile
        {
            get
            {
                try
                {
                    string footerFileName = string.Empty;

                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    if (rootKey == null)
                        rootKey = Registry.CurrentUser.CreateSubKey(@"Software\" + regKeyTradeControl, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    if (rootKey.GetValue(regValEmailFooter, null) != null)
                        footerFileName = rootKey.GetValue(regValEmailFooter).ToString();
                    else
                        rootKey.SetValue(regValEmailFooter, string.Empty, RegistryValueKind.String);

                    return footerFileName;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    string footerFileName = EmailFooterHtmlFile;
                    RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(@"Software\" + regKeyTradeControl, true);
                    rootKey.SetValue(regValEmailFooter, value, RegistryValueKind.String);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Gets or sets the user html email footer file
        /// </summary>
        [ComVisible(true)]
        public string SetEmailFooter()
        {
            try
            {
                string filename = string.Empty;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = Properties.Resources.OpenEmailFooterFileDialogTitle,
                    InitialDirectory = new FileInfo(EmailFooterHtmlFile).DirectoryName,
                    Multiselect = false,
                    Filter = @"Htm File(*.htm)| *.htm|Html File(*.html)|*.html",
                    CheckFileExists = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    filename = openFileDialog.FileName;
                    EmailFooterHtmlFile = filename;
                }

                return filename;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        #endregion

        #region Load Xml
        /// <summary>
        /// Generate Task Xml File for orders/quotes
        /// </summary>
        /// <param name="_templateName">Word template.dot file name</param>
        /// <param name="_taskCode">Task code for generation</param>
        /// <param name="_includeQuotedAttributes">Include attributes where state is set to quoted only</param>
        /// <returns>Xml file successfully written to XmlFolder</returns>
        private bool LoadTask(dbTradeControlDataContext context, string _templateName, string _taskCode, bool _includeQuotedAttributes)
        {
            try
            {

                xsTaskCode taskXml = new xsTaskCode();
                taskXml.Clear();
                xsTaskCode.TaskRow taskRow = taskXml.Task.NewTaskRow();

                vwDocCompany company = (from tb in context.vwDocCompanies
                                        select tb).FirstOrDefault();

                if (company != null)
                {
                    taskRow.CompanyName = company.CompanyName;
                    taskRow.CompanyAddress = company.CompanyAddress;
                    taskRow.CompanyNumber = company.CompanyNumber;
                    taskRow.CompanyEmailAddress = company.CompanyEmailAddress;
                    taskRow.CompanyWebsite = company.CompanyWebsite;
                    taskRow.CompanyPhoneNumber = company.CompanyPhoneNumber;
                    taskRow.CompanyNumber = company.CompanyNumber;
                    taskRow.CompanyVatNumber = company.VatNumber;
                }

                vwDocTaskCode task = (from tb in context.vwDocTaskCodes
                                      where tb.TaskCode == _taskCode
                                      select tb).First();

                documentFileName = $"{Repository}\\{task.AccountCode}_{_templateName}_{task.TaskCode}";

                if (task.EmailAddress == null)
                    taskRow.EmailAddress = string.Empty;
                else
                    taskRow.EmailAddress = task.EmailAddress;
                
                emailAddress = taskRow.EmailAddress;

                taskRow.TaskCode = task.TaskCode;
                taskRow.SecondReference = task.SecondReference;
                taskRow.PaymentTerms = task.PaymentTerms;
                taskRow.TaskStatus = task.TaskStatus;
                taskRow.TaskStatusCode = task.TaskStatusCode;
                taskRow.TaskNotes = task.TaskNotes;
                taskRow.TaskTitle = task.TaskTitle;
                taskRow.TaxCode = task.TaxCode;
                taskRow.TaxRate = (float)task.TaxRate;
                taskRow.TotalCharge = (double)task.TotalCharge;
                taskRow.UnitCharge = task.UnitCharge;
                taskRow.UnitOfMeasure = task.UnitOfMeasure;
                taskRow.ContactName = task.ContactName;
                taskRow.UserName = task.UserName;
                taskRow.AccountCode = task.AccountCode;
                taskRow.AccountName = task.AccountName;
                taskRow.NickName = task.NickName;
                taskRow.InvoiceAddress = task.InvoiceAddress;
                taskRow.ActionOn = task.ActionOn;
                taskRow.Quantity = task.Quantity;

                taskRow.TaxCharge = (double)task.TotalCharge * (double)task.TaxRate;
                taskRow.CollectionAddress = task.CollectionAddress;
                taskRow.DeliveryAddress = task.DeliveryAddress;
                taskRow.CollectionAccountName = task.CollectionAccountName;
                taskRow.DeliveryAccountName = task.DeliveryAccountName;

                taskXml.Task.AddTaskRow(taskRow);

                var attributes = from tb in context.tbAttributes
                                 where tb.TaskCode == _taskCode
                                 orderby tb.PrintOrder
                                 select tb;

                foreach (tbAttribute attribute in attributes)
                {
                    if (_includeQuotedAttributes || attribute.AttributeTypeCode == (short)AttributeType.Order)
                    {
                        xsTaskCode.AttributesRow attribRow = taskXml.Attributes.NewAttributesRow();
                        attribRow.Attribute = attribute.Attribute;
                        attribRow.AttributeDescription = attribute.AttributeDescription;
                        attribRow.AttributeTypeCode = attribute.AttributeTypeCode;
                        taskXml.Attributes.AddAttributesRow(attribRow);
                    }

                }

                var quotes = from tb in context.tbQuotes
                             where tb.TaskCode == _taskCode
                             orderby tb.Quantity
                             select tb;

                foreach (tbQuote quote in quotes)
                {
                    xsTaskCode.QuotedPricesRow priceRow = taskXml.QuotedPrices.NewQuotedPricesRow();
                    priceRow.QuoteQuantity = quote.Quantity;
                    priceRow.RunBackPrice = quote.RunBackPrice;
                    priceRow.RunBackQuantity = quote.RunBackQuantity;
                    priceRow.RunOnPrice = (double)quote.RunOnPrice;
                    priceRow.RunOnQuantity = quote.RunOnQuantity;
                    priceRow.QuotePrice = (double)quote.TotalPrice;
                    taskXml.QuotedPrices.AddQuotedPricesRow(priceRow);
                }

                var schedule = from tb in context.proc_Op(_taskCode)
                               orderby tb.OperationNumber
                               select tb;

                foreach (proc_OpResult op in schedule)
                {
                    xsTaskCode.ScheduleRow opRow = taskXml.Schedule.NewScheduleRow();
                    opRow.Duration = op.Duration;
                    opRow.EndOn = op.EndOn;
                    opRow.Note = op.Note;
                    opRow.Operation = op.Operation;
                    opRow.OperationNumber = op.OperationNumber;
                    opRow.StartOn = op.StartOn;
                    taskXml.Schedule.AddScheduleRow(opRow);
                }

                taskXml.WriteXml(Schema.TaskSingleXmlFileName);

                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


        }

        /// <summary>
        /// Generate Invoice Xml File
        /// </summary>
        /// <param name="_templateName">Word template.dot file name</param>
        /// <param name="_invoiceNumber">Invoice number for generation</param>
        /// <returns>Xml file successfully written to XmlFolder</returns>
        private bool LoadInvoice(dbTradeControlDataContext context, string _templateName, string _invoiceNumber)
        {
            try
            {

                xsInvoiceTask invoiceTask = new xsInvoiceTask();
                invoiceTask.Clear();
                
                xsInvoiceTask.InvoiceRow invoiceTaskRow = invoiceTask.Invoice.NewInvoiceRow();

                vwDocCompany company = (from tb in context.vwDocCompanies
                                        select tb).FirstOrDefault();

                if (company != null)
                {
                    invoiceTaskRow.CompanyName = company.CompanyName;
                    invoiceTaskRow.CompanyAddress = company.CompanyAddress;
                    invoiceTaskRow.CompanyNumber = company.CompanyNumber;
                    invoiceTaskRow.CompanyEmailAddress = company.CompanyEmailAddress;
                    invoiceTaskRow.CompanyWebsite = company.CompanyWebsite;
                    invoiceTaskRow.CompanyPhoneNumber = company.CompanyPhoneNumber;
                    invoiceTaskRow.CompanyNumber = company.CompanyNumber;
                    invoiceTaskRow.CompanyVatNumber = company.VatNumber;
                    invoiceTaskRow.BankAccountNumber = company.BankAccountNumber;
                    invoiceTaskRow.BankAccount = company.BankAccount;
                    invoiceTaskRow.BankSortCode = company.BankSortCode;
                }

                vwDocInvoice invoice = (from tb in context.vwDocInvoices
                                        where tb.InvoiceNumber == _invoiceNumber
                                        select tb).First();

                documentFileName = $"{Repository}\\{invoice.AccountCode}_{_templateName}_{invoice.InvoiceNumber}";

                invoiceTaskRow.AccountCode = invoice.AccountCode;
                invoiceTaskRow.AccountName = invoice.AccountName;
                if (invoice.EmailAddress == null)
                    invoiceTaskRow.EmailAddress = string.Empty;
                else
                    invoiceTaskRow.EmailAddress = invoice.EmailAddress;

                emailAddress = invoice.EmailAddress;

                invoiceTaskRow.InvoiceNumber = invoice.InvoiceNumber;
                invoiceTaskRow.InvoiceType = invoice.InvoiceType;
                invoiceTaskRow.UserName = invoice.UserName;
                invoiceTaskRow.InvoicedOn = invoice.InvoicedOn;
                invoiceTaskRow.DueOn = invoice.DueOn;
                invoiceTaskRow.Notes = invoice.Notes ?? string.Empty;
                invoiceTaskRow.PaymentTerms = invoice.PaymentTerms ?? string.Empty;
                invoiceTaskRow.InvoiceAddress = invoice.InvoiceAddress;
                invoiceTaskRow.TotalInvoiceValue = invoice.InvoiceValue;
                invoiceTaskRow.TotalTaxValue = invoice.TaxValue;
                invoiceTaskRow.InvoiceCharge = invoice.InvoiceValue + invoice.TaxValue; 

                invoiceTask.Invoice.AddInvoiceRow(invoiceTaskRow);

                //Add Invoice Items By Tasks >>>>
                var invoiceTasks = from tb in context.vwDocInvoiceTasks
                                   where tb.InvoiceNumber == _invoiceNumber
                                   orderby tb.ActionedOn
                                   select tb;

                foreach (vwDocInvoiceTask taskRow in invoiceTasks)
                {
                    xsInvoiceTask.InvoiceItemRow item = invoiceTask.InvoiceItem.NewInvoiceItemRow();
                    item.InvoiceNumber = _invoiceNumber;
                    item.ActionedOn = taskRow.ActionedOn.Value;
                    item.ActivityCode = taskRow.ActivityCode;
                    item.CashCode = taskRow.CashCode;                    
                    item.CashDescription = taskRow.CashDescription;
                    item.ItemDescription = taskRow.TaskTitle ?? string.Empty;
                    item.TaskCode = taskRow.TaskCode;
                    item.SecondReference = taskRow.SecondReference ?? string.Empty;
                    item.TaxCode = taskRow.TaxCode;
                    item.UnitOfMeasure = taskRow.UnitOfMeasure;

                    item.UnitValue = taskRow.InvoiceValue / (decimal)taskRow.Quantity;
                    item.Quantity = taskRow.Quantity;

                    item.InvoiceValue = taskRow.InvoiceValue;
                    item.TaxValue = taskRow.TaxValue;
                    item.ItemCharge = taskRow.InvoiceValue + taskRow.TaxValue; ;
                    invoiceTask.InvoiceItem.AddInvoiceItemRow(item);
                }

                var invoiceItems = from tb in context.vwDocInvoiceItems
                                   where tb.InvoiceNumber == _invoiceNumber
                                   select tb;

                foreach (vwDocInvoiceItem itemRow in invoiceItems)
                {
                    xsInvoiceTask.InvoiceItemRow item = invoiceTask.InvoiceItem.NewInvoiceItemRow();
                    item.InvoiceNumber = _invoiceNumber;
                    item.ActionedOn = itemRow.ActionedOn;
                    item.ActivityCode = string.Empty;
                    item.CashCode = itemRow.CashCode;
                    item.CashDescription = itemRow.CashDescription;
                    item.ItemDescription = itemRow.ItemReference ?? string.Empty;
                    item.TaskCode = string.Empty;
                    item.SecondReference = string.Empty;
                    item.TaxCode = itemRow.TaxCode;
                    item.UnitOfMeasure = string.Empty;

                    item.UnitValue = 0;
                    item.Quantity = 0;

                    item.InvoiceValue = itemRow.InvoiceValue;
                    item.TaxValue = itemRow.TaxValue;
                    item.ItemCharge = itemRow.InvoiceValue + itemRow.TaxValue; ;

                    invoiceTask.InvoiceItem.AddInvoiceItemRow(item);

                }

                var invoiceTax = from tb in context.vwTaxSummaries
                                 where tb.InvoiceNumber == _invoiceNumber
                                 orderby tb.TaxCode
                                 select tb;

                foreach (vwTaxSummary taxRow in invoiceTax)
                {
                    xsInvoiceTask.InvoiceVatRow taxCode = invoiceTask.InvoiceVat.NewInvoiceVatRow();
                    taxCode.InvoiceNumber = _invoiceNumber;
                    taxCode.InvoiceValueTotal = (decimal)taxRow.InvoiceValueTotal;
                    taxCode.VatTaxCode = taxRow.TaxCode;
                    taxCode.VatTaxRate = (decimal)taxRow.TaxRate;
                    taxCode.TaxValueTotal = (decimal)taxRow.TaxValueTotal;

                    invoiceTask.InvoiceVat.AddInvoiceVatRow(taxCode);
                }

                invoiceTask.WriteXml(Schema.InvoiceTaskXmlFileName);

                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


        }
        #endregion

    }
}
