using Microsoft.EntityFrameworkCore;
using RedisSampleProject.API.Models;
using RedisSampleProject.API.Repository;
using RedisSampleProject.RedisCache;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseInMemoryDatabase("mydatabase");

});

builder.Services.AddSingleton<RedisService>(serviceprovider =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});
builder.Services.AddSingleton<IDatabase>(serviceprovider =>
{
    RedisService redisService = serviceprovider.GetRequiredService<RedisService>();
    return redisService.Getdb(0);
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      
    dbcontext.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
