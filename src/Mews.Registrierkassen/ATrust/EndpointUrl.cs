using System.Text.RegularExpressions;
using Mews.FiscalChaining.Dto.Identifiers;

namespace Mews.Registrierkassen.ATrust
{
    public sealed class EndpointUrl : StringIdentifier
    {
        public static readonly Regex Pattern = new Regex("https?://.+[^/]");

        public EndpointUrl(string url)
            : base(url, Pattern)
        {
        }
    }
}
