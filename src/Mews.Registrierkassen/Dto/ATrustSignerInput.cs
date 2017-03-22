using System;
using System.Text;
using Mews.Registrierkassen.Dto;
using Newtonsoft.Json;

namespace Mews.Registrierkassen.Signers.Dto
{
    public sealed class ATrustSignerInput
    {

        public ATrustSignerInput(string password, string dataToBeSigned)
        {
            Password = password;
            DataToBeSigned = dataToBeSigned;
        }

        public ATrustSignerInput(string password, Receipt receipt)
            : this(password, receipt.DataToBeSigned)
        {
            Receipt = receipt;
        }

        [JsonProperty("password")]
        public string Password { get; }

        [JsonProperty("jws_payload")]
        public string DataToBeSigned { get; }

        [JsonIgnore]
        public Receipt Receipt { get; }

    }
}
