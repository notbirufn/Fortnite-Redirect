using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortnite_Redirect
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (Fiddler.Setup())
            {
                FiddlerCoreStartupSettings startupSettings = new FiddlerCoreStartupSettingsBuilder().ListenOnPort(9999).DecryptSSL().RegisterAsSystemProxy().Build();

                FiddlerApplication.BeforeRequest += OnBeforeRequest;
                FiddlerApplication.BeforeResponse += OnBeforeResponse;

                FiddlerApplication.Startup(startupSettings);

                Console.ReadKey(true);

                FiddlerApplication.Shutdown();
            }
        }
        private static void OnBeforeRequest(Session session)
        {
            if (session.RequestHeaders["User-Agent"].Split('/')[0] == "FortniteGame" || session.RequestHeaders["User-Agent"].Split('/')[0] == "Fortnite" || session.RequestHeaders["User-Agent"].Split('/')[0] == "EOS-SDK")
            {
                if (session.HTTPMethodIs("CONNECT"))
                {
                    session["x-replywithtunnel"] = "FortniteTunnel";
                    return;
                }
                else
                {
                    Console.WriteLine("LogURL: " + session.fullUrl);
                }

                session.fullUrl = "http://localhost:1911" + session.PathAndQuery;
            }
        }

        private static void OnBeforeResponse(Session session)
        {

        }
    }
}
