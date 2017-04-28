using System.Net;
using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.Identifiers;
using Mews.Registrierkassen.Signers.Dto;
using Newtonsoft.Json;

namespace Mews.Registrierkassen
{
    public sealed class ATrustClient
    {
        static ATrustClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public ATrustClient(ATrustCredentials credentials, ATrustEnvironment environment = ATrustEnvironment.Production)
        {
            Credentials = credentials;
            var environmentDomain = environment == ATrustEnvironment.Production ? "www" : "hs-abnahme";
            var endpointUrl = new EndpointUrl($"https://{environmentDomain}.a-trust.at/asignrkonline/v2/{Credentials.User.Value}");
            HttpClient = new SimpleHttpClient(endpointUrl);
        }

        public ATrustCredentials Credentials { get; }

        private SimpleHttpClient HttpClient { get; }

        public SignerOutput Sign(ATrustSignerInput input)
        {
            var responseBody = HttpClient.PostJson(
                operation: "Sign/JWS",
                requestBody: JsonConvert.SerializeObject(input)
            );
            var signatureResponse = JsonConvert.DeserializeObject<SignatureResponse>(responseBody);
            return new SignerOutput(signatureResponse, input.QrData);
        }

        public CertificateInfo GetCertificateInfo()
        {
            var responseBody = HttpClient.GetJson("Certificate");
            return JsonConvert.DeserializeObject<CertificateInfo>(responseBody);
        }
    }
}
