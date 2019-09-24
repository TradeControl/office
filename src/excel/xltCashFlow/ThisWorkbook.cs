using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace TradeControl.CashFlow
{
    public partial class ThisWorkbook
    {
        CtlActionsPane actionsPane = new CtlActionsPane();

        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            ActionsPane.Controls.Add(actionsPane);
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Save();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, $"{err.Source}.{err.TargetSite.Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }
        
        #endregion

    }
}
