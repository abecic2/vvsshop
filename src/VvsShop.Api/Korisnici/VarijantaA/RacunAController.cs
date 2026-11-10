using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VvsShop.Api.Zajednicko;

namespace VvsShop.Api.Korisnici.VarijantaA;

[ApiController]
[Route("api/racun-a")]
public class RacunAController : ControllerBase
{
    private const string Podrucje = "racun-a";
    private readonly IKorisnikSpremiste _korisnici;

    public RacunAController(IKorisnikSpremiste korisnici) => _korisnici = korisnici;

    [HttpPost("registracija")]
    public IActionResult Registracija(PrijavaZahtjev zahtjev)
    {
        var zapis = new KorisnikZapis(zahtjev.KorisnickoIme, Hesiraj(zahtjev.Lozinka));
        return _korisnici.Dodaj(Podrucje, zapis) ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }

    [HttpPost("prijava")]
    [EnableRateLimiting(OgranicenjaZahtjeva.Prijava)]
    public IActionResult Prijava(PrijavaZahtjev zahtjev)
    {
        var korisnik = _korisnici.Pronadji(Podrucje, zahtjev.KorisnickoIme);
        if (korisnik is null || korisnik.LozinkaHash != Hesiraj(zahtjev.Lozinka))
        {
            return Unauthorized();
        }

        return Ok();
    }

    private static string Hesiraj(string lozinka)
    {
        var bajtovi = MD5.HashData(Encoding.UTF8.GetBytes(lozinka));
        return Convert.ToHexString(bajtovi);
    }
}
