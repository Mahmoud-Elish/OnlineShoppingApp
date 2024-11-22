using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.BL;
using Shopping.DAL;
using Shopping.Interfaces;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Default Ser
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion


#region ConnectionString

builder.Services.AddDbContext<AppDbContext>(options =>
    options.
    UseLazyLoadingProxies().
    UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),

          sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));
#endregion

#region DI
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<ICacheService, RedisCacheService>();
#endregion

#region Redis
builder.Services.AddRedisCache(builder.Configuration);
#endregion


#region ASP Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;

    options.User.RequireUniqueEmail = true;

    //options.Lockout.MaxFailedAccessAttempts = 3;
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
})
.AddEntityFrameworkStores<AppDbContext>();
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "default";
    options.DefaultChallengeScheme = "default";
})
.AddJwtBearer("default",options =>
{
    var secretKey = builder.Configuration.GetValue<string>("Jwt:SecretKey");
    var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKey);
    var key = new SymmetricSecurityKey(secretKeyInBytes);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = key
    };
});
#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));

});
#endregion

var app = builder.Build();

#region Middlewares
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion

#region Roles
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var roles = new[] { "Admin", "Customer" };

//    foreach (var role in roles)
//    {
//        if (!roleManager.RoleExistsAsync(role).Result)
//        {
//            roleManager.CreateAsync(new IdentityRole(role)).Wait();
//        }
//    }
//}
#endregion

app.Run();
