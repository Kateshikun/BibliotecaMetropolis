/* Integrantes 
Daniel Adrian Castillo Garcia CG250400
Rudy Mauricio Gonzales Pineda GP250120
Francisco Josue Santos Lopez SL251022
Anderson Rub�n Portillo Alfaro PA250105
Nahum de Jes�s Flores Gir�n FG250084
 */

using Microsoft.EntityFrameworkCore;
using BibliotecaMetr�polis.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<BibliotecaMetropolisContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
