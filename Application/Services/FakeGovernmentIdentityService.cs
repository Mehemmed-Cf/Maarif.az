// Repository/Services/FakeGovernmentIdentityService.cs
using Application.Services;
using Domain.Models.Stables;
using Domain.Models.ValueObjects;

public partial class FakeGovernmentIdentityService : IGovernmentIdentityService
{
    // Keyed by (SerialNumber, FinCode) → FinData
    private static readonly Dictionary<(string, string), FinData> _seed = new()
    {
        [("AZE8392014", "AB8392K")] = new FinData
        {
            FinCode = "AB8392K",
            FullName = "Kənan Qasımov",
            FatherName = "İlqar",
            BirthDate = new DateTime(2000, 1, 15),
            Gender = GenderType.Male,
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE7482931", "CD7482M")] = new FinData
        {
            FinCode = "CD7482M",
            FullName = "Leyla Səfərova",
            FatherName = "Anar",
            BirthDate = new DateTime(2001, 4, 22),
            Gender = GenderType.Female,
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Education = EducationType.Paid
        },
        [("AZE5928374", "EF5928P")] = new FinData
        {
            FinCode = "EF5928P",
            FullName = "Samir Babayev",
            FatherName = "Ruslan",
            BirthDate = new DateTime(1999, 8, 10),
            Gender = GenderType.Male,
            Department = DepartmentType.ElectricalEngineering,
            Education = EducationType.StateFunded
        },
        [("AZE1029384", "GH1029R")] = new FinData
        {
            FinCode = "GH1029R",
            FullName = "Nigar Cəfərova",
            FatherName = "Nurlan",
            BirthDate = new DateTime(2002, 11, 5),
            Gender = GenderType.Female,
            Department = DepartmentType.EngineeringPhysicsAndElectronics,
            Education = EducationType.Paid
        },
        [("AZE5839201", "IJ5839T")] = new FinData
        {
            FinCode = "IJ5839T",
            FullName = "İlqar Məmmədov",
            FatherName = "Ramil",
            BirthDate = new DateTime(1998, 3, 18),
            Gender = GenderType.Male,
            Department = DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE4728193", "KL4728V")] = new FinData
        {
            FinCode = "KL4728V",
            FullName = "Fidan Əliyeva",
            FatherName = "Şahin",
            BirthDate = new DateTime(2003, 7, 25),
            Gender = GenderType.Female,
            Department = DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies,
            Education = EducationType.Paid
        },
        [("AZE3617284", "MN3617X")] = new FinData
        {
            FinCode = "MN3617X",
            FullName = "Anar Hüseynov",
            FatherName = "Aslan",
            BirthDate = new DateTime(1997, 10, 12),
            Gender = GenderType.Male,
            Department = DepartmentType.MechanicalEngineeringTechnology,
            Education = EducationType.StateFunded
        },
        [("AZE2506173", "OP2506Z")] = new FinData
        {
            FinCode = "OP2506Z",
            FullName = "Zəhra Quliyeva",
            FatherName = "Həsən",
            BirthDate = new DateTime(2000, 2, 28),
            Gender = GenderType.Female,
            Department = DepartmentType.MetallurgyAndMaterialsTechnology,
            Education = EducationType.Paid
        },
        [("AZE1495062", "QR1495B")] = new FinData
        {
            FinCode = "QR1495B",
            FullName = "Ruslan İsmayılov",
            FatherName = "İbrahim",
            BirthDate = new DateTime(1996, 6, 14),
            Gender = GenderType.Male,
            Department = DepartmentType.ChemistryTechnologyRecyclingAndEcology,
            Education = EducationType.StateFunded
        },
        [("AZE9384751", "ST9384D")] = new FinData
        {
            FinCode = "ST9384D",
            FullName = "Aygün Rzayeva",
            FatherName = "Kənan",
            BirthDate = new DateTime(2001, 9, 30),
            Gender = GenderType.Female,
            Department = DepartmentType.Mechanics,
            Education = EducationType.Paid
        },
        [("AZE8273640", "UV8273F")] = new FinData
        {
            FinCode = "UV8273F",
            FullName = "Nurlan Kərimov",
            FatherName = "Samir",
            BirthDate = new DateTime(1995, 12, 8),
            Gender = GenderType.Male,
            Department = DepartmentType.EngineeringMathematicsAndArtificialIntelligence,
            Education = EducationType.StateFunded
        },
        [("AZE7162539", "WX7162H")] = new FinData
        {
            FinCode = "WX7162H",
            FullName = "Şəbnəm Abbasova",
            FatherName = "İlqar",
            BirthDate = new DateTime(2002, 5, 19),
            Gender = GenderType.Female,
            Department = DepartmentType.RadioEngineeringAndTelecommunicationsEngineering,
            Education = EducationType.Paid
        },
        [("AZE6051428", "YZ6051J")] = new FinData
        {
            FinCode = "YZ6051J",
            FullName = "Ramil Qasımov",
            FatherName = "Anar",
            BirthDate = new DateTime(1999, 1, 3),
            Gender = GenderType.Male,
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE5940317", "AA5940L")] = new FinData
        {
            FinCode = "AA5940L",
            FullName = "Səbinə Səfərova",
            FatherName = "Ruslan",
            BirthDate = new DateTime(2000, 4, 11),
            Gender = GenderType.Female,
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Education = EducationType.Paid
        },
        [("AZE4839206", "BB4839N")] = new FinData
        {
            FinCode = "BB4839N",
            FullName = "Şahin Babayev",
            FatherName = "Nurlan",
            BirthDate = new DateTime(1998, 8, 27),
            Gender = GenderType.Male,
            Department = DepartmentType.ElectricalEngineering,
            Education = EducationType.StateFunded
        },
        [("AZE3728195", "CC3728Q")] = new FinData
        {
            FinCode = "CC3728Q",
            FullName = "Günel Cəfərova",
            FatherName = "Ramil",
            BirthDate = new DateTime(2001, 11, 14),
            Gender = GenderType.Female,
            Department = DepartmentType.EngineeringPhysicsAndElectronics,
            Education = EducationType.Paid
        },
        [("AZE2617084", "DD2617S")] = new FinData
        {
            FinCode = "DD2617S",
            FullName = "Aslan Məmmədov",
            FatherName = "Şahin",
            BirthDate = new DateTime(1997, 3, 6),
            Gender = GenderType.Male,
            Department = DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE1506973", "EE1506U")] = new FinData
        {
            FinCode = "EE1506U",
            FullName = "Vüsalə Əliyeva",
            FatherName = "Aslan",
            BirthDate = new DateTime(2003, 7, 2),
            Gender = GenderType.Female,
            Department = DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies,
            Education = EducationType.Paid
        },
        [("AZE0495862", "FF0495W")] = new FinData
        {
            FinCode = "FF0495W",
            FullName = "Həsən Hüseynov",
            FatherName = "Həsən",
            BirthDate = new DateTime(1996, 10, 29),
            Gender = GenderType.Male,
            Department = DepartmentType.MechanicalEngineeringTechnology,
            Education = EducationType.StateFunded
        },
        [("AZE9384750", "GG9384Y")] = new FinData
        {
            FinCode = "GG9384Y",
            FullName = "Könül Quliyeva",
            FatherName = "İbrahim",
            BirthDate = new DateTime(2000, 2, 16),
            Gender = GenderType.Female,
            Department = DepartmentType.MetallurgyAndMaterialsTechnology,
            Education = EducationType.Paid
        },
        [("AZE8273649", "HH8273A")] = new FinData
        {
            FinCode = "HH8273A",
            FullName = "İbrahim İsmayılov",
            FatherName = "Kənan",
            BirthDate = new DateTime(1995, 6, 4),
            Gender = GenderType.Male,
            Department = DepartmentType.ChemistryTechnologyRecyclingAndEcology,
            Education = EducationType.StateFunded
        },
        [("AZE7162538", "II7162C")] = new FinData
        {
            FinCode = "II7162C",
            FullName = "Pərvanə Rzayeva",
            FatherName = "Samir",
            BirthDate = new DateTime(2002, 9, 21),
            Gender = GenderType.Female,
            Department = DepartmentType.Mechanics,
            Education = EducationType.Paid
        },
        [("AZE6051427", "JJ6051E")] = new FinData
        {
            FinCode = "JJ6051E",
            FullName = "Vüqar Kərimov",
            FatherName = "İlqar",
            BirthDate = new DateTime(1999, 12, 13),
            Gender = GenderType.Male,
            Department = DepartmentType.EngineeringMathematicsAndArtificialIntelligence,
            Education = EducationType.StateFunded
        },
        [("AZE5940316", "KK5940G")] = new FinData
        {
            FinCode = "KK5940G",
            FullName = "Türkan Abbasova",
            FatherName = "Anar",
            BirthDate = new DateTime(2001, 5, 6),
            Gender = GenderType.Female,
            Department = DepartmentType.RadioEngineeringAndTelecommunicationsEngineering,
            Education = EducationType.Paid
        },
        [("AZE4839205", "LL4839I")] = new FinData
        {
            FinCode = "LL4839I",
            FullName = "Rəşad Qasımov",
            FatherName = "Ruslan",
            BirthDate = new DateTime(1998, 1, 24),
            Gender = GenderType.Male,
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE3728194", "MM3728K")] = new FinData
        {
            FinCode = "MM3728K",
            FullName = "Röya Səfərova",
            FatherName = "Nurlan",
            BirthDate = new DateTime(2000, 4, 18),
            Gender = GenderType.Female,
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Education = EducationType.Paid
        },
        [("AZE2617083", "NN2617M")] = new FinData
        {
            FinCode = "NN2617M",
            FullName = "Fərid Babayev",
            FatherName = "Ramil",
            BirthDate = new DateTime(1997, 8, 5),
            Gender = GenderType.Male,
            Department = DepartmentType.ElectricalEngineering,
            Education = EducationType.StateFunded
        },
        [("AZE1506972", "OO1506O")] = new FinData
        {
            FinCode = "OO1506O",
            FullName = "Ülviyyə Cəfərova",
            FatherName = "Şahin",
            BirthDate = new DateTime(2003, 11, 22),
            Gender = GenderType.Female,
            Department = DepartmentType.EngineeringPhysicsAndElectronics,
            Education = EducationType.Paid
        },
        [("AZE0495861", "PP0495Q")] = new FinData
        {
            FinCode = "PP0495Q",
            FullName = "Natiq Məmmədov",
            FatherName = "Aslan",
            BirthDate = new DateTime(1996, 3, 15),
            Gender = GenderType.Male,
            Department = DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE9384759", "QQ9384S")] = new FinData
        {
            FinCode = "QQ9384S",
            FullName = "Aytac Əliyeva",
            FatherName = "Həsən",
            BirthDate = new DateTime(2001, 7, 9),
            Gender = GenderType.Female,
            Department = DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies,
            Education = EducationType.Paid
        },
        [("AZE8273648", "RR8273U")] = new FinData
        {
            FinCode = "RR8273U",
            FullName = "Səməd Hüseynov",
            FatherName = "İbrahim",
            BirthDate = new DateTime(1995, 10, 1),
            Gender = GenderType.Male,
            Department = DepartmentType.MechanicalEngineeringTechnology,
            Education = EducationType.StateFunded
        },
        [("AZE7162537", "SS7162W")] = new FinData
        {
            FinCode = "SS7162W",
            FullName = "Banu Quliyeva",
            FatherName = "Kənan",
            BirthDate = new DateTime(2002, 2, 23),
            Gender = GenderType.Female,
            Department = DepartmentType.MetallurgyAndMaterialsTechnology,
            Education = EducationType.Paid
        },
        [("AZE6051426", "TT6051Y")] = new FinData
        {
            FinCode = "TT6051Y",
            FullName = "Tofiq İsmayılov",
            FatherName = "Samir",
            BirthDate = new DateTime(1999, 6, 11),
            Gender = GenderType.Male,
            Department = DepartmentType.ChemistryTechnologyRecyclingAndEcology,
            Education = EducationType.StateFunded
        },
        [("AZE5940315", "UU5940A")] = new FinData
        {
            FinCode = "UU5940A",
            FullName = "Səma Rzayeva",
            FatherName = "İlqar",
            BirthDate = new DateTime(2000, 9, 28),
            Gender = GenderType.Female,
            Department = DepartmentType.Mechanics,
            Education = EducationType.Paid
        },
        [("AZE4839204", "VV4839C")] = new FinData
        {
            FinCode = "VV4839C",
            FullName = "Elşən Kərimov",
            FatherName = "Anar",
            BirthDate = new DateTime(1998, 12, 17),
            Gender = GenderType.Male,
            Department = DepartmentType.EngineeringMathematicsAndArtificialIntelligence,
            Education = EducationType.StateFunded
        },
        [("AZE3728193", "WW3728E")] = new FinData
        {
            FinCode = "WW3728E",
            FullName = "Nəzrin Abbasova",
            FatherName = "Ruslan",
            BirthDate = new DateTime(2001, 5, 2),
            Gender = GenderType.Female,
            Department = DepartmentType.RadioEngineeringAndTelecommunicationsEngineering,
            Education = EducationType.Paid
        },
        [("AZE2617082", "XX2617G")] = new FinData
        {
            FinCode = "XX2617G",
            FullName = "Ceyhun Qasımov",
            FatherName = "Nurlan",
            BirthDate = new DateTime(1997, 1, 20),
            Gender = GenderType.Male,
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE1506971", "YY1506I")] = new FinData
        {
            FinCode = "YY1506I",
            FullName = "Gülşən Səfərova",
            FatherName = "Ramil",
            BirthDate = new DateTime(2003, 4, 15),
            Gender = GenderType.Female,
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Education = EducationType.Paid
        },
        [("AZE0495860", "ZZ0495K")] = new FinData
        {
            FinCode = "ZZ0495K",
            FullName = "Xəyal Babayev",
            FatherName = "Şahin",
            BirthDate = new DateTime(1996, 8, 7),
            Gender = GenderType.Male,
            Department = DepartmentType.ElectricalEngineering,
            Education = EducationType.StateFunded
        },
        [("AZE9384758", "AB9384M")] = new FinData
        {
            FinCode = "AB9384M",
            FullName = "Fərəh Cəfərova",
            FatherName = "Aslan",
            BirthDate = new DateTime(2001, 11, 25),
            Gender = GenderType.Female,
            Department = DepartmentType.EngineeringPhysicsAndElectronics,
            Education = EducationType.Paid
        },
        [("AZE8273647", "CD8273O")] = new FinData
        {
            FinCode = "CD8273O",
            FullName = "Qüdrət Məmmədov",
            FatherName = "Həsən",
            BirthDate = new DateTime(1995, 3, 11),
            Gender = GenderType.Male,
            Department = DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE7162536", "EF7162Q")] = new FinData
        {
            FinCode = "EF7162Q",
            FullName = "Jalə Əliyeva",
            FatherName = "İbrahim",
            BirthDate = new DateTime(2002, 7, 5),
            Gender = GenderType.Female,
            Department = DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies,
            Education = EducationType.Paid
        },
        [("AZE6051425", "GH6051S")] = new FinData
        {
            FinCode = "GH6051S",
            FullName = "Mahir Hüseynov",
            FatherName = "Kənan",
            BirthDate = new DateTime(1999, 10, 22),
            Gender = GenderType.Male,
            Department = DepartmentType.MechanicalEngineeringTechnology,
            Education = EducationType.StateFunded
        },
        [("AZE5940314", "IJ5940U")] = new FinData
        {
            FinCode = "IJ5940U",
            FullName = "Dilarə Quliyeva",
            FatherName = "Samir",
            BirthDate = new DateTime(2000, 2, 9),
            Gender = GenderType.Female,
            Department = DepartmentType.MetallurgyAndMaterialsTechnology,
            Education = EducationType.Paid
        },
        [("AZE4839203", "KL4839W")] = new FinData
        {
            FinCode = "KL4839W",
            FullName = "Pərviz İsmayılov",
            FatherName = "İlqar",
            BirthDate = new DateTime(1998, 6, 26),
            Gender = GenderType.Male,
            Department = DepartmentType.ChemistryTechnologyRecyclingAndEcology,
            Education = EducationType.StateFunded
        },
        [("AZE3728192", "MN3728Y")] = new FinData
        {
            FinCode = "MN3728Y",
            FullName = "Zümrüd Rzayeva",
            FatherName = "Anar",
            BirthDate = new DateTime(2001, 9, 12),
            Gender = GenderType.Female,
            Department = DepartmentType.Mechanics,
            Education = EducationType.Paid
        },
        [("AZE2617081", "OP2617A")] = new FinData
        {
            FinCode = "OP2617A",
            FullName = "Süleyman Kərimov",
            FatherName = "Ruslan",
            BirthDate = new DateTime(1997, 12, 29),
            Gender = GenderType.Male,
            Department = DepartmentType.EngineeringMathematicsAndArtificialIntelligence,
            Education = EducationType.StateFunded
        },
        [("AZE1506970", "QR1506C")] = new FinData
        {
            FinCode = "QR1506C",
            FullName = "Çinarə Abbasova",
            FatherName = "Nurlan",
            BirthDate = new DateTime(2003, 5, 8),
            Gender = GenderType.Female,
            Department = DepartmentType.RadioEngineeringAndTelecommunicationsEngineering,
            Education = EducationType.Paid
        },
        [("AZE0495859", "ST0495E")] = new FinData
        {
            FinCode = "ST0495E",
            FullName = "Elgün Qasımov",
            FatherName = "Ramil",
            BirthDate = new DateTime(1996, 1, 31),
            Gender = GenderType.Male,
            Department = DepartmentType.TransportEquipmentAndManagementTechnologies,
            Education = EducationType.StateFunded
        },
        [("AZE9384757", "UV9384G")] = new FinData
        {
            FinCode = "UV9384G",
            FullName = "Səhər Səfərova",
            FatherName = "Şahin",
            BirthDate = new DateTime(2000, 4, 27),
            Gender = GenderType.Female,
            Department = DepartmentType.TransportLogisticsAndTrafficSafety,
            Education = EducationType.Paid
        }
    };

    public Task<FinData?> VerifyAsync(
        string serialNumber,
        string finCode,
        CancellationToken ct = default)
    {
        var key = (serialNumber.Trim().ToUpper(), finCode.Trim().ToUpper());
        _seed.TryGetValue(key, out var result);

        // Normalize FinCode to uppercase so it matches DB lookup
        if (result != null)
            result.FinCode = result.FinCode.ToUpper();

        return Task.FromResult(result);
    }
}