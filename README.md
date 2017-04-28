# Registrierkassen

A client library using the A-Trust WS to sign QR data.

## Usage example
```csharp
var client = new ATrustClient(Credentials, ATrustEnvironment.Test);
var result = client.Sign(new ATrustSignerInput(
    Credentials.Password,
    new QrData(new Receipt(
        number: new ReceiptNumber("83469"),
        registerIdentifier: new RegisterIdentifier("DEMO-CASH-BOX817"),
        taxData: new TaxData(
            standartRate: new CurrencyValue(29.73m),
            lowerReducedRate: new CurrencyValue(36.41m),
            specialRate: new CurrencyValue(21.19m)
        ),
        turnover: new CurrencyValue(0.0m), 
        certificateSerialNumber: new CertificateSerialNumber("-3667961875706356849"),
        previousJwsRepresentation: new JwsRepresentation("d3YUbS4CoRo="), 
        key: Convert.FromBase64String("RCsRmHn5tkLQrRpiZq2ucwPpwvHJLiMgLvwrwEImddI="),
        created: new LocalDateTime(
            new DateTime(2015, 11, 25, 19, 20, 11),
            LocalDateTime.AustrianTimezone
        )
    )
)));
```
