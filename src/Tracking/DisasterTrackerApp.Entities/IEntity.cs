namespace DisasterTrackerApp.Entities;

public interface IKeyEntity<TKey>
{
    public TKey Id { get; }
}

public interface IAuditable
{
    DateTimeOffset CreatedAt { set; get; }
    DateTimeOffset UpdatedAt { set; get; }
}