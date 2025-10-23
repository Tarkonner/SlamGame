using SlamGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ManagerTests
    {
        [Fact]
        public void AddPlayer()
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");

            Assert.Equal(1, gm.playerList.Count);
        }

        [Fact]
        public void GetInfoFromAllPlayers()
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");
            gm.AddPlayer("2");

            string info = gm.GetAllPlayerInfo();

            string assertInfo = $"1: coordinats {Vector2.Zero};" +
                $"2: coordinats {Vector2.Zero};";

            Assert.Equal(2, gm.playerList.Count);
            Assert.Equal(assertInfo, info);
        }

        [Fact]
        public void RemovePlayer() 
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");
            gm.AddPlayer("2");
            
            gm.RemovePlayer("1");

            string info = gm.GetAllPlayerInfo();

            string assertInfo = $"2: coordinats {Vector2.Zero};";

            Assert.Equal(1, gm.playerList.Count);
            Assert.Equal(assertInfo, info);
        }

        [Fact]
        public void GetInfoFromSpecifikPlayer()
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");
            gm.AddPlayer("2");

            string info = gm.GetPlayerInfo("2");

            Assert.Equal(2, gm.playerList.Count);
            Assert.Equal(Vector2.Zero.ToString(), info);
        }
    }
}
