namespace TradeControl.CashFlow
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class CtlActionsPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnBody = new System.Windows.Forms.Panel();
            this.PnActions = new System.Windows.Forms.Panel();
            this.pnControls = new System.Windows.Forms.Panel();
            this.tabCtl = new System.Windows.Forms.TabControl();
            this.pgOptions = new System.Windows.Forms.TabPage();
            this.chkIncludeBalanceSheet = new System.Windows.Forms.CheckBox();
            this.chkGreyscale = new System.Windows.Forms.CheckBox();
            this.chkIncludeVatDetails = new System.Windows.Forms.CheckBox();
            this.chkIncludeBankType = new System.Windows.Forms.CheckBox();
            this.chkIncludeActivePeriods = new System.Windows.Forms.CheckBox();
            this.chkIncludeTaxAccruals = new System.Windows.Forms.CheckBox();
            this.chkIncludeBankBalance = new System.Windows.Forms.CheckBox();
            this.chkIncludeOrderBook = new System.Windows.Forms.CheckBox();
            this.pgConnection = new System.Windows.Forms.TabPage();
            this.PnConnection = new System.Windows.Forms.Panel();
            this.numCommandTimeout = new System.Windows.Forms.NumericUpDown();
            this.LbCommandTimeout = new System.Windows.Forms.Label();
            this.CbDatabaseName = new System.Windows.Forms.ComboBox();
            this.LbDatabaseName = new System.Windows.Forms.Label();
            this.PnCredentials = new System.Windows.Forms.Panel();
            this.TbPassword = new System.Windows.Forms.TextBox();
            this.LbUserName = new System.Windows.Forms.Label();
            this.TbUserName = new System.Windows.Forms.TextBox();
            this.LbPassword = new System.Windows.Forms.Label();
            this.CbAuthentication = new System.Windows.Forms.ComboBox();
            this.LbAuthentication = new System.Windows.Forms.Label();
            this.TbServerName = new System.Windows.Forms.TextBox();
            this.LbServerName = new System.Windows.Forms.Label();
            this.pgRun = new System.Windows.Forms.TabPage();
            this.BtnCashFlow = new System.Windows.Forms.Button();
            this.BtnBudget = new System.Windows.Forms.Button();
            this.BtnClearWorksheets = new System.Windows.Forms.Button();
            this.pnMessages = new System.Windows.Forms.Panel();
            this.LbMessage = new System.Windows.Forms.Label();
            this.LbSeparator1 = new System.Windows.Forms.Label();
            this.LbSeparator2 = new System.Windows.Forms.Label();
            this.PnBody.SuspendLayout();
            this.PnActions.SuspendLayout();
            this.pnControls.SuspendLayout();
            this.tabCtl.SuspendLayout();
            this.pgOptions.SuspendLayout();
            this.pgConnection.SuspendLayout();
            this.PnConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommandTimeout)).BeginInit();
            this.PnCredentials.SuspendLayout();
            this.pgRun.SuspendLayout();
            this.pnMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnBody
            // 
            this.PnBody.BackColor = System.Drawing.Color.Transparent;
            this.PnBody.Controls.Add(this.PnActions);
            this.PnBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnBody.Location = new System.Drawing.Point(0, 0);
            this.PnBody.Name = "PnBody";
            this.PnBody.Padding = new System.Windows.Forms.Padding(10);
            this.PnBody.Size = new System.Drawing.Size(473, 602);
            this.PnBody.TabIndex = 0;
            // 
            // PnActions
            // 
            this.PnActions.Controls.Add(this.pnControls);
            this.PnActions.Controls.Add(this.LbSeparator1);
            this.PnActions.Controls.Add(this.LbSeparator2);
            this.PnActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnActions.Location = new System.Drawing.Point(10, 10);
            this.PnActions.Name = "PnActions";
            this.PnActions.Padding = new System.Windows.Forms.Padding(10);
            this.PnActions.Size = new System.Drawing.Size(453, 582);
            this.PnActions.TabIndex = 1;
            // 
            // pnControls
            // 
            this.pnControls.Controls.Add(this.tabCtl);
            this.pnControls.Controls.Add(this.pnMessages);
            this.pnControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnControls.Location = new System.Drawing.Point(10, 10);
            this.pnControls.Name = "pnControls";
            this.pnControls.Size = new System.Drawing.Size(433, 536);
            this.pnControls.TabIndex = 8;
            // 
            // tabCtl
            // 
            this.tabCtl.Controls.Add(this.pgOptions);
            this.tabCtl.Controls.Add(this.pgConnection);
            this.tabCtl.Controls.Add(this.pgRun);
            this.tabCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtl.Location = new System.Drawing.Point(0, 0);
            this.tabCtl.Name = "tabCtl";
            this.tabCtl.SelectedIndex = 0;
            this.tabCtl.Size = new System.Drawing.Size(433, 472);
            this.tabCtl.TabIndex = 10;
            // 
            // pgOptions
            // 
            this.pgOptions.Controls.Add(this.chkIncludeBalanceSheet);
            this.pgOptions.Controls.Add(this.chkGreyscale);
            this.pgOptions.Controls.Add(this.chkIncludeVatDetails);
            this.pgOptions.Controls.Add(this.chkIncludeBankType);
            this.pgOptions.Controls.Add(this.chkIncludeActivePeriods);
            this.pgOptions.Controls.Add(this.chkIncludeTaxAccruals);
            this.pgOptions.Controls.Add(this.chkIncludeBankBalance);
            this.pgOptions.Controls.Add(this.chkIncludeOrderBook);
            this.pgOptions.Location = new System.Drawing.Point(4, 22);
            this.pgOptions.Name = "pgOptions";
            this.pgOptions.Padding = new System.Windows.Forms.Padding(10);
            this.pgOptions.Size = new System.Drawing.Size(425, 446);
            this.pgOptions.TabIndex = 0;
            this.pgOptions.Text = "Options";
            this.pgOptions.UseVisualStyleBackColor = true;
            // 
            // chkIncludeBalanceSheet
            // 
            this.chkIncludeBalanceSheet.AutoSize = true;
            this.chkIncludeBalanceSheet.Location = new System.Drawing.Point(25, 160);
            this.chkIncludeBalanceSheet.Name = "chkIncludeBalanceSheet";
            this.chkIncludeBalanceSheet.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeBalanceSheet.TabIndex = 6;
            this.chkIncludeBalanceSheet.Text = "Balance Sheet?";
            this.chkIncludeBalanceSheet.UseVisualStyleBackColor = true;
            // 
            // chkGreyscale
            // 
            this.chkGreyscale.AutoSize = true;
            this.chkGreyscale.Location = new System.Drawing.Point(25, 183);
            this.chkGreyscale.Name = "chkGreyscale";
            this.chkGreyscale.Size = new System.Drawing.Size(79, 17);
            this.chkGreyscale.TabIndex = 7;
            this.chkGreyscale.Text = "Greyscale?";
            this.chkGreyscale.UseVisualStyleBackColor = true;
            // 
            // chkIncludeVatDetails
            // 
            this.chkIncludeVatDetails.AutoSize = true;
            this.chkIncludeVatDetails.Location = new System.Drawing.Point(25, 137);
            this.chkIncludeVatDetails.Name = "chkIncludeVatDetails";
            this.chkIncludeVatDetails.Size = new System.Drawing.Size(187, 17);
            this.chkIncludeVatDetails.TabIndex = 5;
            this.chkIncludeVatDetails.Text = "Vat period and quarterly amounts?";
            this.chkIncludeVatDetails.UseVisualStyleBackColor = true;
            // 
            // chkIncludeBankType
            // 
            this.chkIncludeBankType.AutoSize = true;
            this.chkIncludeBankType.Location = new System.Drawing.Point(25, 69);
            this.chkIncludeBankType.Name = "chkIncludeBankType";
            this.chkIncludeBankType.Size = new System.Drawing.Size(140, 17);
            this.chkIncludeBankType.TabIndex = 2;
            this.chkIncludeBankType.Text = "Bank transaction types?";
            this.chkIncludeBankType.UseVisualStyleBackColor = true;
            // 
            // chkIncludeActivePeriods
            // 
            this.chkIncludeActivePeriods.AutoSize = true;
            this.chkIncludeActivePeriods.Location = new System.Drawing.Point(25, 23);
            this.chkIncludeActivePeriods.Name = "chkIncludeActivePeriods";
            this.chkIncludeActivePeriods.Size = new System.Drawing.Size(136, 17);
            this.chkIncludeActivePeriods.TabIndex = 0;
            this.chkIncludeActivePeriods.Text = "Active period invoices?";
            this.chkIncludeActivePeriods.UseVisualStyleBackColor = true;
            // 
            // chkIncludeTaxAccruals
            // 
            this.chkIncludeTaxAccruals.AutoSize = true;
            this.chkIncludeTaxAccruals.Location = new System.Drawing.Point(25, 114);
            this.chkIncludeTaxAccruals.Name = "chkIncludeTaxAccruals";
            this.chkIncludeTaxAccruals.Size = new System.Drawing.Size(190, 17);
            this.chkIncludeTaxAccruals.TabIndex = 4;
            this.chkIncludeTaxAccruals.Text = "Vat and Corporation Tax accruals?";
            this.chkIncludeTaxAccruals.UseVisualStyleBackColor = true;
            // 
            // chkIncludeBankBalance
            // 
            this.chkIncludeBankBalance.AutoSize = true;
            this.chkIncludeBankBalance.Location = new System.Drawing.Point(25, 91);
            this.chkIncludeBankBalance.Name = "chkIncludeBankBalance";
            this.chkIncludeBankBalance.Size = new System.Drawing.Size(103, 17);
            this.chkIncludeBankBalance.TabIndex = 3;
            this.chkIncludeBankBalance.Text = "Bank balances?";
            this.chkIncludeBankBalance.UseVisualStyleBackColor = true;
            // 
            // chkIncludeOrderBook
            // 
            this.chkIncludeOrderBook.AutoSize = true;
            this.chkIncludeOrderBook.Location = new System.Drawing.Point(25, 46);
            this.chkIncludeOrderBook.Name = "chkIncludeOrderBook";
            this.chkIncludeOrderBook.Size = new System.Drawing.Size(109, 17);
            this.chkIncludeOrderBook.TabIndex = 1;
            this.chkIncludeOrderBook.Text = "Live Order Book?";
            this.chkIncludeOrderBook.UseVisualStyleBackColor = true;
            // 
            // pgConnection
            // 
            this.pgConnection.Controls.Add(this.PnConnection);
            this.pgConnection.Location = new System.Drawing.Point(4, 22);
            this.pgConnection.Name = "pgConnection";
            this.pgConnection.Padding = new System.Windows.Forms.Padding(3);
            this.pgConnection.Size = new System.Drawing.Size(425, 446);
            this.pgConnection.TabIndex = 1;
            this.pgConnection.Text = "Connection";
            this.pgConnection.UseVisualStyleBackColor = true;
            // 
            // PnConnection
            // 
            this.PnConnection.Controls.Add(this.numCommandTimeout);
            this.PnConnection.Controls.Add(this.LbCommandTimeout);
            this.PnConnection.Controls.Add(this.CbDatabaseName);
            this.PnConnection.Controls.Add(this.LbDatabaseName);
            this.PnConnection.Controls.Add(this.PnCredentials);
            this.PnConnection.Controls.Add(this.CbAuthentication);
            this.PnConnection.Controls.Add(this.LbAuthentication);
            this.PnConnection.Controls.Add(this.TbServerName);
            this.PnConnection.Controls.Add(this.LbServerName);
            this.PnConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnConnection.Location = new System.Drawing.Point(3, 3);
            this.PnConnection.Name = "PnConnection";
            this.PnConnection.Padding = new System.Windows.Forms.Padding(10);
            this.PnConnection.Size = new System.Drawing.Size(419, 440);
            this.PnConnection.TabIndex = 1;
            this.PnConnection.Paint += new System.Windows.Forms.PaintEventHandler(this.PnConnection_Paint);
            // 
            // numCommandTimeout
            // 
            this.numCommandTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numCommandTimeout.Dock = System.Windows.Forms.DockStyle.Top;
            this.numCommandTimeout.Location = new System.Drawing.Point(10, 263);
            this.numCommandTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numCommandTimeout.Name = "numCommandTimeout";
            this.numCommandTimeout.Size = new System.Drawing.Size(399, 20);
            this.numCommandTimeout.TabIndex = 11;
            // 
            // LbCommandTimeout
            // 
            this.LbCommandTimeout.AutoSize = true;
            this.LbCommandTimeout.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbCommandTimeout.Location = new System.Drawing.Point(10, 240);
            this.LbCommandTimeout.Name = "LbCommandTimeout";
            this.LbCommandTimeout.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LbCommandTimeout.Size = new System.Drawing.Size(98, 23);
            this.LbCommandTimeout.TabIndex = 10;
            this.LbCommandTimeout.Text = "Command Timeout:";
            // 
            // CbDatabaseName
            // 
            this.CbDatabaseName.Dock = System.Windows.Forms.DockStyle.Top;
            this.CbDatabaseName.FormattingEnabled = true;
            this.CbDatabaseName.Location = new System.Drawing.Point(10, 219);
            this.CbDatabaseName.Name = "CbDatabaseName";
            this.CbDatabaseName.Size = new System.Drawing.Size(399, 21);
            this.CbDatabaseName.TabIndex = 2;
            this.CbDatabaseName.SelectedIndexChanged += new System.EventHandler(this.CbDatabaseName_SelectedIndexChanged);
            this.CbDatabaseName.Enter += new System.EventHandler(this.CbDatabaseName_Enter);
            // 
            // LbDatabaseName
            // 
            this.LbDatabaseName.AutoSize = true;
            this.LbDatabaseName.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbDatabaseName.Location = new System.Drawing.Point(10, 196);
            this.LbDatabaseName.Name = "LbDatabaseName";
            this.LbDatabaseName.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LbDatabaseName.Size = new System.Drawing.Size(56, 23);
            this.LbDatabaseName.TabIndex = 5;
            this.LbDatabaseName.Text = "Database:";
            // 
            // PnCredentials
            // 
            this.PnCredentials.Controls.Add(this.TbPassword);
            this.PnCredentials.Controls.Add(this.LbUserName);
            this.PnCredentials.Controls.Add(this.TbUserName);
            this.PnCredentials.Controls.Add(this.LbPassword);
            this.PnCredentials.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnCredentials.Enabled = false;
            this.PnCredentials.Location = new System.Drawing.Point(10, 92);
            this.PnCredentials.Name = "PnCredentials";
            this.PnCredentials.Size = new System.Drawing.Size(399, 104);
            this.PnCredentials.TabIndex = 4;
            // 
            // TbPassword
            // 
            this.TbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbPassword.Location = new System.Drawing.Point(128, 53);
            this.TbPassword.Name = "TbPassword";
            this.TbPassword.PasswordChar = '*';
            this.TbPassword.Size = new System.Drawing.Size(160, 20);
            this.TbPassword.TabIndex = 1;
            // 
            // LbUserName
            // 
            this.LbUserName.AutoSize = true;
            this.LbUserName.Location = new System.Drawing.Point(9, 20);
            this.LbUserName.Name = "LbUserName";
            this.LbUserName.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LbUserName.Size = new System.Drawing.Size(63, 23);
            this.LbUserName.TabIndex = 9;
            this.LbUserName.Text = "User Name:";
            // 
            // TbUserName
            // 
            this.TbUserName.Location = new System.Drawing.Point(128, 24);
            this.TbUserName.Name = "TbUserName";
            this.TbUserName.Size = new System.Drawing.Size(160, 20);
            this.TbUserName.TabIndex = 0;
            // 
            // LbPassword
            // 
            this.LbPassword.AutoSize = true;
            this.LbPassword.Location = new System.Drawing.Point(9, 49);
            this.LbPassword.Name = "LbPassword";
            this.LbPassword.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LbPassword.Size = new System.Drawing.Size(56, 23);
            this.LbPassword.TabIndex = 6;
            this.LbPassword.Text = "Password:";
            // 
            // CbAuthentication
            // 
            this.CbAuthentication.Dock = System.Windows.Forms.DockStyle.Top;
            this.CbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbAuthentication.FormattingEnabled = true;
            this.CbAuthentication.Items.AddRange(new object[] {
            "Windows Authentication",
            "Sql Server Authentication"});
            this.CbAuthentication.Location = new System.Drawing.Point(10, 71);
            this.CbAuthentication.Name = "CbAuthentication";
            this.CbAuthentication.Size = new System.Drawing.Size(399, 21);
            this.CbAuthentication.TabIndex = 1;
            this.CbAuthentication.SelectedIndexChanged += new System.EventHandler(this.CbAuthentication_SelectedIndexChanged);
            // 
            // LbAuthentication
            // 
            this.LbAuthentication.AutoSize = true;
            this.LbAuthentication.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbAuthentication.Location = new System.Drawing.Point(10, 48);
            this.LbAuthentication.Name = "LbAuthentication";
            this.LbAuthentication.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LbAuthentication.Size = new System.Drawing.Size(78, 23);
            this.LbAuthentication.TabIndex = 2;
            this.LbAuthentication.Text = "Authentication:";
            // 
            // TbServerName
            // 
            this.TbServerName.Dock = System.Windows.Forms.DockStyle.Top;
            this.TbServerName.Location = new System.Drawing.Point(10, 28);
            this.TbServerName.Name = "TbServerName";
            this.TbServerName.Size = new System.Drawing.Size(399, 20);
            this.TbServerName.TabIndex = 0;
            // 
            // LbServerName
            // 
            this.LbServerName.AutoSize = true;
            this.LbServerName.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbServerName.Location = new System.Drawing.Point(10, 10);
            this.LbServerName.Name = "LbServerName";
            this.LbServerName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.LbServerName.Size = new System.Drawing.Size(90, 18);
            this.LbServerName.TabIndex = 0;
            this.LbServerName.Text = "Sql Server Name:";
            // 
            // pgRun
            // 
            this.pgRun.Controls.Add(this.BtnCashFlow);
            this.pgRun.Controls.Add(this.BtnBudget);
            this.pgRun.Controls.Add(this.BtnClearWorksheets);
            this.pgRun.Location = new System.Drawing.Point(4, 22);
            this.pgRun.Name = "pgRun";
            this.pgRun.Padding = new System.Windows.Forms.Padding(3);
            this.pgRun.Size = new System.Drawing.Size(425, 446);
            this.pgRun.TabIndex = 2;
            this.pgRun.Text = "Run";
            this.pgRun.UseVisualStyleBackColor = true;
            // 
            // BtnCashFlow
            // 
            this.BtnCashFlow.Location = new System.Drawing.Point(27, 30);
            this.BtnCashFlow.Name = "BtnCashFlow";
            this.BtnCashFlow.Size = new System.Drawing.Size(179, 36);
            this.BtnCashFlow.TabIndex = 3;
            this.BtnCashFlow.Text = "Cash Flow";
            this.BtnCashFlow.UseVisualStyleBackColor = true;
            this.BtnCashFlow.Click += new System.EventHandler(this.BtnCashFlow_Click);
            // 
            // BtnBudget
            // 
            this.BtnBudget.Location = new System.Drawing.Point(27, 72);
            this.BtnBudget.Name = "BtnBudget";
            this.BtnBudget.Size = new System.Drawing.Size(179, 36);
            this.BtnBudget.TabIndex = 4;
            this.BtnBudget.Text = "Budget";
            this.BtnBudget.UseVisualStyleBackColor = true;
            this.BtnBudget.Click += new System.EventHandler(this.BtnBudget_Click);
            // 
            // BtnClearWorksheets
            // 
            this.BtnClearWorksheets.Location = new System.Drawing.Point(27, 114);
            this.BtnClearWorksheets.Name = "BtnClearWorksheets";
            this.BtnClearWorksheets.Size = new System.Drawing.Size(179, 36);
            this.BtnClearWorksheets.TabIndex = 5;
            this.BtnClearWorksheets.Text = "Clear Worksheets";
            this.BtnClearWorksheets.UseVisualStyleBackColor = true;
            this.BtnClearWorksheets.Click += new System.EventHandler(this.BtnClearWorksheets_Click);
            // 
            // pnMessages
            // 
            this.pnMessages.Controls.Add(this.LbMessage);
            this.pnMessages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnMessages.Location = new System.Drawing.Point(0, 472);
            this.pnMessages.Name = "pnMessages";
            this.pnMessages.Padding = new System.Windows.Forms.Padding(5);
            this.pnMessages.Size = new System.Drawing.Size(433, 64);
            this.pnMessages.TabIndex = 9;
            // 
            // LbMessage
            // 
            this.LbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbMessage.Location = new System.Drawing.Point(5, 5);
            this.LbMessage.Name = "LbMessage";
            this.LbMessage.Size = new System.Drawing.Size(423, 54);
            this.LbMessage.TabIndex = 0;
            this.LbMessage.Text = "-";
            this.LbMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LbSeparator1
            // 
            this.LbSeparator1.AutoSize = true;
            this.LbSeparator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LbSeparator1.Location = new System.Drawing.Point(10, 546);
            this.LbSeparator1.Name = "LbSeparator1";
            this.LbSeparator1.Size = new System.Drawing.Size(0, 13);
            this.LbSeparator1.TabIndex = 2;
            // 
            // LbSeparator2
            // 
            this.LbSeparator2.AutoSize = true;
            this.LbSeparator2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LbSeparator2.Location = new System.Drawing.Point(10, 559);
            this.LbSeparator2.Name = "LbSeparator2";
            this.LbSeparator2.Size = new System.Drawing.Size(0, 13);
            this.LbSeparator2.TabIndex = 7;
            // 
            // CtlActionsPane
            // 
            this.Controls.Add(this.PnBody);
            this.Name = "CtlActionsPane";
            this.Size = new System.Drawing.Size(473, 602);
            this.Load += new System.EventHandler(this.CtlActionsPane_Load);
            this.PnBody.ResumeLayout(false);
            this.PnActions.ResumeLayout(false);
            this.PnActions.PerformLayout();
            this.pnControls.ResumeLayout(false);
            this.tabCtl.ResumeLayout(false);
            this.pgOptions.ResumeLayout(false);
            this.pgOptions.PerformLayout();
            this.pgConnection.ResumeLayout(false);
            this.PnConnection.ResumeLayout(false);
            this.PnConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommandTimeout)).EndInit();
            this.PnCredentials.ResumeLayout(false);
            this.PnCredentials.PerformLayout();
            this.pgRun.ResumeLayout(false);
            this.pnMessages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnBody;
        private System.Windows.Forms.Panel PnActions;
        private System.Windows.Forms.Label LbSeparator1;
        private System.Windows.Forms.Label LbSeparator2;
        private System.Windows.Forms.Panel pnControls;
        private System.Windows.Forms.TabControl tabCtl;
        private System.Windows.Forms.TabPage pgOptions;
        private System.Windows.Forms.TabPage pgConnection;
        private System.Windows.Forms.Panel PnConnection;
        private System.Windows.Forms.ComboBox CbDatabaseName;
        private System.Windows.Forms.Label LbDatabaseName;
        private System.Windows.Forms.Panel PnCredentials;
        private System.Windows.Forms.TextBox TbPassword;
        private System.Windows.Forms.Label LbUserName;
        private System.Windows.Forms.TextBox TbUserName;
        private System.Windows.Forms.Label LbPassword;
        private System.Windows.Forms.ComboBox CbAuthentication;
        private System.Windows.Forms.Label LbAuthentication;
        private System.Windows.Forms.TextBox TbServerName;
        private System.Windows.Forms.Label LbServerName;
        private System.Windows.Forms.CheckBox chkIncludeBankType;
        private System.Windows.Forms.CheckBox chkIncludeActivePeriods;
        private System.Windows.Forms.CheckBox chkIncludeTaxAccruals;
        private System.Windows.Forms.CheckBox chkIncludeBankBalance;
        private System.Windows.Forms.CheckBox chkIncludeOrderBook;
        private System.Windows.Forms.CheckBox chkIncludeVatDetails;
        private System.Windows.Forms.CheckBox chkGreyscale;
        private System.Windows.Forms.TabPage pgRun;
        private System.Windows.Forms.Button BtnCashFlow;
        private System.Windows.Forms.Button BtnBudget;
        private System.Windows.Forms.Button BtnClearWorksheets;
        private System.Windows.Forms.Panel pnMessages;
        private System.Windows.Forms.Label LbMessage;
        private System.Windows.Forms.CheckBox chkIncludeBalanceSheet;
        private System.Windows.Forms.Label LbCommandTimeout;
        private System.Windows.Forms.NumericUpDown numCommandTimeout;
    }
}
