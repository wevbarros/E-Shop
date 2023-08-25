using CouponAPI.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CouponAPI", Version = "v1" }); 
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coupon API V1"));

app.MapGet("v1/coupon", async (AppDbContext db) =>
{
    var coupons = await db.Coupons.ToListAsync();
    return Results.Ok(coupons);
});

app.MapPost("v1/coupon", async (AppDbContext db, Coupon coupon) =>
{
    await db.Coupons.AddAsync(coupon);
    await db.SaveChangesAsync();
    return Results.Created($"/v1/coupon/{coupon.CouponCode}", coupon);
});

app.MapGet("v1/coupon/{couponCode}", async (AppDbContext db , string couponCode) =>
{
    var coupon = await db.Coupons.FindAsync(couponCode);
    if (coupon == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(coupon);
});

app.MapDelete("v1/coupon/{couponCode}", async (AppDbContext db, string couponCode) =>
{
    var coupon = await db.Coupons.FindAsync(couponCode);
    if (coupon == null)
    {
        return Results.NotFound();
    }
    db.Coupons.Remove(coupon);
    await db.SaveChangesAsync();
    return Results.Ok();
});



app.Run();
