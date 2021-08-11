using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsonUtils
{
    public static class Utils
    {
        public static List<ResultKeys> ReadResultKeys(string fileName)
        {
            if (!File.Exists(fileName)) { Console.WriteLine("File not exist."); return null; }

            string fileContent = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(fileContent)) { Console.WriteLine("File is empty."); return null; }

            JObject fileJson = null;
            try
            {
                fileJson = (JObject)JsonConvert.DeserializeObject(fileContent);
                if (fileJson == null) { Console.WriteLine("File is parsed to an empty object."); return null; }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't parse file content as JSON. Error message:");
                Console.WriteLine(ex.Message);
                return null;
            }

            if (fileJson == null) { return null; }

            JToken values = null;
            if (!fileJson?.TryGetValue("value", out values) ?? false)
            {
                Console.WriteLine("Json should contain value property.");
                return null;
            }

            JArray valueArray = values as JArray;
            if (values == null) { Console.WriteLine("value should be an array of objects"); return null; }

            List<ResultKeys> keys = new List<ResultKeys>();
            for (int i = 0; i < valueArray.Count; i++)
            {
                JObject obj = (JObject)valueArray[i];
                keys.Add(new ResultKeys(obj.Property(ResultKeys.AccountingFrameworkName).Value.Value<string>(),
                    obj.Property(ResultKeys.AnalysisDateName).Value.Value<DateTime>(),
                    obj.Property(ResultKeys.ExternalIdName).Value.Value<string>(),
                    obj.Property(ResultKeys.PricingProfileName).Value.Value<string>(),
                    obj.Property(ResultKeys.SimulationLabelName).Value.Value<string>(), i, fileName));
            }

            return keys;
        }

        public static void FindDuplicates(List<ResultKeys> keys)
        {
            Console.WriteLine($"Total value number: {keys.Count}");

            var duplicatesGroups = keys.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Select(x => x).ToList()).ToList();
            for (int i = 0; i < duplicatesGroups.Count; i++)
            {
                Console.WriteLine();
                foreach (var duplicate in duplicatesGroups[i])
                {
                    Console.WriteLine($"Duplicated key value {duplicate}.");
                }
            }
            if (duplicatesGroups.Count == 0)
            {
                Console.WriteLine("No duplicates found.");
            }
        }

        public static void FindMissing(List<ResultKeys> keys1, List<ResultKeys> keys2)
        {
            var duplicated = keys1.Intersect(keys2).ToList();
            var missingIn1 = keys2.Except(keys1).ToList();
            Console.WriteLine($"Total values in first set: {keys1.Count}. {duplicated.Count} are the same, {keys1.Count - duplicated.Count - missingIn1.Count} are different, {missingIn1.Count} are missing.");
            int i;
            for (i = 0; i < Math.Min(10, missingIn1.Count); i++)
            {
                Console.WriteLine($"Missing key value {missingIn1[i]}.");
            }
            if (i + 1 < missingIn1.Count)
            {
                Console.WriteLine("And others...");
            }

            var missingIn2 = keys1.Except(keys2).ToList();
            Console.WriteLine($"Total values in second set: {keys2.Count}. {duplicated.Count} are the same, {keys2.Count - duplicated.Count - missingIn2.Count} are different, {missingIn2.Count} are missing.");
            for (i = 0; i < Math.Min(10, missingIn2.Count); i++)
            {
                Console.WriteLine($"Missing key value {missingIn2[i]}.");
            }
            if (i + 1 < missingIn2.Count)
            {
                Console.WriteLine("And others...");
            }
        }
    }
}
