using DisasterTrackerApp.Models.Auth;
using DisasterTrackerApp.Models.Calendar;

namespace DisasterTrackerApp.BL.Implementation;

public class EventsService
{
    
    public void FetchAllEvents (FetchCalendarEventRequest request)
    {
        
    }
   /* public Flux<FetchedCalendarEvent> fetchAllEvents(AuthToken authToken, List<String> scopes, int userId) {
        return Mono.fromCallable(() ->
                    getCalendarApi(credentialFromToken(authToken, scopes))
                .events()
                .list(CALENDAR_ID)
                .setTimeMin(new DateTime(System.currentTimeMillis()))
                .setTimeZone(DEFAULT_TIME_ZONE)
                .setOrderBy("startTime")
                .setSingleEvents(true)
                .execute())
            .subscribeOn(Schedulers.boundedElastic())
            .flatMapMany(events -> buildCalendarEvents(events, userId));
    }*/
}