using CSVReconciliation.Core.Models;
using CSVReconciliation.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSVReconciliation.Console;

public class Program
{
    static string FolderA = @"C:\LorikProject\CSVReconciliation\samples\FolderA";
    static string FolderB = @"C:\LorikProject\CSVReconciliation\samples\FolderB";
    static string ConfigPath = @"C:\LorikProject\CSVReconciliation\samples\config-invoice.json";
    static string OutputFolder = @"C:\LorikProject\CSVReconciliation\samples\output";
    static int MaxThreads = Environment.ProcessorCount;
    static char Delimiter = ',';

    public static void Main(string[] args)
    {
        var settings = new ReconciliationSettings
        {
            FolderA = FolderA,
            FolderB = FolderB,
            ConfigPath = ConfigPath,
            OutputFolder = OutputFolder,
            MaxThreads = MaxThreads,
            Delimiter = Delimiter
        };

        if (!Directory.Exists(settings.FolderA))
        {
            System.Console.WriteLine("Error: FolderA does not exist: " + settings.FolderA);
            return;
        }

        if (!Directory.Exists(settings.FolderB))
        {
            System.Console.WriteLine("Error: FolderB does not exist: " + settings.FolderB);
            return;
        }

        if (!File.Exists(settings.ConfigPath))
        {
            System.Console.WriteLine("Error: Config file does not exist: " + settings.ConfigPath);
            return;
        }

        var services = new ServiceCollection();
        services.AddReconciliationServices(settings);
        var serviceProvider = services.BuildServiceProvider();

        var runner = serviceProvider.GetRequiredService<ReconciliationRunner>();
        var summary = runner.Run();

        System.Console.WriteLine();
        System.Console.WriteLine("=== Results ===");
        System.Console.WriteLine("Total records in FolderA: " + summary.TotalRecordsInA);
        System.Console.WriteLine("Total records in FolderB: " + summary.TotalRecordsInB);
        System.Console.WriteLine("Matched: " + summary.TotalMatched);
        System.Console.WriteLine("Only in FolderA: " + summary.TotalOnlyInA);
        System.Console.WriteLine("Only in FolderB: " + summary.TotalOnlyInB);
        System.Console.WriteLine("Errors: " + summary.TotalErrors);
        System.Console.WriteLine("Total time: " + summary.TotalProcessingTime.TotalMilliseconds + "ms");
    }
}
