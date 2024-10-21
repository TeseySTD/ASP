using System.Text;
using Library.Data;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("CourseworkProto1IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'CourseworkProto1IdentityDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryContext>(optionsBuilder =>
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=1234567890;database=CourseWorkProto1;",
                                new MySqlServerVersion(new Version(8, 0, 34))));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtService.key))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access-cookie"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Owner", policy => policy.AddRequirements(
        new PermissionRequirement(Permission.Create, Permission.Read, Permission.Update, Permission.Delete)));
    options.AddPolicy("Administrator", policy => policy.AddRequirements(new PermissionRequirement( Permission.Delete)));
});
DependencyService.InjectDependencies(builder.Services);

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<LibraryContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCookiePolicy(new CookiePolicyOptions{
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// DBInitializer.Initialize();

app.Run();
