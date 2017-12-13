using System;
using System.Security.Cryptography.X509Certificates;
using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.Identifiers;
using Mews.Registrierkassen.Offline;
using Xunit;

namespace Mews.Registrierkassen.Tests
{
    public class OfflineTests
    {
        [Fact]
        public void OfflineSignatureWorks()
        {
            var certificate = new X509Certificate2("..\\..\\resource\\certificate\\mews-demo.pfx");
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
                    LocalDateTime.AustrianTimezone
                )
            )));
            Assert.NotNull(result);
        }
    }
}
    