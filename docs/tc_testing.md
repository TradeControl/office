# Installation Test

The following is a record of the pre-release installation test for Trade Control. You require an Office 365 Business subscription. Consult the installation documentation for more details.

## Backend

1. Log into the Azure Portal and add a new Sql database:

> Create a new Sql Server with an admin username and password.
Then add an Sql database on the server called **tcDemo**, configured to Basic.  

2. Commission a new Virtual Machine: Windows Server 2019 Standard DS1 v2 (1 vcpus, 3.5 GiB memory). Networking inbound security RDP enabled.

3. Open the **tcDemo** database and add the VM IP Address to the firewall.

4. Login and turn off IE Enhanced Security and download [tcNodeConfigSetup.zip](https://github.com/tradecontrol/tc-nodecore). Follow the configuration instructions until you have installed one of the demos. 

## Frontend

1. Log into your 365 account and install the 64 bit version of Office.

2. Install ODBC Driver 17 for Sql Server and create a 64 bit ODBC Data Source.
  
3. Extract the following zip files into their own folder: 

- [tcOfficeClient](../src/installation/tcOfficeClient.zip)
- [tcOfficeTemplates](../src/installation/tcOfficeTemplates.zip)
- [tcOfficeMTD](../src/installation/tcOfficeMTD.zip)

4. Install the Office Client and the MTD. Open the TC-Office Client.

5. Specify the Data Source: delete the default Windows connection and modify the Azure template by replacing the tags with their actual values.

6. Connect and accept the Administration settings.

7. From _tcOfficeTemplates\CashFlow_ open the Excel Template, accepting the installation request. 

8. In the new workbook's action pane, enter the connection details (the same entered for the backend configuration). Press the Cash Flow button to test, then save the xlsx on the Desktop.

9. Copy _tcOfficeTemplates\Templates_ to a shared location. Open the templates required and install them. Ignore the missing XML dialog.
 
10. From the client open the Administrator/Documents page. For each document type, use Open Template to specify the location and name of its associated Word templates.

11. Open the Invoice Register. Edit an invoice and try printing it out as a Word document.

## Licence

The Trade Control Code licence is issued by Trade Control Ltd under a [GNU General Public Licence v3.0](https://www.gnu.org/licenses/gpl-3.0.en.html) 

Trade Control Documentation by Trade Control Ltd is licenced under a [Creative Commons Attribution-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-sa/4.0/) 

![Creative Commons](https://i.creativecommons.org/l/by-sa/4.0/88x31.png) 



