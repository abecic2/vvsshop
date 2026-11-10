using System.Security.Cryptography;
using System.Text;

namespace VvsShop.Api.Korisnici.VarijantaB;

internal static class LozinkaHasherB
{
    private static readonly byte[] Sol = Encoding.UTF8.GetBytes("VvsShop.Korisnici.v1");

    public static string Hesiraj(string lozinka)
    {
        var lozinkaBajtovi = Encoding.UTF8.GetBytes(lozinka);
        var ulaz = new byte[Sol.Length + lozinkaBajtovi.Length];
        Sol.CopyTo(ulaz, 0);
        lozinkaBajtovi.CopyTo(ulaz, Sol.Length);

        return Convert.ToBase64String(SHA256.HashData(ulaz));
    }

    public static bool Provjeri(string lozinka, string sacuvaniHash)
    {
        var izracunati = Encoding.ASCII.GetBytes(Hesiraj(lozinka));
        var sacuvani = Encoding.ASCII.GetBytes(sacuvaniHash);
        return CryptographicOperations.FixedTimeEquals(izracunati, sacuvani);
    }
}
