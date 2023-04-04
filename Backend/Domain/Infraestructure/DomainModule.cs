using Autofac;

namespace Domain.Infraestructure
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CVDomain>().As<CVDomain>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<EUSDomain>().As<EUSDomain>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<IBDomain>().As<IBDomain>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<DeleteDomain>().As<DeleteDomain>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<ScraperDomain>().As<ScraperDomain>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<BusquedaDomain>().As<BusquedaDomain>().SingleInstance().InstancePerLifetimeScope();
        }
    }
}