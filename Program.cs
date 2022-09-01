using Microsoft.EntityFrameworkCore;
using WebAPI_Task2.Model;
using WebAPI_Task2.Services;

namespace WebAPI_Task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("DefaltConnection");

            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "WebAPI-Task2", Version = "v1" });
            });
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI-Task2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}