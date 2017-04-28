using System.Text.RegularExpressions;

namespace Mews.Registrierkassen.Dto.Identifiers
{
    public sealed class ATrustUserIdentifier : StringIdentifier
    {
        public static readonly Regex Pattern = new Regex("u.+");

        public ATrustUserIdentifier(string value)
            : base(value, Pattern)
        {
        }
    }
}
