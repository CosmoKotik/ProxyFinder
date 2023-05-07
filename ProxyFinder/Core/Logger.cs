using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFinder.Core
{
    public class Logger
    {
        private List<Progress> _progressBars = new List<Progress>();

        private int _initialSizeWidth;
        private int _initialSizeHeight;
        private string _title;

        private int _currentProgress = 0;

        private StringBuilder _frameBuffer = new StringBuilder();

        private string _currentText = "";

        public Logger(string title)
        {
            this._initialSizeWidth = Console.WindowWidth;
            this._initialSizeHeight = Console.WindowHeight;

            this._title = title;
        }

        public void AddTitle()
        {
            /*string border = "";
            for (int i = 0; i < Console.WindowWidth; i++)
                border += "=";
            Console.WriteLine(border);
            Console.WriteLine("");
            string result = "";
            for (int i = 0; i < (border.Length / 2) - this._title.Length; i++)
                result += " ";
            result += this._title;
            Console.WriteLine(result);
            Console.WriteLine("");
            Console.WriteLine(border);*/

            Console.WriteLine(this._title);
        }

        public void Update()
        {
            if (_initialSizeWidth != Console.WindowWidth || _initialSizeHeight != Console.WindowHeight)
            {
                //Console.SetWindowSize(this._initialSizeWidth, this._initialSizeHeight);
                this._initialSizeWidth = Console.WindowWidth;
                this._initialSizeHeight = Console.WindowHeight;
                Console.Clear();
                AddTitle();
            }

            char fillVal = '█';

            int initialX = Console.CursorLeft;
            int initialY = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _progressBars.Count; i++)
            {
                if (_progressBars[i].Percentage > 100)
                    _progressBars[i].Percentage = 100;

                int progressBarSize = ((Console.WindowWidth / _progressBars.Count) - (_progressBars[i].Percentage.ToString().Length + 5));

                int progressBlockCount = (int)(((double)_progressBars[i].Percentage / 100) * progressBarSize);
                int percent = (int)((double)_progressBars[i].Percentage);

                if (_progressBars[i].Percentage > 99)
                {
                    string text = string.Format("{0}".Pastel(ConsoleColor.Green) + "{1}".Pastel(ConsoleColor.Gray), new string('█', progressBlockCount), new string('|', progressBarSize - progressBlockCount));
                    sb.Append("[");
                    sb.Append(text);
                    sb.Append($"] ");
                    sb.Append($"DONE".Pastel("1bbf0f"));
                }
                else
                {
                    string text = string.Format("{0}".Pastel(ConsoleColor.White) + "{1}".Pastel(ConsoleColor.Gray), new string('█', progressBlockCount), new string('|', progressBarSize - progressBlockCount));

                    sb.Append("[");
                    sb.Append(text);
                    sb.Append($"] ");
                    sb.Append($"{_progressBars[i].Percentage}%");
                }

                if (i + 1 != _progressBars.Count)
                    sb.Append(" ");

                _progressBars[i].Content = sb.ToString();
            }

            string text2 = sb.ToString();

            if (text2.Length < Console.WindowWidth)
                sb.Append(' ', Console.WindowWidth - text2.Length);

            //Console.SetCursorPosition(0, 10);
            Console.Write(sb.ToString());

            /*for (int i = 0; i < (Console.WindowWidth - sb.ToString().Length) / 2; i++)
                sb.Insert(0, " ");*/

            Console.SetCursorPosition(initialX, initialY);
        }

        public void UpdateProgressBar(int index, int value)
        {
            if (value > 100)
                _progressBars[index].Percentage = 100;
            else if (value < 1)
                _progressBars[index].Percentage = 0;
            else
                _progressBars[index].Percentage = value;
        }

        public void CancelProgressBar(int index)
        {
            _progressBars.RemoveAt(index);
        }

        public int AddProgressBar(int value)
        {
            int screenWidth = Console.WindowWidth;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < (Console.WindowWidth / (_progressBars.Count + 1)) - 7; i++)
                sb.Append("|");
            sb.Append($"] {value}%");

            Progress progress = new Progress()
            {
                Percentage = value,
                Content = sb.ToString()
            };
            _progressBars.Add(progress);

            return _progressBars.Count - 1;
        }
    }
}
