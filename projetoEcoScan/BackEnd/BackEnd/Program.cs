using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Configuração do HttpClientFactory
builder.Services.AddHttpClient();

// Adicionar serviços ao container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactNativeApp",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar Migrations Automaticamente e Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
            Console.WriteLine("Database migrations applied.");
        }
        else
        {
            Console.WriteLine("Database is up to date. No migrations to apply.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

// Configure o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    // app.UseExceptionHandler("/Error");
    // app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowReactNativeApp");

app.UseAuthorization();

app.MapControllers();

app.Urls.Clear();
app.Urls.Add("http://localhost:5291");
app.Urls.Add("http://IP DA MAQUINA:5291"); // Substitua pelo seu IP de desenvolvimento na rede local

app.Run();
