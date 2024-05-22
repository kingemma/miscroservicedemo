using Mango.Web.Services;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Below code is for the HttpClient
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("MangoAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponApi"] ?? string.Empty);
});

builder.Services.AddHttpClient<ICouponService, CouponService>();
SD.CouponApiBase = builder.Configuration["ServiceUrls:CouponApi"].TrimEnd('/');
SD.AuthApiBase = builder.Configuration["ServiceUrls:AuthApi"].TrimEnd('/');
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = SD.TokenCookie;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
