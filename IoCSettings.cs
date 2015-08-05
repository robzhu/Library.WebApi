using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Drum;
using HttpEx;
using Library.DomainModel;
using Library.DomainModel.Storage;
using Library.DomainServices;
using Library.Storage;
using Library.Storage.Providers;
using Owin;

namespace Library.WebApi
{
    public static class IoCSettings
    {
        internal static void Configure( IAppBuilder app, HttpConfiguration config )
        {
            IContainer container = null;
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers( Assembly.GetExecutingAssembly() ).InstancePerRequest();
            builder.RegisterWebApiFilterProvider( config );

            RegisterTypes( builder );
            RegisterDrum( config, builder, () =>
                {
                    return container.Resolve<HttpRequestMessage>();
                } );

            container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver( container );

            app.UseAutofacMiddleware( container );
            app.UseAutofacWebApi( config );
            app.UseWebApi( config );

            //config.DependencyResolver.
        }

        private static void RegisterDrum( HttpConfiguration config, ContainerBuilder builder, Func<HttpRequestMessage> requestProvider )
        {
            // Web API routes
            UriMakerContext uriMakerContext = config.MapHttpAttributeRoutesAndUseUriMaker();
            builder.RegisterInstance( uriMakerContext ).ExternallyOwned();
            builder.RegisterHttpRequestMessage( config );
            builder.RegisterGeneric( typeof( UriMaker<> ) ).AsSelf().InstancePerRequest();

            builder.RegisterType<DrumUrlProvider>().As<IUrlProvider>();
        }

        private static void RegisterTypes( ContainerBuilder builder )
        {
            builder.RegisterType<BookService>().As<IBookService>();
            builder.RegisterType<AuthorService>().As<IAuthorService>();
            builder.RegisterType<LendingService>().As<ILendingService>();

            builder.RegisterType<BookResourceAssembler>().AsSelf().PropertiesAutowired( PropertyWiringOptions.AllowCircularDependencies );
            builder.RegisterType<AuthorResourceAssembler>().AsSelf().PropertiesAutowired( PropertyWiringOptions.AllowCircularDependencies );
            builder.RegisterType<LendingRecordResourceAssembler>().AsSelf();

            builder.RegisterType<AuthorDocumentStore>().As<IAuthorStore>();
            builder.RegisterType<BookDocumentStore>().As<IBookStorage>();
            builder.RegisterType<LendingRecordDocumentStore>().As<ILendingRecordStore>();
            
            builder.RegisterType<AuthorRepository>().As<IAuthorRepository>();
            builder.RegisterType<ISBNDBLookupService>().As<IISBNLookupService>();

            builder.RegisterInstance( DocumentStoreIndex.BookStore ).As<IDocumentStore<Book>>();
            builder.RegisterInstance( DocumentStoreIndex.AuthorStore ).As<IDocumentStore<Author>>();
            builder.RegisterInstance( DocumentStoreIndex.LendingRecordStore ).As<IDocumentStore<LendingRecord>>();
        }
    }
}
