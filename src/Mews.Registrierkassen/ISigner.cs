using Mews.Registrierkassen.Dto;

namespace Mews.Registrierkassen
{
    interface ISigner
    {
        SignerOutput Sign(QrData qrData);
    }
}
