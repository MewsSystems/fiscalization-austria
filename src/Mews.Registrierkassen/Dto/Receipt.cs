using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Albireo.Base32;
using Mews.Registrierkassen.Dto.Identifiers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Mews.Registrierkassen.Dto
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
            Created = created ?? DateTimeWithTimeZone.Now;
            Suite = "R1-AT100";
            Key = key;

            var chainValue = ComputeChainValue();
            var encryptedTurnoverBase64 = ComputeTurnoverBase64();
            var encryptedTurnoverBase32 = Base32.Encode(Convert.FromBase64String(encryptedTurnoverBase64));
            var previousSignatureBase32 = Base32.Encode(Convert.FromBase64String(chainValue));

            DataToBeSigned = $"_{Suite}_{RegisterIdentifier.Value}_{Number.Value}_{FormatDate(Created)}_{FormatDecimal(TaxData.StandardRate.Value)}_{FormatDecimal(TaxData.ReducedRate.Value)}_{FormatDecimal(TaxData.LowerReducedRate.Value)}_{FormatDecimal(TaxData.ZeroRate.Value)}_{FormatDecimal(TaxData.SpecialRate.Value)}_{encryptedTurnoverBase64}_{certificateSerialNumber}_{chainValue}";
            OcrDataWithoutSignature = $"_{Suite}_{RegisterIdentifier.Value}_{Number.Value}_{FormatDate(Created)}_{FormatDecimal(TaxData.StandardRate.Value)}_{FormatDecimal(TaxData.ReducedRate.Value)}_{FormatDecimal(TaxData.LowerReducedRate.Value)}_{FormatDecimal(TaxData.ZeroRate.Value)}_{FormatDecimal(TaxData.SpecialRate.Value)}_{encryptedTurnoverBase32}_{certificateSerialNumber}_{previousSignatureBase32}";
        }

        public ReceiptNumber Number { get; }

        public RegisterIdentifier RegisterIdentifier { get; }

        public DateTimeWithTimeZone Created { get; }

        public TaxData TaxData { get; }

        public CurrencyValue Turnover { get; }

        public CertificateSerialNumber CertificateSerialNumber { get; }

        public Signature PreviousSignature { get; }

        public string DataToBeSigned { get; }

        public string OcrDataWithoutSignature { get; }

        public string QrDataWithoutSignature
        {
            get { return DataToBeSigned; }
        }

        public string Suite { get; }

        private byte[] Key { get; }

        private string ComputeTurnoverBase64()
        {
            var sum = TaxData.Sum();
            var counter = (Turnover.Value + sum) * 100;
            var registryReceiptId = RegisterIdentifier.Value + Number.Value;
            var ivBytes = Encoding.UTF8.GetBytes(registryReceiptId);
            var registryReceiptIdHash = Sha256(ivBytes);
            var encryptedValue = AesCtr(registryReceiptIdHash, (long) counter, Key);

            return Convert.ToBase64String(encryptedValue);
        }

        private byte[] AesCtr(byte[] hash, long value, byte[] key)
        {
            var iv = GetEncryptInitializationVector(hash);
            var valueBytes = GetValueBytes(value);

            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            cipher.Init(forEncryption: true, parameters: new ParametersWithIV(new KeyParameter(key), iv));
            cipher.ProcessBytes(valueBytes);
            return cipher.DoFinal();
        }

        private byte[] GetEncryptInitializationVector(byte[] hash)
        {
            return hash.Take(16).ToArray();
        }

        private byte[] GetValueBytes(long value)
        {
            var originalBytes = BitConverter.GetBytes(value);
            var bytesSubset = originalBytes.Take(8);
            var orderedBytes = BitConverter.IsLittleEndian ? bytesSubset.Reverse() : bytesSubset;
            return orderedBytes.ToArray();
        }

        private string ComputeChainValue()
        {
            var input = PreviousSignature?.Base64Value ?? RegisterIdentifier.Value;
            var hash = Sha256(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash.Take(8).ToArray());
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

        private byte[] Sha256(byte[] toBeHashed)
        {
            return new SHA256Managed().ComputeHash(toBeHashed);
        }
    }
}
