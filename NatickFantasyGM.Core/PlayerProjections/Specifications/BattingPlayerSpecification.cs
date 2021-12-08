using NatickFantasyGM.Core.PlayerProjections.Aggregates;
using NatickFantasyGM.Core.PlayerProjections.Specifications.Core;
using System.Linq.Expressions;

namespace NatickFantasyGM.Core.PlayerProjections.Specifications;

public sealed class BattingPlayerSpecification : Specification<Player>
{
    public override Expression<Func<Player, bool>> ToExpression()
    {
        return player => player.Evaluation.Projection.Stats.FirstOrDefault(x => x.StatName == new ValueObjects.StatName("At Bats", "AB")).Value > 1;
    }
}
