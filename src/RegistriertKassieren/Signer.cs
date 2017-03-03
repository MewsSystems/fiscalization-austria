using System.IO;
using System.Net;
using Newtonsoft.Json;
using RegistriertKassieren.Dto;

namespace RegistriertKassieren
{
    public sealed class Signer
    {
        static Signer()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public SignatureData Sign(SignerInput input)
        {
            var responseBody = PostJson(
                url: "https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v1/u123456789/Sign",
                requestBody: JsonConvert.SerializeObject(input)
            );
            return JsonConvert.DeserializeObject<SignatureData>(responseBody);
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
