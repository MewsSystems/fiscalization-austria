using System;
using Albireo.Base32;

namespace Mews.Registrierkassen.Dto
{
    public class ByteValue
    {
        public ByteValue(byte[] value)
        {
            Value = value;
        }

        public byte[] Value { get; }

        public string ToBase32String()
        {
            return Base32.Encode(Value);
        }

        public string ToBase64String()
        {
            return Convert.ToBase64String(Value);
        }

        public string ToBaseNString(BaseEncoding encoding)
        {
            if (encoding == BaseEncoding.Base64)
            {
                return ToBase64String();
            }
            if (encoding == BaseEncoding.Base32)
            {
                return ToBase32String();
            }

            throw new ArgumentException("Unsupported encoding");
        }
    }
}
