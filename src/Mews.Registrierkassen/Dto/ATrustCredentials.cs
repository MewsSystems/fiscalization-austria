using Mews.Registrierkassen.Dto.Identifiers;

namespace Mews.Registrierkassen.Signers.Dto
{
    public sealed class ATrustCredentials
    {
        public ATrustCredentials(ATrustUserIdentifier user, string password)
        {
            User = user;
            Password = password;
        }

        public ATrustUserIdentifier User { get; }

        public string Password { get; }
    }
}
