using System.Text;
using AspNetCoreRateLimit;
using Entities.DataTransferObjects;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace WebApi.Extensions
{   
// This class contains extension methods for configuring services in the WebApi project.

    public static class ServicesExtensions
    {


        /**
         * Configures the SQL context for the service.
         * Adds the DbContext to the container with the specified SQL connection string.
         */
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"))
            );

        }

        /**
         * Configures the repository manager for the service.
         * Adds the RepositoryManager to the container as a scoped service.
         */
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
             services.AddScoped<IRepositoryManager, RepositoryManager>();


        /**
         * Configures the service manager for the service.
         * Adds the IServiceManager to the container as a scoped service.
         */
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();



        // Configures the logger service for the service.
        // Adds the ILoggerService to the container as a singleton service.
        public static void ConfigureLoggerServive(this IServiceCollection servives) =>
            
            servives.AddSingleton<ILoggerService, LoggerManager>();


        /**
         * Configures the action filters for the service.
         * Adds the ValidationFilterAttribute and LogFilterAttribute to the container as scoped services.
         */
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>(); // IoC kaydı 
            services.AddSingleton<LogFilterAttribute>(); // IoC kaydı
            services.AddScoped<ValidateMediaTypeAttribute>(); // IoC kaydı
        }


        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => { 
                
                options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination")
                );



            });
        }



        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }



        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();



                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.hateoas+json");

                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();


                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.apiroot+xml");
                }

            });
        }



        // Versiyonlama konfigürasyonu
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true; // varsayılan versiyonu belirler
                opt.DefaultApiVersion = new ApiVersion(1, 0); // varsayılan versiyonu belirler
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); // versiyonu okuyacak header'ı belirler
                opt.Conventions.Controller<BooksController>().HasApiVersion(new ApiVersion(1,0));
                opt.Conventions.Controller<BooksV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
        }

        // Cache konfigürasyonu
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }


        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(exprationOpt =>
            {
                exprationOpt.MaxAge = 90;
                exprationOpt.CacheLocation = CacheLocation.Public;
            },
            validationOpt =>
            {
                validationOpt.MustRevalidate = false;
            }
                );
        }



        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 60,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


        }



        // Identity servis

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
               
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;

                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 6;
               
              
              

                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();


        }


        public static void ConfigureJWT(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                



            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

            });
        }


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(
                    "v1",
                    new OpenApiInfo{
                        Title = "ASP.Net Web Api",
                        Version = "v1",
                        Description = "ASP.Net Web Api",
                        TermsOfService = new Uri("https://www.google.com.tr"),
                        Contact = new OpenApiContact
                        {
                            Name = "Ertuğrul Akkaya",
                            Email = "ertuakkaya@gmail.com",
                            Url = new Uri("https://www.google.com.tr")

                        }
                    
                    });
                s.SwaggerDoc("v2", new OpenApiInfo { Title = "ASP.Net Web Api", Version = "v2" });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"

                });


                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {

                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }


                });
            });
        }





    }
}
