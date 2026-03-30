using Autofac;
using Application;
using DataAccessLayer;
using Repository;

namespace Presentation.AppCode.DI
{
    public class MaarifModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<DataAccessModule>();

            builder.RegisterAssemblyModules(typeof(DataAccessModule).Assembly);
            builder.RegisterAssemblyModules(typeof(ApplicationModule).Assembly);

            builder.RegisterAssemblyTypes(typeof(IRepositoryReference).Assembly)
                .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(IRepositoryReference).Assembly)
            //    .AsImplementedInterfaces();
        }
    }
}
