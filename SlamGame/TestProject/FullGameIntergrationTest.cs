using SlamGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class FullGameIntergrationTest : IDisposable
    {
        public FullGameIntergrationTest()
        {
            // Runs before each test
            MainProgram.gamesList.Clear();
        }

        public void Dispose()
        {
            MainProgram.gamesList.Clear();
        }

        [Fact]
        public void MakeAGame()
        {
            MainProgram.MakeGame();                       

            Assert.Equal(1, MainProgram.gamesList.Count);

            GameManager targetGame = MainProgram.gamesList[0];

            string p1 = "1";
            string p2 = "2";

            targetGame.AddPlayer(p1);
            targetGame.AddPlayer(p2);

            targetGame.MovePlayer(p1, "m.w");

            // Act
            var info = targetGame.GetAllPlayerPosition();

            // Assert
            Assert.Equal(2, targetGame.playerList.Count); // still valid
            Assert.True(info.ContainsKey("1"));
            Assert.True(info.ContainsKey("2"));
            Assert.Equal(new Vector2(0, 1), info["1"]);
            Assert.Equal(Vector2.Zero, info["2"]);
        }
    }
}
