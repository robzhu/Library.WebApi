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
                if( IsRunningOnMono() )
                {
                    Console.WriteLine( "RUNNING ON MONO" );
                }
                else
                {
                    LaunchDocumentation( fullUrl );
                }

                Console.WriteLine( "Service started at {0}", fullUrl );
                Console.WriteLine( "Press ENTER to stop." );
                Console.ReadLine();
            }
        }

        static bool IsRunningOnMono()
        {
            return Type.GetType( "Mono.Runtime" ) != null;
        }

        static void LaunchDocumentation( string url )
        {
            Process.Start( "chrome.exe", string.Format( "--incognito {0}", url + "/swagger/ui/index" ) );
        }
    }
}