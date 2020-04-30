using System;
using System.Globalization;
using Mews.Fiscalization.Austria.ATrust;
using Mews.Fiscalization.Austria.Dto;
using Mews.Fiscalization.Austria.Dto.Identifiers;
using NUnit.Framework;

namespace Mews.Fiscalization.Austria.Tests
{
    public class ATrustSignerTests
    {
        private static ATrustCredentials Credentials
        {
            get { return new ATrustCredentials(user: new ATrustUserIdentifier("u123456789"), password: "123456789"); }
        }

        [Test]
        public void ATrustSignerWorks()
        {
            var austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            var austrianCulture = CultureInfo.GetCultureInfo("de-AT");
            var signer = new ATrustSigner(Credentials, ATrustEnvironment.Test);
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
                    previousJwsRepresentation: new JwsRepresentation("eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.WDFJeExVRlVNVjloT0RRME1URXpZaTFoTTJRM0xUUmxObU10T0RGak9DMDJOalU0TXpnMk9HVm1NelpmTTE4eU1ERTNMVEV5TFRFeVZERXlPalV6T2pVMlh6QXNNREJmTVRBd0xEQXdYekFzTURCZk1Dd3dNRjh3TERBd1h6ZzNMMnR2YW05RVYwUjNQVjh3TUVJd05qQkJNRUkwTWpFMlJUQXhSRFJmZVROVVp6TXlOV1Z0Y0UwOQ.6mzl1HSWmJyWaUG0pZlNuF29Eg9jocyXSuBxYWnwskE3fpVLd2PTIHG9ecBvQnCW3SokaMiEEgYN969Z4P7i0w"), 
                    key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
                    created: new LocalDateTime(
                        new DateTime(2015, 11, 25, 19, 20, 11),
                        austrianTimeZone
                    )
                ), austrianCulture, austrianTimeZone
            ));
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetCertificateInfoWorks()
        {
            var info = new ATrustSigner(Credentials, ATrustEnvironment.Test).GetCertificateInfo();
            Assert.IsNotNull(info);
        }
    }
}
    