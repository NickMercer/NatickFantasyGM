using System.Linq.Expressions;

namespace NatickFantasyGM.Core.PlayerProjections.Specifications.Core;

internal sealed class OrSpecification<T> : Specification<T>
{
    private Specification<T> _left;
    private Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters.Single());
    }
}
