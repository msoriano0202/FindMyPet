using FindMyPet.MVC.Controllers;
using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Helpers;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.ServiceClients;
using FindMyPet.Shared;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace FindMyPet.MVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            container.RegisterType<IFindMyPetServiceClient, FindMyPetServiceClient>();
            container.RegisterType<IOwnerServiceClient, OwnerServiceClient>();
            container.RegisterType<IPetServiceClient, PetServiceClient>();
            container.RegisterType<IPetSearchServiceClient, PetSearchServiceClient>();

            container.RegisterType<IPetImageMapper, PetImageMapper>();
            container.RegisterType<IPetMapper, PetMapper>();
            container.RegisterType<IOwnerMapper, OwnerMapper>();

            container.RegisterType<IOwnerDataLoader, OwnerDataLoader>();
            container.RegisterType<IPetDataLoader, PetDataLoader>();
            container.RegisterType<IPetSearchDataLoader, PetSearchDataLoader>();

            container.RegisterType<IImageHelper, ImageHelper>();
            container.RegisterType<IGeneralHelper, GeneralHelper>();
            container.RegisterType<IEmailHelper, EmailHelper>();
            container.RegisterType<IGlobalHelper, GlobalHelper>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}