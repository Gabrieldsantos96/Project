using System.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Extensions;

namespace Project.Server.Utils;

public class ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var context = MapException(httpContext, exception);

        return await problemDetailsService.TryWriteAsync(context);
    }

    private static ProblemDetailsContext MapException(HttpContext httpContext, Exception exception)
    {
        const string defaultErrorTitle = "Ocorreu um erro";

        var problemDetails = new ProblemDetails
        {
            Title = defaultErrorTitle,
            Instance = httpContext.Request.Path,
        };

        switch (exception)
        {
            case ArgumentException:
            case DomainException:
                problemDetails.Title = defaultErrorTitle;
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = exception.GetType().Name;
                break;

            case FluentValidation.ValidationException validationException:
                problemDetails.Title = "Erro de validação";
                problemDetails.Detail = validationException.Errors.FirstOrDefault()?.ErrorMessage;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = validationException.GetType().Name;
                break;

            case NotFoundException:
                problemDetails.Title = "Recurso não encontrado";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Type = exception.GetType().Name;
                break;

            case SecurityException:
                problemDetails.Title = "Autorização negada";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status403Forbidden;
                problemDetails.Type = exception.GetType().Name;
                break;

            case AuthenticationException:
                problemDetails.Title = "Falha de autenticação";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Type = exception.GetType().Name;
                break;

            case AggregateException aggregateException:
                var innerException = aggregateException.InnerExceptions.FirstOrDefault();
                if (innerException != null)
                {
                    return MapException(httpContext, innerException);
                }
                break;
            case DbUpdateConcurrencyException:
                problemDetails.Title = "Recurso desatualizado";
                problemDetails.Detail = "O registro foi atualizado por outro usuário. Por favor, recarregue os dados e tente novamente.";
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = exception.GetType().Name;
                break;

            case TimeoutException:
                problemDetails.Title = "Tempo limite excedido";
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status408RequestTimeout;
                problemDetails.Type = exception.GetType().Name;
                break;

            case NotImplementedException:
                problemDetails.Title = defaultErrorTitle;
                problemDetails.Status = StatusCodes.Status501NotImplemented;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = exception.GetType().Name;
                break;


            default:
                problemDetails.Title = defaultErrorTitle;
                problemDetails.Detail = "Erro interno";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Type = exception.GetType().Name;
                break;
        }

        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        return new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        };
    }
}