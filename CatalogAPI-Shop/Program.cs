using System.Text;
using Card.Data;
using Microsoft.EntityFrameworkCore;
using apiYuGiOhAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using apiYuGiOh.Repository;
using apiYuGiOh.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(Settings.secret);
builder.Services.AddAuthentication(configureOptions: x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions: x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(configure: options =>
{
    options.AddPolicy(name: "Admin", configurePolicy: policy => policy.RequireClaim("Admin"));
    options.AddPolicy(name: "User", configurePolicy: policy => policy.RequireClaim("User"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("v1/catalog/login", (User.Models.User user) => 
{
    var userToken = UserRepository.Get(user.Name, user.Password);
    if (userToken is null)
    {
        return Results.Unauthorized();
    }
    var token = TokenService.GenerateToken(userToken);
    return Results.Ok(new { token = token });

});


app.MapGet("v1/catalog/{skip:int}/{take:int}", async (AppDbContext context, int skip, int take) => 
{   
    if (take > 25)
    {
        return Results.BadRequest("Excesso de cartas. O limite é 25 cartas.");
    }
    await context.Database.EnsureCreatedAsync();
    var cards = await context.Cards.Skip(skip).Take(take).ToListAsync();
    return Results.Ok(cards);

}).AllowAnonymous() ;



app.MapGet("v1/catalog/{id}", async (AppDbContext context, Guid id) => 
{

    await context.Database.EnsureCreatedAsync();
    var card = context.Cards?.FindAsync(id);
    if (card is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(card);

}).AllowAnonymous();



app.MapPost("v1/catalog", async (AppDbContext context, Card.Models.Card card) => 
{
    if (card.Rarity != "Rare" && card.Rarity != "Common")
    {
        return Results.BadRequest("A raridade da carta é inválida. Deve ser 'Rare' ou 'Common'.");
    }
    context.Cards?.Add(card);
    await context.SaveChangesAsync();
    return Results.Created($"/cards/{card.Id}", card);

}).RequireAuthorization();


app.MapPut("v1/catalog/{id}", async (AppDbContext context, Guid id, Card.Models.Card card) => 
{
    if (id != card.Id)
    {
        return Results.BadRequest();
    }
    context.Cards?.Update(card);
    await context.SaveChangesAsync();
    return Results.Ok(card);
}).RequireAuthorization();


app.MapPut("v1/catalog/{id}/{rarity}", async (AppDbContext context, Guid id,string rarity) => 
{
    var card = context.Cards?.Find(id);
    if (card is null)
    {
        return Results.NotFound();
    }
    if (rarity != "Rare" && rarity != "Common")
    {
        return Results.BadRequest("A raridade da carta é inválida. Deve ser 'Rare' ou 'Common'.");
    }
    card.Rarity = rarity;
    context.Cards?.Update(card);
    await context.SaveChangesAsync();
    return Results.Ok(card.Rarity);

}).RequireAuthorization();


app.MapDelete("v1/catalog/{id}", async (AppDbContext context, Guid id) => 
{
    var card = context.Cards?.Find(id);
    if (card is null)
    {
        return Results.NotFound();
    }
    context.Cards?.Remove(card);
    await context.SaveChangesAsync();
    return Results.Ok();

}).RequireAuthorization();

app.Run();
