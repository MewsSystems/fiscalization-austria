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
        public static readonly ATrustUserIdentifier UserId = new ATrustUserIdentifier(Environment.GetEnvironmentVariable("user_id") ?? "INSERT_USER_ID");
        public static readonly string Password = Environment.GetEnvironmentVariable("password") ?? "INSERT_PASSWORD";
        public static readonly CertificateSerialNumber CertificateSerialNumber = new CertificateSerialNumber(Environment.GetEnvironmentVariable("certificate_serial_number") ?? "INSERT_CERTIFICATE_SERIAL_NUMBER");
        public static readonly JwsRepresentation JwsRepresentation = new JwsRepresentation(Environment.GetEnvironmentVariable("jws_representation") ?? "INSERT_JWS");
        public static readonly string CertificateKey = Environment.GetEnvironmentVariable("certificate_key") ?? "INSERT_CERTIFICATE_KEY";

        private static ATrustCredentials Credentials
        {
            get { return new ATrustCredentials(user: UserId, password: Password); }
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
                    certificateSerialNumber: CertificateSerialNumber,
                    previousJwsRepresentation: JwsRepresentation, 
                    key: Convert.FromBase64String(CertificateKey),
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
    