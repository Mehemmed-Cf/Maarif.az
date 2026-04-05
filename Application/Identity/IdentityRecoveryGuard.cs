using Infrastructure.Exceptions;

namespace Application.Identity
{
    public static class IdentityRecoveryGuard
    {
        /// <summary>
        /// Self-service rows store a normalized serial; admin rows may have null serial — then gov verify alone is enough.
        /// </summary>
        public static void EnsureSerialMatchesStoredOrUnset(string? storedSerial, string normalizedRequest)
        {
            if (string.IsNullOrEmpty(storedSerial))
                return;
            if (storedSerial != normalizedRequest)
                throw new UnauthorizedException("Verilmiş məlumatlar uyğun gəlmir.");
        }
    }
}
