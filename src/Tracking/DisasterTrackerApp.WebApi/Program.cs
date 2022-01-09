using DisasterTrackerApp.BL;
using DisasterTrackerApp.Dal;
using DisasterTrackerApp.BL.Contract;
using Hangfire;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        //this setting will allow to serialize foreign languages
        options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureAppConfiguration(builder.Host);
builder.Services.AddBalDependencies(builder.Configuration);
builder.Services.AddDalDependencies(builder.Configuration);
var app = builder.Build();
app.Services.GetService<IRecurringJobManager>()
    .AddOrUpdate<INewClosedEventsService>("Check for new closed events", 
        job => job.AddNewClosedEvents(CancellationToken.None), Cron.Daily, TimeZoneInfo.Utc);
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

void ConfigureAppConfiguration(ConfigureHostBuilder configureBuilder)
{
    configureBuilder.ConfigureAppConfiguration((_, config) =>
    {
        config.AddJsonFile("client_credentials.json",
            optional: true,
            reloadOnChange: true);
    });
}