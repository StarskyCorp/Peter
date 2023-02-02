using System;

namespace Peter.Testing;

public interface IServerFixtureInitializer
{
    void Initialize(IServiceProvider services);
}