using App.ScopedService;
using Azure;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using EndWire.Services.ServiceProviders;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

public class TimedTaskService : BackgroundService
{

    //private readonly OrderingBackgroundSettings _settings;

    //private readonly IEventBus _eventBus;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFcmNotification _fcmNotification;
    private readonly ILogger<TimedTaskService> _logger;

    public TimedTaskService(IServiceProvider serviceProvider, IFcmNotification fcmNotification, ILogger<TimedTaskService> logger)
    {
        _serviceProvider = serviceProvider;
        _fcmNotification = fcmNotification;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       // stoppingToken.Register(() => { }
        //    );
        _logger.LogInformation(
           $"{nameof(TimedTaskService)} is running.");

        await DoWorkAsync(stoppingToken);

        //_logger.LogDebug($"GracePeriod background task is stopping.");
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(TimedTaskService)} is working.");

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IScopedProcessingService scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            await scopedProcessingService.DoWorkAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(TimedTaskService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }

}