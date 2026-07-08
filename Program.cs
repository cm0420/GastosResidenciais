using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Middlewares;
using GastosResidenciais.Api.Repositories;
using GastosResidenciais.Api.Repositories.Interfaces;
using GastosResidenciais.Api.Services;
using GastosResidenciais.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------
// Configuração de serviços (Injeção de Dependência)
// ------------------------------------------------------------------

// Banco de dados: PostgreSQL via Npgsql, string de conexão vinda de appsettings.json
// (sobrescrita em produção por variável de ambiente ConnectionStrings__DefaultConnection).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios (camada de acesso a dados)
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// Services (camada de regras de negócio)
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gastos Residenciais API",
        Version = "v1",
        Description = "API para controle de gastos residenciais: pessoas, transações e totais."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// CORS liberado para o front-end em React (Vite/CRA rodando em localhost).
const string corsPolicyName = "FrontEndPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ------------------------------------------------------------------
// Aplica migrations pendentes automaticamente ao subir a aplicação.
// Prático para desenvolvimento; em ambientes produtivos reais,
// normalmente prefere-se rodar as migrations como um passo de deploy separado.
// ------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// ------------------------------------------------------------------
// Pipeline HTTP
// ------------------------------------------------------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();

app.Run();