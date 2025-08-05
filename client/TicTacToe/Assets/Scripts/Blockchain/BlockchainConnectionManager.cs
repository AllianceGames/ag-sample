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
            Brid = "4FC7F780620D35B0BAE620DA69DC1476AA676AE7F11A640C65D88127EFFAA08B",
            NodeUrls = new[]
            {
                "https://node6.testnet.chromia.com",
                "https://node7.testnet.chromia.com",
                "https://node8.testnet.chromia.com
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
            Brid = "D9BAF45A5151E6960C8AE372BF4B4C83DDCA96010F1D4778ACF029B096D17848",
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
