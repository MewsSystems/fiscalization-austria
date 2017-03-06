using System.Text.RegularExpressions;

namespace Mews.Registrierkassen
{
    public class Patterns
    {
        public static readonly Regex ReceiptNumber = new Regex(".+");

        public static readonly Regex RegisterIdentifier = new Regex(".+");

        public static readonly Regex CertificateSerialNumber = new Regex(".+");

        public static readonly Regex UserIdentifier = new Regex(".+");
    }
}
