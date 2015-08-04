using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
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
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers( Assembly.GetExecutingAssembly() ).InstancePerRequest();
            builder.RegisterWebApiFilterProvider( config );

            //register type specific shit here
            RegisterTypes( builder );

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver( container );

            app.UseAutofacMiddleware( container );
            app.UseAutofacWebApi( config );
            app.UseWebApi( config );
        }

        private static void RegisterTypes( ContainerBuilder builder )
        {
            builder.RegisterType<BookService>().As<IBookService>();
            builder.RegisterType<AuthorService>().As<IAuthorService>();

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
