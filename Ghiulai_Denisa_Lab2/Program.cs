using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2;
using Ghiulai_Denisa_Lab2.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Ghiulai_Denisa_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Ghiulai_Denisa_Lab2Context' not found.");

builder.Services.AddDbContext<Ghiulai_Denisa_Lab2Context>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Ghiulai_Denisa_Lab2Context>();
    db.Database.EnsureCreated();
}

app.Run();