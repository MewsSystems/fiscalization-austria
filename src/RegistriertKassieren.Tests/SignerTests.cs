using System;
using Org.BouncyCastle.Security;
using RegistriertKassieren.Dto;
using RegistriertKassieren.Dto.Identifiers;
using Xunit;

namespace RegistriertKassieren.Tests
{
    public class Fact
    {
        [Fact]
        public void SignerWorks()
        {
            var signer = new Signer();
            var result = signer.Sign(new SignerInput(
                password: "123456789",
                receipt: new Receipt(
                    number: new ReceiptNumber("1"),
                    registerIdentifier: new RegisterIdentifier("1"),
                    taxData: new TaxData(),
                    turnover: new CurrencyValue(0.0m), 
                    certificateSerialNumber: new CertificateSerialNumber("1"),
                    previousSignature: null,
                    key: GenerateKey()
                )
            ));
            Console.WriteLine(result.ToString());
        }

        private byte[] GenerateKey()
        {
            var random = new SecureRandom();
            var bytes = new byte[16];
            random.NextBytes(bytes);
            return bytes;
        }
    }
}
    