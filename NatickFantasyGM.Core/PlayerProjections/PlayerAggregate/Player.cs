using Ardalis.GuardClauses;
using Natick.SharedKernel;
using Natick.SharedKernel.Interfaces;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;
using NatickFantasyGM.Core.ValueObjects;

namespace NatickFantasyGM.Core.PlayerProjections.Aggregates;

public class Player : BaseEntity<Guid>, IAggregateRoot
{
    public FullName FullName { get; private set; }
    public Evaluation Evaluation { get; private set; }

    public Player(Guid id, FullName name, Evaluation evaluation)
    {
        Id = Guard.Against.Default(id, nameof(id));
        FullName = name;
        Evaluation = evaluation;
    }
}
