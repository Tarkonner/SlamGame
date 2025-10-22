using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlamGame
{
    public class Player
    {
        public string playerID;
        public Vector2 coordinats = Vector2.Zero;

        public void Move(string input)
        {
            switch (input)
            {
                case "m.a":
                    coordinats.X -= 1;
                    break;
                case "m.d":
                    coordinats.X += 1;
                    break;
                case "m.w":
                    coordinats.Y += 1;
                    break;
                case "m.s":
                    coordinats.Y -= 1;
                    break;
            }
        }
    }
}
