using Ardalis.GuardClauses;
using Natick.SharedKernel;

namespace NatickFantasyGM.Core.ValueObjects;

public class FullName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public FullName(string firstName, string lastName)
    {
        FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(FirstName), "Full name requires first name.");
        LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(LastName), "Full name requires last name.");
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
