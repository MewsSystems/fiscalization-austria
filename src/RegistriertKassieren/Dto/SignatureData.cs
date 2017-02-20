using Newtonsoft.Json;

namespace RegistriertKassieren.Dto
{
    public sealed class SignatureData
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }
    }
}
