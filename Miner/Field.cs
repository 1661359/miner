using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

namespace Miner
{
    public struct Point
    {
        public int x, y;
    }
    class Field
    {
        private const int InitialBombCount = 10;
        private const int BombValue = -1;
        private int _size;
        private int[][] _field;
        private bool[][] _isOpened;
        private bool[][] _bombsMarked;

        

        public void MarkBomb(int i, int j)
        {
            _bombsMarked[i][j] = !_bombsMarked[i][j];
        }

        public void OpenCell(int i, int j)
        {
            _isOpened[i][j] = true;
            if (_field[i][j] == 0)
            {
                var queue = new Queue<Point>();
                queue.Enqueue(new Point() {x = j, y = i});
                while (queue.Count != 0)
                {
                    var cur = queue.Dequeue();
                    for (int k = -1; k < 2; k ++)
                    {
                        for (int l = -1; l < 2; l ++)
                        {
                            var y = cur.y + k;
                            var x = cur.x + l;
                            if (x >= 0 && y >= 0 && x < _size && y < _size)
                            {
                                if (_field[y][x] == 0 && !_isOpened[y][x])
                                {
                                    _isOpened[y][x] = true;
                                    queue.Enqueue(new Point() {x = x, y = y});
                                }
                                else if (_field[y][x] != 0 && !_isOpened[y][x])
                                {
                                    _isOpened[y][x] = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ShowField(Point cursorPoint)
        {
            Console.Write("M  / ");
            for (int i = 0; i < _size; i++)
            {
                Console.Write("{0:00} ", i + 1);
            }
            Console.WriteLine();
            Console.Write("     ");
            for (int i = 0; i < _size; i++)
            {
                Console.Write("---", i);
            }
            Console.WriteLine();
            Console.WriteLine();
            for (var i = 0; i < _size; i++)
            {
                Console.Write("{0:00} | ", i + 1);
                for (var j = 0; j < _size; j++)
                {
                    if (_isOpened[i][j])
                    {
                        if (_field[i][j] == BombValue)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            if (cursorPoint.x == j && cursorPoint.y == i)
                                Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("B");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            if (cursorPoint.x == j && cursorPoint.y == i)
                                Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("{0}", _field[i][j]);
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        if (_bombsMarked[i][j])
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            if (cursorPoint.x == j && cursorPoint.y == i)
                                Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("@");
                            Console.ResetColor();
                        }
                        else
                        {
                            if (cursorPoint.x == j && cursorPoint.y == i)
                                Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("*");
                            Console.ResetColor();
                        }
                    }
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
        }

        public Field(int size, int bombCount = InitialBombCount)
        {
            _size = size;
            _field = new int[size][];
            _isOpened = new bool[size][];
            _bombsMarked = new bool[bombCount][];
            for (var i = 0; i < size; i++)
            {
                _field[i] = new int[size];
                _isOpened[i] = new bool[size];
                _bombsMarked[i] = new bool[size];
                for (var j = 0; j < size; j++)
                {
                    _field[i][j] = 0;
                    _isOpened[i][j] = false;
                }
            }
            MakeBombs(bombCount);
        }


        private void MakeBombs(int bombCount)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < bombCount; i++)
            {
                while (true)
                {
                    var x = random.Next(_size);
                    var y = random.Next(_size);
                    if (_field[y][x] != BombValue)
                    {
                        _field[y][x] = BombValue;
                        break;
                    }
                }
            }
            CalculateWeight();
        }

        private void CalculateWeight()
        {
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    CalculatePointWeight(i, j);
                }
            }
        }

        private void CalculatePointWeight(int i, int j)
        {
            if (_field[i][j] == BombValue)
                return;
            for (int k = -1; k < 2; k ++)
            {
                for (int l = -1; l < 2; l ++)
                {
                    if (k == 0 && l == 0)
                        continue;
                    var y = i + k;
                    var x = j + l;
                    if (x >= 0 && y >= 0 && x < _size && y < _size)
                        if (_field[y][x] == BombValue)
                        {
                            _field[i][j] += 1;
                        }
                }
            }
        }
    }
}