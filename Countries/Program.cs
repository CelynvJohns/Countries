using Countries.Controllers; // Add this namespace
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout based on your needs
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register FavoritesController as a service
builder.Services.AddScoped<FavoritesController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Category",
        pattern: "Home/Category",
        defaults: new { controller = "Home", action = "Category" }
    );

    endpoints.MapControllerRoute(
        name: "Game",
        pattern: "Home/Game",
        defaults: new { controller = "Home", action = "Game" }
    );

    endpoints.MapControllerRoute(
    name: "Ticket",
    pattern: "Ticket/{action=Index}/{id?}",
    defaults: new { controller = "Ticket" }
);


    // The default route
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
