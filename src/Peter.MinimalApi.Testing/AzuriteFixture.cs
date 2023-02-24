using System.Text;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

public class AzuriteFixture : IAsyncLifetime
{
    private IContainer? _azurite;
    public string ConnectionString = null!;
    private ushort _blobEndpointPort;
    private string _accountName = null!;
    private string _destination = null!;

    public async Task InitializeAsync()
    {
        _accountName = $"azurite_{Guid.NewGuid()}";
        _destination =
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), _accountName));
        DeleteDirectory(_destination);
        Directory.CreateDirectory(_destination);

        var accountKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(_accountName));

        const int blobEndpointPort = 10000;
        _azurite = new ContainerBuilder()
            .WithName(_accountName)
            .WithImage("mcr.microsoft.com/azure-storage/azurite")
            .WithPortBinding(blobEndpointPort, assignRandomHostPort: true)
            .WithPortBinding(10001, assignRandomHostPort: true)
            .WithPortBinding(10002, assignRandomHostPort: true)
            .WithEnvironment(new Dictionary<string, string> { { "AZURITE_ACCOUNTS", $"{_accountName}:{accountKey}" } })
            .WithBindMount(_destination, "/data")
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(blobEndpointPort))
            .Build();

        await _azurite.StartAsync();

        _blobEndpointPort = _azurite.GetMappedPublicPort(blobEndpointPort);
        var queueEndpointPort = _azurite.GetMappedPublicPort(10001);
        var tableEndpointPort = _azurite.GetMappedPublicPort(10002);
        ConnectionString =
            @$"DefaultEndpointsProtocol=http;AccountName={_accountName};AccountKey={accountKey};BlobEndpoint=http://127.0.0.1:{_blobEndpointPort}/{_accountName};QueueEndpoint=http://127.0.0.1:{queueEndpointPort}/{_accountName};TableEndpoint=http://127.0.0.1:{tableEndpointPort}/{_accountName};";
    }

    public string BlobStorageUrl(string blobContainerName, string blobName) =>
        $"http://127.0.0.1:{_blobEndpointPort}/{_accountName}/{blobContainerName}/{blobName}";

    public async Task DisposeAsync()
    {
        if (_azurite is not null)
        {
            await _azurite.StopAsync();
        }

        DeleteDirectory(_destination);
    }

    private static void DeleteDirectory(string path)
    {
        try
        {
            if (Path.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
            //Ignore errors in CI/CD
        }
    }
}