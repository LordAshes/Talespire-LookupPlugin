using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LordAshes
{
    class LookUpEngine
    {
        static void Main(string[] args)
        {
            bool log = System.IO.File.Exists(Environment.CurrentDirectory + "/LookUpEngine.log");

            if (log) { Log("Request '" + String.Join(" ", Environment.GetCommandLineArgs().Skip(1)) + "' Made At " + DateTime.UtcNow); }
            // Search for Provider scripts
            foreach (string provider in System.IO.Directory.EnumerateFiles(Environment.CurrentDirectory + "/CustomData/Providers/", "*.js"))
            {
                try
                {
                    if (log) { Log("Using Provider '" + provider + "'"); }
                    // Read the provider specification
                    string[] lines = System.IO.File.ReadAllLines(provider);
                    // Verify that first line is the URL specification
                    if (lines[0].ToUpper().StartsWith("URL: ") && lines[1].ToUpper().StartsWith("SPACE: "))
                    {
                        // Deetrmine the space replace characters for the URL
                        string space = (lines[1].Length >= 8) ? lines[1].Substring(7) : "";
                        // Generate the URL
                        string url = lines[0].Substring(5).Replace("{dir}",Environment.CurrentDirectory).Replace("{search}", String.Join(" ", Environment.GetCommandLineArgs().Skip(1)).Replace(" ", space));
                        if (log) { Log("Using URL '" + url + "'"); }
                        // Get the contents
                        using (WebClient wc = new WebClient())
                        {
                            string content = wc.DownloadString(url);
                            content = WebUtility.HtmlDecode(content);
                            if (log) { Log("Content Found"); }
                            // Build javascript processing function for parsing data
                            string processor = "function ParseData(content)\r\n";
                            processor = processor + "{\r\n";
                            processor = processor + "  var buffer = \"\";\r\n";
                            // Create helper functions
                            processor = processor + "  function Start() { buffer = content; }";
                            processor = processor + "  function FindSection(str) { buffer = buffer.substring(buffer.indexOf(str)); }";
                            processor = processor + "  function GetBetween(start,end) { buffer = buffer.substring(buffer.indexOf(start)+start.length); buffer = buffer.substring(0, buffer.indexOf(end)); return buffer.replace(/^\\s+|\\s+$/gm,'').replace(/<[^>]*>?/gm, '').replace('\\r', '').replace('\\n', ''); }";
                            processor = processor + "  function GetTextBetween(start,end) { buffer = buffer.substring(buffer.indexOf(start)+start.length); buffer = buffer.substring(0, buffer.indexOf(end)); return buffer.replace(/^\\s+|\\s+$/gm,'').replace(/<[^>]*>?/gm, ''); }";
                            // Copy provider script for data extraction
                            foreach (string line in lines.Skip(2))
                            {
                                processor = processor + line + "\r\n";
                            }
                            processor = processor + "}\r\n";
                            // Execute provider script
                            using (ScriptEngine engine = new ScriptEngine("jscript"))
                            {
                                ParsedScript parsed = engine.Parse(processor);
                                content = parsed.CallMethod("ParseData", content).ToString();
                                // Return results
                                foreach (string line in content.Split('\n'))
                                {
                                    if (line.Trim() != "")
                                    {
                                        Console.WriteLine(line.Trim());
                                        if (log) { Log(line.Trim()); }
                                    }
                                }
                            }
                        }
                        Environment.Exit(0);
                        if (log) { Log("Topic Content Returned"); }
                    }
                }
                catch (WebException)
                {
                    // Content not found, try another provider if available
                    if (log) { Log("Content Not Found"); }
                }
            }
            Console.WriteLine("Topic '" + String.Join(" ", Environment.GetCommandLineArgs().Skip(1)) + "' Not Found.");
            if (log) { Log("Topic Not Found"); }
        }

        private static void Log(string txt)
        {
            System.IO.File.AppendAllText(Environment.CurrentDirectory + "/LookUpEngine.log", txt + "\r\n");
        }
    }
}
