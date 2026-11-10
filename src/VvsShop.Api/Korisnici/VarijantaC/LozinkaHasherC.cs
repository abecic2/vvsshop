using System.Globalization;
using System.Security.Cryptography;

namespace VvsShop.Api.Korisnici.VarijantaC;

internal static class LozinkaHasherC
{
    private const int VelicinaSoli = 16;
    private const int VelicinaHasha = 32;
    private const int BrojIteracija = 600_000;
    private static readonly HashAlgorithmName Algoritam = HashAlgorithmName.SHA256;

    public static string Hesiraj(string lozinka)
    {
        var sol = RandomNumberGenerator.GetBytes(VelicinaSoli);
        var hash = Rfc2898DeriveBytes.Pbkdf2(lozinka, sol, BrojIteracija, Algoritam, VelicinaHasha);

        return string.Create(CultureInfo.InvariantCulture,
            $"{BrojIteracija}.{Convert.ToBase64String(sol)}.{Convert.ToBase64String(hash)}");
    }

    public static bool Provjeri(string lozinka, string sacuvaniHash)
    {
        var dijelovi = sacuvaniHash.Split('.');
        if (dijelovi.Length != 3
            || !int.TryParse(dijelovi[0], NumberStyles.None, CultureInfo.InvariantCulture, out var iteracije)
            || iteracije < BrojIteracija)
        {
            return false;
        }

        var sol = new byte[VelicinaSoli];
        var ocekivani = new byte[VelicinaHasha];
        if (!Convert.TryFromBase64String(dijelovi[1], sol, out var duzinaSoli) || duzinaSoli != VelicinaSoli
            || !Convert.TryFromBase64String(dijelovi[2], ocekivani, out var duzinaHasha) || duzinaHasha != VelicinaHasha)
        {
            return false;
        }

        var izracunati = Rfc2898DeriveBytes.Pbkdf2(lozinka, sol, iteracije, Algoritam, VelicinaHasha);
        return CryptographicOperations.FixedTimeEquals(izracunati, ocekivani);
    }
}
