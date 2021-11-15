﻿using Ardalis.GuardClauses;
using Natick.SharedKernel;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

public class StatIdentifier : ValueObject
{
    public string Name { get; }

    public string Abbreviation { get; }

    public StatIdentifier(string name, string abbreviation)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        Abbreviation = Guard.Against.NullOrWhiteSpace(abbreviation, nameof(Abbreviation));
    }

    public override string ToString()
    {
        return $"{Name} - {Abbreviation}";
    }
}
