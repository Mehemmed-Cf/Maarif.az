namespace Application.Modules.SubjectsModule
{
    public class SubjectTopicRowDto
    {
        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public string? TeachingMethods { get; set; }
        public string? Materials { get; set; }
        public string? Equipment { get; set; }
    }

    public class SubjectMaterialRowDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class SubjectLiteratureRowDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string BookName { get; set; } = string.Empty;
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }
    }
}
