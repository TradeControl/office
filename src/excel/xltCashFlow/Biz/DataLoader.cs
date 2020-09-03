using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Drawing;
using TradeControl.CashFlow.Data;

namespace TradeControl.CashFlow
{
    public class DataLoader
    {
        DataContext dataContext;

        ReportMode reportMode;
        WSBudget Budget { get { return Globals.WSBudget; } }
        WSCashFlow CashFlow {  get { return Globals.WSCashFlow; } }

        int ActiveYear { get; set; }
        int ActiveMonth { get; set; }
        DateTime ActiveDate { get; set; }
        public bool Greyscale { get; set; }

        const int firstCol = 4;
        int curRow = 3, curCol = firstCol, periodCol = firstCol, lastCol = firstCol;
    
        public DataLoader() {}

        #region Public Functions
        public bool OpenCashFlow(SqlConnection conn, 
            bool includeActivePeriods = false, 
            bool includeOrderBook = false, 
            bool includeBankBalances = false, 
            bool includeBankTypes = false, 
            bool includeTaxAccruals = false,
            bool includeVatDetails = false,
            bool includeBalanceSheet = false)
        {
            try
            {
                reportMode = ReportMode.CashFlow;

                Budget.Activate(); Budget.Range["A1"].Select();

                CashFlow.BeginInit();
                CashFlow.Unprotect();
                ClearWorksheet(CashFlow);

                dataContext = new DataContext(conn);
                dataContext.Open();

                SetActivePeriod(CashFlow, Properties.Resources.TextStatementTitle);
                InitialiseWorksheet(CashFlow);
                InitialisePeriods(CashFlow);

                LoadCategories(CashFlow, CashType.Trade, includeActivePeriods, includeOrderBook, false);
                LoadCategories(CashFlow, CashType.Money, false, false, false);
                LoadTotals(CashFlow, CashType.Trade, CategoryType.Total);

                LoadCategories(CashFlow, CashType.Tax, includeActivePeriods, false, includeTaxAccruals);
                LoadTotals(CashFlow, CashType.Tax, CategoryType.Total);

                LoadTotalsFormula(CashFlow);
                LoadExpressions(CashFlow);

                if (includeBankTypes)
                    LoadCategories(CashFlow, CashType.Money, false, false, false);

                if (includeBankBalances)
                    LoadBankBalances(CashFlow);

                if (includeVatDetails)
                {
                    LoadVatRecurrenceTotals(CashFlow, includeActivePeriods, includeTaxAccruals);
                    LoadVatPeriodTotals(CashFlow, includeActivePeriods, includeTaxAccruals);
                }

                if (includeBalanceSheet)
                    LoadBalanceSheet(CashFlow);

                dataContext.Close();
                CashFlow.EndInit();

                CashFlow.Activate();
                FreezeFrames();
                CashFlow.Protect();

                return true;
            }
            catch (Exception err)
            {
                dataContext = new DataContext(conn);
                dataContext.ErrorLog($"{ err.Source}.{ err.TargetSite.Name} {err.Message}");
                return false;
            }
        }

        public bool OpenBudget(SqlConnection conn, 
                bool includeActivePeriods = false, 
                bool includeOrderBook = false)
        {
            try
            {
                reportMode = ReportMode.Budget;
                CashFlow.Activate(); CashFlow.Range["A1"].Select();
                Budget.Unprotect();
                ClearWorksheet(Budget);

                dataContext = new DataContext(conn);
                dataContext.Open();

                SetActivePeriod(Budget, Properties.Resources.BudgetStatementTitle);
                InitialiseWorksheet(Budget);
                InitialisePeriods(Budget);

                LoadCategories(Budget, CashType.Trade, includeActivePeriods, includeOrderBook, false);

                dataContext.Close();

                Budget.Activate();
                FreezeFrames();
                Budget.Protect();

                return true;
            }
            catch (Exception err)
            {
                DataContext data = new DataContext(conn);
                data.ErrorLog($"{ err.Source}.{ err.TargetSite.Name} {err.Message}");
                return false;
            }
        }

        public void ClearWorksheets()
        {

            Budget.Unprotect();
            CashFlow.Unprotect();

            ClearWorksheet(Budget);
            ClearWorksheet(CashFlow);

            Budget.Protect();
            CashFlow.Protect();

            CashFlow.Activate();
            
        }

        #endregion

        #region Helper routines
        /// <summary>
        /// Converts a column number into an xls alpha-numeric column name
        /// </summary>
        /// <param name="column">column number</param>
        /// <returns>Alpha-numeric representation</returns>
        static string Column(int column)
        {
            const int startAscii = 65;
            const int endAscii = 90;

            if (column == 0) column = 26;
            if (column + startAscii - 1 > endAscii)
                return $"{Column((column - 1) / 26)}{Column(column % 26)}";
            else
            {
                int i = (startAscii + column - 1);
                byte[] b = new byte[1];
                b[0] = byte.Parse(i.ToString("X"), System.Globalization.NumberStyles.HexNumber);
                char[] c = ASCIIEncoding.UTF8.GetChars(b);
                return $"{c[0]}";
            }
        }

        static int CategoryRow(WorksheetBase ws, string categoryCode)
        {
            return ws.Range["C1"].EntireColumn.Find(What:categoryCode).Row;            
        }

        private void FreezeFrames()
        {
            Excel.Window activeWindow = Globals.ThisWorkbook.Application.ActiveWindow;
            activeWindow.SplitRow = 4;
            activeWindow.SplitColumn = 3;
            activeWindow.FreezePanes = true;
        }
        #endregion

        #region Format Worksheet
        void ClearWorksheet(WorksheetBase worksheet)
        {
            worksheet.UsedRange.ClearContents();
            worksheet.UsedRange.ClearFormats();
            worksheet.UsedRange.ClearComments();
            worksheet.UsedRange.ClearNotes();
            worksheet.UsedRange.Clear();
            worksheet.Cells.Clear();
        }

        void SetActivePeriod(WorksheetBase ws, string title)
        {
            var activePeriod = dataContext.ActivePeriod;
            ActiveYear = activePeriod.YearNumber;
            ActiveMonth = activePeriod.MonthNumber;
            ActiveDate = activePeriod.StartOn;

            ws.Range["A1"].Value2 = string.Format(title, activePeriod.MonthName, activePeriod.Description);
        }

        void InitialiseWorksheet(WorksheetBase ws)
        {
            ws.Cells.Font.Size = 8;
            ws.Range["A1"].Font.Size = 12;
            ws.Range["A1"].Font.Bold = true;

            ws.Range["A2"].Value2 = dataContext.CompanyName;
            ws.Range["A2"].Font.Size = 10;
            ws.Range["A2"].Font.Bold = true;

            ws.Range["A3"].Value2 = Properties.Resources.TextDate;
            ws.Range["A3"].EntireRow.Font.Size = 10;
            ws.Range["A3"].EntireRow.Font.Bold = true;
            ws.Range["B3"].Value2 = DateTime.Now.ToString("dd MMM HH:mm:ss");
            //ws.Range["B3"].NumberFormat = "dd mmmm yyyy";
            ws.Range["B2:B3"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            ws.Range["A4"].Value = Properties.Resources.TextCode;
            ws.Range["B4"].Value = Properties.Resources.TextName;
            ws.Range["A4"].EntireRow.Font.Size = 8;
            ws.Range["A4"].EntireRow.Font.Bold = true;

            ws.Range["A4"].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range["A4"].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Range["A4"].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
            ws.Range["A4"].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Range["B4"].EntireColumn.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThick;

            ws.Cells.Locked = false;
            ws.Range["A:C"].EntireColumn.Locked = true;
            ws.Range["1:4"].EntireRow.Locked = true;

        }

        void InitialisePeriods(WorksheetBase ws)
        {
            var months = dataContext.MonthNames;
            var years = dataContext.ActiveYears;

            curRow = 3;
            curCol = 4;

            foreach (Data.vwActiveYear year in years)
            {
                ws.Cells[3, curCol].Value = $"{year.Description} ({year.CashStatus})";
                curCol -= 1;

                foreach (Data.vwMonth month in months)
                {
                    curCol++;
                    ws.Cells[4, curCol].Value = month.MonthName;
                    if (year.YearNumber == ActiveYear && month.MonthNumber == ActiveMonth)
                    {
                        ws.Cells[1, curCol].EntireColumn.Cells.Interior.Color = Greyscale ? Color.LightGray : Color.Yellow;
                        periodCol = curCol;
                    }
                    ws.Columns[Column(curCol)].NumberFormat = Properties.Resources.FormatNumber;
                    ws.Cells[1, curCol].EntireColumn.ColumnWidth = 11;
                }

                curCol++;
                ws.Cells[3, curCol].Value = year.Description;
                ws.Cells[4, curCol].Value = Properties.Resources.TextTotals;
                ws.Cells[4, curCol].EntireColumn.Cells.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThick;
                ws.Cells[4, curCol].EntireColumn.Cells.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
                ws.Cells[4, curCol].EntireColumn.Locked = true;
                ws.Columns[Column(curCol)].NumberFormat = Properties.Resources.FormatNumber;
                ws.Columns[Column(curCol)].EntireColumn.ColumnWidth = 11;
                ws.Columns[Column(curCol)].EntireColumn.Font.Bold = true;
                curCol++;
            }

            lastCol = curCol - 1;
        }
        #endregion

        #region Load Trade Data
        private void LoadCategories(WorksheetBase ws, CashType cashType, bool includeActivePeriods, bool includeOrderBook, bool includeTaxAccruals)
        {
            int startRow;

            var categories = dataContext.Categories(cashType);
            var years = dataContext.ActiveYears;

            foreach (Data.fnFlowCategoryResult category in categories)
            {
                curRow += 2;
                ws.Range[$"A{curRow - 1}:A{curRow}"].EntireRow.Locked = true;
                startRow = curRow;
                ws.Cells[curRow, 1].Value = category.Category;
                ws.Cells[curRow, 1].Font.Bold = true;
                ws.Cells[curRow, 1].Font.Underline = false;
                if (!Greyscale) ws.Range[ws.Cells[curRow, 1], ws.Cells[curRow, 2]].Interior.Color = Color.LightYellow;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;

                var cashCodes = dataContext.CashCodes(category.CategoryCode);

                foreach (Data.fnFlowCategoryCashCodesResult cashCode in cashCodes)
                {
                    curRow++;
                    ws.Cells[curRow, 1].Value = cashCode.CashCode;
                    ws.Cells[curRow, 2].Value = cashCode.CashDescription;

                    short yearCount = 0;
                    foreach (Data.vwActiveYear year in years)
                    {
                        ++yearCount;
                        LoadCashCodeYear(ws, cashCode.CashCode, year.YearNumber, yearCount, curRow, includeActivePeriods, includeOrderBook, includeTaxAccruals);
                    }
                }

                curRow++;
                ws.Cells[curRow, 1].Value = Properties.Resources.TextTotals;
                ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;

                if (!Greyscale)
                {
                    switch ((CashMode)category.CashModeCode)
                    {
                        case CashMode.Expense:
                            ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.LightSalmon;
                            break;
                        case CashMode.Income:
                            ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.CornflowerBlue;
                            break;
                        case CashMode.Neutral:
                            ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.LightGray;
                            break;
                    }
                }

                if (!Greyscale) ws.Cells[curRow, 3].Font.Color = ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color;
                ws.Cells[curRow, 3].Value = "=" + "\"" + category.CategoryCode + "\"";

                for (int curCol = 4; curCol <= lastCol; curCol++)
                {
                    string formula = $"=SUM({Column(curCol)}{startRow + 1}:{Column(curCol)}{curRow - 1})";
                    if ((CashMode)category.CashModeCode == CashMode.Expense)
                        formula += "*-1";
                    ws.Cells[curRow, curCol].Formula = formula;
                }

                ws.Cells[curRow, 1].EntireRow.Locked = true;
            }


            if (categories.Count() < 2)
                return;

            curRow += 2;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextSummary;
            ws.Cells[curRow, 1].EntireRow.Locked = true;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            if (!Greyscale) ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.LightGreen;

            startRow = curRow;

            foreach (Data.fnFlowCategoryResult category in categories)
            {
                curRow++;
                ws.Cells[curRow, 1].Value = category.Category;
                int catRow = CategoryRow(ws, category.CategoryCode);
                for (int curX = 4; curX <= lastCol; curX++)
                    ws.Cells[curRow, curX].Formula = $"={Column(curX)}{catRow}";
                ws.Cells[curRow, 1].EntireRow.Locked = true;
            }

            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;

            curRow++;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextPeriodTotal;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;

            for (int curCol = 4; curCol <= lastCol; curCol++)
            {
                ws.Cells[curRow, curCol].Formula = $"=SUM({Column(curCol)}{startRow + 1}:{Column(curCol)}{curRow - 1})";
            }

            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Locked = true;
        }

        private void LoadCashCodeYear(WorksheetBase ws, string cashCode, short yearNumber, short yearCount, int row, bool includeActivePeriods, bool includeOrderBook, bool includeTaxAccruals)
        {
            int startCol, curCol;

            var cashData = dataContext.CashCodeValues(cashCode, yearNumber, includeActivePeriods, includeOrderBook, includeTaxAccruals);
            startCol = 4 + ((yearCount - 1) * 13);
            curCol = startCol;

            foreach (Data.proc_FlowCashCodeValuesResult codeValue in cashData)
            {
                switch (reportMode)
                {
                    case ReportMode.CashFlow:
                        ws.Cells[row, curCol].Value = codeValue.InvoiceValue;
                        break;
                    case ReportMode.Budget:
                        ws.Cells[row, curCol].Value = codeValue.InvoiceValue - codeValue.ForecastValue;
                        break;
                }
                curCol++;
            }

            ws.Cells[row, curCol].Formula = $"=SUM({Column(startCol)}{row}:{Column(curCol - 1)}{row})";
        }

        private void LoadTotals(WorksheetBase ws, CashType cashType, CategoryType categoryType)
        {
            var totals = dataContext.CategoriesByType(cashType, categoryType);

            if (totals.Count() < 2)
                return;

            curRow += 2;
            ws.Cells[curRow, 1].EntireRow.Locked = true;
            if (!Greyscale) ws.Cells[curRow, 1].EntireRow.Interior.Color = Color.GreenYellow;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].Value = $"{totals.Select(s => s.CashType).First()} {Properties.Resources.TextTotals}";
            ws.Cells[curRow, 1].Font.Bold = true;
            ws.Cells[curRow, 1].Font.Underline = false;

            foreach (Data.fnFlowCategoriesByTypeResult total in totals)
            {
                curRow++;
                ws.Cells[curRow, 1].EntireRow.Locked = true;
                ws.Cells[curRow, 1].Value = total.Category;
                ws.Cells[curRow, 3].Value = "=" + "\"" + total.CategoryCode + "\"";                
            }

            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
        }
        #endregion

        #region Dynamic Totals and Expressions
        private void LoadTotalsFormula(WorksheetBase ws)
        {
            int codeRow;

            var categories = dataContext.CategoryTotals;
            if (categories.Count() == 0)
                return;

            foreach (Data.vwCategoryTotal category in categories)
            {
                codeRow = CategoryRow(ws, category.CategoryCode);

                var totalCodes = dataContext.CategoryTotalCodes(category.CategoryCode);
                string formula , colName;

                for (int curCol = firstCol; curCol <= lastCol; curCol++)
                {
                    colName = Column(curCol);
                    formula = string.Empty;

                    foreach (string categoryCode in totalCodes)
                    {
                        if (formula.Length == 0)
                            formula = $"={colName}{CategoryRow(ws, categoryCode)}";
                        else
                            formula += $"+{colName}{CategoryRow(ws, categoryCode)}";
                    }

                    ws.Cells[codeRow, curCol].Formula = formula;
                }
            }

        }

        private void LoadExpressions(WorksheetBase ws)
        {
            
            var expressons = dataContext.CategoryExpressions;

            if (expressons.Count() == 0)
                return;

            curRow += 2;
            ws.Cells[curRow, 1].EntireRow.Locked = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            if (!Greyscale) ws.Cells[curRow, 1].EntireRow.Interior.Color = Color.LimeGreen;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextAnalysis;
            ws.Cells[curRow, 1].Font.Bold = true;
            ws.Cells[curRow, 1].Font.Underline = false;

            foreach (Data.vwCategoryExpression expression in expressons)
            {
                curRow++;
                ws.Cells[curRow, 1].EntireRow.Locked = true;
                ws.Cells[curRow, 1].Value = expression.Category;
                ws.Cells[curRow, 3].Value = "=" + "\"" + expression.CategoryCode + "\"";
                if (!Greyscale) ws.Cells[curRow, 3].Font.Color = ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color;

                ws.Range[$"A{curRow}"].EntireRow.NumberFormat = Properties.Resources.FormatPercent;
                ws.Range[$"D{curRow}"].EntireRow.FormatConditions.Add(Excel.XlFormatConditionType.xlCellValue, Excel.XlFormatConditionOperator.xlLess, "0").Font.Color = Color.Red;

                LoadExpression(ws, expression.Expression);
            }

            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
        }


        private void LoadExpression(WorksheetBase ws, string expressionTemplate)
        {
            string expression, category, categoryCode;
            int pos;

            Dictionary<string, int> categoryCodes = new Dictionary<string, int>();

            const char RB = ']';
            const char LB = '[';

            expression = expressionTemplate;
            pos = expression.IndexOf(LB, 0, expression.Length);

            while (pos > 0)
            {
                pos++;
                category = expression.Substring(pos, expression.IndexOf(RB, pos + 1) - pos);

                if (!categoryCodes.ContainsKey(category))
                {
                    categoryCode = dataContext.CategoryCodeFromName(category);
                    categoryCodes.Add(categoryCode, CategoryRow(ws, categoryCode));
                    expression = expression.Replace(category, categoryCode);
                }
                pos = expression.IndexOf(LB, pos, expression.Length - pos);
            }


            for (curCol = firstCol; curCol <= lastCol; curCol++)
            {
                string formula = expression;
                foreach (var code in categoryCodes)
                    formula = formula.Replace($"{LB}{code.Key}{RB}", $"{Column(curCol)}{code.Value}");
                try
                {
                    ws.Cells[curRow, curCol].Formula = $"={formula}";
                }
                catch
                {
                    ws.Cells[curRow, curCol].Formula = $"{formula}";
                }
            }
        }

        #endregion

        #region Bank Balances and Vat Details
        private void LoadBankBalances(WSCashFlow ws)
        {
            var bankAccounts = dataContext.BankAccounts;

            if (bankAccounts.Count() == 0)
                return;

            curRow += 2;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextClosingBalances;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            if (!Greyscale)
            {
                ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.Black;
                ws.Cells[curRow, 1].EntireRow.Cells.Font.Color = Color.White;
                ws.Cells[curRow, 3].Font.Color = Color.White;
            }

            int startRow = curRow;

            foreach (Data.vwBankAccount bankAccount in bankAccounts)
            {
                short yearNumber = 0;

                curRow++;
                ws.Cells[curRow, 1].Value = bankAccount.CashAccountName;

                var balances = dataContext.BankBalances(bankAccount.CashAccountCode);
                foreach (Data.fnFlowBankBalancesResult balance in balances)
                {
                    if (yearNumber == 0)
                    {
                        curCol = firstCol;
                        yearNumber = balance.YearNumber;
                    }

                    if (yearNumber != balance.YearNumber)
                    {
                        ws.Cells[curRow, curCol].Formula = $"={Column(curCol-1)}{curRow}";
                        yearNumber = balance.YearNumber;
                        curCol++;
                    }

                    if (balance.Balance != null)
                    {
                        ws.Cells[curRow, curCol].Value = balance?.Balance;
                        curCol++;
                    }
                    else
                        curCol++;                   
                }

                ws.Cells[curRow, curCol].Formula = $"={Column(curCol - 1)}{curRow}";
                ws.Cells[curRow, 1].EntireColumn.Locked = true;
            }

            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;

            curRow++;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextCompanyBalance;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
            ws.Cells[curRow, 1].EntireRow.Locked = true;

            for (int curCol = firstCol; curCol <= lastCol; curCol++)
            {
                ws.Cells[curRow, curCol].Formula = $"=SUM({Column(curCol)}{startRow + 1}:{Column(curCol)}{curRow - 1})";
            }
        }

        private void LoadVatRecurrenceTotals(WSCashFlow ws, bool includeActivePeriods = false, bool includeTaxAccruals = false)
        {
            curRow += 2;
            ws.Cells[curRow, 1].Value = $"{Properties.Resources.TextVatDueTitle} {dataContext.VatRecurrenceType}";
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            if (!Greyscale)
            {
                ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.Orchid;
                ws.Cells[curRow, 1].EntireRow.Cells.Font.Color = Color.Black;
                ws.Cells[curRow, 3].Font.Color = Color.White;
            }

            VatDetailCodes(ws);

            int startCol = firstCol;
            short yearNumber = 0;
            curRow++;

            var vat_recurrence = dataContext.VatRecurrence;

            foreach (vwFlowVatRecurrence vat_period in vat_recurrence)
            {
                if (yearNumber == 0)
                {
                    curCol = startCol;
                    yearNumber = vat_period.YearNumber;
                }

                if (yearNumber != vat_period.YearNumber)
                {
                    for (int offset = 0; offset < 10; offset++)
                        ws.Cells[curRow + offset, curCol].Formula = $"=SUM({Column(startCol)}{curRow + offset}:{Column(curCol - 1)}{curRow + offset})";
                    yearNumber = vat_period.YearNumber;
                    curCol++;
                    startCol = curCol;
                }

                if (includeActivePeriods || (vat_period.StartOn < ActiveDate))
                {
                    ws.Cells[curRow, curCol].Value = vat_period?.HomeSales;
                    ws.Cells[curRow + 1, curCol].Value = vat_period?.HomePurchases;
                    ws.Cells[curRow + 2, curCol].Value = vat_period?.ExportSales;
                    ws.Cells[curRow + 3, curCol].Value = vat_period?.ExportPurchases;
                    ws.Cells[curRow + 4, curCol].Value = vat_period?.HomeSalesVat;
                    ws.Cells[curRow + 5, curCol].Value = vat_period?.HomePurchasesVat;
                    ws.Cells[curRow + 6, curCol].Value = vat_period?.ExportSalesVat;
                    ws.Cells[curRow + 7, curCol].Value = vat_period?.ExportPurchasesVat;
                    ws.Cells[curRow + 8, curCol].Value = vat_period?.VatAdjustment;
                    ws.Cells[curRow + 9, curCol].Value = vat_period?.VatDue;
                }
                else
                    ws.Range[$"{Column(curCol)}{curRow}:{Column(curCol)}{curRow + 9}"].Value = 0;

                curCol++;
            }

            for (int offset = 0; offset < 10; offset++)
                ws.Cells[curRow + offset, curCol].Formula = $"=SUM({Column(startCol)}{curRow + offset}:{Column(curCol - 1)}{curRow + offset})";

            if (includeTaxAccruals)
                LoadVatRecurrenceAccruals(ws);

            curRow += 9;
        }

        private void LoadVatRecurrenceAccruals(WSCashFlow ws)
        {
            short yearNumber = 0;
            curCol = firstCol;

            var vat_accruals = dataContext.VatRecurrenceAccruals;

            foreach (vwFlowVatRecurrenceAccrual vat_period in vat_accruals)
            {
                if (yearNumber == 0)
                    yearNumber = vat_period.YearNumber;

                if (yearNumber != vat_period.YearNumber)
                {
                    yearNumber = vat_period.YearNumber;
                    curCol++;
                }

                ws.Cells[curRow, curCol].Value = (decimal)ws.Cells[curRow, curCol].Value + vat_period?.HomeSales;
                ws.Cells[curRow + 1, curCol].Value = (decimal)ws.Cells[curRow + 1, curCol].Value + vat_period?.HomePurchases;
                ws.Cells[curRow + 2, curCol].Value = (decimal)ws.Cells[curRow + 2, curCol].Value + vat_period?.ExportSales;
                ws.Cells[curRow + 3, curCol].Value = (decimal)ws.Cells[curRow + 3, curCol].Value + vat_period?.ExportPurchases;
                ws.Cells[curRow + 4, curCol].Value = (decimal)ws.Cells[curRow + 4, curCol].Value + vat_period?.HomeSalesVat;
                ws.Cells[curRow + 5, curCol].Value = (decimal)ws.Cells[curRow + 5, curCol].Value + vat_period?.HomePurchasesVat;
                ws.Cells[curRow + 6, curCol].Value = (decimal)ws.Cells[curRow + 6, curCol].Value + vat_period?.ExportSalesVat;
                ws.Cells[curRow + 7, curCol].Value = (decimal)ws.Cells[curRow + 7, curCol].Value + vat_period?.ExportPurchasesVat;
                ws.Cells[curRow + 9, curCol].Value = (decimal)ws.Cells[curRow + 9, curCol].Value + vat_period?.VatDue;

                curCol++;
            }
        }

        private void LoadVatPeriodTotals(WSCashFlow ws, bool includeActivePeriods = false, bool includeTaxAccruals = false)
        {
            curRow += 2;
            ws.Cells[curRow, 1].Value = $"{Properties.Resources.TextVatDueTitle} {Properties.Resources.TextTotals}";
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            if (!Greyscale)
            {
                ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.Orchid;
                ws.Cells[curRow, 1].EntireRow.Cells.Font.Color = Color.Black;
                ws.Cells[curRow, 3].Font.Color = Color.White;
            }

            VatDetailCodes(ws, false);

            int startCol = firstCol;
            short yearNumber = 0;
            curRow++;

            var vat_totals = dataContext.VatPeriodTotals;

            foreach (vwFlowVatPeriodTotal vat_period in vat_totals)
            {
                if (yearNumber == 0)
                {
                    curCol = startCol;
                    yearNumber = vat_period.YearNumber;
                }

                if (yearNumber != vat_period.YearNumber)
                {
                    for (int offset = 0; offset < 9; offset++)
                        ws.Cells[curRow + offset, curCol].Formula = $"=SUM({Column(startCol)}{curRow + offset}:{Column(curCol - 1)}{curRow + offset})";
                    yearNumber = vat_period.YearNumber;
                    curCol++;
                    startCol = curCol;
                }

                if (includeActivePeriods || (vat_period.StartOn < ActiveDate))
                {
                    ws.Cells[curRow, curCol].Value = vat_period?.HomeSales;
                    ws.Cells[curRow + 1, curCol].Value = vat_period?.HomePurchases;
                    ws.Cells[curRow + 2, curCol].Value = vat_period?.ExportSales;
                    ws.Cells[curRow + 3, curCol].Value = vat_period?.ExportPurchases;
                    ws.Cells[curRow + 4, curCol].Value = vat_period?.HomeSalesVat;
                    ws.Cells[curRow + 5, curCol].Value = vat_period?.HomePurchasesVat;
                    ws.Cells[curRow + 6, curCol].Value = vat_period?.ExportSalesVat;
                    ws.Cells[curRow + 7, curCol].Value = vat_period?.ExportPurchasesVat;
                    ws.Cells[curRow + 8, curCol].Value = vat_period?.VatDue;
                }
                else
                    ws.Range[$"{Column(curCol)}{curRow}:{Column(curCol)}{curRow + 8}"].Value = 0;

                curCol++;
            }

            for (int offset = 0; offset < 9; offset++)
                ws.Cells[curRow + offset, curCol].Formula = $"=SUM({Column(startCol)}{curRow + offset}:{Column(curCol - 1)}{curRow + offset})";

            if (includeTaxAccruals)
                LoadVatPeriodAccruals(ws);

            curRow += 8;
        }

        private void LoadVatPeriodAccruals(WSCashFlow ws)
        {
            short yearNumber = 0;

            curCol = firstCol;

            var vat_accruals = dataContext.VatPeriodAccruals;

            foreach (vwFlowVatPeriodAccrual vat_period in vat_accruals)
            {
                if (yearNumber == 0)
                    yearNumber = vat_period.YearNumber;

                if (yearNumber != vat_period.YearNumber)
                {
                    yearNumber = vat_period.YearNumber;
                    curCol++;
                }

                ws.Cells[curRow, curCol].Value = (decimal)ws.Cells[curRow, curCol].Value + vat_period?.HomeSales;
                ws.Cells[curRow + 1, curCol].Value = (decimal)ws.Cells[curRow + 1, curCol].Value + vat_period?.HomePurchases;
                ws.Cells[curRow + 2, curCol].Value = (decimal)ws.Cells[curRow + 2, curCol].Value + vat_period?.ExportSales;
                ws.Cells[curRow + 3, curCol].Value = (decimal)ws.Cells[curRow + 3, curCol].Value + vat_period?.ExportPurchases;
                ws.Cells[curRow + 4, curCol].Value = (decimal)ws.Cells[curRow + 4, curCol].Value + vat_period?.HomeSalesVat;
                ws.Cells[curRow + 5, curCol].Value = (decimal)ws.Cells[curRow + 5, curCol].Value + vat_period?.HomePurchasesVat;
                ws.Cells[curRow + 6, curCol].Value = (decimal)ws.Cells[curRow + 6, curCol].Value + vat_period?.ExportSalesVat;
                ws.Cells[curRow + 7, curCol].Value = (decimal)ws.Cells[curRow + 7, curCol].Value + vat_period?.ExportPurchasesVat;
                ws.Cells[curRow + 8, curCol].Value = (decimal)ws.Cells[curRow + 8, curCol].Value + vat_period?.VatDue;

                curCol++;
            }            
        }

        private void VatDetailCodes(WSCashFlow ws, bool includeAdjustments = true)
        {
            int row = curRow;

            ws.Cells[++row, 1].Value = Properties.Resources.TextVatHomeSales;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatHomePurchases;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatExportSales;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatExportPurchases;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatHomeSalesVat;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatHomePurchasesVat;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatExportSalesVat;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatExportPurchasesVat;
            if (includeAdjustments)
                ws.Cells[++row, 1].Value = Properties.Resources.TextVatAdjustment;
            ws.Cells[++row, 1].Value = Properties.Resources.TextVatDue;

            ws.Cells[row, 1].EntireRow.Font.Bold = true;
            ws.Cells[row, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[row, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
            ws.Cells[row, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[row, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
        }

        #endregion

        #region balance sheet
        private void LoadBalanceSheet(WSCashFlow ws)
        {
            var balance_sheet = dataContext.BalanceSheet;

            if (balance_sheet.Count() == 0)
                return;

            curRow += 2;

            ws.Cells[curRow, 1].Value = Properties.Resources.TextBalanceSheet;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;

            if (!Greyscale)
            {
                ws.Cells[curRow, 1].EntireRow.Cells.Interior.Color = Color.Red;
                ws.Cells[curRow, 1].EntireRow.Cells.Font.Color = Color.White;
                ws.Cells[curRow, 3].Font.Color = Color.White;
            }

            int startRow = curRow;
            int offset = 1;
            int yearCol = firstCol;
            string assetName = string.Empty;

            foreach (var entry in balance_sheet)
            {
                if (entry.AssetName != assetName)
                {
                    if (assetName != string.Empty)
                        ws.Cells[curRow, curCol].Formula = $"={Column(curCol - offset)}{curRow}";

                    curRow++;
                    assetName = entry.AssetName;
                    ws.Cells[curRow, 1].Value = entry.AssetCode;
                    ws.Cells[curRow, 2].Value = entry.AssetName;
                    curCol = firstCol;
                    yearCol = firstCol;
                }

                if (((curCol - yearCol) % 12 == 0) && (curCol != yearCol))
                {
                    ws.Cells[curRow, curCol].Formula = $"={Column(curCol - offset)}{curRow}";
                    curCol++;
                    yearCol = curCol;
                }
                
                ws.Cells[curRow, curCol].Value = entry.Balance;
                
                curCol++;
            }

            ws.Cells[curRow, curCol].Formula = $"={Column(curCol - offset)}{curRow}";

            curRow++;
            ws.Cells[curRow, 1].Value = Properties.Resources.TextCapital;
            ws.Cells[curRow, 1].EntireRow.Font.Bold = true;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThin;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            ws.Cells[curRow, 1].EntireRow.Cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
            ws.Cells[curRow, 1].EntireRow.Locked = true;

            yearCol = firstCol;

            for (int curCol = firstCol; curCol <= lastCol+1; curCol++)
            {
                if (((curCol - yearCol) % 12 == 0) && (curCol != yearCol))
                {
                    ws.Cells[curRow, curCol].Formula = $"={Column(curCol - offset)}{curRow}";
                    yearCol = curCol + 1;
                }
                else
                    ws.Cells[curRow, curCol].Formula = $"=SUM({Column(curCol)}{startRow + 1}:{Column(curCol)}{curRow - 1})";
            }
        }

        #endregion


    }
}
