using ValidationException = Domain.Exceptions.ValidationException;

namespace Application.PipelineBehaviors;

public class ExceptionHandlingPipelineBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        try
        {
            return await next(cancellationToken);
        }
        catch (ValidationException ex)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogError("validation error for {RequestName} with {@Error}", requestName, ex);
            throw new ValidationException(ex.Errors);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogError("Unhandled exception for {RequestName} with {@Error}", requestName, ex);
            throw new(ex.Message);
        }
    }
}