using System;
using System.Text.RegularExpressions;

namespace Mews.Registrierkassen.Dto.Identifiers
{
    public sealed class Base64UrlSafeString : StringIdentifier
    {
        public static readonly Regex Pattern = new Regex(".+");

        public Base64UrlSafeString(string value)
            : base(value, Pattern)
        {
            Base64UrlUnsafeString = GetUnsafeString();
        }

        public Base64UrlUnsafeString Base64UrlUnsafeString { get; }

        private Base64UrlUnsafeString GetUnsafeString()
        {
            var sanitizedString = Value.Replace('-', '+').Replace('_', '/');
            var paddingLength = sanitizedString.Length % 4;
            return new Base64UrlUnsafeString(paddingLength > 0 ? sanitizedString + new String('=', 4 - paddingLength) : sanitizedString);
        }
    }
}
