using System;
using System.Security.Cryptography.X509Certificates;
using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.Identifiers;
using Xunit;

namespace Mews.Registrierkassen.Tests
{
    public class OfflineTests
    {
        [Fact]
        public void OfflineSignatureWorks()
        {
            var certificate = new X509Certificate2("..\\..\\..\\..\\resource\\certificate\\mews-demo.pfx");
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
                certificateSerialNumber: new CertificateSerialNumber("-3667961875706356849"),
                previousJwsRepresentation: new JwsRepresentation("d3YUbS4CoRo="),
                key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
                created: new LocalDateTime(
                    new DateTime(2015, 11, 25, 19, 20, 11),
                    LocalDateTime.AustrianTimezone
                )
            )));
            Assert.NotNull(result);
        }
    }
}
    