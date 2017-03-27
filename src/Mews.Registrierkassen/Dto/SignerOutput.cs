using System;
using System.Text;

namespace Mews.Registrierkassen.Dto
{
    public sealed class SignerOutput
    {
        public SignerOutput(SignatureResponse response, Receipt receipt = null)
        {
            SignatureResponse = response;
            if (receipt != null)
            {
                var base64SignatureUrlUnsafe = Base64UrlSafeDecode(response.Signature);
                QrCodeRepresentationWithSignature = $"{receipt.QrDataWithoutSignature}_{base64SignatureUrlUnsafe}";
                OcrCodeRepresentationWithSignature = $"{receipt.OcrDataWithoutSignature}_{base64SignatureUrlUnsafe}";
            }
        }

        public SignatureResponse SignatureResponse { get; }

        public string QrCodeRepresentationWithSignature { get; }

        public string OcrCodeRepresentationWithSignature { get; }

        private string Base64UrlSafeDecode(string base64UrlSafeString)
        {
            if (string.IsNullOrEmpty(base64UrlSafeString))
            {
                throw new ArgumentException("The decoded string is empty or null.");
            }

            var sanitizedString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            var paddingLength = sanitizedString.Length % 4;
            return paddingLength > 0 ? sanitizedString + new string('=', 4 - paddingLength) : sanitizedString;
        }
    }
}
