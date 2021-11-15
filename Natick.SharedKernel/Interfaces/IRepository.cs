using Ardalis.Specification;

namespace Natick.SharedKernel.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }
