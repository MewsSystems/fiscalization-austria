namespace Mews.Registrierkassen.Dto
{
    public class JwsRepresentation
    {
        public JwsRepresentation(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}
