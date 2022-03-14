using Microsoft.EntityFrameworkCore;
using OraEmp.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using OraEmp.Domain.Entities;
using Xunit;

namespace Infrastructure.UnitTests.Persistence;

public class TestSelectEmp
{
    private DataContext Context { get; }

    public TestSelectEmp(DataContext context)
    {
        Context = context;
    }

    [Fact]
    public void ContextIsNotNull()
    {
        Assert.NotNull(Context);
    }

    [Fact]
    public void SomeCoutriesArePresent()
    {
        var countriesList = Context.Set<Countries>().ToList();
        Assert.True(countriesList.Count > 0);
    }
}