using System;
using System.IO;
using System.Net;
using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.ATrust;
using Newtonsoft.Json;

namespace Mews.Registrierkassen
{
    public sealed class ATrustSigner
    {
        static ATrustSigner()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public SignatureData Sign(ATrustSignerInput input)
        {
            var responseBody = PostJson(
                url: $"https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v1/{input.Credentials.User.Value}/Sign",
                requestBody: JsonConvert.SerializeObject(input)
            );
            return JsonConvert.DeserializeObject<SignatureData>(responseBody);
        }

        public bool VerifyCredentials(ATrustCredentials credentials)
        {
            try
            {
                return Sign(new ATrustSignerInput(credentials, "test string")).Signature != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string PostJson(string url, string requestBody)
        {
            var request = CreateJsonRequest(url, requestBody);
            var response = request.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private HttpWebRequest CreateJsonRequest(string url, string requestBody)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(requestBody);

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = data.Length;
            webRequest.GetRequestStream().Write(data, 0, data.Length);

            return webRequest;
        }
    }
}
