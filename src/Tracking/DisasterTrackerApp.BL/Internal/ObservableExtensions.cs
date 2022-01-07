using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace DisasterTrackerApp.BL.Internal;

public static class ObservableExtensions
{
    public static IObservable<T> RetryWithBackoffStrategy<T>(
        this IObservable<T> source, 
        int retryCount = 3,
        Func<Exception, bool> retryOnError = null,
        IScheduler scheduler = null)
    {
        scheduler ??= Scheduler.Default;

        if (retryOnError == null)
            retryOnError = e => true;

        int attempt = 0;

        return Observable.Defer(() =>
            {
                return ((++attempt == 1) ? source : source.DelaySubscription(TimeSpan.FromSeconds(Math.Pow((attempt-1), 2)), scheduler))
                    .Select(item => new Tuple<bool, T, Exception>(true, item, null))
                    .Catch<Tuple<bool, T, Exception>, Exception>(e => retryOnError(e)
                        ? Observable.Throw<Tuple<bool, T, Exception>>(e)
                        : Observable.Return(new Tuple<bool, T, Exception>(false, default(T), e)));
            })
            .Retry(retryCount)
            .SelectMany(t => t.Item1
                ? Observable.Return(t.Item2)
                : Observable.Throw<T>(t.Item3));
    }

    public static IObservable<Timestamped<T>> LogWithThread<T>(
        this IObservable<T> observable, 
        string msg = "")
    {
        return Observable.Defer(() =>
        {
            Console.WriteLine("{0} Subscription happened on Thread: {1}", msg,
                Thread.CurrentThread.ManagedThreadId);
            return observable.Timestamp().Do(
                x => Console.WriteLine("{0} - OnNext({1}) Thread: {2}", msg, x,
                    Thread.CurrentThread.ManagedThreadId),
                ex =>
                {
                    Console.WriteLine("{0} – OnError Thread:{1}", msg,
                        Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("\t {0}", ex);
                },
                () => Console.WriteLine("{0} - OnCompleted() Thread {1}", msg,
                    Thread.CurrentThread.ManagedThreadId))
                ;
        });
    }
}