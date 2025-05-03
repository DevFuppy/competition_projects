using EFcoreRepoPractice.Application.Queries;
using EFcoreRepoPractice.Data;
using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//公司
var connectionStringAjaxClass2 = builder.Configuration.GetConnectionString("ajaxClass");
builder.Services.AddDbContext<AjaxClassContext>(options => options.UseSqlServer(connectionStringAjaxClass2));

//在家
var connectionStringAjaxClass = builder.Configuration.GetConnectionString("atHome");
builder.Services.AddDbContext<AjaxClassContext>(options => options.UseSqlServer(connectionStringAjaxClass));


//其實是語法糖
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//通用版
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<GetMemberDetailHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}")
    pattern: "{controller=Member}/{action=GetAll}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
