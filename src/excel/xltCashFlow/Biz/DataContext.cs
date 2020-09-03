using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;

namespace TradeControl.CashFlow
{
    public enum EventLogType { Error, Warning, Information };
    public enum CashType { Trade, Tax, Money };
    public enum ReportMode { CashFlow, Budget };
    public enum CashMode { Expense, Income, Neutral };    
    public enum CategoryType { CashCode, Total, Expression };
    public enum TaxType { CorporationTax, Vat, NI, General };

    public class DataContext
    {
        Data.dbTradeControlDataContext db;

        public DataContext(SqlConnection connection)
        {
            db = new Data.dbTradeControlDataContext(connection);
        }

        #region public methods
        public string ErrorLog(string message)
        {
            string logCode = string.Empty;

            Open();

            db.proc_EventLog(message, (short)EventLogType.Error, ref logCode);

            Close();

            return logCode;

        }

        public void Open()
        {
            if (db.Connection.State != ConnectionState.Open)
                db.Connection.Open();
        }

        public void Close()
        {
            db.Connection.Close();
        }

        #endregion

        #region datasets
        public Data.vwActivePeriod ActivePeriod
        {
            get
            {
                return db.vwActivePeriods.Select(f => f).First();   
            }
        }

        public string CompanyName
        {
            get
            {
                return db.vwHomeAccounts.Select(f => f).First().AccountName;
            }
        }

        public IOrderedQueryable<Data.vwActiveYear> ActiveYears
        {
            get
            {
                return db.vwActiveYears.Select(f => f).OrderBy(y => y.YearNumber);
            }
        }

        public IOrderedQueryable<Data.vwMonth> MonthNames
        {
            get
            {
                return db.vwMonths.Select(f => f).OrderBy(m => m.StartOn);
            }
        }

        public string VatRecurrenceType
        {
            get
            {
                return db.vwFlowTaxTypes.Where(t => t.TaxTypeCode == (short)TaxType.Vat).Select(s => s.Recurrence).Single().ToUpper();
            }
        }
        public IOrderedQueryable<Data.fnFlowCategoryResult> Categories(CashType cashType)
        {
            return from r in db.fnFlowCategory((short)cashType)
                   orderby r.DisplayOrder, r.Category
                   select r;
        }

        public IOrderedQueryable<Data.fnFlowCategoryCashCodesResult> CashCodes(string categoryCode)
        {
            return from r in db.fnFlowCategoryCashCodes(categoryCode)
                   orderby r.CashDescription
                   select r;
        }

        public IOrderedEnumerable<Data.proc_FlowCashCodeValuesResult> CashCodeValues(string cashCode, short yearNumber, bool includeActivePeriods, bool includeOrderBook, bool includeTaxAccruals)
        {
            return from r in db.proc_FlowCashCodeValues(cashCode, yearNumber, includeActivePeriods, includeOrderBook, includeTaxAccruals)
                   orderby r.StartOn
                   select r;
        }

        public IOrderedQueryable<Data.fnFlowCategoriesByTypeResult> CategoriesByType(CashType cashType, CategoryType categoryType )
        {
            return from r in db.fnFlowCategoriesByType((short)cashType, (short)categoryType)
                   orderby r.DisplayOrder, r.Category
                   select r;
        }

        public IOrderedQueryable<Data.vwCategoryTotal> CategoryTotals
        {
            get
            {
                return from r in db.vwCategoryTotals
                       orderby r.DisplayOrder, r.Category
                       select r;
            }
        }

        public List<string> CategoryTotalCodes(string categoryCode)
        {
            return (from r in db.fnFlowCategoryTotalCodes(categoryCode)
                   orderby r.CategoryCode
                   select r.CategoryCode).ToList();                   
        }

        public IOrderedQueryable<Data.vwCategoryExpression> CategoryExpressions
        {
            get
            {
                return from r in db.vwCategoryExpressions
                       orderby r.DisplayOrder, r.Category
                       select r;
            }
        }

        public string CategoryCodeFromName(string category)
        {
            string categoryCode = string.Empty;
            db.proc_FlowCategoryCodeFromName(category, ref categoryCode);
            return categoryCode;
        }

        public IOrderedQueryable<Data.vwBankAccount> BankAccounts
        {
            get
            {
                return from r in db.vwBankAccounts
                       orderby r.DisplayOrder, r.CashAccountCode
                       select r;
            }
        }

        public IOrderedQueryable<Data.fnFlowBankBalancesResult> BankBalances(string cashAccountCode)
        {
            return from r in db.fnFlowBankBalances(cashAccountCode)
                   orderby r.YearNumber, r.StartOn
                   select r;
        }

        public IOrderedQueryable<Data.vwFlowVatRecurrence> VatRecurrence
        {
            get
            {
                return from r in db.vwFlowVatRecurrences
                       orderby r.StartOn
                       select r;
            }
        }

        public IOrderedQueryable<Data.vwFlowVatPeriodTotal> VatPeriodTotals
        {
            get
            {
                return from r in db.vwFlowVatPeriodTotals
                       orderby r.StartOn
                       select r;
            }
        }

        public IOrderedQueryable<Data.vwFlowVatPeriodAccrual> VatPeriodAccruals
        {
            get
            {
                return from r in db.vwFlowVatPeriodAccruals
                       orderby r.StartOn
                       select r;
            }
        }

        public IOrderedQueryable<Data.vwFlowVatRecurrenceAccrual> VatRecurrenceAccruals
        {
            get
            {
                return from r in db.vwFlowVatRecurrenceAccruals
                       orderby r.StartOn
                       select r;
            }
        }

        public IOrderedQueryable<Data.vwBalanceSheet> BalanceSheet
        {
            get
            {
                return from r in db.vwBalanceSheets
                       orderby r.EntryNumber
                       select r;
            }
        }
        #endregion
    }
}
