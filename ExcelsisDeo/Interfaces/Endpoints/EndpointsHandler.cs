namespace ExcelsisDeo.Interfaces.Endpoints
{
    public interface IRequest { }

    public interface IRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        ValueTask<IResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler
    {
        ValueTask<IResult> HandleAsync(CancellationToken cancellationToken);
    }
}
