using Autofac;

namespace CV_Domain.Infraestructure
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CVDomain>().As<CVDomain>().SingleInstance().InstancePerLifetimeScope();
        }
    }
}