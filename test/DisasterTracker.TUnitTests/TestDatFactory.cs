using System;
using System.Collections.Generic;
using Bogus;
using DisasterTrackerApp.Entities;

namespace DisasterTracker.TUnitTests;

public static class TestDatFactory
{
    private const int _calendarEventsIds = 0;
    public static List<CalendarEvent> GetFakeCaleedarEventsData(int count)
    {
        var testOrders = new Faker<CalendarEvent>()
            .StrictMode(true)
            .RuleFor(o => o.Id, f => Guid.NewGuid())
            .RuleFor(e=>e.Location,f=>f.Address.City())
            .RuleFor(e=>e.Summary, f=>f.Name.JobTitle())
            .RuleFor(e=>e.GoogleEventId, f=>f.Random.String())
            .RuleFor(r=>r.CreatedAt, f=>DateTimeOffset.Now)
            .RuleFor(r=>r.EndTs, f=>f.Date.Past())
            .RuleFor(r=>r.StartedTs, f=>f.Date.Recent())
            .RuleFor(r=>r.UpdatedAt,f=>DateTimeOffset.Now);

        return testOrders.Generate(count);
    }
    
}