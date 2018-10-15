using HtmlAgilityPack;
using System;

namespace FontAwesomeExtractor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Goto https://fontawesome.com/cheatsheet 
            //Inspect  the source, locate the body, expand it, locate <div class="ph4 ph6-ns pv6 ph0-pr pv0-pr bg-white"> minimize the it, right click, copy.
            //Add an html file to your project and set build to 'copy always' and paste the code into the file.
            string path = @"test.html";

            //Initializes the Variable to use
            HtmlDocument htmlDoc = new HtmlDocument();
            //load html into variable
            htmlDoc.Load(path);

            //Load the three sessions (solid, regular, brands ) into the var 
            HtmlNodeCollection htmlSessions = htmlDoc.DocumentNode.SelectNodes("//section");
            //Iterate through sessions to process.
            Console.WriteLine($@"public class FontAwesome");
            Console.WriteLine(@"{");
            foreach (HtmlNode session in htmlSessions)
            {
                Console.WriteLine($@"public static class {Processor.Edit(session.Id)}");
                Console.WriteLine(@"{");
                HtmlNodeCollection session_articles = session.SelectNodes("//article");
                //Iterate through the Article List to process
                foreach (HtmlNode article in session_articles)
                {

                    string title = article.Id;
                    HtmlNode dlNode = article.ChildNodes[1];
                    HtmlNode ddNode = dlNode.ChildNodes[5];
                    string unicode = ddNode.InnerText;
                    string output = $@" public static string {Processor.Edit(title)} = ""\u{unicode}"";"; //&#x
                    Console.WriteLine(output);
                }
                Console.WriteLine("}");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            Console.WriteLine("}");

            Console.ReadKey();
        }
    }
}
