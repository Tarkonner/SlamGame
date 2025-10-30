using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlamGame;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TokenUtils;
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
            var token = TestingTokenGenerator.GenerateJwtToken("TestPlayer");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/player/create");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(1, GameManager.instance.playerList.Count);
        }

        [Fact]
        public async Task MoveAPlayer()
        {
            // Generate a JWT token for the player
            var token = TestingTokenGenerator.GenerateJwtToken("testPlayer");

            // 1️ Create the player
            var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/player/create");
            createRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createResponse = await _client.SendAsync(createRequest);
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            // 2️ Move the player
            var moveRequest = new HttpRequestMessage(HttpMethod.Post, "/api/player/move?direction=m.w");
            moveRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var moveResponse = await _client.SendAsync(moveRequest);
            Assert.Equal(HttpStatusCode.OK, moveResponse.StatusCode);

            // 3️ Get all player info
            var infoRequest = new HttpRequestMessage(HttpMethod.Get, "/api/player/allPosition");
            infoRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var infoResponse = await _client.SendAsync(infoRequest);
            Assert.Equal(HttpStatusCode.OK, infoResponse.StatusCode);

            // Deserialize JSON to check player position
            var jsonString = await infoResponse.Content.ReadAsStringAsync();
            var allPlayers = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

            var x = allPlayers["testPlayer"].GetProperty("x").GetSingle();
            var y = allPlayers["testPlayer"].GetProperty("y").GetSingle();
            Assert.Equal(0, x);
            Assert.Equal(1, y);
        }

        [Fact]
        public async Task MoveASpecefikPlayer()
        {
            // Generate JWT tokens for two players
            var token1 = TestingTokenGenerator.GenerateJwtToken("player1");
            var token2 = TestingTokenGenerator.GenerateJwtToken("player2");

            // --- Create both players ---
            var create1 = new HttpRequestMessage(HttpMethod.Post, "/api/player/create");
            create1.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token1);
            var response1 = await _client.SendAsync(create1);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var create2 = new HttpRequestMessage(HttpMethod.Post, "/api/player/create");
            create2.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token2);
            var response2 = await _client.SendAsync(create2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            // --- Move only player1 using token1 ---
            var moveRequest = new HttpRequestMessage(HttpMethod.Post, "/api/player/move?direction=m.w");
            moveRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token1);
            var moveResponse = await _client.SendAsync(moveRequest);
            Assert.Equal(HttpStatusCode.OK, moveResponse.StatusCode);

            // --- Get all positions using token1 ---
            var infoRequest = new HttpRequestMessage(HttpMethod.Get, "/api/player/allPosition");
            infoRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token1);
            var infoResponse = await _client.SendAsync(infoRequest);
            Assert.Equal(HttpStatusCode.OK, infoResponse.StatusCode);

            var jsonString = await infoResponse.Content.ReadAsStringAsync();
            var allPlayers = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

            // --- Assert that player1 moved ---
            var x1 = allPlayers["player1"].GetProperty("x").GetSingle();
            var y1 = allPlayers["player1"].GetProperty("y").GetSingle();
            Assert.Equal(0, x1); // or expected X after move
            Assert.Equal(1, y1); // or expected Y after move

            // --- Assert that player2 did NOT move ---
            var x2 = allPlayers["player2"].GetProperty("x").GetSingle();
            var y2 = allPlayers["player2"].GetProperty("y").GetSingle();
            Assert.Equal(0, x2); // initial position
            Assert.Equal(0, y2); // initial position
        }

        [Fact]
        public async Task AuterizontonCheck()
        {
            // Generate a JWT token
            var token = TestingTokenGenerator.GenerateJwtToken();

            // Create the request
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/player/validata");
            HttpClientExtensions.AddBearerToken(request, token);

            // Send the request
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
