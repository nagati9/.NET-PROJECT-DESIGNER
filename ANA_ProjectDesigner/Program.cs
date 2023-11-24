using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Services.Implementations;
using ANA_ProjectDesigner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();


builder.Services.AddDbContext<MyDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnectionString")));

builder.Services.AddScoped<IProjectsService, ProjectsService>();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Profil}/{action=Login}/{id?}");

app.Run();
