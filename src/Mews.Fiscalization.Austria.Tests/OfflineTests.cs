using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Mews.Fiscalization.Austria.Dto;
using Mews.Fiscalization.Austria.Dto.Identifiers;
using Mews.Fiscalization.Austria.Offline;
using NUnit.Framework;
using TimeZoneConverter;

namespace Mews.Fiscalization.Austria.Tests
{
    public class OfflineTests
    {
        [Test]
        public void OfflineSignatureWorks()
        {
            var base64Certificate = "MIIENQIBAzCCA/EGCSqGSIb3DQEHAaCCA+IEggPeMIID2jCCAcsGCSqGSIb3DQEHAaCCAbwEggG4MIIBtDCCAbAGCyqGSIb3DQEMCgECoIHMMIHJMBwGCiqGSIb3DQEMAQMwDgQI3fVWOywGIBQCAgfQBIGokQJpqms9ZNZHHVcc2nuJd3CMdWy//wYLNploeYUkMJVQ5uVM7KllROx1V5quZMY7Fonivpt/i6Q7cI1WWpxQfre/J808OqDbp0+dYuNM+10EGgAlkXX1BjYGx/g6ClIfPW8bd6pxIOLb6rBasekRqGQ9aGyIAzz2DYYZuuxJRDPrHXZ3mJ1RHiKDbGiRGFyOeI+owkzDDUBdCLwMAlR8JTp5RhzwI2jXMYHRMBMGCSqGSIb3DQEJFTEGBAQBAAAAMFsGCSqGSIb3DQEJFDFOHkwAewBEADQAQgA2ADYANgA4ADgALQBFADUANgA2AC0ANABCAEMAQgAtADkANwA3ADcALQBDADIAMgA0AEYAQgBBADYARQAwADEANQB9MF0GCSsGAQQBgjcRATFQHk4ATQBpAGMAcgBvAHMAbwBmAHQAIABTAG8AZgB0AHcAYQByAGUAIABLAGUAeQAgAFMAdABvAHIAYQBnAGUAIABQAHIAbwB2AGkAZABlAHIwggIHBgkqhkiG9w0BBwagggH4MIIB9AIBADCCAe0GCSqGSIb3DQEHATAcBgoqhkiG9w0BDAEDMA4ECGoozgOA5OKyAgIH0ICCAcBj0YqVMzSNzr3WmvamjTnBWqjlJS3+CPhlJO3ilzc/DZCJ9Eme6976o9Z4vDIPJRN5VSuMnGB1hCRCv8HOs6VlvpxqfxIUpfyxuyFHLESeI6az0WmUQPOPGk3ibCH2cPm9KnbknyUEoG0k/pv/60gcbjZ5LxsW8GK8P4p1BO04nB57WQX0KVxRglSCY7NPsjbqz28ItThATNa1LMG/sT2RIaEgH+zSrNXOHXMgdZ0EaFNDgphIL2uY5U3bAqE7cKkKsNTWS11anggJKRcI05eM0ZI/9JfngV2y/F8Tk7Yxiz66ga/mb991LtYHAsithmJJu/XlOsK5mmfc89wO9lk5WYMKVUQVDBhGhkNH4RwkYtT0o3ZV3MXaFCdGvKp1x1DnHfdFaKj8p8prCmDed9prMWIhYaRNJ+Fyqcv3CkP249bn1f4nZbxJuSsYd3SgAy5GLggcGFvhu9+j9zdi9104IgGJnq6o6NjsLYFV0dqK+vt1P2tXF0OFng9UHL7OobyVRJAt1tD+6bb2yW/g4B/oz4Qx4ZLAHEn+dV4LhdDolfj1Zrow8hOpxp79I2OABdnbEsmn0bw0kcfohnQ3fFQHMDswHzAHBgUrDgMCGgQUkXasbVygdx7ozXzfZFjUQCtbcEUEFBwkd8XAO484xTwDQ1OWbkc8g/UoAgIH0A==";
            var certificate = new X509Certificate2(Convert.FromBase64String(base64Certificate));

            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var europeTimeZone = "Central Europe Standard Time";
            var austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById(isWindows ? europeTimeZone : TZConvert.WindowsToIana(europeTimeZone));
            var austrianCulture = CultureInfo.GetCultureInfo("de-AT");

            var signer = new OfflineSigner(certificate);
            var result = signer.Sign(new QrData(new Receipt(
                number: new ReceiptNumber("83469"),
                registerIdentifier: new RegisterIdentifier("DEMO-CASH-BOX817"),
                taxData: new TaxData(
                    standartRate: new CurrencyValue(29.73m),
                    lowerReducedRate: new CurrencyValue(36.41m),
                    specialRate: new CurrencyValue(21.19m)
                ),
                turnover: new CurrencyValue(0.0m),
                certificateSerialNumber: new CertificateSerialNumber(certificate.SerialNumber),
                previousJwsRepresentation: new JwsRepresentation("eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.WDFJeExVRlVNVjloT0RRME1URXpZaTFoTTJRM0xUUmxObU10T0RGak9DMDJOalU0TXpnMk9HVm1NelpmTTE4eU1ERTNMVEV5TFRFeVZERXlPalV6T2pVMlh6QXNNREJmTVRBd0xEQXdYekFzTURCZk1Dd3dNRjh3TERBd1h6ZzNMMnR2YW05RVYwUjNQVjh3TUVJd05qQkJNRUkwTWpFMlJUQXhSRFJmZVROVVp6TXlOV1Z0Y0UwOQ.6mzl1HSWmJyWaUG0pZlNuF29Eg9jocyXSuBxYWnwskE3fpVLd2PTIHG9ecBvQnCW3SokaMiEEgYN969Z4P7i0w"),
                key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
                created: new LocalDateTime(
                    new DateTime(year: 2015, month: 11, day: 25, hour: 19, minute: 20, second: 11),
                    austrianTimeZone
                )
            ), austrianCulture, austrianTimeZone));
            Assert.IsNotNull(result);
        }
    }
}