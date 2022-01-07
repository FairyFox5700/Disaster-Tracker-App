using DisasterTrackerApp.BL;
using DisasterTrackerApp.Dal;
using DisasterTrackerApp.WebApi.HttpClients.Contract;
using DisasterTrackerApp.WebApi.HttpClients.Implementation;
using DisasterTrackerApp.WebApi.Internal;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// add configuration providers
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("client_credentials.json",
        optional: true,
        reloadOnChange: true);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder
    .Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder
        .Configuration.GetConnectionString("DefaultConnection"),
        new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true,
    }));

builder.Services.AddHttpClient<IDisasterEventsClient, DisasterEventsClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["DisasterEventsUrl"]);
    })
    .AddPolicyHandler(PolicyStrategies.GetRetryPolicy())
    .AddPolicyHandler(PolicyStrategies.GetCircuitBreakerPolicy());
// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
builder.Services.AddDalDependencies(builder.Configuration);
builder.Services.AddBlDependencies(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllers();

app.Run();
