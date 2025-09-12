using System.Threading.Tasks;
using Chromia.Transport;
using System.Linq;
using Chromia;

public struct BlockchainConfig
{
    public string[] NodeUrls;
    public string Brid;
    public int ChainId;

    public static BlockchainConfig TicTacToe()
    {
#if AG_DEVNET
        return new()
        {
            ChainId = 2,
            NodeUrls = new[]
            {
            "http://localhost:7740/"
        }
        };
#else
        return new()
        {
            Brid = "E1EEB672E5CA8750A63C5BAF713069A06E06ED4B7FAA7A7F964EBD7042100669",
            NodeUrls = new[]
            {
                "https://node6.testnet.chromia.com",
                "https://node7.testnet.chromia.com",
                "https://node8.testnet.chromia.com"
            }
        };
#endif
    }

    public static BlockchainConfig AllianceGames()
    {
#if AG_DEVNET
        return new()
        {
            ChainId = 1,
            NodeUrls = new[]
            {
            "http://localhost:7740/"
        }
        };
#else
        return new()
        {
            Brid = "49F8C1D6CBE07C93139370E258959DA1825B3BC59D97C4F92F4E1CF2755C586E",
            NodeUrls = new[]
            {
                "https://node6.testnet.chromia.com",
                "https://node7.testnet.chromia.com",
                "https://node8.testnet.chromia.com"
            }
        };
#endif
    }
}

public class BlockchainConnectionManager
{
    public ITransport Transport { get; set; }
    public ChromiaClient AlliancesGamesClient;
    public ChromiaClient TicTacToeClient;

    public async Task Connect()
    {
        Transport = new AllianceGamesSdk.Unity.UnityTransport();
        ChromiaClient.SetTransport(Transport);
        AlliancesGamesClient = await InternalConnect(BlockchainConfig.AllianceGames());
        TicTacToeClient = await InternalConnect(BlockchainConfig.TicTacToe());
    }

    private async Task<ChromiaClient> InternalConnect(BlockchainConfig config)
    {
        if (!string.IsNullOrEmpty(config.Brid))
        {
            return await ChromiaClient.Create(config.NodeUrls.ToList(), Buffer.From(config.Brid));
        }
        else
        {
            return await ChromiaClient.Create(config.NodeUrls.ToList(), config.ChainId);
        }
    }
}
