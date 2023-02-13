using AM.Web.Shortening;

using Microsoft.AspNetCore.Mvc;

var config = new ConfigurationBuilder()
    .AddJsonFile ("appsettings.json");

var builder = WebApplication.CreateBuilder (args);

builder.Services.AddControllers();

const string fileName = "links.json";
var shortener = new InMemoryShortener();
if (File.Exists (fileName))
{
    shortener = InMemoryShortener.GetInstance (fileName);
}
builder.Services.AddSingleton<ILinkShortener> (shortener);

// Add services to the container
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler ("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// app.MapGet ("/go/{shortUrl}", context =>
// {
//     context.Response.Redirect ("https://google.com", false);
//     return Task.CompletedTask;
// });

app.Run();
