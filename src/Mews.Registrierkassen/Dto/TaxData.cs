namespace Mews.Registrierkassen.Dto
{
    public sealed class TaxData
    {
        public TaxData(
            CurrencyValue standartRate = null,
            CurrencyValue reducedRate = null,
            CurrencyValue lowerReducedRate = null,
            CurrencyValue zeroRate = null,
            CurrencyValue specialRate = null
        )
        {
            StandardRate = standartRate ?? Fallback;
            ReducedRate = reducedRate ?? Fallback;
            LowerReducedRate = lowerReducedRate ?? Fallback;
            ZeroRate = zeroRate ?? Fallback;
            SpecialRate = specialRate ?? Fallback;
        }

        public CurrencyValue StandardRate { get; }

        public CurrencyValue ReducedRate { get; }

        public CurrencyValue LowerReducedRate { get; }

        public CurrencyValue ZeroRate { get; }

        public CurrencyValue SpecialRate { get; }

        private CurrencyValue Fallback
        {
            get { return new CurrencyValue(0); }
        }

        public decimal Sum()
        {
            return StandardRate.Value + ReducedRate.Value + LowerReducedRate.Value + ZeroRate.Value + SpecialRate.Value;
        }
    }
}
