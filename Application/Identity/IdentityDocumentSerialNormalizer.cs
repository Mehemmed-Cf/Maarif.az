namespace Application.Identity
{
    public static class IdentityDocumentSerialNormalizer
    {
        public static string Normalize(string? serial)
        {
            if (string.IsNullOrWhiteSpace(serial))
                return string.Empty;
            return serial.Trim().ToUpperInvariant();
        }
    }
}
