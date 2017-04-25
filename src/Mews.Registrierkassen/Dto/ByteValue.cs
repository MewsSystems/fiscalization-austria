using System;

namespace Mews.Registrierkassen.Dto
{
    public class ByteValue
    {
        public ByteValue(byte[] value)
        {
            Value = value;
        }

        public byte[] Value { get; }

        public string ToBase64String()
        {
            return Convert.ToBase64String(Value);
        }
    }
}
