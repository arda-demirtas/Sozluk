using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Sozluk.Data.Abstract.Entry;
using Sozluk.Data.Abstract.Title;
using Sozluk.Data.Abstract.User;
using Sozluk.Data.Concrete;
using Sozluk.Data.Concrete.EntryF;
using Sozluk.Data.Concrete.TitleF;
using Sozluk.Data.Concrete.UserF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SozlukContext>(options=>
{
    var config = builder.Configuration;
    var conntectionString = config.GetConnectionString("sqlite");
    options.UseSqlite(conntectionString);
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/users/login";
});

builder.Services.AddScoped<IEntryReadRepository, EntryReadRepository>();
builder.Services.AddScoped<IEntryWriteRepository, EntryWriteRepository>();
builder.Services.AddScoped<ITitleReadRepository, TitleReadRepository>();
builder.Services.AddScoped<ITitleWriteRepository, TitleWriteRepository>();
builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();
builder.Services.AddScoped<IUserWriteRepository, UserWriteRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "titles",
    pattern: "titles/index/{page}",
    defaults: new { controller = "Titles", action = "Index" }
);


app.MapControllerRoute(
    name: "titles",
    pattern: "/title/{slug}/{page}",
    defaults: new { controller = "Titles", action = "Title" }
);

app.MapControllerRoute(
    name: "profile",
    pattern: "/profile/{nickname}",
    defaults: new { controller = "Users", action = "Profile" }
);

app.MapDefaultControllerRoute();

app.Run();
