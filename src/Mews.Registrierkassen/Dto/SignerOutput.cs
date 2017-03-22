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
                QrCodeRepresentationWithSignature = $"{receipt.QrDataWithoutSignature}_{response.Signature}";
                OcrCodeRepresentationWithSignature = $"{receipt.OcrDataWithoutSignature}_{response.Signature}";
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
            var finalBase64String = paddingLength > 0 ? sanitizedString + new string('=', 4 - paddingLength) : sanitizedString;

            var encodedDataAsBytes = Convert.FromBase64String(finalBase64String);
            return Encoding.UTF8.GetString(encodedDataAsBytes);
        }
    }
}
