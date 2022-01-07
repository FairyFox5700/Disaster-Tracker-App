using DisasterTrackerApp.BL;
using DisasterTrackerApp.Dal;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.HttpClients.Implementation;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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