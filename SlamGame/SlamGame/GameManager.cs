using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlamGame
{
    public class GameManager
    {
        public GameManager() 
        {
            Program.OnGameLoop += Update;
        }

       

        public void Update()
        {
        }

        public void Dispose()
        {
            Program.OnGameLoop -= Update;
        }
    }
}
