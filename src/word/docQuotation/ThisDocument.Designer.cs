﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 414
namespace docQuotation {
    
    
    /// 
    [Microsoft.VisualStudio.Tools.Applications.Runtime.StartupObjectAttribute(0)]
    [global::System.Security.Permissions.PermissionSetAttribute(global::System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
    public sealed partial class ThisDocument : Microsoft.Office.Tools.Word.DocumentBase {
        
        internal Microsoft.Office.Tools.ActionsPane ActionsPane;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl ContactName;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl AccountName;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl InvoiceAddress;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl NickName;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl TaskCode;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl TaskTitle;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl Attribute;
        
        internal Microsoft.Office.Tools.Word.RichTextContentControl AttributeDescription;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl QuoteQuantity;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl QuotePrice;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl RunOnQuantity;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl RunOnPrice;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl RunBackQuantity;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl RunBackPrice;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl CompanyName;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl UserName;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        private global::System.Object missing = global::System.Type.Missing;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        internal Microsoft.Office.Interop.Word.Application ThisApplication;
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        public ThisDocument(global::Microsoft.Office.Tools.Word.Factory factory, global::System.IServiceProvider serviceProvider) : 
                base(factory, serviceProvider, "ThisDocument", "ThisDocument") {
            Globals.Factory = factory;
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void Initialize() {
            base.Initialize();
            this.ThisApplication = this.GetHostItem<Microsoft.Office.Interop.Word.Application>(typeof(Microsoft.Office.Interop.Word.Application), "Application");
            Globals.ThisDocument = this;
            global::System.Windows.Forms.Application.EnableVisualStyles();
            this.InitializeCachedData();
            this.InitializeControls();
            this.InitializeComponents();
            this.InitializeData();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void FinishInitialization() {
            this.InternalStartup();
            this.OnStartup();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void InitializeDataBindings() {
            this.BeginInitialization();
            this.BindToData();
            this.EndInitialization();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeCachedData() {
            if ((this.DataHost == null)) {
                return;
            }
            if (this.DataHost.IsCacheInitialized) {
                this.DataHost.FillCachedData(this);
            }
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeData() {
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void BindToData() {
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StartCaching(string MemberName) {
            this.DataHost.StartCaching(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StopCaching(string MemberName) {
            this.DataHost.StopCaching(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool IsCached(string MemberName) {
            return this.DataHost.IsCached(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void BeginInitialization() {
            this.BeginInit();
            this.ActionsPane.BeginInit();
            this.ContactName.BeginInit();
            this.AccountName.BeginInit();
            this.InvoiceAddress.BeginInit();
            this.NickName.BeginInit();
            this.TaskCode.BeginInit();
            this.TaskTitle.BeginInit();
            this.Attribute.BeginInit();
            this.AttributeDescription.BeginInit();
            this.QuoteQuantity.BeginInit();
            this.QuotePrice.BeginInit();
            this.RunOnQuantity.BeginInit();
            this.RunOnPrice.BeginInit();
            this.RunBackQuantity.BeginInit();
            this.RunBackPrice.BeginInit();
            this.CompanyName.BeginInit();
            this.UserName.BeginInit();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void EndInitialization() {
            this.UserName.EndInit();
            this.CompanyName.EndInit();
            this.RunBackPrice.EndInit();
            this.RunBackQuantity.EndInit();
            this.RunOnPrice.EndInit();
            this.RunOnQuantity.EndInit();
            this.QuotePrice.EndInit();
            this.QuoteQuantity.EndInit();
            this.AttributeDescription.EndInit();
            this.Attribute.EndInit();
            this.TaskTitle.EndInit();
            this.TaskCode.EndInit();
            this.NickName.EndInit();
            this.InvoiceAddress.EndInit();
            this.AccountName.EndInit();
            this.ContactName.EndInit();
            this.ActionsPane.EndInit();
            this.EndInit();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeControls() {
            this.ActionsPane = Globals.Factory.CreateActionsPane(null, null, "ActionsPane", "ActionsPane", this);
            this.ContactName = Globals.Factory.CreatePlainTextContentControl(null, null, "29662354", "ContactName", this);
            this.AccountName = Globals.Factory.CreatePlainTextContentControl(null, null, "29662359", "AccountName", this);
            this.InvoiceAddress = Globals.Factory.CreatePlainTextContentControl(null, null, "29662361", "InvoiceAddress", this);
            this.NickName = Globals.Factory.CreatePlainTextContentControl(null, null, "30982916", "NickName", this);
            this.TaskCode = Globals.Factory.CreatePlainTextContentControl(null, null, "10208699", "TaskCode", this);
            this.TaskTitle = Globals.Factory.CreatePlainTextContentControl(null, null, "30982867", "TaskTitle", this);
            this.Attribute = Globals.Factory.CreatePlainTextContentControl(null, null, "30982870", "Attribute", this);
            this.AttributeDescription = Globals.Factory.CreateRichTextContentControl(null, null, "30982871", "AttributeDescription", this);
            this.QuoteQuantity = Globals.Factory.CreatePlainTextContentControl(null, null, "30982892", "QuoteQuantity", this);
            this.QuotePrice = Globals.Factory.CreatePlainTextContentControl(null, null, "30982894", "QuotePrice", this);
            this.RunOnQuantity = Globals.Factory.CreatePlainTextContentControl(null, null, "30982897", "RunOnQuantity", this);
            this.RunOnPrice = Globals.Factory.CreatePlainTextContentControl(null, null, "30982901", "RunOnPrice", this);
            this.RunBackQuantity = Globals.Factory.CreatePlainTextContentControl(null, null, "30982906", "RunBackQuantity", this);
            this.RunBackPrice = Globals.Factory.CreatePlainTextContentControl(null, null, "30982912", "RunBackPrice", this);
            this.CompanyName = Globals.Factory.CreatePlainTextContentControl(null, null, "22442944", "CompanyName", this);
            this.UserName = Globals.Factory.CreatePlainTextContentControl(null, null, "22442949", "UserName", this);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeComponents() {
            // 
            // ActionsPane
            // 
            this.ActionsPane.AutoSize = false;
            this.ActionsPane.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            // 
            // ContactName
            // 
            this.ContactName.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // AccountName
            // 
            this.AccountName.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // InvoiceAddress
            // 
            this.InvoiceAddress.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // NickName
            // 
            this.NickName.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // TaskCode
            // 
            this.TaskCode.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // TaskTitle
            // 
            this.TaskTitle.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // Attribute
            // 
            this.Attribute.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // AttributeDescription
            // 
            this.AttributeDescription.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // QuoteQuantity
            // 
            this.QuoteQuantity.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // QuotePrice
            // 
            this.QuotePrice.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // RunOnQuantity
            // 
            this.RunOnQuantity.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // RunOnPrice
            // 
            this.RunOnPrice.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // RunBackQuantity
            // 
            this.RunBackQuantity.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // RunBackPrice
            // 
            this.RunBackPrice.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // CompanyName
            // 
            this.CompanyName.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // UserName
            // 
            this.UserName.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // ThisDocument
            // 
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool NeedsFill(string MemberName) {
            return this.DataHost.NeedsFill(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnShutdown() {
            this.UserName.Dispose();
            this.CompanyName.Dispose();
            this.RunBackPrice.Dispose();
            this.RunBackQuantity.Dispose();
            this.RunOnPrice.Dispose();
            this.RunOnQuantity.Dispose();
            this.QuotePrice.Dispose();
            this.QuoteQuantity.Dispose();
            this.AttributeDescription.Dispose();
            this.Attribute.Dispose();
            this.TaskTitle.Dispose();
            this.TaskCode.Dispose();
            this.NickName.Dispose();
            this.InvoiceAddress.Dispose();
            this.AccountName.Dispose();
            this.ContactName.Dispose();
            this.ActionsPane.Dispose();
            base.OnShutdown();
        }
    }
    
    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
    internal sealed partial class Globals {
        
        /// 
        private Globals() {
        }
        
        private static ThisDocument _ThisDocument;
        
        private static global::Microsoft.Office.Tools.Word.Factory _factory;
        
        private static ThisRibbonCollection _ThisRibbonCollection;
        
        internal static ThisDocument ThisDocument {
            get {
                return _ThisDocument;
            }
            set {
                if ((_ThisDocument == null)) {
                    _ThisDocument = value;
                }
                else {
                    throw new System.NotSupportedException();
                }
            }
        }
        
        internal static global::Microsoft.Office.Tools.Word.Factory Factory {
            get {
                return _factory;
            }
            set {
                if ((_factory == null)) {
                    _factory = value;
                }
                else {
                    throw new System.NotSupportedException();
                }
            }
        }
        
        internal static ThisRibbonCollection Ribbons {
            get {
                if ((_ThisRibbonCollection == null)) {
                    _ThisRibbonCollection = new ThisRibbonCollection(_factory.GetRibbonFactory());
                }
                return _ThisRibbonCollection;
            }
        }
    }
    
    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "15.0.0.0")]
    internal sealed partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonCollectionBase {
        
        /// 
        internal ThisRibbonCollection(global::Microsoft.Office.Tools.Ribbon.RibbonFactory factory) : 
                base(factory) {
        }
    }
}
