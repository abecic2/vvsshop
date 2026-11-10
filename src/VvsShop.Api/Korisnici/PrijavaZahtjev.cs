using System.ComponentModel.DataAnnotations;

namespace VvsShop.Api.Korisnici;

public sealed record PrijavaZahtjev(
    [Required, StringLength(50, MinimumLength = 3)] string KorisnickoIme,
    [Required, StringLength(128, MinimumLength = 12)] string Lozinka);
