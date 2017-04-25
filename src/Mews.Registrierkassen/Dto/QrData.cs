namespace Mews.Registrierkassen.Dto
{
    public sealed class QrData : MachineReadableData
    {
        public QrData(Receipt receipt)
            : base(receipt, BaseEncoding.Base64)
        {
        }
    }
}

