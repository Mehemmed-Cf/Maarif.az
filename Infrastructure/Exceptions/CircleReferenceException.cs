namespace Infrastructure.Exceptions
{
    public class CircleReferenceException : Exception
    {
        public string Property { get; set; }
        public CircleReferenceException(string propertyName)
            : base($"Circle reference occured by {propertyName}")
        {
            this.Property = propertyName;
        }
    }
}
