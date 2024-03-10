using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;

using (var db = new SomeDbContext())
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    var user = new User
    {
        FirstName = "Willow",
        LastName = "Vickers",
        Account = new()
    };
    
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
}

// using (var db = new SomeDbContext())
// {
//     var user = await db.Users.SingleAsync();
//     db.Update(user);
//     await db.SaveChangesAsync();
// }

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
    public int? UserId { get; set; }
    public User User { get; set; }
}

public enum Status
{
    None,
    New,
    Active,
    Deactivated,
    Retired
}

public class StatusValueGenerator : ValueGenerator<Status>
{
    public override Status Next(EntityEntry entry) => Status.New;
    public override bool GeneratesTemporaryValues => false;
}

public class SomeDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(
                @"Data Source=localhost;Database=Keys;Integrated Security=True;Trust Server Certificate=True;ConnectRetryCount=0")
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.Property(e => e.Id).UseHiLo();
            //b.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
        });

        modelBuilder.Entity<Account>(b =>
        {
            b.Property(e => e.Id).UseHiLo();
            //b.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
        });
    }
}