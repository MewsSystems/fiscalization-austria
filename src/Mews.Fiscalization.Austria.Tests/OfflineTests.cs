using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Mews.Fiscalization.Austria.Dto;
using Mews.Fiscalization.Austria.Dto.Identifiers;
using Mews.Fiscalization.Austria.Offline;
using NUnit.Framework;
using TimeZoneConverter;

namespace Mews.Fiscalization.Austria.Tests
{
    public class OfflineTests
    {
        public static readonly string CertificateData = "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";
        public static readonly string CertificatePassword = "eet";

        [Test]
        public void OfflineSignatureWorks()
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(CertificateData), CertificatePassword, X509KeyStorageFlags.DefaultKeySet);
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var europeTimeZone = "Central Europe Standard Time";
            var austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById(isWindows ? europeTimeZone : TZConvert.WindowsToIana(europeTimeZone));
            var austrianCulture = CultureInfo.GetCultureInfo("de-AT");

            var signer = new OfflineSigner(certificate);
            var result = signer.Sign(new QrData(new Receipt(
                number: new ReceiptNumber("83469"),
                registerIdentifier: new RegisterIdentifier("DEMO-CASH-BOX817"),
                taxData: new TaxData(
                    standartRate: new CurrencyValue(29.73m),
                    lowerReducedRate: new CurrencyValue(36.41m),
                    specialRate: new CurrencyValue(21.19m)
                ),
                turnover: new CurrencyValue(0.0m),
                certificateSerialNumber: new CertificateSerialNumber(certificate.SerialNumber),
                previousJwsRepresentation: new JwsRepresentation("eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.WDFJeExVRlVNVjloT0RRME1URXpZaTFoTTJRM0xUUmxObU10T0RGak9DMDJOalU0TXpnMk9HVm1NelpmTTE4eU1ERTNMVEV5TFRFeVZERXlPalV6T2pVMlh6QXNNREJmTVRBd0xEQXdYekFzTURCZk1Dd3dNRjh3TERBd1h6ZzNMMnR2YW05RVYwUjNQVjh3TUVJd05qQkJNRUkwTWpFMlJUQXhSRFJmZVROVVp6TXlOV1Z0Y0UwOQ.6mzl1HSWmJyWaUG0pZlNuF29Eg9jocyXSuBxYWnwskE3fpVLd2PTIHG9ecBvQnCW3SokaMiEEgYN969Z4P7i0w"),
                key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
                created: new LocalDateTime(
                    new DateTime(year: 2015, month: 11, day: 25, hour: 19, minute: 20, second: 11),
                    austrianTimeZone
                )
            ), austrianCulture, austrianTimeZone));
            Assert.IsNotNull(result);
        }
    }
}