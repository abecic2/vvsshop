using System.Collections.Concurrent;

namespace VvsShop.Api.Korisnici;

public sealed class MemorijskoKorisnikSpremiste : IKorisnikSpremiste
{
    private readonly ConcurrentDictionary<(string Podrucje, string Ime), KorisnikZapis> _korisnici = new();

    public bool Dodaj(string podrucje, KorisnikZapis korisnik) =>
        _korisnici.TryAdd(Kljuc(podrucje, korisnik.KorisnickoIme), korisnik);

    public KorisnikZapis? Pronadji(string podrucje, string korisnickoIme) =>
        _korisnici.GetValueOrDefault(Kljuc(podrucje, korisnickoIme));

    private static (string, string) Kljuc(string podrucje, string korisnickoIme) =>
        (podrucje, korisnickoIme.ToUpperInvariant());
}
