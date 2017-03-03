using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Albireo.Base32;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using RegistriertKassieren.Dto.Identifiers;

namespace RegistriertKassieren.Dto
{
    public sealed class Receipt
    {
        public Receipt(
            ReceiptNumber number,
            RegisterIdentifier registerIdentifier,
            TaxData taxData,
            CurrencyValue turnover,
            CertificateSerialNumber certificateSerialNumber,
            Signature previousSignature,
            byte[] key,
            DateTimeWithTimeZone created = null
        )
        {
            Number = number ?? throw new ArgumentException("The receipt number has to be specified.");
            RegisterIdentifier = registerIdentifier ?? throw new ArgumentException("The register identifier has to be specified.");
            TaxData = taxData ?? throw new ArgumentException("The tax data have to be specified.");
            Turnover = turnover ?? throw new ArgumentException("The turnover has to be specified.");
            CertificateSerialNumber = certificateSerialNumber ?? throw new ArgumentException("The certificate serial number has to be specified.");
            PreviousSignature = previousSignature;
            CertificateSerialNumber = certificateSerialNumber;
            Created = created ?? DateTimeWithTimeZone.Now;
            Suite = "R1-AT0";
            Key = key;

            var chainValue = ComputeChainValue();
            var encryptedTurnoverBase64 = ComputeTurnoverBase64();
            var encryptedTurnoverBase32 = Base32.Encode(Convert.FromBase64String(encryptedTurnoverBase64));
            var previousSignatureBase32 = Base32.Encode(Convert.FromBase64String(chainValue));

            SignatureData = $"_{Suite}_{RegisterIdentifier.Value}_{Number.Value}_{FormatDate(Created)}_{FormatDecimal(TaxData.StandardRate.Value)}_{FormatDecimal(TaxData.ReducedRate.Value)}_{FormatDecimal(TaxData.LowerReducedRate.Value)}_{FormatDecimal(TaxData.ZeroRate.Value)}_{FormatDecimal(TaxData.SpecialRate.Value)}_{encryptedTurnoverBase64}_{CertificateSerialNumber}_{chainValue}";
            OcrData = $"_{Suite}_{RegisterIdentifier.Value}_{Number.Value}_{FormatDate(Created)}_{FormatDecimal(TaxData.StandardRate.Value)}_{FormatDecimal(TaxData.ReducedRate.Value)}_{FormatDecimal(TaxData.LowerReducedRate.Value)}_{FormatDecimal(TaxData.ZeroRate.Value)}_{FormatDecimal(TaxData.SpecialRate.Value)}_{encryptedTurnoverBase32}_{CertificateSerialNumber}_{previousSignatureBase32}";
        }

        public ReceiptNumber Number { get; }

        public RegisterIdentifier RegisterIdentifier { get; }

        public DateTimeWithTimeZone Created { get; }

        public TaxData TaxData { get; }

        public CurrencyValue Turnover { get; }

        public CertificateSerialNumber CertificateSerialNumber { get; }

        public Signature PreviousSignature { get; }

        public string SignatureData { get; }

        public string OcrData { get; }

        public string Suite { get; }

        private byte[] Key { get; }

        private string ComputeTurnoverBase64()
        {
            var sum = TaxData.Sum();
            var counter = (Turnover.Value + sum) * 100;
            var registryReceiptId = RegisterIdentifier.Value + Number.Value;
            var registryReceiptIdHash = Sha256(Encoding.UTF8.GetBytes(registryReceiptId));
            var encryptedValue = SymmetricEncrypt(registryReceiptIdHash, (long) counter, Key);

            return Convert.ToBase64String(encryptedValue);
        }

        private byte[] SymmetricEncrypt(string hash, long value, byte[] key)
        {
            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            var iv = Encoding.UTF8.GetBytes(hash).Take(16).ToArray();
            cipher.Init(forEncryption: true, parameters: new ParametersWithIV(new KeyParameter(key), iv));
            cipher.ProcessBytes(BitConverter.GetBytes(value));
            return cipher.DoFinal();
        }

        private string ComputeChainValue()
        {
            var input = PreviousSignature?.Base64Value ?? RegisterIdentifier.Value;
            var hash = Sha256(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
        }

        private string FormatDate(DateTimeWithTimeZone dateTimeWithTimeZone)
        {
            var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithTimeZone.DateTime, dateTimeWithTimeZone.TimeZoneInfo);
            var dateTimeAustria = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, DateTimeWithTimeZone.AustrianTimezone);
            var date = DateTime.SpecifyKind(new DateTime(dateTimeAustria.Ticks - dateTimeAustria.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Local);
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        private string FormatDecimal(decimal amount)
        {
            return String.Format(System.Globalization.CultureInfo.GetCultureInfo("de-AT"), "{0:F2}", amount);
        }

        private string Sha256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256.ComputeHash(toBeHashed));
            }
        }
    }
}
