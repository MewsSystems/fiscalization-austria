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
        public string Signature
        {
            get { return JwsRepresentation.Split('.')[2]; }
        }
    }
}
