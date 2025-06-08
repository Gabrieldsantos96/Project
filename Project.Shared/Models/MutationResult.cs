using System.Reflection;

namespace Project.Shared.Models;
public interface IMutationResult
{
    bool Success { get; set; }
    string Message { get; set; }
    int StatusCode { get; set; }
}
public static class HttpCodes
{
    public const int Status200OK = 200;
    public const int Status201Created = 201;
    public const int Status400BadRequest = 400;
    public const int Status404NotFound = 404;
}
public class MutationResult : IMutationResult
{
    private int _statusCode;
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public int StatusCode
    {
        get => _statusCode;
        set
        {
            if (!IsValidStatusCode(value))
            {
                throw new ArgumentException($"Invalid HTTP status code: {value}. Must be a valid HttpCodes constant.");
            }
            _statusCode = value;
        }
    }

    private static bool IsValidStatusCode(int statusCode)
    {
        return typeof(HttpCodes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(int))
            .Select(f => (int)f.GetValue(null)!)
            .Contains(statusCode);
    }

    public static MutationResult Ok(string message = "Operação realizada com sucesso")
    {
        return new MutationResult
        {
            Success = true,
            Message = message,
            StatusCode = HttpCodes.Status200OK
        };
    }

    public static MutationResult Created(string message = "Recurso criado")
    {
        return new MutationResult
        {
            Success = true,
            Message = message,
            StatusCode = HttpCodes.Status201Created
        };
    }

    public static MutationResult BadRequest(string message = "Solicitação inválida")
    {
        return new MutationResult
        {
            Success = false,
            Message = message,
            StatusCode = HttpCodes.Status400BadRequest
        };
    }

    public static MutationResult NotFound(string message = "Recurso não encontrado")
    {
        return new MutationResult
        {
            Success = false,
            Message = message,
            StatusCode = HttpCodes.Status404NotFound
        };
    }
}