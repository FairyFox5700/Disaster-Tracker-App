using DisasterTrackerApp.Dal;
using DisasterTrackerApp.WebApi.HttpClients.Contract;
using DisasterTrackerApp.WebApi.HttpClients.Implementation;
using DisasterTrackerApp.WebApi.Internal;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.HttpClients.Implementation;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Implementation;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(builder
    .Configuration.GetConnectionString("DisasterTrackerConnection")));


builder.Services.AddHttpClient<IDisasterEventsClient, DisasterEventsClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["DisasterEventsUrl"]);
    })
    .AddPolicyHandler(PolicyStrategies.GetRetryPolicy())
    .AddPolicyHandler(PolicyStrategies.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IClosedDisasterEventsClient, ClosedDisasterEventsClient>(client => 
{
    client.BaseAddress = new Uri("https://eonet.gsfc.nasa.gov/api/v3/events/geojson?days=2000&status=closed");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62");
});
// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
builder.Services.AddScoped<INewClosedEventsService, NewClosedEventsService>();
builder.Services.AddDalDependencies(builder.Configuration);

var app = builder.Build();

app.Services.GetService<IRecurringJobManager>().AddOrUpdate<INewClosedEventsService>("Check for new closed events",job => job.AddNewClosedEvents(), Cron.Daily, TimeZoneInfo.Utc);

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