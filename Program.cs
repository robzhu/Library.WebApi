using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace Library.WebApi
{
    class Program
    {
        static void Main( string[] args )
        {
            var url = "http://*:5000";
            var fullUrl = url.Replace( "*", "localhost" );
            using( WebApp.Start( url ) )
            {
                Console.WriteLine( "Service started at {0}", fullUrl );
                Console.WriteLine( "Press ENTER to stop." );
                LaunchDocumentation( fullUrl );
                Console.ReadLine();
            }
        }

        static void LaunchDocumentation( string url )
        {
            Process.Start( "chrome.exe", string.Format( "--incognito {0}", url + "/swagger/ui/index" ) );
        }
    }
}