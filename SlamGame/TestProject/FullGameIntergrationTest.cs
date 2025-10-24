using SlamGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

            string assertInfo = $"{p1}: coordinats {new Vector2(0, 1)};" +
                $"{p2}: coordinats {Vector2.Zero};";

            Assert.Equal(assertInfo, targetGame.GetAllPlayerInfo());
        }
    }
}
