using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class FilePairFinder
{
    public List<FilePair> Find(string folderA, string folderB)
    {
        var filesA = Directory.GetFiles(folderA, "*.csv");
        var filesB = Directory.GetFiles(folderB, "*.csv");

        var namesA = filesA.Select(f => Path.GetFileName(f)).ToList();
        var namesB = filesB.Select(f => Path.GetFileName(f)).ToList();

        var allNames = namesA.Union(namesB).ToList();
        var pairs = new List<FilePair>();

        foreach (var name in allNames)
        {
            var pair = new FilePair();
            pair.BaseName = Path.GetFileNameWithoutExtension(name);
            pair.FileA = Path.Combine(folderA, name);
            pair.FileB = Path.Combine(folderB, name);
            pair.FileAExists = namesA.Contains(name);
            pair.FileBExists = namesB.Contains(name);
            pairs.Add(pair);
        }

        return pairs;
    }
}
