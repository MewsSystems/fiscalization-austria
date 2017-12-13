namespace Mews.Registrierkassen.Dto
{
    public sealed class SignerOutput
    {
        public SignerOutput(Signature signature, QrData qrData = null)
        {
            Signature = signature;
            if (qrData != null)
            {
                SignedQrData = new SignedQrData(qrData, signature.Value);
            }
        }

        public Signature Signature { get; }

        public SignedQrData SignedQrData { get; }
    }
}
