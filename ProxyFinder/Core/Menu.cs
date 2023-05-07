using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProxyFinder.Core
{
    public class Menu
    {
        public event Action<object> OnMenuEnter;

        private List<MenuSelection> _ms = new List<MenuSelection>();

        private int _currentSelection = 0;
        private int _selectableAmount = 0;
        private Vector2 _startPosition = Vector2.Zero;

        private Menu _currentMenu;

        public Menu(Vector2 startPost)
        {
            _startPosition = startPost;
            _currentMenu = this;
        }

        public void AddMenuSelection(string name, Menu menu)
        {
            MenuSelection ms = new MenuSelection();
            ms.Name = name;
            ms.Index = this._ms.Count;
            ms.Position = new Vector2(_startPosition.X, _startPosition.Y + (_ms.Count * 2));
            ms.SelectMenu = menu;

            ms.CanClick = true;
            _selectableAmount++;

            this._ms.Add(ms);
        }

        public void AddText(string description)
        {
            MenuSelection ms = new MenuSelection();
            ms.Index = this._ms.Count;
            ms.Position = new Vector2(_startPosition.X, _startPosition.Y + (_ms.Count * 2));
            ms.Description = description;

            ms.CanClick = false;

            this._ms.Add(ms);
        }

        public void MoveCursorDown()
        {
            this._currentMenu._currentSelection++;

            if (this._currentMenu._currentSelection >= this._currentMenu._selectableAmount)
                this._currentMenu._currentSelection = 0;

            Update();
        }

        public void MoveCursorUp()
        {
            this._currentMenu._currentSelection--;

            if (this._currentMenu._currentSelection < 0)
                this._currentMenu._currentSelection = this._currentMenu._selectableAmount - 1;

            Update();
        }

        public void EnterMenuSelection()
        {
            int clearAmount = this._currentMenu._ms.Count - 1;

            if (this._currentMenu._ms[this._currentMenu._currentSelection].CanClick)
            {
                this._currentMenu = this._currentMenu._ms[this._currentMenu._currentSelection].SelectMenu;

                this._currentMenu.OnMenuEnter(this._currentMenu);
            }
            else
            {
                int offset = 0;
                for (int i = this._currentMenu._currentSelection; i < this._currentMenu._ms.Count; i++)
                {
                    if (this._currentMenu._ms[i].CanClick)
                        break;
                    offset++;
                }
                this._currentMenu = this._currentMenu._ms[this._currentMenu._currentSelection + offset].SelectMenu;

                this._currentMenu.OnMenuEnter(this._currentMenu);
            }

            Update(clearAmount);
        }

        public void Update(int clearAmout = 0)
        {
            StringBuilder sb = new StringBuilder();
            int offset = 0;

            for (int i = 0; i < this._currentMenu._ms.Count; i++)
            {
                if (!this._currentMenu._ms[i].CanClick)
                {
                    sb.Append(" " + this._currentMenu._ms[i].Description);
                    sb.Append(' ', Console.WindowWidth - this._currentMenu._ms[i].Description.Length - 2);
                    sb.AppendLine("");
                    offset++;
                    continue;
                }

                if (i != this._currentMenu._currentSelection + offset)
                    sb.Append(" " + this._currentMenu._ms[i].Name);
                else
                    sb.Append("> " + this._currentMenu._ms[i].Name);

                sb.Append(' ', Console.WindowWidth - this._currentMenu._ms[i].Name.Length - 2);
                sb.AppendLine("");
            }

            if (clearAmout > 0)
                for (int i = 0; i < clearAmout; i++)
                {
                    sb.Append(' ', Console.WindowWidth - 1);
                    sb.AppendLine("");
                }

            Console.SetCursorPosition((int)_startPosition.X, (int)_startPosition.Y);
            Console.Write(sb.ToString());
        }

    }
}
