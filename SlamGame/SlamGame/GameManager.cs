using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlamGame
{
    public class GameManager
    {
        public static GameManager instance;

        public Dictionary<string, Player> playerList { get; private set; } = new();

        public GameManager() 
        {
            instance = this;

            MainProgram.OnGameLoop += Update;
        }

       

        public void Update()
        {
        }

        public void Dispose()
        {
            MainProgram.OnGameLoop -= Update;
        }

        public void AddPlayer(string id)
        {
            Player spawn = new(id);
            playerList.Add(spawn.playerID ,spawn);
        }

        public void RemovePlayer(string id)
        {
            playerList.Remove(id);
        }

        public Vector2 GetPlayerInfo(string id)
        {
            return playerList[id].coordinats;
        }

        public Dictionary<string, Vector2> GetAllPlayerPosition()
        {
            var dict = new Dictionary<string, Vector2>();

            foreach (var item in playerList)
            {
                Player targetPlayer = item.Value;
                dict[targetPlayer.playerID] = targetPlayer.coordinats;
            }

            return dict;
        }

        public Dictionary<string, Player> GetAllPlayerInfo()
        {
            return playerList;
        }

        public void MovePlayer(string id, string moveDirection)
        {
            Player targetPlayer = playerList[id];
            targetPlayer.Move(moveDirection);
        }
    }
}
