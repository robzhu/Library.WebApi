using CommandLine;

namespace Library.WebApi
{
    public class Options
    {
        public static Options Parse( string[] args )
        {
            var options = new Options();
            Parser.Default.ParseArguments( args, options );

            if( !options.RootUrlIsSpecified() )
            {
                options.RootUrl = options.GetListenUrl().Replace( "*", "localhost" );
            }
            return options;
        }

        [Option( "rootUrl", HelpText = "The root url this service is running as. Ex: https://www.library.com http://local-dev:4000/" )]
        public string RootUrl { get; set; }

        [Option( "port", DefaultValue = 5000, HelpText = "The port to run the service on." )]
        public int Port { get; set; }

        public bool RootUrlIsSpecified()
        {
            return !string.IsNullOrEmpty( RootUrl );
        }

        public string GetListenUrl()
        {
            return "http://*:" + Port;
        }
    }
}
