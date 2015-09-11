using System;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace Library.WebApi
{
    class Service : ServiceBase
    {
        public const string Version = "0.02";
        private IDisposable _webApp = null;

        public void OnDebug()
        {
            OnStart( null );
        }

        protected override void OnStart( string[] args )
        {
            _webApp = WebApp.Start( Program.Options.GetListenUrl() );
            Console.WriteLine( "Library Web API {0}: tracks book inventory and who is borrowing which book", Version );
            Console.WriteLine( "Service started at {0}", Program.Options.RootUrl );
        }

        protected override void OnStop()
        {
            if( _webApp != null )
            {
                _webApp.Dispose();
            }
            base.OnStop();
        }
    }
}