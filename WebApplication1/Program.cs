using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Supabase;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var supabaseSection = builder.Configuration.GetSection("SupabaseSetting");
if (!supabaseSection.Exists())
{
    throw new Exception("Секция 'SupabaseSetting' не найдена в конфигурации");
}

var supabaseUrl = supabaseSection["Url"];
var supabaseKey = supabaseSection["Key"];

if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    throw new Exception("Supabase URL или Key не настроены");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Client>>();
    return new Client(supabaseUrl, supabaseKey, new SupabaseOptions
    {
        AutoConnectRealtime = true,
        AutoRefreshToken = true,
    });
});

builder.Services.AddScoped<SupaBaseContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    var supabase = app.Services.GetRequiredService<Client>();
    await supabase.InitializeAsync();
    Console.WriteLine("Supabase успешно инициализирован");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка инициализации Supabase: {ex.Message}");
}

app.Run();
// Добавьте это перед app.Run() для регистрации контроллеров
app.MapControllers();