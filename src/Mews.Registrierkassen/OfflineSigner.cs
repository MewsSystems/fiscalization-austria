using System.Security.Cryptography.X509Certificates;
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
            var jwsRepresentation = JWT.Encode(
                qrData.Value,
                Certificate.GetECDsaPrivateKey(),
                JwsAlgorithm.ES256
            );
            return new SignerOutput(new SignatureResponse { JwsRepresentation = jwsRepresentation }, qrData);
        }
    }
}
