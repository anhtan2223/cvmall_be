using Application.Core.Extensions;
using Microsoft.OpenApi.Models;
using WebAPI.Extensions;
using System.Reflection;
using Application.Common.Extensions;
using Infrastructure;
using Infrastructure.Contracts;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var _services = builder.Services;
var _configuration = builder.Configuration;

// Add services to the container.
_services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
_services.AddEndpointsApiExplorer();
_services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "DMS api",
        Description = "DMS api documents",
        TermsOfService = new Uri("https://example.com/terms")

    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
    ;

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var MyAllowSpecificOrigins = "_myAllowOrigins";

// Register core extention: AutoMapper, FluentValidation
_services.Configure<AuthSetting>(_configuration.GetSection("AuthSetting"));
_services.AddHttpContextAccessor();
_services.AddDatabase(_configuration);
_services.AddCoreExtention();
_services.AddInfrastructureServices(_configuration);
_services.AddServiceContext(_configuration);
_services.AddCoreService(_configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.ConfigureCoreDb();
app.UseStaticFiles();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();

