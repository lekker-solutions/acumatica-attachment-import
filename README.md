# Acumatica File Attachment Import
##### Utility for Bulk Importing of Attachments to Acumatica


This utility takes a .csv file with a combination of Record Type, Key values of that record, file path of the soon to be attachment, and an optional fileName, and uploads the file to the specified record

#### Csv Format

Doc Type | File Path | File Name (optional) | Key 1 | Key 2 | Key 3 | Key 4 | Key 5
-------- | --------- | -------------------- | ----- | ----- | ----- | ----- | -----
SalesOrder | C:\Temp\attachment.pdf | The utility will take 'attachment.pdf' if this is blank | SO | 00001| | | 


## Supported Records


Doc Type |
-------- |
SalesOrder |
PurchaseOrder |
ARInvoice |
APInvoice |
ARPayment |
APPayment |



