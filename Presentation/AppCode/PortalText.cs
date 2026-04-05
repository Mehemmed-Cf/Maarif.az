namespace Presentation.AppCode
{
    public static class PortalText
    {
        public static string InitialsFrom(string? fullName)
        {
            var parts = (fullName ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => "?",
                1 => parts[0][..1].ToUpperInvariant(),
                _ => (parts[0][..1] + parts[^1][..1]).ToUpperInvariant()
            };
        }

        public static string WeekTypeAz(Domain.Models.Stables.WeekType w) => w switch
        {
            Domain.Models.Stables.WeekType.Both => "Hər həftə",
            Domain.Models.Stables.WeekType.Upper => "Üst həftə",
            Domain.Models.Stables.WeekType.Lower => "Alt həftə",
            _ => "—"
        };
    }
}
