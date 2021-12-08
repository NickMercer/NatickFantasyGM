using Natick.SharedKernel.Interfaces;
using NatickFantasyGM.Core.PlayerProjections.Aggregates;
using NatickFantasyGM.Core.PlayerProjections.Specifications.Core;

namespace NatickFantasyGM.Core.PlayerProjections.Interfaces;

public interface IPlayerRepository : IRepository<Player>
{
    IReadOnlyList<Player> GetAll();

    IReadOnlyList<Player> GetBySpec(Specification<Player> specification);
}
