using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Application.Services.Classes;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Infrastructure.Data;
using Activity_Scheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hangfire;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("Activity_Scheduler.Infrastructure"))
);
builder.Services.AddScoped<IActivityScheduler,Activity_SchedulerService>();
builder.Services.AddScoped<IActivitySchedulerRepository,ActivitySchedulerRepository>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire((sp, config)=>{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ActivityScheduler}/{action=Index}/{id?}");

app.Run();
