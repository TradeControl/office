using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeControl.Documents.Word
{
    public enum DocType { Quotation, SalesOrder, PurchaseEnquiry, PurchaseOrder, SalesInvoice, CreditNote, DebitNote };
    public enum AttributeType { Order, Quote };
    public enum TaskStatus { Pending, Opened, Closed, Charged, Cancelled, Archived };

    public static class Conversion
    {

        /// <summary>
        /// Returns decimal in string format 1,000
        /// </summary>
        static public string ToQuantity(this decimal _quantity)
        {
            double convValue = (double)_quantity;
            return convValue.ToString("#,#");
        }

        static public string ToQuantity(this double _quantity)
        {
            return _quantity.ToString("#,#");
        }

        /// <summary>
        /// Returns decimal in string format £1,000.00
        /// </summary>
        static public string ToCurrency(this decimal _currency)
        {
            double convValue = (double)_currency;
            return convValue.ToString("C");
        }

        static public string ToCurrency(this double _currency)
        {
            return _currency.ToString("C");
        }
        /// <summary>
        /// Returns decimal in string format £1.0000
        /// </summary>
        static public string ToCurrency4dp(this decimal _currency)
        {
            double convValue = (double)_currency;
            return convValue.ToString("C4");
        }

        static public string ToCurrency4dp(this double _currency)
        {
            return _currency.ToString("C4");
        }

        /// <summary>
        /// Returns decimal in string format 1.0%
        /// </summary>
        static public string ToPercentage(this decimal _percent)
        {
            double convValue = (double)_percent;
            return convValue.ToString("P1");
        }

        static public string ToPercentage(this double _percent)
        {
            return _percent.ToString("P1");
        }

        static public string ToPercentage(this float _percent)
        {
            return _percent.ToString("P1");
        }
    }
}
