namespace Peter.MinimalApi.Testing;

public interface IAuthenticatedApiFixtureInitializer
{
    void Initialize(IServiceProvider services);
}