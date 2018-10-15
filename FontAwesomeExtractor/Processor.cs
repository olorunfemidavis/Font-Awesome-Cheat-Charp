using System;
using System.Globalization;

namespace FontAwesomeExtractor
{
    internal class Processor
    {
        public static string Edit(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower()).
                Replace("-", "_").Replace("500Px", $@"Unknown{new Random().Next(0, 100)}");
        }
    }
}
