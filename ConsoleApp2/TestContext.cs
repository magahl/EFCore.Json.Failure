using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp2;

public class TestContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
        optionsBuilder.UseSqlServer("Server=(local);Database=Json.Failure;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=True;TrustServerCertificate=true",
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfig());
    }
}

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");
        builder.HasKey(x => x.Id);
        builder.OwnsMany(x => x.Addresses, navigationBuilder =>
        {
            navigationBuilder.ToJson();
        });
    }
}

public class Person
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public virtual List<Address> Addresses { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string Zip { get; set; }
}
