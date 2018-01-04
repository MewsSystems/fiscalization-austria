using Mews.Registrierkassen.Dto;

namespace Mews.Registrierkassen
{
    public interface ISigner
    {
        SignerOutput Sign(QrData qrData);
    }
}
