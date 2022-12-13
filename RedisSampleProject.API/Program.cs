using Microsoft.EntityFrameworkCore;
using RedisSampleProject.API.Models;
using RedisSampleProject.API.Repository;
using RedisSampleProject.API.Services;
using RedisSampleProject.RedisCache;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository>(serviceProvider =>
{
    AppDbContext appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
    ProductRepository productRepositroy = new ProductRepository(appDbContext);
    RedisService redisService = serviceProvider.GetRequiredService<RedisService>();

    return new ProductRepositoryWithCacheDecorater(productRepositroy, redisService);
});

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
