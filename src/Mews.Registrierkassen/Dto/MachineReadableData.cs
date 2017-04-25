using System;
namespace Mews.Registrierkassen.Dto
{
    public abstract class MachineReadableData
    {
        protected MachineReadableData(Receipt receipt, BaseEncoding baseEncoding)
        {
            Receipt = receipt;
            BaseEncoding = baseEncoding;
            Value = ComputeValue();
        }

        public Receipt Receipt { get; }

        public BaseEncoding BaseEncoding { get; }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        private string FormatDate(LocalDateTime localDateTime)
        {
            var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(localDateTime.DateTime, localDateTime.TimeZoneInfo);
            var dateTimeAustria = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, LocalDateTime.AustrianTimezone);
            var date = DateTime.SpecifyKind(new DateTime(dateTimeAustria.Ticks - dateTimeAustria.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Local);
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        private string FormatDecimal(decimal amount)
        {
            return String.Format(System.Globalization.CultureInfo.GetCultureInfo("de-AT"), "{0:F2}", amount);
        }

        private string ComputeValue()
        {
            var fieldValues = new[]
            {
                Receipt.Suite,
                Receipt.RegisterIdentifier.Value,
                Receipt.Number.Value,
                FormatDate(Receipt.Created),
                FormatDecimal(Receipt.TaxData.StandardRate.Value),
                FormatDecimal(Receipt.TaxData.ReducedRate.Value),
                FormatDecimal(Receipt.TaxData.LowerReducedRate.Value),
                FormatDecimal(Receipt.TaxData.ZeroRate.Value),
                FormatDecimal(Receipt.TaxData.SpecialRate.Value),
                Receipt.EncryptedTurnover.ToBaseNString(BaseEncoding),
                Receipt.CertificateSerialNumber.Value,
                Receipt.ChainValue.ToBaseNString(BaseEncoding)
            };

            return $"_{String.Join("_", fieldValues)}";
        }
    }
}
