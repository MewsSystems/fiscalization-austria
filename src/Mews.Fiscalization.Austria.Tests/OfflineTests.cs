using System;
using System.Globalization;
using System.Runtime.InteropServices;
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
            var certificateInfo = new CertificateInfo
            {
                Algorithm = "ES256",
                CertificateSerialNumber = "559528438",
                CertificateSerialNumberHex = "2159B9F6",
                Certificate = "MIIENQIBAzCCA/EGCSqGSIb3DQEHAaCCA+IEggPeMIID2jCCAcsGCSqGSIb3DQEHAaCCAbwEggG4MIIBtDCCAbAGCyqGSIb3DQEMCgECoIHMMIHJMBwGCiqGSIb3DQEMAQMwDgQI3fVWOywGIBQCAgfQBIGokQJpqms9ZNZHHVcc2nuJd3CMdWy//wYLNploeYUkMJVQ5uVM7KllROx1V5quZMY7Fonivpt/i6Q7cI1WWpxQfre/J808OqDbp0+dYuNM+10EGgAlkXX1BjYGx/g6ClIfPW8bd6pxIOLb6rBasekRqGQ9aGyIAzz2DYYZuuxJRDPrHXZ3mJ1RHiKDbGiRGFyOeI+owkzDDUBdCLwMAlR8JTp5RhzwI2jXMYHRMBMGCSqGSIb3DQEJFTEGBAQBAAAAMFsGCSqGSIb3DQEJFDFOHkwAewBEADQAQgA2ADYANgA4ADgALQBFADUANgA2AC0ANABCAEMAQgAtADkANwA3ADcALQBDADIAMgA0AEYAQgBBADYARQAwADEANQB9MF0GCSsGAQQBgjcRATFQHk4ATQBpAGMAcgBvAHMAbwBmAHQAIABTAG8AZgB0AHcAYQByAGUAIABLAGUAeQAgAFMAdABvAHIAYQBnAGUAIABQAHIAbwB2AGkAZABlAHIwggIHBgkqhkiG9w0BBwagggH4MIIB9AIBADCCAe0GCSqGSIb3DQEHATAcBgoqhkiG9w0BDAEDMA4ECGoozgOA5OKyAgIH0ICCAcBj0YqVMzSNzr3WmvamjTnBWqjlJS3+CPhlJO3ilzc/DZCJ9Eme6976o9Z4vDIPJRN5VSuMnGB1hCRCv8HOs6VlvpxqfxIUpfyxuyFHLESeI6az0WmUQPOPGk3ibCH2cPm9KnbknyUEoG0k/pv/60gcbjZ5LxsW8GK8P4p1BO04nB57WQX0KVxRglSCY7NPsjbqz28ItThATNa1LMG/sT2RIaEgH+zSrNXOHXMgdZ0EaFNDgphIL2uY5U3bAqE7cKkKsNTWS11anggJKRcI05eM0ZI/9JfngV2y/F8Tk7Yxiz66ga/mb991LtYHAsithmJJu/XlOsK5mmfc89wO9lk5WYMKVUQVDBhGhkNH4RwkYtT0o3ZV3MXaFCdGvKp1x1DnHfdFaKj8p8prCmDed9prMWIhYaRNJ+Fyqcv3CkP249bn1f4nZbxJuSsYd3SgAy5GLggcGFvhu9+j9zdi9104IgGJnq6o6NjsLYFV0dqK+vt1P2tXF0OFng9UHL7OobyVRJAt1tD+6bb2yW/g4B/oz4Qx4ZLAHEn+dV4LhdDolfj1Zrow8hOpxp79I2OABdnbEsmn0bw0kcfohnQ3fFQHMDswHzAHBgUrDgMCGgQUkXasbVygdx7ozXzfZFjUQCtbcEUEFBwkd8XAO484xTwDQ1OWbkc8g/UoAgIH0A==",
                CertificationBodies = new []
                {
                    "MIIEATCCAumgAwIBAgIEOWntwTANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDkxN1oXDTI0MTExODEzNDkxN1owgaExCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxIzAhBgNVBAsMGmEtc2lnbi1QcmVtaXVtLVRlc3QtU2lnLTAyMSMwIQYDVQQDDBphLXNpZ24tUHJlbWl1bS1UZXN0LVNpZy0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANwJSfWpRaziThddTTup72CltlXl8oc7HQoK2SWsYQwZGAd5nJZbwbI4K8VFKlNnK72Zl8UhmQ2FxhzS6u+Q+qEzJOM2xTfA2NB6A9/KFpTJXUjvCHgRvW16EEF9YpYXxKTSK+QrYCXAC5rL6SuYOzgA7Q1ivq+zLbyXxroux2zVEBIiaBGpZhOHGDFJk6h/4QelIqzd2TIDCRzvhmLDVmhqX2C1NQb5kZuMgrxxOhG5Cr1F8solkwyu43JiM+apY4bZJVQBwi9ATBMz5tfdoLRslQy1BCQ4X+b6u/2856gucU+1e/wa5pB9Ff0eP+xy+j2DZOXLNd8m/IQvnshjNusCAwEAAaNLMEkwDwYDVR0TAQH/BAUwAwEB/zARBgNVHQ4ECgQIRgafjkGOFb0wEwYDVR0jBAwwCoAIQg8xWXA9iecwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQBq/owq5eGvhxegchLvnMjPnE9gTYIHEvMq8DN6h2J7pTEhKG2o09LLn/pNHWRjKENU/LqZBIAJ5zebm5XqzB631BYcuu1abyPFfpMdAL9X4zFuDvg9EGaTir2c81XaBYzVSLN7fxmNLKSmMwUt0JQQyqpe3V9iyoBE/WcQyKmKaEp7mCZsGFBm6KmJgqD6TPb7X9bWUr3yx6Z5gek71f3vQi69m1x811azXlxu1i/XFnVpzxkrKRXJWC+wnQRxXmU7YnMzYPOA7UOpUG6J+7tYi29hY3EpMgyXM/T/BL5MdyzBefbPVzLHng5zVaXNpO0ENCrlUyi1m3Yd/7QPDdJM",
                    "MIID4DCCAsigAwIBAgIEOWntvzANBgkqhkiG9w0BAQUFADCBlTELMAkGA1UEBhMCQVQxSDBGBgNVBAoMP0EtVHJ1c3QgR2VzLiBmLiBTaWNoZXJoZWl0c3N5c3RlbWUgaW0gZWxla3RyLiBEYXRlbnZlcmtlaHIgR21iSDEdMBsGA1UECwwUQS1UcnVzdC1UZXN0LVF1YWwtMDIxHTAbBgNVBAMMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMB4XDTE0MTEyNDE0NDc0N1oXDTI0MTExODEzNDc0N1owgZUxCzAJBgNVBAYTAkFUMUgwRgYDVQQKDD9BLVRydXN0IEdlcy4gZi4gU2ljaGVyaGVpdHNzeXN0ZW1lIGltIGVsZWt0ci4gRGF0ZW52ZXJrZWhyIEdtYkgxHTAbBgNVBAsMFEEtVHJ1c3QtVGVzdC1RdWFsLTAyMR0wGwYDVQQDDBRBLVRydXN0LVRlc3QtUXVhbC0wMjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANMBok2fNNtIEcf7Sw47vprkUeti6Y64Rc5rrAjh7cGwo4Jp5LyfvEVdv9AMNiuOX7ywd1xW99UZWtZ8MzXvWM5M6trLkeBYnCukwc9DqawXcuXXCYwgTuisFTmYO6GVJNr1iE/LJdSKbu5AVDS3FwXixqyJkjv/xWIwU4q86oATW8++8wb6Lu+fQlhBbn3Kqpavt6K+lwWSCb+8vIhB47IlKhJZwGqXfGV9l9dDgKYUbZiv3BBa+MRBUTvIcahEKz8hG2E8W4EgCwzISMpeStJtRHo/tJnA90KfSBTcz0txrxpHwqFgKwJvgW6nIjY1Sv5MfY5YJiEWv0d7UUkvlScCAwEAAaM2MDQwDwYDVR0TAQH/BAUwAwEB/zARBgNVHQ4ECgQIQg8xWXA9iecwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3DQEBBQUAA4IBAQApqSvkQyfbO2yDWewHwo1Zl32uGz41KMP5FYtA3BIcqh89paHwrW9KfcrybdUIneVz4iSnpyrDrS4LavfP8h/Hl1kRmVZRUBsOJRvqc1fiC2B6IJRHrmayb/DbXuyoOsk7Sr8M9xtAD3SzJCRkBrtjz/U/xQdU9TfV9SQyPN3qI+SR25/LRZDhOKcIFJduVpTYzbnKTIkl3OUrHXVq5xddxX6XP8bUjT+SqGiDf15H6N5flNBsvolMSo0OoQXFiDuY33frQSrSbHbA2p/MptwxA8JgGh4lrbgZZxjTvpO1wATBLDc3wGZkNuy+tNrrHAmE08B7fiExULHxzfaZEWSF"
                }
            };

            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var europeTimeZone = "Central Europe Standard Time";
            var austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById(isWindows ? europeTimeZone : TZConvert.WindowsToIana(europeTimeZone));
            var austrianCulture = CultureInfo.GetCultureInfo("de-AT");

            var signer = new OfflineSigner(certificateInfo);
            var result = signer.Sign(new QrData(new Receipt(
                number: new ReceiptNumber("83469"),
                registerIdentifier: new RegisterIdentifier("DEMO-CASH-BOX817"),
                taxData: new TaxData(
                    standartRate: new CurrencyValue(29.73m),
                    lowerReducedRate: new CurrencyValue(36.41m),
                    specialRate: new CurrencyValue(21.19m)
                ),
                turnover: new CurrencyValue(0.0m),
                certificateSerialNumber: new CertificateSerialNumber(certificateInfo.CertificateSerialNumber),
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