using System;
using System.Globalization;

namespace FontAwesomeExtractor
{
    internal class Processor
    {
        private static Random rand = new Random();

        public static string Edit(string input)
        {
            var output = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()).
                Replace("-", "_").Replace("500Px", $@"Unknown{rand.Next(0, 100)}");

            // fixing special member name
            switch (output)
            {
                case "Equals":
                    output += rand.Next(0, 100);
                    break;
            }

            return output;
        }
    }
}
