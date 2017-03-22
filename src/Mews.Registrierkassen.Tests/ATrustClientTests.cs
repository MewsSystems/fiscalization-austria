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
            var client = new ATrustClient(Credentials);
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
                    previousSignature: new Signature("d3YUbS4CoRo="), 
                    key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
                    created: new DateTimeWithTimeZone(
                        new DateTime(2015, 11, 25, 19, 20, 11),
                        DateTimeWithTimeZone.AustrianTimezone
                    )
                )
            ));
            Assert.NotNull(result);
        }

        [Fact]
        public void VerificationWorks()
        {
            Assert.True(new ATrustClient(Credentials).VerifyCredentials());
        }

        [Fact]
        public void GetCertificateInfoWorks()
        {
            var info = new ATrustClient(Credentials).GetCertificateInfo();
            Assert.NotNull(info);
        }
    }
}
    