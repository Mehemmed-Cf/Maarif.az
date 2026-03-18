using Application.Services;
using Autofac;
using FluentValidation.AspNetCore;
using Infrastructure.Abstracts;

namespace Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<JwtService>()
                .As<IJwtService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CryptoService>()
                .As<ICryptoService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileService>()
            .As<IFileService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<ValidatorInterceptor>()
                .As<IValidatorInterceptor>()
                .SingleInstance();
        }
    }
}
