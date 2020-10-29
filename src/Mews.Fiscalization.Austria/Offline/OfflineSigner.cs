using System;
using System.Data;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Mews.Fiscalization.Austria.Dto;
using Mews.Fiscalization.Austria.Dto.Identifiers;
using Microsoft.IdentityModel.Tokens;

namespace Mews.Fiscalization.Austria.Offline
{
    public class OfflineSigner : ISigner
    {
        public OfflineSigner(CertificateInfo certificateInfo)
        {
            Certificate = ParseCertificate(certificateInfo);
            CertificateInfo = certificateInfo;
        }

        private X509Certificate2 Certificate { get; }

        private CertificateInfo CertificateInfo { get; }

        public SignerOutput Sign(QrData qrData)
        {
            //// This is a manual JWS implementation as RKSV does not use standard signature format. Do not migrate to jose-jwt
            var jwsHeaderBase64Url = Base64UrlEncoder.Encode("{\"alg\":\"ES256\"}"); // Fixed value for RKSV
            var jwsPayloadBase64Url = Base64UrlEncoder.Encode(qrData.Value);
            var jwsDataToBeSigned = jwsHeaderBase64Url + "." + jwsPayloadBase64Url;

            var bytes = Certificate.GetECDsaPrivateKey().SignData(Encoding.UTF8.GetBytes(jwsDataToBeSigned), HashAlgorithmName.SHA256);
            var jwsSignatureBase64Url = Base64UrlEncoder.Encode(bytes);

            var jwsRepresentation = $"{jwsHeaderBase64Url}.{jwsPayloadBase64Url}.{jwsSignatureBase64Url}";
            return new SignerOutput(new JwsRepresentation(jwsRepresentation), qrData);
        }

        public CertificateInfo GetCertificateInfo()
        {
            return CertificateInfo;
        }

        private X509Certificate2 ParseCertificate(CertificateInfo certificateInfo)
        {
            return new X509Certificate2(Convert.FromBase64String(certificateInfo.Certificate));
        }
    }
}