using Autofac;

namespace IB_Domain.Infraestructure
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IBDomain>().As<IBDomain>().SingleInstance().InstancePerLifetimeScope();
        }
    }
}