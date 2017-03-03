using System.Text.RegularExpressions;

namespace RegistriertKassieren
{
    public class Patterns
    {
        public static readonly Regex ReceiptNumber = new Regex(".+");

        public static readonly Regex RegisterIdentifier = new Regex(".+");

        public static readonly Regex CertificateSerialNumber = new Regex(".+");
    }
}
