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
        public JwsSignature Signature
        {
            get { return new JwsSignature(JwsRepresentation.Split('.')[2]); }
        }
    }
}
