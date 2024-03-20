// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp2;
using Microsoft.EntityFrameworkCore;

var context = new TestContext();
await Seed(context);
var persons = await context.Persons.AsNoTracking().Select(x => new Person
{
    FirstName = x.FirstName,
    Id = x.Id,
    Addresses = x.Addresses.Select(a => new Address
    {
        Street = a.Street,
        Zip = a.Zip
    }).ToList()
}).ToListAsync();

Console.WriteLine(persons.Count);

async Task Seed(TestContext testContext)
{
    await testContext.Persons.AddAsync(new Person()
    {
        FirstName = "Magnus",
        Id = Guid.NewGuid(),
        Addresses =
        [
            new Address()
            {
                Street = "Street",
                Zip = "55555"
            },
            new Address()
            {
                Street = "Street",
                Zip = "44444"
            }
        ]
    });

    await context.SaveChangesAsync();
}