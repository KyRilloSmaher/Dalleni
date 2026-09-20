using Dalleni.Application.Commans;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Application.Mappers;
using Dalleni.Application.Services;
using Dalleni.Application.Services.BackgroundJobs;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.Settings;
using Dalleni.Infrasstructure.Handlers;
using Dalleni.Infrastructure.Commans;
using Dalleni.Infrastructure.ExternalServices;
using Dalleni.Infrastructure.ExternalServices.FilesUploader;
using Dalleni.Infrastructure.Persisitanse;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.Infrastructure.Services.BackgroundJobs;
using Dalleni.Infrastructure.Services.Notifications;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
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
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IQuestionTagRepository, QuestionTagRepository>();
            services.AddScoped<IOfficialEntityRepository, OfficialEntityRepository>();
            services.AddScoped<IOfficialEntityMembershipRepository,OfficialEntityMembershipRepository>();
            services.AddScoped<IOfficialEntityInvitationRepository ,OfficialEntityInvitationRepository>();
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
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            //services.Scan(scan => scan
            //    .FromAssembliesOf<TokenService>()
            //    .AddClasses()
            //    .AsMatchingInterfaces()
            //    .WithScopedLifetime());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IInvitationTokenGeneratorService , InvitationTokenGeneratorService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<AzureBlobImageUploaderService>();
            services.AddScoped<CloudinaryImageUploaderService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationPushService,FirebaseNotificationPushService>();
            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddScoped<INotificationDeliveryJob,NotificationDeliveryJob>();
            //services.AddScoped<INotificationRealtimeService,NotificationRealtimeService>();
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
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken =
                            context.Request.Query["access_token"];

                        var path =
                            context.HttpContext.Request.Path;

                        // SignalR sends the JWT through the access_token
                        // query string when establishing the connection.
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs/notifications"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = async context =>
                    {
                        var userId =
                            context.Principal?
                                .FindFirstValue(ClaimTypes.NameIdentifier);

                        var securityStamp =
                            context.Principal?
                                .FindFirst("security_stamp")?
                                .Value;

                        if (string.IsNullOrEmpty(userId) ||
                            string.IsNullOrEmpty(securityStamp))
                        {
                            context.Fail("Invalid token claims.");
                            return;
                        }

                        if (!Guid.TryParse(userId, out var parsedUserId))
                        {
                            context.Fail("Invalid user ID.");
                            return;
                        }

                        var dbContext =
                            context.HttpContext.RequestServices
                                .GetRequiredService<ApplicationDbContext>();

                        var user = await dbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(
                                u => u.Id == parsedUserId);

                        if (user == null)
                        {
                            context.Fail("User not found.");
                            return;
                        }

                        if (!string.Equals(
                                user.SecurityStamp,
                                securityStamp,
                                StringComparison.Ordinal))
                        {
                            context.Fail(
                                "Security stamp mismatch - token revoked.");
                        }
                    }
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId =
                    configuration["Authentication:Google:ClientId"];

                options.ClientSecret =
                    configuration["Authentication:Google:ClientSecret"];

                options.CallbackPath = "/signin-google";
            });

            // ---------- AutoMapper ----------
            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());


            // ------------ Firebase --------------
              var firebaseOptions =
                    configuration
                        .GetSection(FirebaseOptions.SectionName)
                        .Get<FirebaseOptions>();

                if (firebaseOptions is null ||
                    string.IsNullOrWhiteSpace(firebaseOptions.ServiceAccountPath))
                {
                    throw new InvalidOperationException(
                        "Firebase configuration is missing.");
                }
                var serviceAccountPath =Path.Combine(AppContext.BaseDirectory,firebaseOptions.ServiceAccountPath);

                if (!File.Exists(serviceAccountPath))
                {
                    throw new FileNotFoundException(
                        "Firebase service account file was not found.",
                        serviceAccountPath);
                }

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseOptions.ServiceAccountPath)
                });

                
            return services;
        }
    }
}
