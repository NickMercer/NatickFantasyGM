using Ardalis.GuardClauses;
using Natick.SharedKernel;
using Natick.SharedKernel.Interfaces;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;
using NatickFantasyGM.Core.ValueObjects;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate;

public class Player : BaseEntity<int>, IAggregateRoot
{
    public FullName FullName { get; private set; }
    public Evaluation Evaluation { get; private set; }

    public Player(int id, FullName name, Evaluation evaluation)
    {
        Id = Guard.Against.Default(id, nameof(id));
        FullName = name;
        Evaluation = evaluation;
    }
}
