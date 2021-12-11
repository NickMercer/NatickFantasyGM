using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate;
using NatickFantasyGM.Core.PlayerProjections.Specifications.Core;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System.Linq.Expressions;

namespace NatickFantasyGM.Core.PlayerProjections.Specifications;

public sealed class PitchingPlayerSpecification : Specification<Player>
{
    public override Expression<Func<Player, bool>> ToExpression()
    {
        return player => player.Evaluation.Projection.Stats.FirstOrDefault(x => x.StatName == new StatName("Innings Pitched", "IP")).Value > 1;
    }
}
