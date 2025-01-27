using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Server.Extensions;
using URL_Shortener.BLL.Mapper;
using URL_Shortener.DAL;
using URL_Shortener.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<UserAccount, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/api/auth/login";
            //options.AccessDeniedPath = "/Account/AccessDenied"; 
            //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //options.Cookie.SameSite = SameSiteMode.None;
            //options.Cookie.HttpOnly = true; 
            //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy =>
       policy.RequireRole("User", "Admin"));
});

builder.Services.AddRazorPages();

builder.Services.AddResponseCaching();

builder.Services.AddApplication();

var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://localhost:7498")
               //.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
               //.SetIsOriginAllowed(origin => true));
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.MapRazorPages();

app.MapFallbackToFile("/index.html");

app.Run();
