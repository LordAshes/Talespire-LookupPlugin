using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;


namespace LordAshes
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(LordAshes.FileAccessPlugin.Guid)]
    [BepInDependency(LordAshes.ChatServicePlugin.Guid)]
    public partial class LookupPlugin : BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "Lookup Plug-In";              
        public const string Guid = "org.lordashes.plugins.lookup";
        public const string Version = "1.1.0.0";

        // Configuration
        private string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+"/";
        private string data = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/CustomData/Sources/";

        void Awake()
        {
            UnityEngine.Debug.Log("Lookup Plugin: Active.");

            ChatServicePlugin.handlers.Add("/lu", (message, sender, source) => { return LookupHander(message, sender, source); });
            ChatServicePlugin.handlers.Add("/fd", (message, sender, source) => { return LookupHander(message, sender, source); });
            ChatServicePlugin.handlers.Add("/fa", (message, sender, source) => { return LookupHander(message, sender, source); });

            Utility.PostOnMainPage(this.GetType());
        }

        private string LookupHander(string message, string sender, ChatServicePlugin.ChatSource source)
        {
            string cmd = message.Trim().Substring(0, message.Trim().IndexOf(" ")).ToUpper();
            string keyword = message.Trim().Substring(message.Trim().IndexOf(" ")+1).Trim();
            switch (cmd)
            {
                case "/LU":
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                        {
                            WorkingDirectory = dir,
                            FileName = dir+"LookUpEngine.exe",
                            Arguments = keyword,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true
                        };
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        return output;
                    }
                case "/FD":
                    foreach(string content in System.IO.Directory.EnumerateFiles(data,"*."))
                    {
                        string contents = FileAccessPlugin.File.ReadAllText(content);
                        if(contents.Contains(keyword))
                        {
                            contents = "<color=black>Topic: " + System.IO.Path.GetFileNameWithoutExtension(content)+"\r\n<color=grey>"+contents.Replace(keyword, "<color=red>" + keyword + "<color=grey>");
                            return contents;
                        }
                    }
                    break;
                case "/FA":
                    string returnContents = "";
                    foreach (string content in System.IO.Directory.EnumerateFiles(data, "*."))
                    {
                        string contents = FileAccessPlugin.File.ReadAllText(content);
                        if (contents.Contains(keyword))
                        {
                            returnContents = returnContents + "<color=black>Topic: " + System.IO.Path.GetFileNameWithoutExtension(content) + "\r\n<color=grey>" + contents.Replace(keyword, "<color=red>" + keyword + "<color=grey>")+"\r\n\r\n";
                        }
                    }
                    if (returnContents != "") { return returnContents; }
                    break;
                default:
                    Debug.Log("Lookup Plugin: Logic corruption. Chat Service handler fired when message does not start with /LU or /FD");
                    break;
            }
            return ((cmd=="/LU") ? "Topic" : "Sequenc") + " '" + keyword + "' Not Found.";
        }

        void Update()
        {
        }
    }
}
