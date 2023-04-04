using Autofac;

namespace EUS_Domain.Infraestructure
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EUSDomain>().As<EUSDomain>().SingleInstance().InstancePerLifetimeScope();
        }
    }
}