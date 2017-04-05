using System;
using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.Identifiers;
using Mews.Registrierkassen.Signers.Dto;
using Xunit;

namespace Mews.Registrierkassen.Tests
{
    public class Fact
    {
        private static ATrustCredentials Credentials
        {
            get { return new ATrustCredentials(user: new ATrustUserIdentifier("u123456789"), password: "123456789"); }
        }

        [Fact]
        public void ATrustSignerWorks()
        {
            var client = new ATrustClient(Credentials, ATrustEnvironment.Test);
            var result = client.Sign(new ATrustSignerInput(
                Credentials.Password,
                receipt: new Receipt(
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
                )
            ));
            Assert.NotNull(result);
        }

        [Fact]
        public void GetCertificateInfoWorks()
        {
            var info = new ATrustClient(Credentials, ATrustEnvironment.Test).GetCertificateInfo();
            Assert.NotNull(info);
        }
    }
}
    