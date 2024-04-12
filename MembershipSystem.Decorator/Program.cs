

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMemoryCache();

// technique 1 
//builder.Services.AddScoped<IProductRepository>(provider =>
//{
//    var context = provider.GetRequiredService<AppIdentityDbContext>();
//    var memoryCache = provider.GetRequiredService<IMemoryCache>();
//    var logService = provider.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
//    var productRepository = new ProductRepository(context);

//    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
//    var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logService);

//    return logDecorator;
//});

// technique 2
//builder.Services.AddScoped<IProductRepository, ProductRepository>()
//    .Decorate<IProductRepository, ProductRepositoryCacheDecorator>()
//    .Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();


// technique 3
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

    var context = sp.GetRequiredService<AppIdentityDbContext>();
    var memoryCache = sp.GetRequiredService<IMemoryCache>();
    var productRepository = new ProductRepository(context);
    var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

    if (httpContextAccessor.HttpContext.User.Identity.Name == "user1")
    {
        var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
        return cacheDecorator;
    }

    var logDecorator = new ProductRepositoryLoggingDecorator(productRepository, logService);

    return logDecorator;
});

SeedData.AddSeedData(builder);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (a, b) =>
{
    try
    {
        await b();
    }
    catch (Exception ex)
    {

    }
});

app.UseRouting();

app.UseAuthentication(); //Kimlik Do�rulama
app.UseAuthorization(); //Yetkilendirme

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
