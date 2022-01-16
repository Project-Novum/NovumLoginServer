using Microsoft.EntityFrameworkCore;
using NovumLoginServer.DBModels;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllersWithViews();

var redisConfig = builder.Configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>();
builder.Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

InitializeDatabase(app);

app.Run();

static void InitializeDatabase(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

    using var ctx = scope?.ServiceProvider.GetRequiredService<DBContext>();
    ctx?.Database.Migrate();
}