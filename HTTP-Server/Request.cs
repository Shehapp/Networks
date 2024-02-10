using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
            this.headerLines = new Dictionary<string, string>();
        }

        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {

            //TODO: parse the receivedRequest using the \r\n delimeter   
            requestLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)
            {
                return false;
            }

            // Parse Request line
            if (!ParseRequestLine())
            {
                return false;
            }

            // Validate blank line exists
            if (!ValidateBlankLine())
            {
                return false;
            }

            // Load header lines into HeaderLines dictionary
            if (!LoadHeaderLines())
            {
                return false;
            }

            return true;
        }

        private bool ParseRequestLine()
        {
            string[] requestlineparts = requestLines[0].Split(' ');
            if (requestlineparts.Length != 3)
            {
                return false;
            }

            // Validate Method
            method = (RequestMethod)Enum.Parse(typeof(RequestMethod), requestlineparts[0]);
            if (method == null)
            {
                return false;
            }


            // Validate URI
            if (!ValidateIsURI(requestlineparts[1]))
            {
                return false;
            }

            relativeURI = requestlineparts[1];


            // Validate HTTP Version
            string httpversion = requestlineparts[2]
                .Replace(".", "")
                .Replace("/", "");

            httpVersion = (HTTPVersion)Enum.Parse(typeof(HTTPVersion), httpversion);
            if (httpVersion == null)
            {
                return false;
            }

            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            string[] ok;
            for (int i = 1; requestLines[i] != ""; i++)
            {
                try
                {
                    ok = requestLines[i].Split(':');
                    headerLines.Add(ok[0], ok[1]);
                }
                catch (Exception e)
                {
                    return false;
                }

            }

            return true;
        }

        private bool ValidateBlankLine()
        {

            if (requestLines[requestLines.Length - 2] != "")
            {
                return false;
            }

            return true;

        }

    }
}
