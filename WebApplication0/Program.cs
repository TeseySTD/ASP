using Microsoft.EntityFrameworkCore;
using WebApplication0.Data;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<ApplicationContext>(optionsBuilder =>
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=1234567890;database=wab_app_0_asp;",
                                    new MySqlServerVersion(new Version(8, 0, 34))));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error/","?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Initialize();
app.Run();
