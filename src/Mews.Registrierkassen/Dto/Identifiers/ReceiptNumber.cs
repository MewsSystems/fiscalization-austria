namespace Mews.Registrierkassen.Dto.Identifiers
{
    public class ReceiptNumber : StringIdentifier
    {
        public ReceiptNumber(string value)
            : base(value, Patterns.ReceiptNumber)
        {
        }
    }
}
