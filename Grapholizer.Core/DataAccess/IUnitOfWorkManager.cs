using System;

namespace Grapholizer.Core.DataAccess
{
  public interface IUnitOfWorkManager<UnitOfWork> : IDisposable
  {
    UnitOfWork State { get; }
  }
}
