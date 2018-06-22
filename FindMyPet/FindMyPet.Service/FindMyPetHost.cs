using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using FindMyPet.MyServiceStack.Providers;
using FindMyPet.Shared;
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
    public class FindMyPetHost : AppHostHttpListenerBase
    {
        private readonly IGlobalHelper _globalHelper;

        public FindMyPetHost() : this(new GlobalHelper())
        {
        }

        /// <summary>
        /// This default constructor passes the name of our service “PersonService” 
        /// as well as all assemblies that need to be loaded – in this case we only need to
        /// use the current assembly so I have passed that using typeof()
        /// </summary>
        public FindMyPetHost(IGlobalHelper globalHelper) 
            : base("Find My Pet Service", typeof(FindMyPetHost).Assembly)
        {
            if (globalHelper == null)
                throw new ArgumentNullException(nameof(globalHelper));

            _globalHelper = globalHelper;
        }

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

            container.RegisterAutoWiredAs<PetAlertProvider, IPetAlertProvider>();
            container.RegisterAutoWiredAs<PetAlertMapper, IPetAlertMapper>();
            container.RegisterAutoWiredAs<PetAlertDataAccess, IPetAlertDataAccess>();

            container.RegisterAutoWiredAs<PetImageProvider, IPetImageProvider>();
            container.RegisterAutoWiredAs<PetImageMapper, IPetImageMapper>();
            container.RegisterAutoWiredAs<PetImageDataAccess, IPetImageDataAccess>();

            container.RegisterAutoWiredAs<PetSearchProvider, IPetSearchProvider>();
            container.RegisterAutoWiredAs<PetSearchMapper, IPetSearchMapper>();
            container.RegisterAutoWiredAs<PetSearchDataAccess, IPetSearchDataAccess>();

            container.RegisterAutoWiredAs<AdminProvider, IAdminProvider>();
            container.RegisterAutoWiredAs<AdminDataAccess, IAdminDataAccess>();

            container.RegisterAutoWiredAs<BaseDataAccess<OwnerTableModel>, IBaseDataAccess<OwnerTableModel>>();
            container.RegisterAutoWiredAs<BaseDataAccess<PetTableModel>, IBaseDataAccess<PetTableModel>>();
            container.RegisterAutoWiredAs<BaseDataAccess<PetImageTableModel>, IBaseDataAccess<PetImageTableModel>>();
            container.RegisterAutoWiredAs<BaseDataAccess<PetAlertTableModel>, IBaseDataAccess<PetAlertTableModel>>();
        }

        private string ConnectionString
        {
            get
            {
                return _globalHelper.GetConnectionStringByEnvironment("FindMyPet");
            }
        }
    }
}