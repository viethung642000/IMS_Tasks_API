using BE.Data.Contexts;
using BE.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BE.Helpers;
using BE.Services.Managers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

#region Register Repository
builder.Services.AddScoped<ITasksManager, TasksManager>();
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure DbContext
#region Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbContextSetting:ConnectionString"], b => b.MigrationsAssembly("BE"));
});
#endregion

// Configure Cors
builder.Services.AddCors(otps =>
{
    otps.AddPolicy("AppCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
// Register instance
/* Configure */
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));
/* Add instance */
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<EncryptionHelper>();
// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // Get Jwt Setting
    var jwtSetting = builder.Configuration.GetSection("JwtSetting").Get<JwtSetting>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // default True
        ValidateAudience = false, // default True

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret)),

        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AppCorsPolicy");

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
