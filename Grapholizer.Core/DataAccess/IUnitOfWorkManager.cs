using System;

namespace Grapholizer.Core.DataAccess
{
  public interface IUnitOfWorkManager<UnitOfWork>
  {
    UnitOfWork State { get; }
  }
}
