using ServicesImpl;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NameValueCollection networkSettings =
                (NameValueCollection)ConfigurationManager.GetSection("networkSettings");

            LocalFileSystem fileSystem = new LocalFileSystem();
            MessageDispatcher md = new MessageDispatcher(fileSystem, networkSettings["IPAddress"], int.Parse(networkSettings["Port"]));
            md.Start();
        }
    }
}
