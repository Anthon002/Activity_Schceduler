using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Application.Services.Classes;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Infrastructure.Data;
using Activity_Scheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("Activity_Scheduler.Infrastructure"))
);
builder.Services.AddScoped<IActivityScheduler,Activity_SchedulerService>();
builder.Services.AddScoped<IActivitySchedulerRepository,ActivitySchedulerRepository>();


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
