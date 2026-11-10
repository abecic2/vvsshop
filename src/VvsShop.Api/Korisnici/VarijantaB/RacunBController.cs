using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VvsShop.Api.Zajednicko;

namespace VvsShop.Api.Korisnici.VarijantaB;

[ApiController]
[Route("api/racun-b")]
public class RacunBController : ControllerBase
{
    private const string Podrucje = "racun-b";
    private readonly IKorisnikSpremiste _korisnici;

    public RacunBController(IKorisnikSpremiste korisnici) => _korisnici = korisnici;

    [HttpPost("registracija")]
    public IActionResult Registracija(PrijavaZahtjev zahtjev)
    {
        var zapis = new KorisnikZapis(zahtjev.KorisnickoIme, LozinkaHasherB.Hesiraj(zahtjev.Lozinka));
        return _korisnici.Dodaj(Podrucje, zapis) ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }

    [HttpPost("prijava")]
    [EnableRateLimiting(OgranicenjaZahtjeva.Prijava)]
    public IActionResult Prijava(PrijavaZahtjev zahtjev)
    {
        var korisnik = _korisnici.Pronadji(Podrucje, zahtjev.KorisnickoIme);
        if (korisnik is null || !LozinkaHasherB.Provjeri(zahtjev.Lozinka, korisnik.LozinkaHash))
        {
            return Unauthorized();
        }

        return Ok();
    }
}
