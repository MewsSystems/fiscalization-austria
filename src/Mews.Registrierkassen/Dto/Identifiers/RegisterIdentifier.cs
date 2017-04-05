using System.Text.RegularExpressions;

namespace Mews.Registrierkassen.Dto.Identifiers
{
    public class RegisterIdentifier : StringIdentifier
    {
        public static readonly Regex Pattern = new Regex(".+");

        public RegisterIdentifier(string value)
            : base(value, Pattern)
        {
        }
    }
}
