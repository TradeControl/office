# Change Log

The following record logs changes to Trade Control, first released on **2019.09.24**. Checked items are included in the latest master commit.

## 3.9

- [x] Remove SNK signing from VSTO templates
- [x] Invoice Register - sales invoice report query not found [3.9.2]
- [x] Task Edit - activity code load item not found [3.9.2]
- [x] Task Edit - load cash vat code instead of organisation [3.9.2]
- [x] Documents - add bank details to default Sales Invoice [3.9.2]
- [x] Documents - activate email as pdf [3.9.2]
- [x] Documents - locate email address by spooled flag [3.9.2]
- [x] Payment Entry - NI default value incorrect [3.9.2] 
 
## 3.10

- [x] 64 bit release 
- [x] ClickOnce manifests Sectigo RSA code signing

## 3.11

Includes interface changes required for the [Trade Control Network](https://github.com/tradecontrol/tc-network) project

- [x] Org transmission type
- [x] Change log pages in Task and Invoice Editors 
- [x] Move service log and task/invoice change logs to Administration
- [x] Cleardown logs function in Administration
- [x] Purchase Order Delivery template not digitally signed
- [x] Manual open flag for generating Word template data
- [x] Opening Organisation Edit in datasheet view generates an error

## 3.12

Node version [3.26.1](https://github.com/tradecontrol/tc-nodecore)

- [x] Change ```FLOAT``` types to ```DECIMAL``` - VSTO Cash Flow, COM Interop assembly, Document Schema and client.

## 3.13

Node version [3.27.1](https://github.com/tradecontrol/tc-nodecore). First release of the [Trade Control Network](https://github.com/tradecontrol/tc-network)

- [x] Network Allocations
- [x] Network Invoices
- [x] Cash Code mirrors
- [x] Activity Code mirrors

## 3.14

Node version [3.28.3](https://github.com/tradecontrol/tc-nodecore). Changes for the [Trade Control Bitcoin](https://github.com/tradecontrol/tc-bitcoin) project.

- [x] Remove default windows currency symbol and convert money types to ```DECIMAL(18,5)```
- [x] Add bitcoin payment address to invoice mirrors
- [x] Hide payment entry and account transfer forms if Unit of Charge is BTC
- [x] Bitcoin miner cash code and account in Administration for fee processing
- [x] VSTO templates: data context money fields to decimal
- [x] MTD decimals v1.2.2

## 3.15

Node version [3.29.2](https://github.com/tradecontrol/tc-nodecore). [Balance Sheets](docs/tc_demo_balance_sheets.md)

- [x] Account types in cash accounts
- [x] Asset entry form
- [x] Global coin type in Administration options
- [x] Asset categories in Cash Totals for Gross/Net Profit calculations
- [x] Fix CashFlow.xlsx vat decimal isssue
- [x] Fix closing bank balances without period transactions
- [x] Asset movement and depreciation in CashFlow.xlsx P&L 
- [x] New Balance Sheet option in CashFlow.xlsx
- [x] Client _clsInit_ class - on connect, configure the client to use the OS UOA where coin type is Fiat (Bitcoin UOA is not supported by the OS and the ```money``` type has insufficient decimals).

## 3.16

Node version [3.29.4](https://github.com/tradecontrol/tc-nodecore). 

- [x] Add command timeout to the Cash Flow actions pane (default 60 secs)
- [x] Fix P&L asset liabilities polarity error
- [x] [Balance Sheet Audit Report](docs/Org_BalanceSheetAudit.pdf) 
- [x] [Organisation Statement Report](docs/Org_Statement.pdf)
- [x] [Debtor/Creditor Audit Report](docs/Org_AssetStatementAudit.pdf)
- [x] Remove references to Paid Values on Invoice Details and Cash Statements
