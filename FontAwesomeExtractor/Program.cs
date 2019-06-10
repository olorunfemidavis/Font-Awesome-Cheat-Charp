using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;

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

            var destination = Path.Combine(Directory.GetCurrentDirectory(), "../../../Output.cs");
            using (var fStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                using (var sw = new StreamWriter(fStream))
                {
                    //Load the three sessions (solid, regular, brands ) into the var 
                    HtmlNodeCollection htmlSessions = htmlDoc.DocumentNode.SelectNodes("//section");
                    //Iterate through sessions to process.
                    Output(sw, "public class FontAwesome");

                    const string closeBrace = "}";
                    const string openBrace = "{";
                    Output(sw, openBrace);
                    foreach (HtmlNode session in htmlSessions)
                    {
                        Output(sw, $"\tpublic static class {Processor.Edit(session.Id)}");
                        Output(sw, $"\t{openBrace}");

                        HtmlNodeCollection session_articles = session.SelectNodes("//article");

                        // to exclude duplicates
                        var hSet = new HashSet<string>();
                        //Iterate through the Article List to process
                        foreach (HtmlNode article in session_articles)
                        {

                            string title = article.Id;
                            HtmlNode dlNode = article.ChildNodes[1];
                            HtmlNode ddNode = dlNode.ChildNodes[5];
                            string unicode = ddNode.InnerText;

                            if (hSet.Add(unicode))
                            {
                                string output = $@" public static string {Processor.Edit(title)} = ""\u{unicode}"";"; //&#x
                                Output(sw, $"\t\t{output}");
                            }
                        }
                        hSet.Clear();

                        Output(sw, $"\t{closeBrace}");

                        Output(sw);
                        Output(sw);
                        Output(sw);
                    }
                    Output(sw, closeBrace);

                    sw.Flush();
                }
            }

            Console.ReadKey();
        }

        private static void Output(StreamWriter writer, string content = null)
        {
            if (content is null)
            {
                Console.Out.WriteLine();
                writer.WriteLine();
            }
            else
            {
                Console.Out.WriteLine(content);
                writer.WriteLine(content);
            }
        }
    }
}
