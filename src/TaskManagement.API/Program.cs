using Asp.Versioning;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using TaskManagement.API.Middlewares;
using TaskManagement.Application.Common.Authentication;
using TaskManagement.Application.Common.Dtos.Validators.Auth;
using TaskManagement.Application.Common.Responses;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Identity;
using TaskManagement.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);


var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>();
// Add services to the container.
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.SaveToken = true;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings!.Secret)),

                ValidateIssuer = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,

                ValidAudience = jwtSettings.Audience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero,

                RoleClaimType = ClaimTypes.Role
            };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "this is API Task Management application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Yousef Mohamed",
            Email = "youaefmahmed481@gmail.com",
            Url = new Uri("https://yourwebsite")
        }
    });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme.",

            Name = "Authorization",

            In = ParameterLocation.Header,

            Type = SecuritySchemeType.ApiKey,

            Scheme = "Bearer"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion =
        new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified =
        true;

    options.ReportApiVersions = true;

    options.ApiVersionReader =
        new UrlSegmentApiVersionReader();
});


builder.Services.AddAuthorization();
builder.Services.AddControllers().AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<
        LoginRequestDtoValidator>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response =
            ApiResponse<string>.FailureResponse(
                "Validation Failed",
                errors);

        return new BadRequestObjectResult(response);
    };
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext =
    services.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedAsync(roleManager);

    await UserSeeder.SeedAsync(userManager);
}
app.UseGlobalExceptionHandling();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}
app.Run();
