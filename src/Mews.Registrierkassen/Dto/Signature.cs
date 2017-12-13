using Mews.Registrierkassen.Dto.Identifiers;
using Newtonsoft.Json;

namespace Mews.Registrierkassen.Dto
{
    public sealed class Signature
    {
        [JsonProperty("result")]
        public string JwsRepresentation { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonIgnore]
        public JwsSignature Value
        {
            get { return new JwsSignature(JwsRepresentation.Split('.')[2]); }
        }
    }
}
