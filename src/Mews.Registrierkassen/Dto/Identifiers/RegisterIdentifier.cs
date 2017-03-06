namespace Mews.Registrierkassen.Dto.Identifiers
{
    public class RegisterIdentifier : StringIdentifier
    {
        public RegisterIdentifier(string value)
            : base(value, Patterns.RegisterIdentifier)
        {
        }
    }
}
