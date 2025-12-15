
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using FirebaseAdmin.Messaging;
using Serilog;

namespace EndWire.API
{
    public static class Program
    {
        public static readonly string _namespace = typeof(Program).Namespace;
        private const int DefaultPollingInterval = 1;
        public static void Main(string[] args)
        {
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //var tracePath = Path.Join(path, $"Log_CustomerService_{DateTime.Now.ToString("yyyyMMdd-HHmm")}.txt");
            //Trace.Listeners.Add(new TextWriterTraceListener(tracePath));
            //Trace.AutoFlush = true;

            CreateHostBuider(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuider(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(x => x.AddConfiguration(GetConfiguration()))
            .ConfigureAppConfiguration((context, config) =>
                {
                    var configuration = config.Build();
                    var secretName = configuration["SecretManagerSettings:SecretName"];
                    var pollingInterval = DefaultPollingInterval;
                    int.TryParse(configuration["SecretManagerSettings:SecretPollingInterval"], out int pollingIntervalValue);

                    /*config.AddSecretsManager(configurator: options =>
                    {
                        options.AcceptedSecretArns = new List<string>{secretName};
                        options.PollingInterval = TimeSpan.FromHours(pollingInterval);
                    });*/
                })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .CaptureStartupErrors(true);
                })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                    | ActivityTrackingOptions.TraceId
                    | ActivityTrackingOptions.ParentId;
                });
                //loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
                loggingBuilder.AddFile(hostingContext.Configuration.GetSection("Logging")); // Serilog Logging
                //loggingBuilder.AddFile("Logs/endwire-{Date}.txt");

                //loggingBuilder.AddSerilog();
            });
    

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddJsonFile("storedProcedures.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}





/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/