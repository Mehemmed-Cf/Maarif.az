using Autofac;

namespace Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Scans the Repository assembly and registers every class whose name ends
            // with "Repository" against all interfaces it implements.
            // This means StudentRepository → IStudentRepository + IAsyncRepository<Student>,
            // SubjectRepository → ISubjectRepository + IAsyncRepository<Subject>, etc.
            // New repositories are picked up automatically — no manual registration needed.
            builder.RegisterAssemblyTypes(typeof(IRepositoryReference).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}