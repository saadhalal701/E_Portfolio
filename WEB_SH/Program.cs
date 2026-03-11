using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using WEB_SH.Entities;
using WEB_SH.Data;
using WEB_SH.Repository;
using WEB_SH.Services;

var builder = WebApplication.CreateBuilder(args);

// AJOUTEZ CE BLOC POUR LE LOGGING (avant AddControllersWithViews)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // Pour voir les logs dans la console
builder.Logging.AddDebug();    // Pour le débogage
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Tous les niveaux de logs

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration de la base de données
builder.Services.AddDbContext<MyDbContextPortfolio>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration des sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Repository - Ajoutez tous vos repositories
builder.Services.AddScoped<PersonneRepos>();
builder.Services.AddScoped<CompetenceRepos>();
builder.Services.AddScoped<MessageRepos>();
builder.Services.AddScoped<ProjetRepos>();
builder.Services.AddScoped<IPasswordHasher<Personne>, PasswordHasher<Personne>>();

// Services (si vous en avez)
builder.Services.AddScoped<ProfilService>();

// AJOUT IMPORTANT : Pour le Anti-Forgery Token et la validation
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

// AJOUT IMPORTANT : Pour l'accès au contexte HTTP
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// AJOUTEZ CE MIDDLEWARE POUR CAPTER LES ERREURS
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Écrit directement dans la console
        Console.WriteLine($"=== ERREUR GRAVE DÉTECTÉE ===");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        Console.WriteLine($"=== FIN ERREUR ===");

        // Relance l'exception pour que l'application sache qu'il y a une erreur
        throw;
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// AJOUTEZ CE MIDDLEWARE POUR LA GESTION D'ERREURS DÉTAILLÉE (développement uniquement)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Pour l'autorisation
app.UseAuthentication(); 
app.UseAuthorization();

// AJOUT IMPORTANT : Configuration de la session DOIT venir après UseRouting
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();