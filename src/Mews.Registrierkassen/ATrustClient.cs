using System;
using System.IO;
using System.Net;
using System.Text;
using Mews.Registrierkassen.Dto;
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

        public ATrustClient(ATrustCredentials credentials)
        {
            Credentials = credentials;
            ApiUrl = $"https://hs-abnahme.a-trust.at/asignrkonline/v2/{Credentials.User.Value}";
        }

        public ATrustCredentials Credentials { get; }

        public string ApiUrl { get; }

        public SignerOutput Sign(ATrustSignerInput input)
        {
            var responseBody = PostJson(
                operation: "Sign/JWS",
                requestBody: JsonConvert.SerializeObject(input)
            );
            var signatureResponse = JsonConvert.DeserializeObject<SignatureResponse>(responseBody);
            return new SignerOutput(signatureResponse, input.Receipt);
        }

        public bool VerifyCredentials()
        {
            try
            {
                return Sign(new ATrustSignerInput(
                    password: Credentials.Password,
                    dataToBeSigned: "teststring"
                )).SignatureResponse.Signature != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public CertificateInfo GetCertificateInfo()
        {
            var responseBody = GetJson("Certificate");
            return JsonConvert.DeserializeObject<CertificateInfo>(responseBody);
        }

        private string PostJson(string operation, string requestBody)
        {
            var request = CreateJsonPostRequest($"{ApiUrl}/{operation}", requestBody);
            var response = request.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private string GetJson(string operation)
        {
            var request = CreateGetRequest($"{ApiUrl}/{operation}");
            var response = request.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private HttpWebRequest CreateRequest(string method, string url)
        {
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method;
            return webRequest;
        }

        private HttpWebRequest CreateGetRequest(string url)
        {
            return CreateRequest("GET", url);
        }

        private HttpWebRequest CreateJsonPostRequest(string url, string requestBody)
        {
            var data = Encoding.UTF8.GetBytes(requestBody);

            var webRequest = CreateRequest("POST", url);
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = data.Length;
            webRequest.GetRequestStream().Write(data, 0, data.Length);

            return webRequest;
        }
    }
}
