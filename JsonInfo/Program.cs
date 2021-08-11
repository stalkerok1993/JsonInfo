using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using JsonUtils;

namespace JsonInfo
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) { Console.WriteLine("Wrong call format."); ShowHelp(); return; }

            var allKeys = new List<ResultKeys>();
            foreach (string fileName in args)
            {
                List<ResultKeys> keys = Utils.ReadResultKeys(fileName);
                Console.WriteLine($"Checking data from file {fileName}.");
                Utils.FindDuplicates(keys);
                allKeys.AddRange(keys);
            }

            if (args.Length > 1)
            {
                Console.WriteLine("Checking data from all files.");
                Utils.FindDuplicates(allKeys);
            }
        }

        

        private static void ShowHelp()
        {
            Console.WriteLine(
@"The next syntax should be used
jsoninfo [file_name] {[file2_name] ...}");
        }
    }

    
}
