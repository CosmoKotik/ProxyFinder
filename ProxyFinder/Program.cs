using Pastel;
using ProxyFinder.Core;
using System.Numerics;

namespace ProxyFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger log = new Logger("   ___                         ___ _           _           \r\n  / _ \\_ __ _____  ___   _    / __(_)_ __   __| | ___ _ __ \r\n / /_)/ '__/ _ \\ \\/ / | | |  / _\\ | | '_ \\ / _` |/ _ \\ '__|\r\n/ ___/| | | (_) >  <| |_| | / /   | | | | | (_| |  __/ |   \r\n\\/    |_|  \\___/_/\\_\\\\__, | \\/    |_|_| |_|\\__,_|\\___|_|   \r\n                     |___/                                 ");
            Menu menu = new Menu(new Vector2(0, 9));


            Menu startMenu = new Menu(new Vector2(0, 9));
            startMenu.AddMenuSelection("stop", menu);
            startMenu.AddText(" Stops the checker");

            Menu aboutMenu = new Menu(new Vector2(0, 9));
            aboutMenu.AddMenuSelection("back", menu);
            aboutMenu.AddText(" Created by".Pastel(ConsoleColor.Gray) + ": CosmoKotik".Pastel(ConsoleColor.White));
            aboutMenu.AddText(" Discord".Pastel("0d40b8") + ": CosmoKotik#0623".Pastel(ConsoleColor.White));
            aboutMenu.AddText(" Telegram".Pastel("0d9cb8") + ": @cosmokotikxuy".Pastel(ConsoleColor.White));
            aboutMenu.AddText(" Website".Pastel("32a852") + ": https://cosmokott.ru".Pastel(ConsoleColor.White));

            log.AddTitle();

            menu.AddMenuSelection("Start", startMenu);
            menu.AddMenuSelection("About", aboutMenu);

            menu.OnMenuEnter += OnMenuEnter;
            startMenu.OnMenuEnter += OnStartMenuEnter;
            aboutMenu.OnMenuEnter += OnAboutMenuEnter;

            menu.Update();

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        menu.MoveCursorDown();
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        menu.MoveCursorUp();
                        break;
                    case ConsoleKey.Enter:
                        menu.EnterMenuSelection();
                        break;
                        /*case ConsoleKey.LeftArrow:
                            Console.WriteLine("Down");
                            break;
                        case ConsoleKey.RightArrow:
                            Console.WriteLine("Down");
                            break;*/
                }

                Thread.Sleep(1);
            }
        }

        private static void OnAboutMenuEnter(object obj)
        {
            
        }

        private static void OnStartMenuEnter(object obj)
        {
            
        }

        private static void OnMenuEnter(object obj)
        {
            
        }
    }
}