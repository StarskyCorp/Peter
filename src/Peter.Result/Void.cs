namespace Peter.Result;

/// <summary>
///     Void value
/// </summary>
/// <remarks>
///     https://en.wikipedia.org/wiki/Unit_type
///     https://github.com/dotnet/reactive/blob/main/Rx.NET/Source/src/System.Reactive/Unit.cs
///     https://github.com/jbogard/MediatR/blob/master/src/MediatR.Contracts/Unit.cs
/// </remarks>
public readonly struct Void : IEquatable<Void>
{
    public bool Equals(Void other) => true;

    public override bool Equals(object? obj) => obj is Void;

    public override int GetHashCode() => 0;

    public override string ToString() => "()";

    public static bool operator ==(Void first, Void second) => true;

    public static bool operator !=(Void first, Void second) => false;

    public static Void Default => default;
}