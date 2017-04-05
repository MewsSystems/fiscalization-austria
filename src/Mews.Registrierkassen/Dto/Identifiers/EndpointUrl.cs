using System.Text.RegularExpressions;

namespace Mews.Registrierkassen.Dto.Identifiers
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
