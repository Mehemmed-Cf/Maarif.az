// Repository/Services/FakeGovernmentIdentityService.cs
using Application.Services;
using Domain.Models.Stables;
using Domain.Models.ValueObjects;

public class FakeGovernmentIdentityService : IGovernmentIdentityService
{
    // Keyed by (SerialNumber, FinCode) → FinData
    private static readonly Dictionary<(string, string), FinData> _seed = new()
    {
        [("AZE1234567", "ABC1234")] = new FinData
        {
            FinCode = "ABC1234",
            FullName = "Əli Həsənov",
            FatherName = "Murad",
            BirthDate = new DateTime(2000, 5, 12),
            Gender = GenderType.Male
        },
        [("AZE7654321", "XYZ9876")] = new FinData
        {
            FinCode = "XYZ9876",
            FullName = "Aytən Məmmədova",
            FatherName = "Cavid",
            BirthDate = new DateTime(2001, 3, 20),
            Gender = GenderType.Female
        },
        // add more seed records as needed

        [("AZE2489137", "QW1948A")] = new FinData
        {
            FinCode = "QW1948A",
            FullName = "Elvin Məmmədli",
            FatherName = "Rauf",
            BirthDate = new DateTime(1998, 4, 17),
            Gender = GenderType.Male
        },
        [("AZE3917642", "TR5821K")] = new FinData
        {
            FinCode = "TR5821K",
            FullName = "Aysu Əliyeva",
            FatherName = "Cavid",
            BirthDate = new DateTime(2001, 9, 8),
            Gender = GenderType.Female
        },
        [("AZE5271849", "MN7304P")] = new FinData
        {
            FinCode = "MN7304P",
            FullName = "Tural Hüseynov",
            FatherName = "Elman",
            BirthDate = new DateTime(1997, 12, 26),
            Gender = GenderType.Male
        },
        [("AZE6842391", "LP4617D")] = new FinData
        {
            FinCode = "LP4617D",
            FullName = "Nərmin Quliyeva",
            FatherName = "Fərid",
            BirthDate = new DateTime(2002, 2, 14),
            Gender = GenderType.Female
        },
        [("AZE7391582", "VK9053H")] = new FinData
        {
            FinCode = "VK9053H",
            FullName = "Orxan Əliyev",
            FatherName = "Kamran",
            BirthDate = new DateTime(1999, 6, 3),
            Gender = GenderType.Male
        },
        [("AZE8457629", "SD2846N")] = new FinData
        {
            FinCode = "SD2846N",
            FullName = "Lalə Məmmədova",
            FatherName = "Rəşad",
            BirthDate = new DateTime(2000, 11, 21),
            Gender = GenderType.Female
        },
        [("AZE9124685", "PX6731R")] = new FinData
        {
            FinCode = "PX6731R",
            FullName = "Murad İsmayılov",
            FatherName = "Natiq",
            BirthDate = new DateTime(1996, 8, 30),
            Gender = GenderType.Male
        },
        [("AZE1647398", "HZ4189C")] = new FinData
        {
            FinCode = "HZ4189C",
            FullName = "Günay Rzayeva",
            FatherName = "Səməd",
            BirthDate = new DateTime(2003, 5, 12),
            Gender = GenderType.Female
        },
        [("AZE2759461", "JK5927T")] = new FinData
        {
            FinCode = "JK5927T",
            FullName = "Eldar Kərimov",
            FatherName = "Tofiq",
            BirthDate = new DateTime(1995, 1, 19),
            Gender = GenderType.Male
        },
        [("AZE6083147", "NC8472V")] = new FinData
        {
            FinCode = "NC8472V",
            FullName = "Sevda Abbasova",
            FatherName = "Vüqar",
            BirthDate = new DateTime(2001, 7, 27),
            Gender = GenderType.Female
        },

    };

    public Task<FinData?> VerifyAsync(
        string serialNumber,
        string finCode,
        CancellationToken ct = default)
    {
        var key = (serialNumber.Trim().ToUpper(), finCode.Trim().ToUpper());
        _seed.TryGetValue(key, out var result);
        return Task.FromResult(result);
    }
}