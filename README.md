# VvsShop – Korisnici

Edukativni ASP.NET Core Web API napisan za predmet *Verifikacija i validacija softvera*. Nije namijenjen za produkcijsku upotrebu.

Modul `Korisnici` ima tri implementacije registracije i prijave, u folderima `VarijantaA`, `VarijantaB` i `VarijantaC`.

## Build i pokretanje

Potreban je .NET 10 SDK. Projekat ne koristi vanjske NuGet pakete.

```bash
dotnet build
dotnet run --project src/VvsShop.Api
```

Primjeri zahtjeva su u `src/VvsShop.Api/VvsShop.Api.http` (Visual Studio, VS Code s REST Client ekstenzijom ili Rider).
