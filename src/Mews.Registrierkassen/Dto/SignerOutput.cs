using System;
using System.Text;
using Mews.Registrierkassen.Dto.Identifiers;

namespace Mews.Registrierkassen.Dto
{
    public sealed class SignerOutput
    {
        public SignerOutput(SignatureResponse response, Receipt receipt = null)
        {
            SignatureResponse = response;
            if (receipt != null)
            {
                var base64SignatureUrlUnsafe = response.Signature.Base64UrlUnsafeString;
                QrCodeRepresentationWithSignature = $"{receipt.QrDataWithoutSignature}_{base64SignatureUrlUnsafe.Value}";
                OcrCodeRepresentationWithSignature = $"{receipt.OcrDataWithoutSignature}_{base64SignatureUrlUnsafe.Value}";
            }
        }

        public SignatureResponse SignatureResponse { get; }

        public string QrCodeRepresentationWithSignature { get; }

        public string OcrCodeRepresentationWithSignature { get; }
    }
}
