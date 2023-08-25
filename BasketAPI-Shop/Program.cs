using Basket.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Basket.API", Version = "v1" });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.MapGet("v1/cart/{name}", async (AppDbContext db, string name) =>
{
    var cart = await db.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserName == name);

    if (cart is null)
    {

        var newCart = new ShoppingCart(name);
        await db.ShoppingCarts.AddAsync(newCart);
        await db.SaveChangesAsync();
        return Results.Ok(newCart);

    }

    return Results.Ok(cart);
});

app.MapPost("v1/cart", async (AppDbContext db, ShoppingCart cart) =>
{
    var existingCart = await db.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserName == cart.UserName);

    if (existingCart is null)
    {
        await db.ShoppingCarts.AddAsync(cart);
        await db.SaveChangesAsync();
        return Results.Ok(cart);
    }

    existingCart.Items.AddRange(cart.Items);
    await db.SaveChangesAsync();
    return Results.Ok(existingCart);
});

app.MapPost("v1/cart/{name}/{coupon}", async (AppDbContext db, string name, string coupon) =>
{
    var cart = await db.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserName == name);


    if (cart is null)
    {
        return Results.NotFound();
    }
    if (cart.CouponApplied)
    {
        return Results.BadRequest("Cupom já foi aplicado anteriormente.");
    }

    if (coupon == "50RARE")
    {
        foreach (var item in cart.Items )
        {
             if (item.Rarity == "rare")
            {
                item.Price *= 0.5m;
            }
        }
        cart.CouponApplied = true;
    }


    await db.SaveChangesAsync();
    return Results.Ok(cart);
});

app.MapDelete("v1/cart/{name}", async (AppDbContext db, string name) =>
{
    var cart = await db.ShoppingCarts.FindAsync(name);
    if (cart is null)
    {
        return Results.NotFound();
    }
    db.ShoppingCarts.Remove(cart);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();
