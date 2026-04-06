using Autofac;

namespace DataAccessLayer
{
    // DataContext is registered only in Program.cs via AddDbContext; do not re-register here after Autofac Populate.
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
