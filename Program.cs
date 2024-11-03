using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Services.Storage;
using zstore.net.Services.Cart;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
Env.Load();

// Add services to the container.
// Add the Razor Pages services to the container
builder.Services.AddRazorPages();
// Add the database context to the container
builder.Services.AddDbContext<ZStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZStoreConnection")));
// Add the storage service to the container
builder.Services.AddScoped<IStorageService>(provider =>
{
    return builder.Configuration.GetValue<string>("Storage:Type") switch
    {
        "FileSystem" => new FileSystemStorageService(builder.Environment),
        _ => throw new InvalidOperationException("Invalid storage service type"),
    };
});
// Add the http context accessor to the container
builder.Services.AddHttpContextAccessor();
// Add the session service to the container
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
});
// Add the cart service to the container
builder.Services.AddScoped<ICartService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return configuration.GetValue<string>("Cart:Storage") switch
    {
        "Session" => new SessionCartService(provider.GetRequiredService<IHttpContextAccessor>()),
        "Cookie" => new CookieCartService(provider.GetRequiredService<IHttpContextAccessor>()),
        _ => throw new InvalidOperationException("Invalid cart storage type"),
    };
});
// Configure two authentication schemes
builder.Services.AddAuthentication()
    // Admin authentication scheme
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Auth/Login";
        options.AccessDeniedPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.Cookie.Name = "AdminAuth";
    })
    // Client authentication scheme
    .AddCookie("ClientAuth", options =>
    {
        options.LoginPath = "/Client/Auth/Login";
        options.AccessDeniedPath = "/Client/Auth/Login";
        options.LogoutPath = "/Client/Auth/Logout";
        options.Cookie.Name = "ClientAuth";
    });
// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireRole("Admin")
              .AuthenticationSchemes = ["AdminAuth"]);
              
    options.AddPolicy("ClientOnly", policy => 
        policy.RequireRole("Client")
              .AuthenticationSchemes = ["ClientAuth"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
