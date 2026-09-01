# EU & UK VAT Validator & Rate Engine — .NET / C# Client

[![NuGet version](https://img.shields.io/nuget/v/RapidApi.VatValidatorClient.svg)](https://www.nuget.org/packages/RapidApi.VatValidatorClient/)
[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/57865358-8bafe64c-1441-4fe3-ba7a-2d60bdeb7dc5)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![RapidAPI Listing](https://img.shields.io/badge/RapidAPI-Dedicated%20Listing-blueviolet)](https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine)

Official zero-dependency .NET / C# client for **EU & UK VAT Validator & Rate Engine**.

> Deterministic offline checksum validator for EU-27 and UK VAT numbers with instant 2026 standard & reduced tax rate lookups.

> 🔑 **Get your Dedicated API Key:** [Subscribe to EU & UK VAT Validator & Rate Engine on RapidAPI](https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine)

---

## 🚀 Installation

```bash
dotnet add package RapidApi.VatValidatorClient
```

---

## ⚡ Quickstart

```csharp
using System;
using System.Threading.Tasks;
using RapidApi.VatValidator;

class Program
{
    static async Task Main()
    {
        using var client = new VatValidatorClient(new RapidApiConfig
        {
            ApiKey = "YOUR_RAPIDAPI_KEY" // Get key from https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine
        });

        var result = await client.ValidateAsync(new
        {
            // Enter validation payload
        });

        Console.WriteLine($"Success: {result.Success}");
    }
}
```

---

## 🔗 Links
- 📖 [RapidAPI Documentation & Key](https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine)

## 📄 License
MIT © Noor Mkdad
