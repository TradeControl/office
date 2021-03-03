﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradeControl.Tax.Office
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="tcNode")]
	public partial class dbTradeControlDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public dbTradeControlDataContext() : 
				base(global::TradeControl.Tax.Office.Properties.Settings.Default.tcNodeConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public dbTradeControlDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public dbTradeControlDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public dbTradeControlDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public dbTradeControlDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<vwTaxVatTotal> vwTaxVatTotals
		{
			get
			{
				return this.GetTable<vwTaxVatTotal>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="Cash.vwTaxVatTotals")]
	public partial class vwTaxVatTotal
	{
		
		private short _YearNumber;
		
		private string _Description;
		
		private string _Period;
		
		private System.Nullable<System.DateTime> _StartOn;
		
		private System.Nullable<double> _HomeSales;
		
		private System.Nullable<double> _HomePurchases;
		
		private System.Nullable<double> _ExportSales;
		
		private System.Nullable<double> _ExportPurchases;
		
		private System.Nullable<double> _HomeSalesVat;
		
		private System.Nullable<double> _HomePurchasesVat;
		
		private System.Nullable<double> _ExportSalesVat;
		
		private System.Nullable<double> _ExportPurchasesVat;
		
		private System.Nullable<double> _VatAdjustment;
		
		private System.Nullable<double> _VatDue;
		
		public vwTaxVatTotal()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_YearNumber", DbType="SmallInt NOT NULL")]
		public short YearNumber
		{
			get
			{
				return this._YearNumber;
			}
			set
			{
				if ((this._YearNumber != value))
				{
					this._YearNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this._Description = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Period", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string Period
		{
			get
			{
				return this._Period;
			}
			set
			{
				if ((this._Period != value))
				{
					this._Period = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> StartOn
		{
			get
			{
				return this._StartOn;
			}
			set
			{
				if ((this._StartOn != value))
				{
					this._StartOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomeSales", DbType="Float")]
		public System.Nullable<double> HomeSales
		{
			get
			{
				return this._HomeSales;
			}
			set
			{
				if ((this._HomeSales != value))
				{
					this._HomeSales = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomePurchases", DbType="Float")]
		public System.Nullable<double> HomePurchases
		{
			get
			{
				return this._HomePurchases;
			}
			set
			{
				if ((this._HomePurchases != value))
				{
					this._HomePurchases = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExportSales", DbType="Float")]
		public System.Nullable<double> ExportSales
		{
			get
			{
				return this._ExportSales;
			}
			set
			{
				if ((this._ExportSales != value))
				{
					this._ExportSales = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExportPurchases", DbType="Float")]
		public System.Nullable<double> ExportPurchases
		{
			get
			{
				return this._ExportPurchases;
			}
			set
			{
				if ((this._ExportPurchases != value))
				{
					this._ExportPurchases = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomeSalesVat", DbType="Float")]
		public System.Nullable<double> HomeSalesVat
		{
			get
			{
				return this._HomeSalesVat;
			}
			set
			{
				if ((this._HomeSalesVat != value))
				{
					this._HomeSalesVat = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HomePurchasesVat", DbType="Float")]
		public System.Nullable<double> HomePurchasesVat
		{
			get
			{
				return this._HomePurchasesVat;
			}
			set
			{
				if ((this._HomePurchasesVat != value))
				{
					this._HomePurchasesVat = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExportSalesVat", DbType="Float")]
		public System.Nullable<double> ExportSalesVat
		{
			get
			{
				return this._ExportSalesVat;
			}
			set
			{
				if ((this._ExportSalesVat != value))
				{
					this._ExportSalesVat = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExportPurchasesVat", DbType="Float")]
		public System.Nullable<double> ExportPurchasesVat
		{
			get
			{
				return this._ExportPurchasesVat;
			}
			set
			{
				if ((this._ExportPurchasesVat != value))
				{
					this._ExportPurchasesVat = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VatAdjustment", DbType="Float")]
		public System.Nullable<double> VatAdjustment
		{
			get
			{
				return this._VatAdjustment;
			}
			set
			{
				if ((this._VatAdjustment != value))
				{
					this._VatAdjustment = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VatDue", DbType="Float")]
		public System.Nullable<double> VatDue
		{
			get
			{
				return this._VatDue;
			}
			set
			{
				if ((this._VatDue != value))
				{
					this._VatDue = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
