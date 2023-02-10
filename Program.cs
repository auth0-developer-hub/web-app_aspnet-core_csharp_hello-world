using App.Services;
using dotenv.net;

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

var app = builder.Build();

var requiredVars =
    new string[] {
          "PORT",
    };

foreach (var key in requiredVars)
{
    var value = app.Configuration.GetValue<string>(key);

    if (value == "" || value == null)
    {
        throw new Exception($"Config variable missing: {key}.");
    }
}

app.UseStaticFiles();
app.UseExceptionHandler("/error");
app.UseStatusCodePagesWithReExecute("/error");

app.MapControllers();
app.Urls.Add($"http://+:{app.Configuration.GetValue<string>("PORT")}");

app.Run();
