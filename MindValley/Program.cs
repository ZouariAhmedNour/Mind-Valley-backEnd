using MindValley.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
        });

        // Add services to the container
        builder.Services.AddControllers();

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure DbContext
        builder.Services.AddDbContext<MindValleyContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MindValleyContextConnection"))
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MindValley API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI at the root
            });
        }

        app.UseHttpsRedirection();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthorization();

        app.MapControllers();

        var url = "https://20.20.17.5:7035";
        app.Run(url);
    }
}