using Newtonsoft.Json;

namespace RegistriertKassieren.Dto
{
    public sealed class SignerInput
    {
        public SignerInput(string password)
        {
            Password = password;
            ToBeSigned = GetDataToBeSigned();
        }

        [JsonProperty("password")]
        public string Password { get; }

        [JsonProperty("to_be_signed")]
        public string ToBeSigned { get; }

        private string GetDataToBeSigned()
        {
            return "c2FtcGxlIHRleHQgZm9yIHNpZ25pbmcgd2l0aCByZWdpc3RyaWVya2Fzc2UubW9iaWxlIG9ubGluZSBzZXJ2aWNl";
        }
    }
}
