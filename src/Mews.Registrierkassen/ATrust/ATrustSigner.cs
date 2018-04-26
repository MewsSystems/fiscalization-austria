using System.Net;
using Mews.FiscalChaining.Dto;
using Mews.FiscalChaining.Dto.Identifiers;
using Newtonsoft.Json;
using Mews.FiscalChaining;

namespace Mews.Registrierkassen.ATrust
{
    public sealed class ATrustSigner : ISigner
    {
        static ATrustSigner()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public ATrustSigner(ATrustCredentials credentials, ATrustEnvironment environment = ATrustEnvironment.Production)
        {
            Credentials = credentials;
            var environmentDomain = environment == ATrustEnvironment.Production ? "www" : "hs-abnahme";
            var endpointUrl = new EndpointUrl($"https://{environmentDomain}.a-trust.at/asignrkonline/v2/{Credentials.User.Value}");
            HttpClient = new SimpleHttpClient(endpointUrl);
        }

        public ATrustCredentials Credentials { get; }

        private SimpleHttpClient HttpClient { get; }

        public SignerOutput Sign(QrData qrData)
        {
            var input = new ATrustSignerInput(Credentials.Password, qrData);
            var responseBody = HttpClient.PostJson(
                operation: "Sign/JWS",
                requestBody: JsonConvert.SerializeObject(input)
            );
            var response = JsonConvert.DeserializeObject<ATrustSignerResponse>(responseBody);
            return new SignerOutput(new JwsRepresentation(response.JwsRepresentation), input.QrData);
        }

        public CertificateInfo GetCertificateInfo()
        {
            var responseBody = HttpClient.GetJson("Certificate");
            return JsonConvert.DeserializeObject<CertificateInfo>(responseBody);
        }
    }
}
