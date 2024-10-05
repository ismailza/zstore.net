using DotNetEnv;
using zstore.net.Data;
using zstore.net.Services.Storage;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddScoped<IStorageService>(provider => {
    return builder.Configuration.GetValue<string>("Storage:Type") switch
    {
        "FileSystem" => new FileSystemStorageService(builder.Environment),
        _ => throw new InvalidOperationException("Invalid storage service type"),
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
