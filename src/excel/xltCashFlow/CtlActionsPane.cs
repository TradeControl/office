using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using Office = Microsoft.Office.Core;
using System.Linq;

namespace TradeControl.CashFlow
{
    partial class CtlActionsPane : UserControl
    {
        enum AuthenticationMode { Windows, SqlServer };

        public CtlActionsPane()
        {
            InitializeComponent();
        }

        #region Events
        private void CtlActionsPane_Load(object sender, EventArgs e)
        {         
            SqlUserName = Properties.Settings.Default.SqlUserName;
            SqlServerName = Properties.Settings.Default.SqlServerName;
            DatabaseName = Properties.Settings.Default.DatabaseName;
            Authentication = (AuthenticationMode)Properties.Settings.Default.AuthenticationMode;
            IncludeActivePeriods = Properties.Settings.Default.IncludeActivePeriods;
            IncludeOrderBook = Properties.Settings.Default.IncludeOrderBook;
            IncludeBankBalances = Properties.Settings.Default.IncludeBankBalances;
            IncludeTaxAccruals = Properties.Settings.Default.IncludeTaxAccruals;
            IncludeVatDetails = Properties.Settings.Default.IncludeVatDetails;
            Greyscale = Properties.Settings.Default.Greyscale;

            if (Authentication == AuthenticationMode.SqlServer)
            {
                tabCtl.SelectedTab = pgConnection;
                TbPassword.Focus();
            }
        }

        private void CbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            PnCredentials.Enabled = Authentication == AuthenticationMode.SqlServer;
        }

        private void CbDatabaseName_Enter(object sender, EventArgs e)
        {
            LoadDatabases();
        }


        private void BtnCashFlow_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;

                SqlConnection conn = this.Connection;
                if (conn == null)
                    LbMessage.Text = Properties.Resources.ConnectionFailed;
                else
                {
                    DataLoader loader = new DataLoader();
                    loader.Greyscale = Greyscale;
                    if (loader.OpenCashFlow(conn, 
                            includeActivePeriods: IncludeActivePeriods, 
                            includeOrderBook: IncludeOrderBook, 
                            includeBankBalances: IncludeBankBalances, 
                            includeBankTypes: IncludeBankTypes, 
                            includeTaxAccruals: IncludeTaxAccruals,
                            includeVatDetails: IncludeVatDetails))
                        LbMessage.Text = string.Empty;
                    else
                        LbMessage.Text = Properties.Resources.GenerationFailed;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnBudget_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;

                SqlConnection conn = this.Connection;
                if (conn == null)
                    LbMessage.Text = Properties.Resources.ConnectionFailed;
                else
                {
                    DataLoader loader = new DataLoader();
                    loader.Greyscale = Greyscale;
                    if (loader.OpenBudget(conn, IncludeActivePeriods, IncludeOrderBook))
                        LbMessage.Text = string.Empty;
                    else
                        LbMessage.Text = Properties.Resources.GenerationFailed;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnClearWorksheets_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.Default;

                DataLoader loader = new DataLoader();
                loader.ClearWorksheets();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region Properties
        public SqlConnection Connection
        {
            get
            {
                if (DbAuthenticated)
                    return new SqlConnection(DbConnectionString);
                else
                    return null;
            }
        }

        AuthenticationMode Authentication
        {
            get
            {
                Properties.Settings.Default.AuthenticationMode = CbAuthentication.SelectedIndex;
                return (AuthenticationMode)CbAuthentication.SelectedIndex;
            }
            set
            {
                CbAuthentication.SelectedIndex = (int)value;                
            }
        }

        private string SqlServerName
        {
            get
            {
                Properties.Settings.Default.SqlServerName = TbServerName.Text;
                return TbServerName.Text;
            }
            set
            {
                TbServerName.Text = value;
            }
        }

        private string DatabaseName
        {
            get
            {
                Properties.Settings.Default.DatabaseName = CbDatabaseName.Text;
                return CbDatabaseName.Text;
            }
            set
            {
                CbDatabaseName.Text = value;
            }
        }
        private string SqlUserName
        {
            get
            {
                Properties.Settings.Default.SqlUserName = TbUserName.Text;
                return TbUserName.Text;
            }
            set
            {
                TbUserName.Text = value;
            }
        }

        private string Password { get { return TbPassword.Text; } }

        public bool IncludeActivePeriods
        {
            get
            {
                Properties.Settings.Default.IncludeActivePeriods = chkIncludeActivePeriods.Checked;
                return chkIncludeActivePeriods.Checked;
            }
            set
            {
                chkIncludeActivePeriods.Checked = value;
            }
        }

        public bool IncludeOrderBook
        {
            get
            {
                Properties.Settings.Default.IncludeOrderBook = chkIncludeOrderBook.Checked;
                return chkIncludeOrderBook.Checked;
            }
            set
            {
                chkIncludeOrderBook.Checked = value;
            }
        }

        public bool IncludeBankBalances
        {
            get
            {
                Properties.Settings.Default.IncludeBankBalances = chkIncludeBankBalance.Checked;
                return chkIncludeBankBalance.Checked;
            }
            set
            {
                chkIncludeBankBalance.Checked = value;
            }
        }

        public bool IncludeBankTypes
        {
            get
            {
                Properties.Settings.Default.IncludeBankTypes = chkIncludeBankType.Checked;
                return chkIncludeBankType.Checked;
            }
            set
            {
                chkIncludeBankType.Checked = value;
            }
        }
        public bool IncludeTaxAccruals
        {
            get
            {
                Properties.Settings.Default.IncludeTaxAccruals = chkIncludeTaxAccruals.Checked;
                return chkIncludeTaxAccruals.Checked;
            }
            set
            {
                chkIncludeTaxAccruals.Checked = value;
            }
        }

        public bool IncludeVatDetails
        {
            get
            {
                Properties.Settings.Default.IncludeVatDetails = chkIncludeVatDetails.Checked;
                return chkIncludeVatDetails.Checked;
            }
            set
            {
                chkIncludeVatDetails.Checked = value;
            }
        }

        public bool Greyscale
        {
            get
            {
                Properties.Settings.Default.Greyscale = chkGreyscale.Checked;
                return chkGreyscale.Checked;
            }
            set
            {
                chkGreyscale.Checked = value;
            }
        }
        #endregion

        #region Database
        private void LoadDatabases()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (SvrAuthenticated)
                {

                    string[] excluded_dbs = new string[] { "model", "msdb", "master", "tempdb" };
                    List<string> dbnames = new List<string>();

                    using (SqlConnection connection = new SqlConnection(SvrConnectionString))
                    {
                        connection.Open();

                        dbnames = connection.GetSchema(SqlClientMetaDataCollectionNames.Databases).Select().OrderBy(s => s.Field<string>("database_name"))
                                        .Where(f => !excluded_dbs.Contains(f.Field<string>("database_name"))).Select(f => f.Field<string>("database_name")).ToList<string>();

                        connection.Close();
                    }

                    CbDatabaseName.DataSource = dbnames;
                    if (CbDatabaseName.Items.Contains(Properties.Settings.Default.DatabaseName))
                        DatabaseName = Properties.Settings.Default.DatabaseName;
                }
                else
                {
                    CbDatabaseName.DataSource = null;
                    CbDatabaseName.Text = string.Empty;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public string DbConnectionString
        {
            get
            {
                try
                {
                    string connectionStr = string.Empty;
                    switch (Authentication)
                    {
                        case AuthenticationMode.Windows:
                            connectionStr = $"Data Source={SqlServerName};Initial Catalog={DatabaseName};Integrated Security=True";
                            break;
                        case AuthenticationMode.SqlServer:
                            connectionStr = $"Data Source={SqlServerName};Initial Catalog={DatabaseName};Persist Security Info=True;User Id={SqlUserName};Password={Password};";
                            break;
                    }

                    return connectionStr;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
            }
        }

        private bool DbAuthenticated
        {
            get
            {
                try
                {
                    if ((DatabaseName.Length > 0 && SqlServerName.Length > 0)
                        && (Authentication == AuthenticationMode.SqlServer && Password.Length > 0 || Authentication == AuthenticationMode.Windows))
                        using (SqlConnection connection = new SqlConnection(DbConnectionString))
                        {
                            connection.Open();
                            return connection.State == System.Data.ConnectionState.Open;
                        }
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string SvrConnectionString
        {
            get
            {
                try
                {
                    string connectionStr = string.Empty;
                    switch (Authentication)
                    {
                        case AuthenticationMode.Windows:
                            connectionStr = $"Data Source={SqlServerName};Integrated Security=True";
                            break;
                        case AuthenticationMode.SqlServer:
                            connectionStr = $"Data Source={SqlServerName};Persist Security Info=True;User Id={SqlUserName};Password={Password};";
                            break;
                    }

                    return connectionStr;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
            }
        }

        private bool SvrAuthenticated
        {
            get
            {
                try
                {
                    if ((SqlServerName.Length > 0)
                        && (Authentication == AuthenticationMode.SqlServer && Password.Length > 0 || Authentication == AuthenticationMode.Windows))
                        using (SqlConnection connection = new SqlConnection(SvrConnectionString))
                        {
                            connection.Open();
                            return connection.State == System.Data.ConnectionState.Open;
                        }
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion


    }
}
