using System.Text.RegularExpressions;
using Mews.FiscalChaining.Dto.Identifiers;

namespace Mews.Registrierkassen.ATrust
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
