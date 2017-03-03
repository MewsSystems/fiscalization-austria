using System;
using System.Text;
using Newtonsoft.Json;

namespace RegistriertKassieren.Dto
{
    public sealed class SignerInput
    {
        public SignerInput(string password, Receipt receipt)
        {
            Password = password;
            Receipt = receipt;

            ToBeSigned = Convert.ToBase64String(Encoding.UTF8.GetBytes(receipt.SignatureData));
        }

        [JsonProperty("password")]
        public string Password { get; }

        [JsonProperty("to_be_signed")]
        public string ToBeSigned { get; }

        private Receipt Receipt { get; }
    }
}
