using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using FindMyPet.MyServiceStack.Providers;
using FindMyPet.TableModel;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace FindMyPet.MyServiceStack
{
    public class FindMyPetHost : AppHostHttpListenerBase //AppHostBase //
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["FindMyPet"]
                                           .ConnectionString;
            }
        }

        /// <summary>
        /// This default constructor passes the name of our service “PersonService” 
        /// as well as all assemblies that need to be loaded – in this case we only need to
        /// use the current assembly so I have passed that using typeof()
        /// </summary>
        public FindMyPetHost() : base("Find My Pet Service", typeof(FindMyPetHost).Assembly)
        { }

        /// <summary>
        /// This method is used to configure things like Inversion-of-Control Containers
        /// </summary>
        /// <param name=“container”></param>
        public override void Configure(Funq.Container container)
        {
            // Add our IDbConnectionFactory to the container, 
            // this will allow all of our services to share a single connection factory
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConnectionString, SqlServerOrmLiteDialectProvider.Instance));

            Plugins.Add(new SwaggerFeature());

            //// Below we refer to the connection factory that we just registered
            //// with the container and use it to create our table(s).
            //using (var db = container.Resolve<IDbConnectionFactory>().Open())
            //{
            //    // We’re just creating a single table, but you could add
            //    // as many as you need.  Also note the “overwrite: false” parameter,
            //    // this will only create the table if it doesn’t already exist.
            //    db.CreateTable<OwnerTable>(overwrite: false);
            //    db.CreateTable<PetTable>(overwrite: false);
            //    db.CreateTable<OwnerPetTable>(overwrite: false);
            //    db.CreateTable<ParameterGroupTable>(overwrite: false);
            //    db.CreateTable<ParameterValueTable>(overwrite: false);
            //}

            container.RegisterAutoWiredAs<OwnerProvider, IOwnerProvider>();
            container.RegisterAutoWiredAs<OwnerMapper, IOwnerMapper>();
            container.RegisterAutoWiredAs<OwnerDataAccess, IOwnerDataAccess>();

            container.RegisterAutoWiredAs<PetProvider, IPetProvider>();
            container.RegisterAutoWiredAs<PetMapper, IPetMapper>();
            container.RegisterAutoWiredAs<PetDataAccess, IPetDataAccess>();

            container.RegisterAutoWiredAs<PetImageProvider, IPetImageProvider>();
            container.RegisterAutoWiredAs<PetImageMapper, IPetImageMapper>();
            container.RegisterAutoWiredAs<PetImageDataAccess, IPetImageDataAccess>();

            container.RegisterAutoWiredAs<BaseDataAccess<OwnerTableModel>, IBaseDataAccess<OwnerTableModel>>();
            container.RegisterAutoWiredAs<BaseDataAccess<PetTableModel>, IBaseDataAccess<PetTableModel>>();
            container.RegisterAutoWiredAs<BaseDataAccess<PetImageTableModel>, IBaseDataAccess<PetImageTableModel>>();
        }
    }
}