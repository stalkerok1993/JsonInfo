using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2) { Console.WriteLine("Wrong utility call format."); ShowHelp(); return; }

            string file = args[0];
            if (!File.Exists(file)) { Console.WriteLine("File not exist."); return; }
            var keys = new List<long>();
            try
            {
                var strings = File.ReadAllText(file).Split(", ").ToList();
                keys = strings.Select(x => long.Parse(x)).ToList();
                Console.WriteLine($"{keys.Count} keys found.");
            }
            catch (Exception ex) { Console.WriteLine("Error processing the file."); Console.WriteLine(ex.Message); return; }
            
            string command = args[1];
            switch (command)
            {
                case "get":
                    if (args.Length != 3) { Console.WriteLine("Wrong get command format."); ShowHelp(); return; }
                    int position;
                    if (!int.TryParse(args[2], out position)) { Console.WriteLine($"Can't parse position {args[2]} to a number."); return; }
                    Console.WriteLine($"key[{position}] = {keys[position]}.");
                    break;
                case "find":
                    if (args.Length != 3) { Console.WriteLine("Wrong find command format."); ShowHelp(); return; }
                    if (args[2].ToLower() == "duplicates")
                    {
                        var duplicates = keys.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);
                        if (duplicates.Count() > 0)
                        {
                            Console.WriteLine($"Duplicated keys found: {string.Join(", ", duplicates)}.");
                        }
                        else
                        {
                            Console.WriteLine("No duplicated keys found.");
                        }
                        
                    }
                    else
                    {
                        long key;
                        if (!long.TryParse(args[2], out key)) { Console.WriteLine($"Can't parse {args[2]} as integer key."); return; }
                        List<int> positions = keys.Select((x, i) => new KeyValuePair<long, int>(x, i)).ToList().FindAll(x => x.Key == key).Select(x => x.Value).ToList();
                        Console.WriteLine($"Key is present at positions: {string.Join(", ", positions)}");
                    }
                    break;
                default:
                    Console.WriteLine("Commnad is not recognized.");
                    ShowHelp();
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine(@"Utility call format:
csvkeys [file] [command] [args]

[file]   : file name
[command]: get | find
[args]   : for 'get' command: number to take key from
           for 'find' command: key to find index for or 'duplicates' to find all existing duplicates");
        }
    }
}
