using JsonUtils;
using System;
using System.Collections.Generic;

namespace JsonCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) { Console.WriteLine("Wrong call format."); ShowHelp(); return; }

            var allKeys1 = new List<ResultKeys>();
            var allKeys2 = new List<ResultKeys>();
            var allKeys = allKeys1;
            foreach (string fileName in args)
            {
                if (fileName.ToLower() == "_and_") { allKeys = allKeys2; continue; }

                List<ResultKeys> keys = Utils.ReadResultKeys(fileName);
                Console.WriteLine($"Checking data from file {fileName}.");
                allKeys.AddRange(keys);
            }

            Utils.FindMissing(allKeys1, allKeys2);
        }

        private static void ShowHelp()
        {
            Console.WriteLine(
@"The next syntax should be used
jsoncompare [file_name] {[file2_name] ...} _and_ [file_name2] {[file_name3] ...}");
        }
    }
}
