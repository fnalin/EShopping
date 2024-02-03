using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumers;
using Ordering.Application;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationServices();
        services.AddInfraServices(Configuration);
        services.AddAutoMapper(typeof(Startup));
        services.AddSwaggerGen((c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo(){Title = "Ordering.API", Version = "v1"});
        }));
        services.AddHealthChecks().Services.AddDbContext<OrderContext>();
        services.AddScoped<BasketOrderingConsumer>();
        services.AddMassTransit(config =>
        {
            config.AddConsumer<BasketOrderingConsumer>();
            config.UsingRabbitMq((ctx, cfg)=>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                {
                    c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
                });
            });
        });
        services.AddMassTransitHostedService();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI((c=>c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1")));
        }

        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _=>true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}