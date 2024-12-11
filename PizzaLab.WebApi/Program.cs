using PizzaLab.Web.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using PizzaLab.Data;
using PizzaLab.Services.Data.Interfaces;

namespace Pizza.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<PizzaLabDbContext>(opt =>
                opt.UseSqlServer(connectionString));

            builder.Services.AddApplicationServices(typeof(IMenuService));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(setup =>
            {
                setup.AddPolicy("PizzaRestaurantSystem", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://localhost:7255")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseCors("PizzaRestaurantSystem");

            app.Run();
        }
    }
}