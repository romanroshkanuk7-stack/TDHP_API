using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TDHP_API.Services;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ──────────────────────────────────────────────
            builder.Services.AddDbContext<THDPContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── Identity ──────────────────────────────────────────────
            builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<THDPContext>();

            // ── JWT Authentication ────────────────────────────────────
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            
            // ── CORS (allow Next.js frontend) ─────────────────────────
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                    else
                    {
                        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://thdp.smarthomeroshkajr.cc", "https://mach-thdp-production.up.railway.app")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                });
            });

            // ── Services (DI) ─────────────────────────────────────────
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IWorkshopService, WorkshopService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<IPerformanceCategoryService, PerformanceCategoryService>();
            builder.Services.AddScoped<IPlayService, PlayService>();

            // ── Controllers & Swagger ─────────────────────────────────
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TDHP API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // ── Seed Admin User ───────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var adminUser = userManager.FindByEmailAsync("admin@tdhp.cz").GetAwaiter().GetResult();
                if (adminUser == null)
                {
                    var user = new UserEntity
                    {
                        UserName = "admin",
                        Email = "admin@tdhp.cz",
                        EmailConfirmed = true
                    };
                    var result = userManager.CreateAsync(user, "AdminPassword123!").GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Admin user seeded successfully.");
                    }
                    else
                    {
                        logger.LogError("Failed to seed admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                // ── Seed Performance Categories & Plays & Workshops ──────
                var catService = scope.ServiceProvider.GetRequiredService<IPerformanceCategoryService>();
                var playService = scope.ServiceProvider.GetRequiredService<IPlayService>();
                var workshopService = scope.ServiceProvider.GetRequiredService<IWorkshopService>();
                catService.SeedDefaultCategoriesAsync().GetAwaiter().GetResult();
                playService.SeedDefaultPlaysAsync().GetAwaiter().GetResult();
                workshopService.SeedDefaultWorkshopsAsync().GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
