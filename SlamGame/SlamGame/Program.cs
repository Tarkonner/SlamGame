using SlamGame;
using System.Threading.Tasks;

public class Program
{
    public static bool gameLoopRunning = true;
    private static CommandSystem _commandSystem = new();
    const int loopTime = 1000;

    public static event Action OnGameLoop;

    private static async Task Main(string[] args)
    {       
        Console.WriteLine("Hello, World!");

        await _commandSystem.RunAsync();

        while (gameLoopRunning)
        {
            Thread.Sleep(loopTime);
            OnGameLoop?.Invoke();
        }
    }
}


