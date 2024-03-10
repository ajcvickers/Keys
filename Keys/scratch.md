```csharp
            b.Property(e => e.Status)
                .ValueGeneratedOnAdd()
                .HasDefaultValue(Status.New);

            b.Property(e => e.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property(e => e.DisplayName)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
```

```csharp
    db.Add(new User
    {
        FirstName = "Willow",
        LastName = "Vickers",
    });

    await db.SaveChangesAsync();
```

```csharp
using (var db = new SomeDbContext())
{
    var user = await db.Users.SingleAsync();
    db.Update(user);
    await db.SaveChangesAsync();
}
```

```csharp
        Status = Status.Deactivated,
        Created = DateTime.UtcNow,
```

```csharp

public class StatusValueGenerator : ValueGenerator<Status>
{
    public override Status Next(EntityEntry entry) => Status.New;
    public override bool GeneratesTemporaryValues => false;
}
```

```csharp
            b.Property(e => e.Status)
                .HasValueGenerator<StatusValueGenerator>();
```

```csharp
  public Guid Id { get; set; }

    Console.WriteLine();
    Console.WriteLine($"Key value before Add: '{user.Id}'");
    Console.WriteLine();

    db.Add(user);

    Console.WriteLine();
    Console.WriteLine($"Instance key value after Add: '{user.Id}'");
    Console.WriteLine($"Tracked key value before Add: '{db.Entry(user).Property(e => e.Id).CurrentValue}'");
    Console.WriteLine();
    
    await db.SaveChangesAsync();

    Console.WriteLine();
    Console.WriteLine($"Key value after SaveChanges: '{user.Id}'");
    Console.WriteLine();



        Id = Guid.NewGuid(),


```

```csharp
using (var db = new SomeDbContext())
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    
    db.Add(new User
    {
        FirstName = "Willow",
        LastName = "Vickers",
    });

    await db.SaveChangesAsync();
}

User user;

using (var db = new SomeDbContext())
{
    user = await db.Users.SingleAsync();
}

user.Account = new Account();

using (var db = new SomeDbContext())
{
    db.Update(user);
    await db.SaveChangesAsync();
}

public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Account Account { get; set; }
}

public class Account
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

```

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using (var db = new SomeDbContext())
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
}

public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string DisplayName { get; set; } = null!;
    public Status Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public enum Status
{
    None,
    New,
    Active,
    Deactivated,
    Retired
}

public class SomeDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
	
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(@"Data Source=localhost;Database=Keys;Integrated Security=True;Trust Server Certificate=True;ConnectRetryCount=0")
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
	
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
        });
    }
}
```