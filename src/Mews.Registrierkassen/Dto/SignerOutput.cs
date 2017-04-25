namespace Mews.Registrierkassen.Dto
{
    public sealed class SignerOutput
    {
        public SignerOutput(SignatureResponse response, QrData qrData = null)
        {
            SignatureResponse = response;
            if (qrData != null)
            {
                SignedQrData = new SignedQrData(qrData, response.Signature);
            }
        }

        public SignatureResponse SignatureResponse { get; }

        public SignedQrData SignedQrData { get; }
    }
}
