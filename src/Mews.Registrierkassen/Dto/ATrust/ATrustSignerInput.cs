using System;
using System.Text;
using Newtonsoft.Json;

namespace Mews.Registrierkassen.Dto.ATrust
{
    public sealed class ATrustSignerInput
    {

        public ATrustSignerInput(ATrustCredentials credentials, string dataToBeSigned)
        {
            Credentials = credentials;
            DataToBeSigned = dataToBeSigned;
        }

        public ATrustSignerInput(ATrustCredentials credentials, Receipt receipt)
            : this(credentials, Convert.ToBase64String(Encoding.UTF8.GetBytes(receipt.SignatureData)))
        {
            Receipt = receipt;
        }

        [JsonProperty("password")]
        public string Password
        {
            get { return Credentials.Password; }
        }

        [JsonProperty("to_be_signed")]
        public string DataToBeSigned { get; }

        public ATrustCredentials Credentials { get; }

        public Receipt Receipt { get; }

    }
}
