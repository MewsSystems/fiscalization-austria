using System.IO;
using System.Net;
using Newtonsoft.Json;
using RegistriertKassieren.Dto;

namespace RegistriertKassieren
{
    public sealed class Signer
    {
        public SignatureData Sign(SignerInput input)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var url = "https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v1/u123456789/Sign";
            var request = JsonConvert.SerializeObject(input);

            var data = System.Text.Encoding.UTF8.GetBytes(request);


            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            webRequest.ContentLength = data.Length;
            webRequest.GetRequestStream().Write(data, 0, data.Length);
            var webResponse = webRequest.GetResponse() as HttpWebResponse;

            var reader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            var responseText = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<SignatureData>(responseText);
        }
    }
}
