// Program.cs (minimal working integration)
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add MVC support
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles(); // for wwwroot

app.UseRouting();

app.UseAuthorization();

// Serve Razor views
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Don't map fallback to Angular unless routing demands it
// app.MapFallbackToFile("index.html"); <-- REMOVE this for combined view

app.Run();
