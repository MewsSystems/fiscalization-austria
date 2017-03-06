using Mews.Registrierkassen.Dto;
using Mews.Registrierkassen.Dto.ATrust;
using Mews.Registrierkassen.Dto.Identifiers;
using Xunit;

namespace Mews.Registrierkassen.Tests
{
    public class Fact
    {
        [Fact]
        public void SignerWorks()
        {
            var signer = new ATrustSigner();
            var result = signer.Sign(new ATrustSignerInput(
                new ATrustCredentials(
                    user: new ATrustUserIdentifier("u123456789"),
                    password: "123456789"
                ),
                receipt: new Receipt(
                    number: new ReceiptNumber("1"),
                    registerIdentifier: new RegisterIdentifier("1"),
                    taxData: new TaxData(),
                    turnover: new CurrencyValue(0.0m), 
                    certificateSerialNumber: new CertificateSerialNumber("1"),
                    previousSignature: null,
                    key: AesKeyGenerator.GetNext()
                )
            ));
            Assert.NotNull(result);
        }
    }
}
    