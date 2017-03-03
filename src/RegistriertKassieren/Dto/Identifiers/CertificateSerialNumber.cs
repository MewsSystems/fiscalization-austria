namespace RegistriertKassieren.Dto.Identifiers
{
    public sealed class CertificateSerialNumber : StringIdentifier
    {
        public CertificateSerialNumber(string value)
            : base(value, Patterns.CertificateSerialNumber)
        {
        }
    }
}
