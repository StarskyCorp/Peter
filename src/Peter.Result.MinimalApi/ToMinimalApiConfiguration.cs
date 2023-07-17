using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace Peter.Result.MinimalApi;

public class ToMinimalApiConfiguration : ICloneable
{
    private List<Type> _nullTypes = new() { typeof(Void) };

    private ConcurrentDictionary<Type, Func<object, IResult>> _customHandlers = new();

    public void AddNullType(Type type)
    {
        _nullTypes.Add(type);
    }

    internal bool ExistsNullType(Type? type) => type is not null && _nullTypes.Contains(type);

    public void RegisterCustomHandler(Type type, Func<object, IResult> handler)
    {
        _customHandlers[type] = handler;
    }

    internal Func<object, IResult>? GetCustomHandler(Type type) =>
        _customHandlers.TryGetValue(type, out var handler) ? handler : null;

    public object Clone()
    {
        return new ToMinimalApiConfiguration
        {
            _nullTypes = new List<Type>(_nullTypes),
            _customHandlers = new ConcurrentDictionary<Type, Func<object, IResult>>(_customHandlers)
        };
    }
}