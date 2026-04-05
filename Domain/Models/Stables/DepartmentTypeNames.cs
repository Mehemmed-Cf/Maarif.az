namespace Domain.Models.Stables
{
    /// <summary>
    /// Maps <see cref="DepartmentType"/> to the exact <c>Department.Name</c> values seeded in the database
    /// (must stay in sync with <c>DataSeeder.SeedDepartmentsAsync</c>).
    /// </summary>
    public static class DepartmentTypeNames
    {
        public static string ToSeededDepartmentName(this DepartmentType type) => type switch
        {
            DepartmentType.TransportEquipmentAndManagementTechnologies =>
                "Nəqliyyat texnikası və idarəetmə texnologiyaları",
            DepartmentType.TransportLogisticsAndTrafficSafety =>
                "Nəqliyyat logistikası və yol hərəkətinin təhlükəsizliyi",
            DepartmentType.ElectricalEngineering => "Elektrotexnika",
            DepartmentType.EngineeringPhysicsAndElectronics =>
                "Mühəndis fizikası və elektronika",
            DepartmentType.EnergyEfficiencyAndGreenEnergyTechnologies =>
                "Enerji səmərəliliyi və yaşıl enerji texnologiyaları",
            DepartmentType.MachineDesignMechatronicsAndIndustrialTechnologies =>
                "Maşın konstruksiyası, mexatronika və sənaye texnologiyaları",
            DepartmentType.MechanicalEngineeringTechnology => "Maşınqayırma texnologiyası",
            DepartmentType.MetallurgyAndMaterialsTechnology =>
                "Metallurgiya və materiallar texnologiyası",
            DepartmentType.ChemistryTechnologyRecyclingAndEcology =>
                "Kimya texnologiyası, emal və ekologiya",
            DepartmentType.Mechanics => "Mexanika",
            DepartmentType.EngineeringMathematicsAndArtificialIntelligence =>
                "Mühəndis riyaziyyatı və süni intellekt",
            DepartmentType.RadioEngineeringAndTelecommunicationsEngineering =>
                "Radioelektronika və telekommunikasiya mühəndisliyi",
            DepartmentType.ComputerTechnologies => "Kompüter texnologiyaları",
            DepartmentType.CyberSecurity => "Kibertəhlükəsizlik",
            DepartmentType.SpecialTechnologiesAndEquipment =>
                "Xüsusi texnologiyalar və avadanlıq",
            DepartmentType.DefenseSystemsAndTechnologicalIntegration =>
                "Müdafiə sistemləri və texnoloji inteqrasiya",
            DepartmentType.HumanitarianSubjects => "Humanitar fənlər",
            DepartmentType.ForeignLanguages => "Xarici dillər",
            DepartmentType.IndustrialEngineeringAndSustainableEconomy =>
                "Sənaye mühəndisliyi və davamlı iqtisadiyyat",
            DepartmentType.BusinessManagement => "Biznes idarəetməsi",
            DepartmentType.DigitalEconomyAndFinancialTechnologies =>
                "Rəqəmsal iqtisadiyyat və maliyyə texnologiyaları",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
