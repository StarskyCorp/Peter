namespace Peter.MinimalApi.Testing;

public interface IServerFixtureInitializer
{
    void Initialize(IServiceProvider services);
}