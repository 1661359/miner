using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    public class Program
    {
        private static bool _isRunning;
        private static Field _field;
        private const int Size = 20;
        private const int BombsCount = 50;
        private static Point _cursorPoint;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            _cursorPoint = new Point() { x = 0, y = 0 };
            _field = new Field(Size, BombsCount);
            _isRunning = true;
            while (_isRunning)
            {
                Console.SetCursorPosition(0,0);
                _field.ShowField(_cursorPoint);
                GetCommand();
            }
        }

        private static void GetCommand()
        {
            var input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.Q:
                    _isRunning = false;
                    break;
                case ConsoleKey.R:
                    _field = new Field(Size, BombsCount);
                    break;
                case ConsoleKey.B:
                    _field.MarkBomb(i: _cursorPoint.y, j: _cursorPoint.x);

                    break;
                case ConsoleKey.Enter:
                    _field.OpenCell(i: _cursorPoint.y, j: _cursorPoint.x);

                    break;
                case ConsoleKey.UpArrow:
                    if (_cursorPoint.y > 0)
                        _cursorPoint.y--;

                    break;
                case ConsoleKey.DownArrow:
                    if (_cursorPoint.y < Size - 1)
                        _cursorPoint.y++;

                    break;
                case ConsoleKey.LeftArrow:
                    if (_cursorPoint.x > 0)
                        _cursorPoint.x--;

                    break;
                case ConsoleKey.RightArrow:
                    if (_cursorPoint.x < Size - 1)
                        _cursorPoint.x++;

                    break;
            }
        }
    }
}