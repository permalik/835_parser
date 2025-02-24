using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        List<string[]> transactions = new();
        List<string> currentTransaction = new();

        using (FileStream fs = new FileStream("test.835", FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new StreamReader(fs))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.StartsWith("ST*"))
                {
                    currentTransaction = new List<string>();
                }

                currentTransaction.Add(line);

                if (line.StartsWith("SE*"))
                {
                    transactions.Add(currentTransaction.ToArray());
                }
            }
        }

        for (int i = 0; i < transactions.Count; i++)
        {
            bool hasN3 = false;
            bool hasN4 = false;

            foreach (var entry in transactions[i])
            {
                if (entry.StartsWith("N3*")) hasN3 = true;
                if (entry.StartsWith("N4*")) hasN4 = true;
            }

            Console.WriteLine($"Transaction {i + 1}:");
            foreach (var entry in transactions[i])
            {
                Console.WriteLine(entry);
            }

            Console.WriteLine($"Includes Address Information: {(hasN3 && hasN4 ? "Yes" : "No")}");
            Console.WriteLine(new string('-', 40));
        }
    }
}

