using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MorePizza
{
    class Program
    {
        public static string PathToFile { get; set; }

        static void Main(string[] args)
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var thisAppPath = appPathMatcher.Match(exePath).Value;
            
            string outputPath = Path.Combine(thisAppPath, "OutputFiles"); 
            //string PathToFile = Path.Combine(thisAppPath, "InputFiles\\a_example.in");
            //string PathToFile = Path.Combine(thisAppPath, "InputFiles\\b_small.in");
            //string PathToFile = Path.Combine(thisAppPath, "InputFiles\\c_medium.in");
            //string PathToFile = Path.Combine(thisAppPath, "InputFiles\\d_quite_big.in");
            string PathToFile = Path.Combine(thisAppPath, "InputFiles\\e_also_big.in");

            int MaxSlices;
            int Types;
            int[] SlisesInPizza;
            List<int> result = new List<int>();
            bool findGreateResult = false;
            List<Pizzas> res = new List<Pizzas>();

            using (StreamReader sr = new StreamReader(PathToFile))
            {
                var line = sr.ReadLine();
                var Parts = line.Split(' ');
                MaxSlices = int.Parse(Parts[0]);
                Types = int.Parse(Parts[1]);

                line = sr.ReadLine();
                Parts = line.Split(' ');
                SlisesInPizza = new int[Types];

                for (int i = 0; i < Types; i++)
                {
                    SlisesInPizza[i] = int.Parse(Parts[i]);
                }
            }

            var count = 0;
            int start = 1;

            while (start < Types / 2)
            {
                for (int i = Types - start; i >= 0; i--)
                {
                    if (count + SlisesInPizza[i] <= MaxSlices)
                    {
                        count += SlisesInPizza[i];
                        result.Add(i);
                    }

                }

                if (count == MaxSlices)
                {
                    // we find greate result
                    // break all calculations
                    findGreateResult = true;

                    var sortedList = result.OrderBy(c => c).ToList();


                    using (StreamWriter sw = new StreamWriter(outputPath + "\\" + Path.GetFileNameWithoutExtension(PathToFile) + ".out"))
                    {
                        sw.WriteLine(result.Count);
                        foreach (var item in sortedList)
                        {
                            sw.Write(item);
                            sw.Write(' ');              
                        }
                        sw.WriteLine();
                    }
                    Console.WriteLine($"Score: {MaxSlices}");

                    break;

                }
                else
                {
                    start++;
                    count = 0;
                    res.Add(new Pizzas
                    {
                        TypesOfPizza = result.Count,
                        ArrayOfIndexes = result.OrderBy(c => c).ToList()
                    });

                    result = new List<int>();
                }
            }

            if (!findGreateResult)
            {
                // if no greate result => out max result
                var maxSutableRes = res.OrderByDescending(x => x.TypesOfPizza).First();
                var score = 0;


                using (StreamWriter sw = new StreamWriter(outputPath + "\\" + Path.GetFileNameWithoutExtension(PathToFile) + ".out"))
                {
                    sw.WriteLine(maxSutableRes.TypesOfPizza);
                    foreach (var item in maxSutableRes.ArrayOfIndexes)
                    {
                        sw.Write(item);
                        sw.Write(' ');
                        score += SlisesInPizza[item];
                    }
                    sw.WriteLine();
                }

                Console.WriteLine($"Score: {score}");
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
        
    }

    public class Pizzas
    {
        public int TypesOfPizza { get; set; }
        public List<int> ArrayOfIndexes { get; set; }
        
    }
}
