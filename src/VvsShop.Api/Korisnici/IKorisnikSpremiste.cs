namespace VvsShop.Api.Korisnici;

public interface IKorisnikSpremiste
{
    bool Dodaj(string podrucje, KorisnikZapis korisnik);

    KorisnikZapis? Pronadji(string podrucje, string korisnickoIme);
}
