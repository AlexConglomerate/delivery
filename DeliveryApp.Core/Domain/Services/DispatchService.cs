using Primitives;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class DispatchService
{
    [ExcludeFromCodeCoverage]
    private DispatchService()
    {
    }

    private DispatchService(int id) : this()
    {
    }

    public void Dispatch(IUnitOfWork unitOfWork)
    {
    }
}