using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace Library.WebApi
{
    class Program
    {
        public static Options Options { get; private set; }

        static void Main( string[] args )
        {
            Options = Library.WebApi.Options.Parse( args );

            if( IsRunningOnMono() )
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Service()
                };
                ServiceBase.Run( ServicesToRun );
            }
            else
            {
                //so we don't have to bother installing/uninstalling services on windows
                Service svc = new Service();
                svc.OnDebug();
                LaunchDocumentation( Program.Options.RootUrl );
                Thread.Sleep( Timeout.Infinite );
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