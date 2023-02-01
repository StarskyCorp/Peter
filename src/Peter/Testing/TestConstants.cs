namespace Peter.Testing;

public static class TestConstants
{
    internal static class Authentication
    {
        public const string TestScheme = "TestScheme";
        public const string TestAuthType = "TestAuthType";
        public static string HeaderName => $"X-TestServerAuthentication-{TestScheme}";
    }
}