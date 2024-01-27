using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using MediatR;

namespace Discount.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateDiscountCommandHandler));
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddAutoMapper(typeof(Startup));
        services.AddGrpc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<DiscountService>();
            endpoints.MapGet( // Não está funcionando no Mac. Achei essa explicação: https://stackoverflow.com/questions/58649775/can-i-combine-a-grpc-and-webapi-app-into-a-net-core-3-0-in-c
                "/",
                async ctx => 
                    await ctx.Response.WriteAsync("Comunication with gRPC endpoints must be made through a gRPC client.")
            );
        });
    }
}