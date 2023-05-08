using Finances.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Finances.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Instead of this below, I create AddInfrastructure
//builder.Services.AddDbContext<ExpanseDbContext>(options
//    => options.UseSqlServer(builder.Configuration.GetConnectionString("Expanse") ?? throw new InvalidOperationException("Connection string 'Expanse' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();