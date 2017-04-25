using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            JwsRepresentation previousJwsRepresentation,
            byte[] key,
            LocalDateTime created = null)
        {
            Number = number ?? throw new ArgumentException("The receipt number has to be specified.");
            RegisterIdentifier = registerIdentifier ?? throw new ArgumentException("The register identifier has to be specified.");
            TaxData = taxData ?? throw new ArgumentException("The tax data have to be specified.");
            Turnover = turnover ?? throw new ArgumentException("The turnover has to be specified.");
            CertificateSerialNumber = certificateSerialNumber ?? throw new ArgumentException("The certificate serial number has to be specified.");
            PreviousJwsRepresentation = previousJwsRepresentation;
            Created = created ?? LocalDateTime.Now;
            Suite = "R1-AT1";
            Key = key;
            ChainValue = ComputeChainValue();
            EncryptedTurnover = EncryptTurnover();
        }

        public ReceiptNumber Number { get; }

        public RegisterIdentifier RegisterIdentifier { get; }

        public LocalDateTime Created { get; }

        public TaxData TaxData { get; }

        public CurrencyValue Turnover { get; }

        public CertificateSerialNumber CertificateSerialNumber { get; }

        public JwsRepresentation PreviousJwsRepresentation { get; }

        public ChainValue ChainValue { get; }

        public EncryptedTurnover EncryptedTurnover { get; }
        
        public string Suite { get; }

        private byte[] Key { get; }

        private EncryptedTurnover EncryptTurnover()
        {
            var sum = TaxData.Sum();
            var counter = (Turnover.Value + sum) * 100;
            var registryReceiptId = RegisterIdentifier.Value + Number.Value;
            var initializationVectorBytes = Encoding.UTF8.GetBytes(registryReceiptId);
            var registryReceiptIdHash = Sha256(initializationVectorBytes);
            var encryptedValue = AesCtr(registryReceiptIdHash, (long) counter, Key);

            return new EncryptedTurnover(encryptedValue);
        }

        private byte[] AesCtr(byte[] hash, long value, byte[] key)
        {
            var initializationVector = GetEncryptInitializationVector(hash);
            var valueBytes = GetValueBytes(value);

            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            cipher.Init(forEncryption: true, parameters: new ParametersWithIV(new KeyParameter(key), initializationVector));
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

        private ChainValue ComputeChainValue()
        {
            var input = PreviousJwsRepresentation?.Value ?? RegisterIdentifier.Value;
            var hash = Sha256(Encoding.UTF8.GetBytes(input));
            return new ChainValue(hash.Take(8).ToArray());
        }

        private byte[] Sha256(byte[] toBeHashed)
        {
            return new SHA256Managed().ComputeHash(toBeHashed);
        }
    }
}
