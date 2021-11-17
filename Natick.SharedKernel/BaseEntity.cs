using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natick.SharedKernel;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }

    public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

    public override bool Equals(object obj)
    {
        var other = obj as BaseEntity<TId>;

        if (other == null) 
            return false;

        if (ReferenceEquals(this, other))
            return true;
    
        if(GetType() != other.GetType())
            return false;

        if(Id.Equals(default(TId)) || other.Id.Equals(default(TId)))
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(BaseEntity<TId> a, BaseEntity<TId> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TId> a, BaseEntity<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
