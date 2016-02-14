using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeW4G2.Models
{
    [Serializable] //Это строка весьма очень необходима, чтобы позволить сериализировать весь класс
    public class Drawer
    {
        public ConsoleColor color;
        public char sign;
        public List<Point> body = new List<Point>();

        public Drawer() { }

        public void Draw()
        {
            Console.ForegroundColor = color;
            foreach(Point p in body)
            {
                Console.SetCursorPosition(p.x, p.y);
                Console.Write(sign);
            }
        }

        //Проверяет местоположение на существование змеи или стены в случае еды или стены в случае змеи
        public bool positionCheck(List<Point> a, Point b)
        {
            foreach (Point p in a)
            {
                if (p.x == b.x && p.y == b.y)
                {
                    return true;
                }
            }
            return false;
        }

        public Point newRandomCoordinates()
        // Генерирует спрайты в любой позиции в заданном диапазоне экрана, 
        //если змейка, то она появляется в тех местах, где не присутствуют стены
        //если еда, то везде, кроме в тех местах, где стоят стены или змейка
        {
            return new Point((new Random().Next()) % Console.WindowWidth - 1, (new Random().Next()) % Game.ConsoleHeight);
        }

        //Сохраняет все объекты игры, чтобы в дальнейшем, дать игроку возможность, возобновить игру
        public void Save()
        {

            Drawer sprite = Game.snake;// Сериализирует класс змеи
            string fileName = "snake.dat";// Название файла, где будут хранится все данные о текущем классе

            if (sign == '#')
            {
                fileName = "wall.dat";
                sprite = Game.wall;// Сериализирует класс стены
            }
            if (sign == '$')
            {
                fileName = "food.dat";
                sprite = Game.food;// Сериализирует класс еды
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            
            try
            {
                formatter.Serialize(fs, sprite);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        //Позволяет возобновить игру
        public void Resume()
        {

            string fileName = "snake.dat";
            if (sign == '#')
                fileName = "wall.dat";
            if (sign == '$')
                fileName = "food.dat";

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                if (sign == '#')
                    Game.wall = formatter.Deserialize(fs) as Wall;
                if (sign == 'o')
                    Game.snake = formatter.Deserialize(fs) as Snake;
                if (sign == '$')
                    Game.food = formatter.Deserialize(fs) as Food;
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

        }

    }
}