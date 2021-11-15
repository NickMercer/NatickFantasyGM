using Ardalis.SmartEnum;

namespace NatickFantasyGM.Core.PlayerProjections.Enums;

public sealed class ProjectionSourceEnum : SmartEnum<ProjectionSourceEnum>
{
    public static readonly ProjectionSourceEnum ZiPS = new ProjectionSourceEnum("ZiPS", 1);
    public static readonly ProjectionSourceEnum Steamer = new ProjectionSourceEnum("Steamer", 2);
    public static readonly ProjectionSourceEnum Owner = new ProjectionSourceEnum("Owner", 3);

    public ProjectionSourceEnum(string name, int value) : base(name, value)
    {
    }
}
