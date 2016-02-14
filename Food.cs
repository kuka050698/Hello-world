using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeW4G2.Models
{
    [Serializable]
    public class Food : Drawer
    {
        public Food() {
            color = ConsoleColor.Yellow;
            sign = '$';
            setNewPosition();
        }

        public void setNewPosition()
        {
            int x = newRandomCoordinates().x;
            int y = newRandomCoordinates().y;
            while (true)
            {
                bool invalidPositioning = positionCheck(Game.snake.body, new Point(x, y));
                if (!invalidPositioning)
                {
                    if(Game.wall.body.Count > 0)
                    {
                        invalidPositioning = positionCheck(Game.wall.body, new Point(x, y));
                        if (invalidPositioning)
                        {
                            x = newRandomCoordinates().x;
                            y = newRandomCoordinates().y;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    x = newRandomCoordinates().x;
                    y = newRandomCoordinates().y;
                }
            }

            if (body.Count == 0)
                body.Add(new Point(x, y));
            else
            {
                body[0].x = x;
                body[0].y = y;
            }

        }
    }
}
