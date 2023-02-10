using App.Services;
using Auth0.AspNetCore.Authentication;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((configBuilder) =>
{
    DotEnv.Load();
    configBuilder.Sources.Clear();
    configBuilder
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<IJsonEncoder, JsonEncoder>();

builder.Services.AddAuth0WebAppAuthentication(configureOptions =>
{
    configureOptions.Domain = builder.Configuration.GetValue<string>("AUTH0_DOMAIN", "");
    configureOptions.ClientId = builder.Configuration.GetValue<string>("AUTH0_CLIENT_ID", "");
    configureOptions.ClientSecret = builder.Configuration.GetValue<string>("AUTH0_CLIENT_SECRET", "");
    configureOptions.CallbackPath = builder.Configuration.GetValue<string>("AUTH0_CALLBACK_PATH", "");
    configureOptions.Scope = "openid profile email";
    configureOptions.SkipCookieMiddleware = true;
})
.WithAccessToken(options =>
{
    options.Audience = builder.Configuration.GetValue<string>("AUTH0_AUDIENCE", "");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});

var app = builder.Build();

var requiredVars =
    new string[] {
          "PORT",
          "AUTH0_DOMAIN",
          "AUTH0_CLIENT_ID",
          "AUTH0_CLIENT_SECRET",
          "AUTH0_CALLBACK_PATH",
          "AUTH0_AUDIENCE",
          "API_SERVER_URL"
    };

foreach (var key in requiredVars)
{
    var value = app.Configuration.GetValue<string>(key);

    if (value == "" || value == null)
    {
        throw new Exception($"Config variable missing: {key}.");
    }
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseExceptionHandler("/error");
app.UseStatusCodePagesWithReExecute("/error");

app.MapControllers();
app.Urls.Add($"http://+:{app.Configuration.GetValue<string>("PORT")}");

app.Run();
