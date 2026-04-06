using Domain.Models.Stables;
using Domain.Models.ValueObjects;

namespace Application.Services;

/// <summary>
/// QA / pilot identities for the fake government API only. These are <b>not</b> auto-registered:
/// use the public student or teacher registration forms with the listed serial + FIN pairs.
/// Default password after successful registration matches normal flow: <see cref="DefaultRegistrationPassword"/>.
/// </summary>
public static class PilotTestIdentityData
{
    public const string DefaultRegistrationPassword = "Education123!";

    /// <summary>Copy-paste reference: serial (AZE…) + FIN for student register.</summary>
    public static IReadOnlyList<(string SerialNumber, string FinCode, string Label)> StudentPairs { get; } =
    [
        ("AZE9000001", "PT90001", "Pilot Tələbə 1"),
        ("AZE9000002", "PT90002", "Pilot Tələbə 2"),
        ("AZE9000003", "PT90003", "Pilot Tələbə 3"),
        ("AZE9000004", "PT90004", "Pilot Tələbə 4"),
        ("AZE9000005", "PT90005", "Pilot Tələbə 5")
    ];

    /// <summary>Copy-paste reference: serial (MUE…) + FIN for teacher register.</summary>
    public static IReadOnlyList<(string SerialNumber, string FinCode, string Label)> TeacherPairs { get; } =
    [
        ("MUE0100091", "PTT9001", "Pilot Müəllim 1"),
        ("MUE0100092", "PTT9002", "Pilot Müəllim 2")
    ];

    /// <summary>Full fake-government payloads (same keys as <see cref="StudentPairs"/>).</summary>
    public static IReadOnlyDictionary<(string Serial, string Fin), FinData> Students { get; } =
        new Dictionary<(string, string), FinData>
        {
            [("AZE9000001", "PT90001")] = new FinData
            {
                FinCode = "PT90001",
                FullName = "Pilot Tələbə 1",
                FatherName = "Test",
                BirthDate = new DateTime(2002, 1, 10),
                Gender = GenderType.Male,
                Department = DepartmentType.ComputerTechnologies,
                Education = EducationType.StateFunded
            },
            [("AZE9000002", "PT90002")] = new FinData
            {
                FinCode = "PT90002",
                FullName = "Pilot Tələbə 2",
                FatherName = "Test",
                BirthDate = new DateTime(2002, 2, 11),
                Gender = GenderType.Female,
                Department = DepartmentType.CyberSecurity,
                Education = EducationType.Paid
            },
            [("AZE9000003", "PT90003")] = new FinData
            {
                FinCode = "PT90003",
                FullName = "Pilot Tələbə 3",
                FatherName = "Test",
                BirthDate = new DateTime(2002, 3, 12),
                Gender = GenderType.Male,
                Department = DepartmentType.ElectricalEngineering,
                Education = EducationType.StateFunded
            },
            [("AZE9000004", "PT90004")] = new FinData
            {
                FinCode = "PT90004",
                FullName = "Pilot Tələbə 4",
                FatherName = "Test",
                BirthDate = new DateTime(2002, 4, 13),
                Gender = GenderType.Female,
                Department = DepartmentType.Mechanics,
                Education = EducationType.Paid
            },
            [("AZE9000005", "PT90005")] = new FinData
            {
                FinCode = "PT90005",
                FullName = "Pilot Tələbə 5",
                FatherName = "Test",
                BirthDate = new DateTime(2002, 5, 14),
                Gender = GenderType.Male,
                Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
                Education = EducationType.StateFunded
            }
        };

    /// <summary>Full fake-government payloads (same keys as <see cref="TeacherPairs"/>).</summary>
    public static IReadOnlyDictionary<(string Serial, string Fin), TeacherGovernmentData> Teachers { get; } =
        new Dictionary<(string, string), TeacherGovernmentData>
        {
            [("MUE0100091", "PTT9001")] = new TeacherGovernmentData
            {
                FinCode = "PTT9001",
                FullName = "Pilot Müəllim 1",
                BirthDate = new DateTime(1985, 6, 1),
                Department = DepartmentType.ComputerTechnologies,
                Experience = 10.0
            },
            [("MUE0100092", "PTT9002")] = new TeacherGovernmentData
            {
                FinCode = "PTT9002",
                FullName = "Pilot Müəllim 2",
                BirthDate = new DateTime(1987, 8, 15),
                Department = DepartmentType.CyberSecurity,
                Experience = 8.0
            }
        };
}
