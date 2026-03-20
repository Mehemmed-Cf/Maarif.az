using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Presentation.AppCode.DI
{
    public class MaarifServiceProviderFactory : AutofacServiceProviderFactory
    {
        public MaarifServiceProviderFactory()
            :base(OnRegister)
        {
        }

        private static void OnRegister(ContainerBuilder builder)
        {
            builder.RegisterModule<MaarifModule>();
        }
    }
}
