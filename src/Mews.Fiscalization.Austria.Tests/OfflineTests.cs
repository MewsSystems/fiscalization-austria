using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
        public static readonly string CertificateData = "MIIDegIBAzCCA0AGCSqGSIb3DQEHAaCCAzEEggMtMIIDKTCCAh8GCSqGSIb3DQEHBqCCAhAwggIMAgEAMIICBQYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQIysKJTUa/j7ECAggAgIIB2OXrliSOs0s125ku/HqYjm97whSPjRvbLEG6bJuVm2dlJeVUpE+ifQ3lWzwcx/xC075vWQCtiTQs8Be8vFJ3tTZIoe/cjKJCCgQ8tVp6ypprUB35LEWej73TLd0vR3yRHZnqOuFOY0MR989kUanltBJ1F8R9z5/NcHENQe1dXCd/P07wpzKEDgrQaF3Ty/arOGgGh9t76g8RHdVIuz0p3nfO3jQcbdxShKoGRuJEvChUkiwFitT4FDRKSQsCJCFeQsSkxJm5bHfxISEMhBJ34JvcXKFcmrDtXOOoCbABxzIN9xlXhRZdoYRupEl8GRdVO2shl5EmFHwGFKnRqi7We+lcRfwqc6Sc+LNVAZk2Im3L99s8CQ0wIyyBEZKNI21dfg24UBX2r/e8TwODIbJrkRo26NRl+8NgfBn9GxRaPAPWZVnfM3MA7frr9rA6WFFmrgPXZN3kjBcdUrUlYaBdoW0wVb7YnMi1fxpg4e59jLWJ+ECf6Yq0+ap1B0mUPb3Yns73p3H5AzcyS/Fqg5GFNjYR79o/JX3EwTqlBcv9NRod4Y+uz9VUGtffYFL6kHgGLIcq3yIxLmgq5kq2ZHnwuLwXhoMK273SIWzpoEn63LW8R1aH2CrJJrUwggECBgkqhkiG9w0BBwGggfQEgfEwge4wgesGCyqGSIb3DQEMCgECoIG0MIGxMBwGCiqGSIb3DQEMAQMwDgQIfcUOa9/ENxgCAggABIGQ7wRqhPFOQwvVPmWzVfvpe4dH8X2du6koJsE8Aae246pfZjB2lCOfSU45IE+8YjLSA1qdl4GWCUiralfWkewB6H80xCV+fFVvAyMurg/pb2lhB5Rn+/XGLPQMFOemlx3W1agYh0Og7qIH7bcFGx3ULyeb4F0MpX27zErxFBa8oC8PhZLtu1h9Vr7WQmIEjod0MSUwIwYJKoZIhvcNAQkVMRYEFCchXQb0E101J24QyRXeMdxpV9MYMDEwITAJBgUrDgMCGgUABBRqJGaEHLMJXcqHQy74YWk5sGlnGwQICXKiEWvjLeICAggA";
        public static readonly string CertificatePassword = "eet";

        [Test]
        public void OfflineSignatureWorks()
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(CertificateData), "test");
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