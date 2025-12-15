using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Dapper.FluentMap;
using Microsoft.EntityFrameworkCore;
using EndWire.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;
using EndWire.Infrastructure;
using EndWire.Services.ServiceProviders;
using EndWire.Infrastructure.Repositories.Mobile;
using EndWire.Infrastructure.DapperEntityMaps;
using EndWire.API.AutoMapperProfiles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EndWire.Domain.ConfigurationOptions;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Configuration;
using App.ScopedService;
using Microsoft.AspNetCore.HttpLogging;

namespace EndWire.API
{
    public class Startup
    {
        private readonly string _endwireOrigins = "_endwireOrigins";
        public IConfiguration Configuration { get; }    
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            app.UseForwardedHeaders();

            if(env.IsDevelopment()) 
            {
                // 02/18/23
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EndWire V1");
                });
                // 02/18/23

                app.UseDeveloperExceptionPage();    
            }
            
            loggerFactory.AddFile(Configuration.GetSection("Logging:LogFilePath").ToString());

            //app.UseCookiePolicy();
            app.UseRouting();

            app.UseHttpLogging();

            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseCors(_endwireOrigins);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });

        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCustomDbContext(Configuration);

            services.AddMvcCore(config =>
            {
                //config.Filters.Add(new AuthorizationFilter());
                //config.Filters.Add(LogEnrichmentFilter());
                config.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            }
            ).AddApiExplorer();

            services.Configure
                <CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context=>true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;  

            });

            // 02/18/23
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EndWire API", Version = "v1" });
            });
            //services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddMemoryCache();

            //Log.Logger = new LoggerConfiguration().ReadFrom.

            //services.AddLogging(configuration=>configuration.AddSerilog());
            //services.AddSingleton(Log.Logger);

            //services.AddControllers(cfg =>
            //{
            //cfg.ReturnHttpNotAcceptable = true;
            //})
            //.AddXmlDataContractSerializerFormatters()

            services.AddHttpLogging(logging =>
            {
                // Customize HTTP logging here.
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("my-response-header");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });

            // 02/18/23

            //services.AddAutoMapper(this.GetType().Assembly, typeof(CommunicationDataMappingProfile).Assembly);
            services.AddAutoMapper(typeof(Startup));
            //services.AddAutoMapper(config => config.AddProfile(typeof(NoteMappingProfile)));
            services.AddControllers(options =>
            {
                //options.ModelBinderProviders.Insert(0, new CommunicationsBinderProvider());
                //options.ModelBinderProviders.Insert(0, new CommunicationsBinderProvider());
                //options.ModelBinderProviders.Insert(0, new CommunicationsBinderProvider());
            });

            services.Configure<ForwardedHeadersOptions>(options=>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            //services.AddScoped<EndWireContext>();

            services.AddScoped<ILoginProvider, LoginProvider>();
            services.AddScoped<ILoginRepository, LoginRepository>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<INudgeProvider, NudgeProvider>();
            services.AddScoped<INudgeRepository, NudgeRepository>();

            services.AddScoped<IReminderProvider, ReminderProvider>();
            services.AddScoped<IReminderRepository, ReminderRepository>();

            services.AddScoped<IResourceProvider, ResourceProvider>();
            services.AddScoped<IResourceRepository, ResourceRepository>();

            services.AddSingleton<IFcmNotification, FcmNotification>();

            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITimerRepsitory, TimerRepsitory>();
            services.AddScoped<IRtcCallRepository, RtcCallRepository>();

            services.AddScoped<IRtcServiceProvider, RtcServiceProvider>(); 

            services.AddScoped<IBroadcastProvider, BroadcastProvider>(); 
            services.AddScoped<IBroadcastRepository, BroadcastRepository>();

            services.AddOptions();
            services.Configure<StoredProcedures>(storedProcedures => Configuration.GetSection("StoredProcedures").Bind(storedProcedures));

            //services.AddSingleton<IHostedService, TimedTaskService>();
            services.AddHostedService<TimedTaskService>();
            services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores= true;

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new NoteMap());
            });

            //services.AddApplicationInsighsTelemetry();
        }

    }

    public static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services//.AddEntityFrameworkSqlServer()
                .AddDbContext<EndWireContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionString:endwire"], sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                });
            return services;
        }
    }
}
