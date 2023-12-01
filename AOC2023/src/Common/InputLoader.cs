using System.Collections.Generic;
using System.IO;
using Lutra.Utility;

namespace AOC2023.Common;

public static class InputLoader
{
    public static List<string> LoadInputFromAssets(string filename)
    {
        List<string> extractedStrings = new List<string>();
        using (var stream = AssetManager.LoadStream(filename))
        {
            using (var reader = new StreamReader(stream))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    extractedStrings.Add(line);
                }
            }
        }

        return extractedStrings;
    }
}