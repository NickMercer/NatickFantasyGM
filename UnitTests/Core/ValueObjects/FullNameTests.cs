using NatickFantasyGM.Core.ValueObjects;
using System;
using Xunit;

namespace UnitTests.Core.ValueObjects;

public class FullNameTests
{
    [Theory]
    [InlineData("Lol", "NoPlox")]
    [InlineData("Ken", "Griffey Jr.")]
    [InlineData("Archie", "Mercer")]
    [InlineData("Emma", "The Cat, Mercer")]
    public void Constructor_ValidName_SuccessfulCreation(string first, string last)
    {
        var fullName = new FullName(first, last);

        Assert.Equal(first, fullName.FirstName);
        Assert.Equal(last, fullName.LastName);
        Assert.Equal($"{first} {last}", fullName.ToString());
    }

    [Theory]
    [InlineData("", "NoPlox")]
    [InlineData("Ken", "")]
    [InlineData("", "")]
    [InlineData(null, "The Cat, Mercer")]
    [InlineData(null, null)]
    [InlineData("", null)]
    public void Constructor_InvalidName_ThrowsArgumentException(string first, string last)
    {
        void Action() => new FullName(first, last);

        Assert.ThrowsAny<ArgumentException>(Action);
    }
}
