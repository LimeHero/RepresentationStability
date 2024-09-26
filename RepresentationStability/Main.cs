using Polynomials;
using IntegerMethods;
using RepStability;

namespace MainProject;
public class MainMethod
{
    /// <summary>
    /// Execution Point
    /// </summary>
    static void Main()
    {
        int maxDegree = 50;
        SaveToResultsCsv(1, 24, maxDegree);

        int aBoxes = 0;
        for (int i = 1; i < aBoxes; i++)
        {
            foreach (Partition part in Partition.AllPartitions(i))
            {
                Console.WriteLine(part);
                Console.WriteLine(YoungToPoly(part, maxDegree));
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Wrapper function which returns power series with coefficients the stable cohomology 
    /// d_i(k) for a partition k
    /// </summary>
    /// <param name="k"></param>
    /// <param name="maxDegree"></param>
    /// <returns></returns>
    public static LaurentPolynomial YoungToPoly(Partition k, int maxDegree = 10)
    {
        CharacterPolynomial p = YoungToChar.PartToCharPoly(k);
        return PolyStatistic.CharPolyToPowSeries(p, maxDegree);
    }

    /// <summary>
    /// Saves the coefficients of the power series in z to a results.csv file on the desktop.
    /// Saved in a csv format. All coefficients are taken to be positive (since alternate in sign predictably)
    /// Computed for all young tableau with a number of boxes k satisfying a <= k < b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static void SaveToResultsCsv(int a, int b, int maxDegree)
    {
        List<string> lines = new();
        string s = "perms,";
        for (int i = 0; i < maxDegree; i++)
            s += "i=" + i + ",";
        s += "i=" + maxDegree;
        lines.Add(s);
        for (int i = a; i < b; i++)
        {
            foreach (Partition part in Partition.AllPartitions(i))
            {
                Console.WriteLine(part);
                LaurentPolynomial term = YoungToPoly(part, maxDegree);

                string line = "\"" + part.ToStringWithCharSpacing(",") + "\",";

                for (int j = 0; j < maxDegree; j++)
                    line += term[j].Abs() + ",";

                line += term[maxDegree].Abs();

                lines.Add(line);
            }
        }

        // Set a variable to the Documents path.
        string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // Write the string array to a new file named "WriteLines.csv".
        using StreamWriter outputFile = new(Path.Combine(docPath, "results.csv"));
        foreach (string line in lines)
            outputFile.WriteLine(line);
    }

}
