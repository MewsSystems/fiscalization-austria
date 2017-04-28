using Org.BouncyCastle.Security;

namespace Mews.Registrierkassen
{
    public sealed class AesKeyGenerator
    {
        public static byte[] GetNext()
        {
            var random = new SecureRandom();
            var bytes = new byte[32];
            random.NextBytes(bytes);
            return bytes;
        }
    }
}
