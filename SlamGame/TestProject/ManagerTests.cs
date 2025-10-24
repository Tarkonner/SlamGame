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

            // Act
            var info = gm.GetAllPlayerPosition();

            // Assert
            Assert.Equal(2, gm.playerList.Count); // still valid
            Assert.True(info.ContainsKey("1"));
            Assert.True(info.ContainsKey("2"));
            Assert.Equal(Vector2.Zero, info["1"]);
            Assert.Equal(Vector2.Zero, info["2"]);
        }

        [Fact]
        public void RemovePlayer() 
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");
            gm.AddPlayer("2");
            
            gm.RemovePlayer("1");

            var info = gm.GetAllPlayerPosition();

            Assert.Equal(1, gm.playerList.Count);
            Assert.True(info.ContainsKey("2"));
            Assert.Equal(new Vector2(0, 0), info["2"]);
        }

        [Fact]
        public void GetInfoFromSpecifikPlayer()
        {
            GameManager gm = new GameManager();

            gm.AddPlayer("1");
            gm.AddPlayer("2");

            Vector2 info = gm.GetPlayerInfo("2");

            Assert.Equal(2, gm.playerList.Count);
            Assert.Equal(Vector2.Zero, info);
        }

        [Fact]
        public void MovePlayer()
        {
            GameManager gm = new GameManager();

            string playerID = "1";

            gm.AddPlayer(playerID);

            gm.MovePlayer(playerID, "m.w");

            Vector2 info = gm.GetPlayerInfo(playerID);
            Vector2 expect = new Vector2(0, 1);

            Assert.Equal(expect, info);
        }
    }
}
