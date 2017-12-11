using System.Security.Cryptography.X509Certificates;
using System.Text;
using Jose;
using Mews.Registrierkassen.Dto;

namespace Mews.Registrierkassen
{
    public class OfflineSigner
    {
        public OfflineSigner(X509Certificate2 certificate)
        {
            Certificate = certificate;
        }

        public X509Certificate2 Certificate { get; set; }

        public SignerOutput Sign(QrData qrData)
        {
            var qrValueBase64 = Base64Url.Encode(Encoding.UTF8.GetBytes(qrData.Value));
            var jwsRepresentation = JWT.Encode(qrValueBase64, Certificate.GetECDsaPrivateKey(), JwsAlgorithm.ES256);
            return new SignerOutput(new SignatureResponse { JwsRepresentation = jwsRepresentation }, qrData);
        }
    }
}
