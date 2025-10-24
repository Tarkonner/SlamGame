using System;
using System.Collections.Generic;
using System.Linq;
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

        public string GetPlayerInfo(string id)
        {
            string collectData = string.Empty;

            collectData = playerList[id].coordinats.ToString();

            return collectData;
        }

        public string GetAllPlayerInfo()
        {
            string collectData = string.Empty;

            foreach (var item in playerList)
            {
                Player targetPlayer = item.Value;

                collectData += $"{targetPlayer.playerID}: coordinats {targetPlayer.coordinats};";
            }

            return collectData;
        }

        public void MovePlayer(string id, string moveDirection)
        {
            Player targetPlayer = playerList[id];
            targetPlayer.Move(moveDirection);
        }
    }
}
