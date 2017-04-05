using System.Text.RegularExpressions;

namespace Mews.Registrierkassen.Dto.Identifiers
{
    public class Base64UrlUnsafeString : StringIdentifier
    {
        public static readonly Regex Pattern = new Regex(".+");

        public Base64UrlUnsafeString(string value)
            : base(value, Pattern)
        {
        }
    }
}
