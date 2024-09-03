using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration is already added by default, but we can explicitly add it if needed
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Get Firebase configuration from appsettings.json
var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];
var firebaseCredentialsPath = builder.Configuration["Firebase:CredentialsPath"];

// If CredentialsPath is not set in appsettings.json, use the default path
if (string.IsNullOrEmpty(firebaseCredentialsPath))
{
    firebaseCredentialsPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "firebase-credentials.json");
    Console.WriteLine($"Using Firebase credentials at: {firebaseCredentialsPath}");

}

// Initialize Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebaseCredentialsPath),
    ProjectId = firebaseProjectId
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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