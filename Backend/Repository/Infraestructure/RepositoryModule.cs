using Autofac;
using Data;
using Microsoft.Extensions.Logging;

namespace Repository.Infraestructure
{
	public class RepositoryModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register((ctx, p) =>
			{
				var loggerFactory = ctx.Resolve<ILoggerFactory>();
				var logger = loggerFactory.CreateLogger<DataRepository>();
				return new DataRepository(logger, ctx.Resolve<HealthCenterContext>());
			}).As<DataRepository>().InstancePerLifetimeScope();

			builder.Register((ctx, p) =>
			{
				var loggerFactory = ctx.Resolve<ILoggerFactory>();
				var logger = loggerFactory.CreateLogger<TipoRepository>();
				return new TipoRepository(logger, ctx.Resolve<HealthCenterContext>());
			}).As<TipoRepository>().InstancePerLifetimeScope();
		}
	}
}
