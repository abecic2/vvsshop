using System.Threading.RateLimiting;
using VvsShop.Api.Korisnici;
using VvsShop.Api.Zajednicko;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter(opcije =>
{
    opcije.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    opcije.AddPolicy(OgranicenjaZahtjeva.Prijava, kontekst =>
        RateLimitPartition.GetFixedWindowLimiter(
            $"{kontekst.Connection.RemoteIpAddress}|{kontekst.Request.Path}",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddSingleton<IKorisnikSpremiste, MemorijskoKorisnikSpremiste>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapControllers();

app.Run();
