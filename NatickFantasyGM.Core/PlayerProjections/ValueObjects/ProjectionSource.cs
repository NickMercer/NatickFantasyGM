using Ardalis.GuardClauses;
using Natick.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class ProjectionSource : ValueObject<ProjectionSource>
{
    public static readonly ProjectionSource ZiPS = new ProjectionSource(1, "ZiPS");
    public static readonly ProjectionSource Steamer = new ProjectionSource(2, "Steamer");
    public static readonly ProjectionSource Owner = new ProjectionSource(3, "Owner");

    public int SourceId { get; }

    public string SourceName { get; set; }
    public static IReadOnlyCollection<ProjectionSource> ThirdPartySources => new List<ProjectionSource> 
    { 
        ZiPS, 
        Steamer 
    }.AsReadOnly();

    public static IReadOnlyCollection<ProjectionSource> Sources => new List<ProjectionSource>
    {
        ZiPS,
        Steamer,
        Owner
    }.AsReadOnly();

    public ProjectionSource(int sourceId, string sourceName)
    {
        SourceId = Guard.Against.NegativeOrZero(sourceId, nameof(SourceId));
        SourceName = Guard.Against.NullOrWhiteSpace(sourceName, nameof(SourceName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SourceId;
        yield return SourceName;
    }
}
