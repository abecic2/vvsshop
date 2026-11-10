using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VvsShop.Api.Zajednicko;

namespace VvsShop.Api.Korisnici.VarijantaC;

[ApiController]
[Route("api/racun-c")]
public class RacunCController : ControllerBase
{
    private const string Podrucje = "racun-c";
    private static readonly Lazy<string> ZamjenskiHash =
        new(() => LozinkaHasherC.Hesiraj(Guid.NewGuid().ToString("N")));

    private readonly IKorisnikSpremiste _korisnici;

    public RacunCController(IKorisnikSpremiste korisnici) => _korisnici = korisnici;

    [HttpPost("registracija")]
    public IActionResult Registracija(PrijavaZahtjev zahtjev)
    {
        var zapis = new KorisnikZapis(zahtjev.KorisnickoIme, LozinkaHasherC.Hesiraj(zahtjev.Lozinka));
        return _korisnici.Dodaj(Podrucje, zapis) ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }

    [HttpPost("prijava")]
    [EnableRateLimiting(OgranicenjaZahtjeva.Prijava)]
    public IActionResult Prijava(PrijavaZahtjev zahtjev)
    {
        var korisnik = _korisnici.Pronadji(Podrucje, zahtjev.KorisnickoIme);
        var lozinkaIspravna = LozinkaHasherC.Provjeri(
            zahtjev.Lozinka, korisnik?.LozinkaHash ?? ZamjenskiHash.Value);

        if (korisnik is null || !lozinkaIspravna)
        {
            return Unauthorized();
        }

        return Ok();
    }
}
