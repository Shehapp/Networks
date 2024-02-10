using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {

        private static string redirectionMatrixPath = "redirectionRules.txt";

        static void Main(string[] args)
        {
            // TODO[2]: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server

            CreateRedirectionRulesFile();

            new Server(1000, redirectionMatrixPath).StartServer();

            Console.WriteLine("Server Started");

        }

        static void CreateRedirectionRulesFile()
        {
            // TODO[1]: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"

            FileStream fs = new FileStream(redirectionMatrixPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("aboutus.html,aboutus2.html");
            sw.WriteLine("index.html,aboutus.html");
            sw.Close();
            fs.Close();
        }

    }
}
