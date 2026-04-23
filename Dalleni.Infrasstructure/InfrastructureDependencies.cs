using Dalleni.Application.Commans;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Application.Mappers;
using Dalleni.Application.Services;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.Settings;
using Dalleni.Infrasstructure.Handlers;
using Dalleni.Infrastructure.ExternalServices;
using Dalleni.Infrastructure.ExternalServices.FilesUploader;
using Dalleni.Infrastructure.Persisitanse;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Dalleni.Infrasstructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {

           // ---------- Database & Identity ----------
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("Local")
                    ?? throw new InvalidOperationException("Connection string 'Local' is not configured.");

                options.UseSqlServer(connectionString);
            });

            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            // ---------- Repositories & Services ----------

            //services.Scan(scan => scan
            //    .FromAssembliesOf< QuestionRepository>()
            //    .AddClasses()
            //    .AsMatchingInterfaces()
            //    .WithScopedLifetime());
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IQuestionTagRepository, QuestionTagRepository>();
            services.AddScoped<IOfficialEntityRepository, OfficialEntityRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ISavedQuestionsRepository, SavedQuestionRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            services.AddScoped<IExternalLoginRepository, ExternalLoginRepository>();
            services.AddScoped<IUserManager<ApplicationUser>, UserManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWork<ApplicationUser>, UnitOfWork>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            //services.Scan(scan => scan
            //    .FromAssembliesOf<TokenService>()
            //    .AddClasses()
            //    .AsMatchingInterfaces()
            //    .WithScopedLifetime());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<AzureBlobImageUploaderService>();
            services.AddScoped<CloudinaryImageUploaderService>();

            services.AddScoped<IImageUploaderServiceFactory, ImageUploaderServiceFactory>();

           services.AddScoped<IImageUploaderService>(sp =>
            {
                var factory = sp.GetRequiredService<IImageUploaderServiceFactory>();
                return factory.Create();
            });

            // ---------- Settings ----------
            services.Configure<CloudinarySettings>(configuration.GetSection("cloudinary"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<AzureSearchSettings>(configuration.GetSection("SearchSettings"));


            // ---------- Authentication ----------
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                        var securityStamp = context.Principal?.FindFirst("security_stamp")?.Value;

                        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(securityStamp))
                        {
                            context.Fail("Invalid token claims.");
                            return;
                        }

                        var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                        var user = await dbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

                        if (user == null)
                        {
                            context.Fail("User not found.");
                            return;
                        }

                        if (!string.Equals(user.SecurityStamp, securityStamp, StringComparison.Ordinal))
                        {
                            context.Fail("Security stamp mismatch - token revoked.");
                        }
                    }
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });

            // ---------- AutoMapper ----------
            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
