using System.Linq.Expressions;

namespace NatickFantasyGM.Core.PlayerProjections.Specifications.Core;

internal sealed class IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return x => true;
    }
}
