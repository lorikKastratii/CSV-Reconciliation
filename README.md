# CSV Reconciliation Tool

compares CSV files between two folders and generates reconciliation reports. uses multithreading to process multiple file pairs in parallel.

## Configuration

edit the paths in `Program.cs`:

```csharp
static string FolderA = @"C:\path\to\your\FolderA";
static string FolderB = @"C:\path\to\your\FolderB";
static string ConfigPath = @"C:\path\to\config.json";
static string OutputFolder = @"C:\path\to\output";
```

## Config file format

create a JSON config file that tells the tool which fields to match on:

```json
{
  "matchingFields": ["InvoiceId"],
  "caseSensitive": false,
  "trim": true
}
```

- `matchingFields` - list of column names to match records on
- `caseSensitive` - whether to match case sensitively
- `trim` - whether to trim whitespace before matching

## Output

results are written to the output folder:
- `{filename}-matched.csv` - records that match
- `{filename}-only-in-folderA.csv` - records only in FolderA
- `{filename}-only-in-folderB.csv` - records only in FolderB
- `{filename}-reconcile-summary.json` - summary stats
- `summary.json` - overall summary for all file pairs

## Project structure

```
CSVReconciliation/
├── src/
│   ├── CSVReconciliation.Core/     - core library with all logic
│   └── CSVReconciliation.Console/  - console app
├── tests/
│   └── CSVReconciliation.Tests/    - unit tests
└── samples/                         - sample CSV files and config
```

## How it works

1. reads matching config from JSON file
2. finds all CSV file pairs with same name in both folders
3. parses CSV files into records
4. matches records based on configured fields
5. runs reconciliation in parallel for multiple file pairs
6. writes matched/unmatched records to output files
7. Notes

- sample CSV files are in the `samples` folder
- uses `Parallel.ForEach` for multithreading
- control thread count with `MaxThreads` in `Program.cs`
- 16 unit tests included