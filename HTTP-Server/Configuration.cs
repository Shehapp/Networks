using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    static class Configuration
    {
        public static string ServerHTTPVersion = "HTTP/1.1";
        public static string ServerType = "FCIS_Server";
        public static Dictionary<string, string> RedirectionRules = new Dictionary<string, string>();
        public static string RootPath = "J:\\college\\networks\\Networks\\HTTP-Server\\static_html";
        public static string RedirectionDefaultPageName = "Redirect.html";
        public static string BadRequestDefaultPageName = "BadRequest.html";
        public static string NotFoundDefaultPageName = "NotFound.html";
        public static string InternalErrorDefaultPageName = "InternalError.html";
    }
}
