using System.Security.Cryptography;
using System.Text;
using Mews.Fiscalization.Austria.Dto;
using Mews.Fiscalization.Austria.Dto.Identifiers;
using Microsoft.IdentityModel.Tokens;

namespace Mews.Fiscalization.Austria.Offline
{
    public class OfflineSigner : ISigner
    {
        public OfflineSigner(Certificate certificate)
        {
            Certificate = certificate;
        }

        public Certificate Certificate { get; }

        public SignerOutput Sign(QrData qrData)
        {
            //// This is a manual JWS implementation as RKSV does not use standard signature format. Do not migrate to jose-jwt
            var jwsHeaderBase64Url = Base64UrlEncoder.Encode("{\"alg\":\"ES256\"}"); // Fixed value for RKSV
            var jwsPayloadBase64Url = Base64UrlEncoder.Encode(qrData.Value);
            var jwsDataToBeSigned = jwsHeaderBase64Url + "." + jwsPayloadBase64Url;

            var bytes = Certificate.PrivateKey.SignData(Encoding.UTF8.GetBytes(jwsDataToBeSigned), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var jwsSignatureBase64Url = Base64UrlEncoder.Encode(bytes);

            var jwsRepresentation = $"{jwsHeaderBase64Url}.{jwsPayloadBase64Url}.{jwsSignatureBase64Url}";
            return new SignerOutput(new JwsRepresentation(jwsRepresentation), qrData);
        }
    }
}