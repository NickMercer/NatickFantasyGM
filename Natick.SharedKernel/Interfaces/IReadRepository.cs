using Ardalis.Specification;

namespace Natick.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }
