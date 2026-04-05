using Domain.Models.Stables;
using Domain.Models.ValueObjects;

public partial class FakeGovernmentIdentityService
{
    private static readonly Dictionary<(string, string), TeacherGovernmentData> _teacherSeed = new()
    {
        [("MUE0100001", "TM01001")] = new TeacherGovernmentData
        {
            FinCode = "TM01001",
            FullName = "Elşad Məmmədov",
            BirthDate = new DateTime(1985, 3, 12),
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Experience = 12.5
        },
        [("MUE0100002", "TM01002")] = new TeacherGovernmentData
        {
            FinCode = "TM01002",
            FullName = "Gülxar Həsənova",
            BirthDate = new DateTime(1988, 7, 21),
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Experience = 9.0
        },
        [("MUE0100003", "TM01003")] = new TeacherGovernmentData
        {
            FinCode = "TM01003",
            FullName = "Rəşad Əliyev",
            BirthDate = new DateTime(1982, 11, 5),
            Department = DepartmentType.ElectricalEngineering,
            Experience = 15.0
        },
        [("MUE0100004", "TM01004")] = new TeacherGovernmentData
        {
            FinCode = "TM01004",
            FullName = "Nərgiz Quliyeva",
            BirthDate = new DateTime(1990, 1, 18),
            Department = DepartmentType.EngineeringPhysicsAndElectronics,
            Experience = 7.5
        },
        [("MUE0100005", "TM01005")] = new TeacherGovernmentData
        {
            FinCode = "TM01005",
            FullName = "Tural İsmayılov",
            BirthDate = new DateTime(1980, 9, 30),
            Department = DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies,
            Experience = 18.0
        },
        [("MUE0100006", "TM01006")] = new TeacherGovernmentData
        {
            FinCode = "TM01006",
            FullName = "Sevinc Rəhimova",
            BirthDate = new DateTime(1987, 4, 14),
            Department = DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies,
            Experience = 10.0
        },
        [("MUE0100007", "TM01007")] = new TeacherGovernmentData
        {
            FinCode = "TM01007",
            FullName = "Orxan Kərimov",
            BirthDate = new DateTime(1983, 6, 8),
            Department = DepartmentType.MechanicalEngineeringTechnology,
            Experience = 14.0
        },
        [("MUE0100008", "TM01008")] = new TeacherGovernmentData
        {
            FinCode = "TM01008",
            FullName = "Lalə Yusifova",
            BirthDate = new DateTime(1991, 2, 25),
            Department = DepartmentType.MetallurgyAndMaterialsTechnology,
            Experience = 6.0
        },
        [("MUE0100009", "TM01009")] = new TeacherGovernmentData
        {
            FinCode = "TM01009",
            FullName = "Vüsal Bağırov",
            BirthDate = new DateTime(1979, 12, 1),
            Department = DepartmentType.ChemistryTechnologyRecyclingAndEcology,
            Experience = 20.0
        },
        [("MUE0100010", "TM01010")] = new TeacherGovernmentData
        {
            FinCode = "TM01010",
            FullName = "Aygün Soltanova",
            BirthDate = new DateTime(1986, 5, 17),
            Department = DepartmentType.Mechanics,
            Experience = 11.0
        },
        [("MUE0100011", "TM01011")] = new TeacherGovernmentData
        {
            FinCode = "TM01011",
            FullName = "Ramin Əhmədov",
            BirthDate = new DateTime(1984, 8, 9),
            Department = DepartmentType.EngineeringMathematicsAndArtificialIntelligence,
            Experience = 13.5
        },
        [("MUE0100012", "TM01012")] = new TeacherGovernmentData
        {
            FinCode = "TM01012",
            FullName = "Könül Ələkbərova",
            BirthDate = new DateTime(1989, 10, 22),
            Department = DepartmentType.RadioEngineeringAndTelecommunicationsEngineering,
            Experience = 8.0
        },
        [("MUE0100013", "TM01013")] = new TeacherGovernmentData
        {
            FinCode = "TM01013",
            FullName = "Emil Nəbiyev",
            BirthDate = new DateTime(1981, 1, 7),
            Department = DepartmentType.ComputerTechnologies,
            Experience = 16.0
        },
        [("MUE0100014", "TM01014")] = new TeacherGovernmentData
        {
            FinCode = "TM01014",
            FullName = "Ülkər Məlikova",
            BirthDate = new DateTime(1992, 3, 19),
            Department = DepartmentType.CyberSecurity,
            Experience = 5.0
        },
        [("MUE0100015", "TM01015")] = new TeacherGovernmentData
        {
            FinCode = "TM01015",
            FullName = "Cavid Əsgərov",
            BirthDate = new DateTime(1978, 7, 3),
            Department = DepartmentType.SpecialTechnologiesAndEquipment,
            Experience = 22.0
        },
        [("MUE0100016", "TM01016")] = new TeacherGovernmentData
        {
            FinCode = "TM01016",
            FullName = "Nərmin Zeynalova",
            BirthDate = new DateTime(1988, 11, 28),
            Department = DepartmentType.DefenseSystemsAndTechnologicalIntegration,
            Experience = 9.5
        },
        [("MUE0100017", "TM01017")] = new TeacherGovernmentData
        {
            FinCode = "TM01017",
            FullName = "Elçin Hüseynov",
            BirthDate = new DateTime(1985, 4, 11),
            Department = DepartmentType.HumanitarianSubjects,
            Experience = 12.0
        },
        [("MUE0100018", "TM01018")] = new TeacherGovernmentData
        {
            FinCode = "TM01018",
            FullName = "Gülnar Əhmədova",
            BirthDate = new DateTime(1990, 9, 6),
            Department = DepartmentType.ForeignLanguages,
            Experience = 7.0
        },
        [("MUE0100019", "TM01019")] = new TeacherGovernmentData
        {
            FinCode = "TM01019",
            FullName = "Samir Cəfərov",
            BirthDate = new DateTime(1983, 2, 14),
            Department = DepartmentType.IndustrialEngineeringAndSustainableEconomy,
            Experience = 14.5
        },
        [("MUE0100020", "TM01020")] = new TeacherGovernmentData
        {
            FinCode = "TM01020",
            FullName = "Nigar Əliyeva",
            BirthDate = new DateTime(1987, 6, 30),
            Department = DepartmentType.BusinessManagement,
            Experience = 10.5
        },
        [("MUE0100021", "TM01021")] = new TeacherGovernmentData
        {
            FinCode = "TM01021",
            FullName = "Murad Qasımov",
            BirthDate = new DateTime(1980, 12, 20),
            Department = DepartmentType.DigitalEconomyAndFinancialTechnologies,
            Experience = 17.0
        }
    };

    public Task<TeacherGovernmentData?> VerifyTeacherAsync(
        string serialNumber,
        string finCode,
        CancellationToken ct = default)
    {
        var key = (serialNumber.Trim().ToUpperInvariant(), finCode.Trim().ToUpperInvariant());
        if (!_teacherSeed.TryGetValue(key, out var src))
            return Task.FromResult<TeacherGovernmentData?>(null);

        var fin = src.FinCode.Trim().ToUpperInvariant();
        return Task.FromResult<TeacherGovernmentData?>(new TeacherGovernmentData
        {
            FinCode = fin,
            FullName = src.FullName,
            BirthDate = src.BirthDate,
            Department = src.Department,
            Experience = src.Experience,
            MobileNumber = src.MobileNumber
        });
    }
}
