using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagement.Application.Common.Authentication;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Authentication;
using TaskManagement.Infrastructure.Caching;
using TaskManagement.Infrastructure.Identity;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")));
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        services.AddIdentity<ApplicationUser, IdentityRole>()
     .AddEntityFrameworkStores<ApplicationDbContext>()
     .AddDefaultTokenProviders();
        services.AddScoped<ICacheService,
    RedisCacheService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IApplicationDbContext>(
       provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(CreateTaskCommand))));
        services.AddValidatorsFromAssembly(
           Assembly.GetExecutingAssembly());

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration =
                configuration["Redis:ConnectionString"];

            options.InstanceName = "TaskManagement_";
        });

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService,
            CurrentUserService>();
        return services;
    }
}