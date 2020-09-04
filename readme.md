# Trade Control - Office

An MS Office client for the [Trade Control](https://github.com/tradecontrol/tc-nodecore) management information system.

It is a free open source alternative to conventional accounting and order processing apps for Office Business 365 subscribers.


## Installation

[Change Log](changelog.md)

[Instructions](docs/tc_client_installation.md)

## Demos

- [Modelling a Bill of Materials](docs/tc_demo_manufacturing.md): a theoretical demonstration, wherein a bill of materials is modelled and ordered. It reveals the manufacturing origins of the design, but also more advanced features that you could apply when working out how to model your own activities. 

- [MIS for SMEs](docs/tc_demo_services.md): a practical implementation for a fictitious print management company. This is most likely the way you would use the app in your own business. 

- [Generating a Balance Sheet](docs/tc_demo_balance_sheets.md): how to generate your company accounts without the services of an  accountant.

Supporting material describes how [cash transactions](docs/tc_cash_codes.md) are modelled.

For supply-chain scheduling, refer to the demo in the [Trade Control Network](https://github.com/tradecontrol/tc-network) repository. If you are interested in making Bitcoin your Unit of Account, install the [Commercial Wallet](https://github.com/tradecontrol/tc-bitcoin) and follow the instructions provided.

### Visual Studio Solution

The repository can be opened in VS 2019 from TradeControl.sln in src. The solution requires the following dependencies from the VS Installer.

- .NET Development Tools 
- Visual Studio Tools for Office 
- Linq to Sql Tools 

Then add the extensions:

- Visual Studio Installer Projects
- [A Markdown Editor](https://github.com/madskristensen/MarkdownEditor)
 
## Versioning

Each client is released with a [SemVer](http://semver.org/) of its own and a backend version that it is compatible with. When connecting to the Azure Sql instance, the versions are compared, and the connection is rejected if they do not match. This information is stored in the hidden table _tbLocalVersion_. 

## Donations

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=C55YGUTBJ4N36)

## Credits

[Greenprint Consultancy Ltd](http://www.green-print.net), environmentally responsible printing services.

## Licence

The Trade Control Code licence is issued by Trade Control Ltd under a [GNU General Public Licence v3.0](https://www.gnu.org/licenses/gpl-3.0.en.html) 

Trade Control Documentation by Trade Control Ltd is licenced under a [Creative Commons Attribution-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-sa/4.0/) 

![Creative Commons](https://i.creativecommons.org/l/by-sa/4.0/88x31.png) 




