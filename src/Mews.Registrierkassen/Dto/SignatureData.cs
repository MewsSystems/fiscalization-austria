using Newtonsoft.Json;

namespace Mews.Registrierkassen.Dto
{
    public sealed class SignatureData
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }
    }
}
