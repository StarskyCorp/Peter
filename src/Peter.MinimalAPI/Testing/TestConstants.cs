namespace Peter.MinimalApi.Testing;

public static class TestConstants
{
    internal static class Authentication
    {
        public const string TestScheme = "TestScheme";
        public const string TestAuthType = "TestAuthType";
        public const string HeaderName = $"X-TestServerAuthentication-{TestScheme}";
    }
}