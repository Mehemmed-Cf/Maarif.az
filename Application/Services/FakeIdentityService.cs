using Infrastructure.Abstracts;
namespace Application.Services
{
    public class FakeIdentityService : IIdentityService
    {
        public int? GetPrincipialId()
        {
            return 1;
        }
    }
}
