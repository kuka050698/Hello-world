using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeW4G2.Models
{
    [Serializable]
    public class Snake : Drawer
    {

        public Snake()
        {
            color = ConsoleColor.White;
            sign = 'o';
            respawnSnake();
        }

        public void respawnSnake()
        {
            if(body.Count!=0)
            {
                body.Clear();
            }
            int x = newRandomCoordinates().x;
            int y = newRandomCoordinates().y;
            if (Game.wall.body.Count > 0)
            {
                while (true)
                {
                    bool invalidPositioning = positionCheck(Game.wall.body, new Point(x, y));
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
            }
            body.Add(new Point(x, y));
        }

        public void move(int dx, int dy)
        {
            // Данная строка кода, проверяет следуещее движение змейки
            // Если вдруг игрок будет двигаться вниз, а в тот момент голова змеи будет направлена вверх, то данный код среагирует на это событие и предовратит движение змей
            // Если вдруг головка змейки столкнется с его остальной частью тела, то игрок проиграет
            bool preventMotion = false;
             
            if (body.Count > 1)
            {
                if (body[0].x + dx == body[1].x && body[0].y + dy == body[1].y)
                    preventMotion = true;
                else
                {
                    for (int i = 1; i < body.Count; i++)
                    {
                        if (body[0].x + dx == body[i].x && body[0].y + dy == body[i].y)
                        {
                            Game.GameOver = true;
                            break;
                        }
                    }
                }
            }

            if (!Game.GameOver && !preventMotion)
            {
                for (int i = body.Count - 1; i > 0; i--)
                {
                    body[i].x = body[i - 1].x;
                    body[i].y = body[i - 1].y;
                }

                int prevXpos = body[0].x, prevYpos = body[0].y;

                body[0].x += dx;
                body[0].y += dy;

                // check snake eating food
                if (body[0].x == Game.food.body[0].x &&
                    body[0].y == Game.food.body[0].y)
                {
                    Point coordinatesForNewTail = new Point();
                    if (body.Count < 2)
                    {
                        coordinatesForNewTail.y = prevYpos;
                        coordinatesForNewTail.x = prevXpos;

                        if (prevXpos == Console.WindowWidth - 1 || body[0].x - prevXpos == 1) //слева
                        {
                            if (body[0].x == 0)
                                coordinatesForNewTail.x = Console.WindowWidth - 1;
                            else if (body[0].x == Console.WindowWidth - 1)
                                coordinatesForNewTail.x = prevXpos;
                            coordinatesForNewTail.y = body[0].y;
                        }
                        else if (prevXpos == 0 || body[0].x - prevXpos == -1) //справа
                        {
                            if (body[0].x == 0)
                                coordinatesForNewTail.x = prevXpos;
                            else if (body[0].x == Console.WindowWidth - 1)
                                coordinatesForNewTail.x = 0;
                            coordinatesForNewTail.y = body[0].y;
                        }

                        if (prevYpos == Game.ConsoleHeight || body[0].y - prevYpos == 1) //сверху
                        {
                            if (body[0].y == 0)
                                coordinatesForNewTail.y = Game.ConsoleHeight;
                            else if (body[0].y == Game.ConsoleHeight)
                                coordinatesForNewTail.y = prevYpos;
                            coordinatesForNewTail.x = body[0].x;
                        }
                        else if (prevYpos == 0 || body[0].y - prevYpos == -1) //снизу
                        {
                            if (body[0].y == 0)
                                coordinatesForNewTail.y = prevYpos;
                            else if (body[0].y == Game.ConsoleHeight)
                                coordinatesForNewTail.y = 0;
                            coordinatesForNewTail.x = body[0].x;
                        }
                    }
                    else
                    {
                        if (body[body.Count - 1].x == body[body.Count - 2].x)
                        {
                            if (body[body.Count - 1].y - body[body.Count - 2].y == -1) //сверху
                                coordinatesForNewTail.y = body[body.Count - 1].y - 1;
                            if (body[body.Count - 1].y - body[body.Count - 2].y == 1) //снизу
                                coordinatesForNewTail.y = body[body.Count - 1].y + 1;
                            coordinatesForNewTail.x = body[body.Count - 1].x;
                        }
                        else if (body[body.Count - 1].y == body[body.Count - 2].y)
                        {
                            if (body[body.Count - 1].x - body[body.Count - 2].x == -1) //слева
                                coordinatesForNewTail.x = body[body.Count - 1].x - 1;
                            if (body[body.Count - 1].x - body[body.Count - 2].x == 1) //справа
                                coordinatesForNewTail.x = body[body.Count - 1].x + 1;
                            coordinatesForNewTail.y = body[body.Count - 1].y;
                        }
                    }

                    body.Add(coordinatesForNewTail);
                    Game.food.setNewPosition();
                    Game.countOfEatenFood++;
                    Game.points++;

                    //  Если змейка съест 4 еды, то она переходит на следующий уровень, счетчик аннулируется, а достижения увеличиваются и еда вновь появляется в другом месте
                    if (Game.countOfEatenFood == 4)
                    {
                        Game.numberOfLvl++;
                        Game.countOfEatenFood = 0;
                        respawnSnake();
                        Game.food.setNewPosition();
                    }

                }
            }
            
            
        }
        public bool CheckCollisionWithWalls()
        {
            foreach (Point p in Game.wall.body)
                if (p.x == body[0].x && p.y == body[0].y)
                    return true;
            return false;
        }
    }
}
