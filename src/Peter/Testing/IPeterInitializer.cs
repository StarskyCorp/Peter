using System;

namespace Peter.Testing;

public interface IPeterInitializer
{
    void Initialize(IServiceProvider services);
}