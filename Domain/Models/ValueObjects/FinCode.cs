namespace Domain.Models.ValueObjects
{
    public class FinCode
    {
        public string Value { get; }

        // Azerbaijan FIN codes are exactly 7 characters, letters and digits
        private FinCode(string value) => Value = value;

        public static FinCode Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("FIN code cannot be empty.");

            value = value.Trim().ToUpper();

            if (value.Length != 7)
                throw new Exception("FIN code must be exactly 7 characters.");

            if (!value.All(c => char.IsLetterOrDigit(c)))
                throw new Exception("FIN code can only contain letters and digits.");

            return new FinCode(value);
        }

        public override string ToString() => Value;
    }
}