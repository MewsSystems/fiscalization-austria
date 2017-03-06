namespace Mews.Registrierkassen.Dto.Identifiers
{
    public sealed class CertificateSerialNumber : StringIdentifier
    {
        public CertificateSerialNumber(string value)
            : base(value, Patterns.CertificateSerialNumber)
        {
        }
    }
}
