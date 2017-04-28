namespace Mews.Registrierkassen.Dto
{
    public sealed class EncryptedTurnover : ByteValue
    {
        public EncryptedTurnover(byte[] value)
            : base(value)
        {
        }
    }
}
