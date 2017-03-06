namespace Mews.Registrierkassen.Dto.Identifiers
{
    public sealed class ATrustUserIdentifier : StringIdentifier
    {
        public ATrustUserIdentifier(string value)
            : base(value, Patterns.UserIdentifier)
        {
        }
    }
}
