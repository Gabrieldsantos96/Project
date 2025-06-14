using System.Reflection;
using System.Text.Json;

namespace Project.Domain.Models;
public interface IMutationResult
{
    bool Success { get; set; }
    string Message { get; set; }
    int StatusCode { get; set; }
    string? Data { get; }
}
public static class HttpCodes
{
    public const int Status200OK = 200;
    public const int Status201Created = 201;
    public const int Status400BadRequest = 400;
    public const int Status404NotFound = 404;
}
public class MutationResult<T> : IMutationResult where T : class
{
    private int _statusCode;
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    private T? DataJson { get; set; }
    public string Data => DataJson is not null ? JsonSerializer.Serialize(DataJson) : string.Empty!;
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
            .Select(f => (int) f.GetValue(null)!)
            .Contains(statusCode);
    }

    public static MutationResult<T> Ok(string message = "Operação realizada com sucesso", T? data = default)
    {
        return new MutationResult<T>
        {
            Success = true,
            Message = message,
            StatusCode = HttpCodes.Status200OK,
            DataJson = data

        };
    }
    public static MutationResult<T> Created(string message = "Recurso criado", T? data = default)
    {
        return new MutationResult<T>
        {
            Success = true,
            Message = message,
            StatusCode = HttpCodes.Status201Created,
            DataJson = data
        };
    }

    public static MutationResult<T> BadRequest(string message = "Solicitação inválida", T? data = default)
    {
        return new MutationResult<T>
        {
            Success = false,
            Message = message,
            StatusCode = HttpCodes.Status400BadRequest,
            DataJson = data
        };
    }

    public static MutationResult<T> NotFound(string message = "Recurso não encontrado", T? data = default)
    {
        return new MutationResult<T>
        {
            Success = false,
            Message = message,
            StatusCode = HttpCodes.Status404NotFound,
            DataJson = data
        };
    }
}
