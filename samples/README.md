# Sample Data

this folder contains example CSV files for testing the reconciliation tool.

## Files

### Invoices
- **FolderA/invoices.csv** - 5 invoices (INV001-INV005)
- **FolderB/invoices.csv** - 5 invoices (INV001-INV003, INV006-INV007)
- **config-invoice.json** - matches on InvoiceId field

**Expected result:**
- 3 matched (INV001, INV002, INV003)
- 2 only in FolderA (INV004, INV005)
- 2 only in FolderB (INV006, INV007)

### Products
- **FolderA/products.csv** - 5 products (P001-P005)
- **FolderB/products.csv** - 5 products (P001-P003, P006-P007)
- **config-products.json** - matches on ProductId and ProductName

**Expected result:**
- 3 matched (P001, P002, P003)
- 2 only in FolderA (P004, P005)
- 2 only in FolderB (P006, P007)

### Customers
- **FolderA/customers.csv** - 3 customers (C001-C003)
- **FolderB/customers.csv** - 3 customers (C001, C002, C004)
- **config-customers.json** - matches on CustomerId (case sensitive)

**Expected result:**
- 2 matched (C001, C002)
- 1 only in FolderA (C003)
- 1 only in FolderB (C004)

## How to use

1. run the tool pointing to these sample folders
2. check the output folder for results
3. compare with expected results above
