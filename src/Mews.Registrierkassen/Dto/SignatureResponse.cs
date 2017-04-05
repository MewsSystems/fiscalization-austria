using Mews.Registrierkassen.Dto.Identifiers;
using Newtonsoft.Json;

namespace Mews.Registrierkassen.Dto
{
    public sealed class SignatureResponse
    {
        [JsonProperty("result")]
        public string JwsRepresentation { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonIgnore]
        public Base64UrlSafeString Signature
        {
            get { return new Base64UrlSafeString(JwsRepresentation.Split('.')[2]); }
        }
    }
}
