using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlamGame;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public partial class MainProgram
{
    public static bool gameLoopRunning = true;
    private static CommandSystem _commandSystem = new();
    const int loopTime = 1000;

    public static event Action OnGameLoop;

    public static Dictionary<int, GameManager> gamesList = new();


    private static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Game Server...");

        string key = Environment.GetEnvironmentVariable("Slam")
                                                   ?? throw new InvalidOperationException("JWT secret key not set in environment variable 'Slam'");
        var keyBytes = Encoding.UTF8.GetBytes(key);        

        // Create and configure the web host
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        // Register controllers
        builder.Services.AddControllers();

        // Allow Swagger UI for testing endpoints easily
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Build the app
        var app = builder.Build();

        // Enable Swagger in development mode
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
               
        //Threads
        // Run the web server in the background
        var webTask = app.RunAsync();

        // Run your game command system and loop
        await _commandSystem.RunAsync();

        while (gameLoopRunning)
        {
            Thread.Sleep(loopTime);
            OnGameLoop?.Invoke();
        }

        // Stop web server when game loop ends
        await app.StopAsync();
        await webTask;

        //Begin game
        MakeGame();
    }

    public static void MakeGame()
    {
        gamesList.Add(gamesList.Count, new GameManager());
    }
}


