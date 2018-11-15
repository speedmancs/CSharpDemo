using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexDemo
{
    class MatchCollectionDemo
    {
        static void Main(string[] args)
        {
            string detectionPattern = @"(?<id>[^:]+):(?<mac>[0-9a-f]{12}?)\s*";
            string commandOutput = "HEthernet:506B4BA7A368";
            Regex regexPattern = new Regex(detectionPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matches = regexPattern.Matches(commandOutput);
            if (matches == null || matches.Count == 0)
            {
                Console.WriteLine("No found");
            }
            else
            {
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups["mac"].Value);
                }
            }
        }
    }
}
