using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JogoTecnicas
{
    public class Collisions
    {
        //Colisão entre dois retangulos
        public bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);
        }

    }
}
