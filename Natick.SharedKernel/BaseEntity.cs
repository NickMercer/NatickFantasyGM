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
}
