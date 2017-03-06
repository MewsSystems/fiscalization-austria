using System;

namespace Mews.Registrierkassen.Dto
{
    public class Signature
    {
        public Signature(string base64Value)
        {
            Base64Value = base64Value;
        }

        public string Base64Value { get; }

        public override string ToString()
        {
            return Base64Value;
        }
    }
}
