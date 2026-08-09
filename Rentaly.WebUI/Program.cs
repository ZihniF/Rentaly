using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.EntityFramework;
using Rentaly.DtoLayer.RentalDtos;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ICarDal, EfCarDal>();
builder.Services.AddScoped<ICarModelService, CarModelManager>();
builder.Services.AddScoped<ICarModelDal, EfCarModelDal>();
builder.Services.AddScoped<IHomeContentService, HomeContentManager>();
builder.Services.AddScoped<IHomeContentDal, EfHomeContentDal>();

builder.Services.AddScoped<IBranchService, BranchManager>();
builder.Services.AddScoped<IBranchDal, EfBranchDal>();

builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<IBrandDal, EfBrandDal>();

builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<ICustomerDal, EfCustomerDal>();

builder.Services.AddScoped<IValidator<CreateRentalDto>, CreateRentalValidator>();
builder.Services.AddScoped<IValidator<CreateBookingDto>, CreateBookingValidator>();
builder.Services.AddScoped<IValidator<UpdateRentalStatusDto>, UpdateRentalStatusValidator>();
builder.Services.AddScoped<IRentalDal, EfRentalDal>();
builder.Services.AddScoped<IRentalService, RentalManager>();

builder.Services.AddDbContext<RentalyContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();
builder.Services.Configure<Rentaly.WebUI.Services.SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<Rentaly.WebUI.Services.IReservationEmailService, Rentaly.WebUI.Services.ReservationEmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<RentalyContext>();
        await context.Database.MigrateAsync();
        await RentalyDataSeeder.SeedExpandedFleetAsync(context);
    }
    catch (Exception exception)
    {
        logger.LogWarning(exception, "Geliştirme veritabanı migration işlemi uygulanamadı.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapFallbackToController("NotFoundPage", "Home");

app.Run();
