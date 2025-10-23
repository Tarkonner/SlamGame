using SlamGame;
using System.Numerics;

namespace TestProject
{
    public class PlayerTesting
    {
        [Fact]
        public void MoveLeft()
        {
            Player player = new("");

            player.Move("m.a");

            var expected = new Vector2(-1, 0);
            Assert.Equal(expected, player.coordinats);
        }

        [Fact]
        public void MoveRight()
        {
            Player player = new("");

            player.Move("m.d");

            var expected = new Vector2(1, 0);
            Assert.Equal(expected, player.coordinats);
        }

        [Fact]
        public void MoveUp()
        {
            Player player = new("");

            player.Move("m.w");

            var expected = new Vector2(0, 1);
            Assert.Equal(expected, player.coordinats);
        }

        [Fact]
        public void MoveDown()
        {
            Player player = new("");

            player.Move("m.s");

            var expected = new Vector2(0, -1);
            Assert.Equal(expected, player.coordinats);
        }
    }
}