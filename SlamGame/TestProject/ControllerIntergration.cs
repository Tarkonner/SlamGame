using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlamGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit;

namespace TestProject
{
    public class ControllerIntergration : IClassFixture<WebApplicationFactory<MainProgram>>, IAsyncLifetime
    {
        private readonly HttpClient _client;

        public ControllerIntergration(WebApplicationFactory<MainProgram> factory)
        {
            // Create an in-memory HTTP client to call your API
            _client = factory.CreateClient();
        }

        public Task InitializeAsync()
        {
            // Runs before each test
            MainProgram.gamesList.Clear();
            MainProgram.MakeGame();
            GameManager.instance = new GameManager();
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // Runs after each test
            MainProgram.gamesList.Clear();
            GameManager.instance = null!;
            return Task.CompletedTask;
        }

        [Fact]
        public async Task CreatePlayer_ReturnsOk()
        {
            string player = "playerTest";
            var response = await _client.PostAsync($"/api/player/create?id={player}", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains($"Player {player} created", content);
        }

        [Fact]
        public async Task MovePlayer()
        {
            string playerID = "testPlayer";

            //Create player
            await _client.PostAsync($"/api/player/create?id={playerID}", null);

            //Move player
            var moveRespond = await _client.PostAsync($"/api/player/move?id={playerID}&direction=m.w", null);
            Assert.Equal(HttpStatusCode.OK, moveRespond.StatusCode);

            //Get all player info
            var infoRespond = await _client.GetAsync("/api/player/allPosition");
            Assert.Equal(HttpStatusCode.OK, infoRespond.StatusCode);

            var jsonString = await infoRespond.Content.ReadAsStringAsync();

            //Deserialize JSON to check player position
            var allPlayers = JsonSerializer
                .Deserialize<Dictionary<string, JsonElement>>(jsonString);

            var x = allPlayers["testPlayer"].GetProperty("x").GetSingle();
            var y = allPlayers["testPlayer"].GetProperty("y").GetSingle();
            Assert.Equal(0, x);
            Assert.Equal(1, y);
        }


    }
}
