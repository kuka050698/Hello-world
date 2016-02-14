using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeW4G2.Models
{
    class Game
    {
        public static int ConsoleHeight = 22; // Высота задана по умлочанию, потребуется для того, чтобы выводить индикатор очков и текущего уровня
        public static int countOfEatenFood = 0, numberOfLvl = 1, points = 0; // Индикатор очков для прохождения следуещего уровня, общих очков и текущего уровня
        public static Wall wall = new Wall();
        public static Snake snake = new Snake();
        public static Food food = new Food();
        public static bool GameOver = false;
        
        public Game()
        {
            wall.setLevel(1);
            while (!GameOver)
            {
                Draw();
                ConsoleKeyInfo button = Console.ReadKey();
                if (button.Key == ConsoleKey.UpArrow)
                    snake.move(0, -1);
                if (button.Key == ConsoleKey.DownArrow)
                    snake.move(0, 1);
                if (button.Key == ConsoleKey.LeftArrow)
                    snake.move(-1, 0);
                if (button.Key == ConsoleKey.RightArrow)
                    snake.move(1, 0);
                if (snake.CheckCollisionWithWalls())
                    GameOver = true;
                if (button.Key == ConsoleKey.F1)
                    Save();
                if (button.Key == ConsoleKey.F2)
                    Resume();
                if (Game.snake.body[0].x > Console.WindowWidth-1) //Если змейка перешла размер ширины экрана, то продолжает свое движение с нулевой позиции по горизонтали
                    Game.snake.body[0].x = 0;
                if (Game.snake.body[0].x < 0) //Если змейка вышла за пределы нулевой позиций по горизонтали, то продолжает свое движение с другого конца экрана по горизонтали
                    Game.snake.body[0].x = Console.WindowWidth - 1;
                if (Game.snake.body[0].y > ConsoleHeight)  //Если змейка вышла за пределы высоты экрана, то продолжает свое движение с верхней начальной позиции
                    Game.snake.body[0].y = 0;
                if (Game.snake.body[0].y < 0)  //Если змейка вышла за пределы экрана по вертикали, то продолжает свое движение с нижней доступной точки экрана
                    Game.snake.body[0].y = ConsoleHeight;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Game Over");
            Console.ReadKey();
        }

        public void showStats()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(2, 23);
            Console.WriteLine("Level {0}", numberOfLvl);
            Console.SetCursorPosition(2, 24);
            Console.WriteLine("{0} points", points);
        }

        public void Draw()
        {
            Console.Clear();
            showStats();
            wall.Draw();
            food.Draw();
            snake.Draw();
        }

        public void Save()
        {
            snake.Save();
            food.Save();
            wall.Save();
        }

        public void Resume()
        {
            snake.Resume();
            food.Resume();
            wall.Resume();
        }

    }
}
