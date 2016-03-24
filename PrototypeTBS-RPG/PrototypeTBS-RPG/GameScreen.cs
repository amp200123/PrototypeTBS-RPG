using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PrototypeTBS_RPG.Specializations;

namespace PrototypeTBS_RPG
{
    class GameScreen : Screen
    {
        public Tile[,] map { get; private set; }

        private Character characterA;
        private Tile selectedTile;
        private List<PopupMenuBar> menuItems;
        private bool renderMenu;

        private MouseState oldMouseState;
        private KeyboardState oldKeyState;

        public GameScreen(ContentManager content, EventHandler screenEvent, string fileName)
            : base(screenEvent)
        {
            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();

            menuItems = new List<PopupMenuBar>();

            characterA = new Character("Character A", new Knight(content));

            Tile plain = new Tile(content,content.Load<Texture2D>("Tiles/Plain"), 0, 0, 0);

            map = ImportLevel(content, fileName);

            int centerTileY = (int)(Math.Round(map.GetLength(0) / 2.0f)) - 1;
            int centerTileX = (int)(Math.Round(map.GetLength(1) / 2.0f)) - 1;
            Tile centerTile = map[centerTileY, centerTileX];

            centerTile.position.X = Game1.WINDOW_WIDTH / 2;
            centerTile.position.Y = Game1.WINDOW_HEIGHT / 2;

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //y pos
                    if (y < centerTileY)
                        map[y, x].position.Y = centerTile.position.Y - (40 * (centerTileY - y));
                    else if (y == centerTileY)
                        map[y, x].position.Y = centerTile.position.Y;
                    else if (y > centerTileY)
                        map[y, x].position.Y = centerTile.position.Y + (40 * (y - centerTileY));

                    //x pos
                    if (x < centerTileX)
                        map[y, x].position.X = centerTile.position.X - (40 * (centerTileX - x));
                    else if (x == centerTileX)
                        map[y, x].position.X = centerTile.position.X;
                    else if (x > centerTileX)
                        map[y, x].position.X = centerTile.position.X + (40 * (x - centerTileX));
                }
            }

            map[1, 1].charOnTile = characterA;
        }

        public override void Update(GameTime gametime)
        {
            MouseState newMouseState = Mouse.GetState();
            KeyboardState newKeyState = Keyboard.GetState();

            bool hasSelectedTile = false;
            foreach (Tile tile in map)
            {
                if (newMouseState.X <= tile.boundingRectangle.Right && newMouseState.X > tile.boundingRectangle.Left &&
                    newMouseState.Y <= tile.boundingRectangle.Bottom && newMouseState.Y > tile.boundingRectangle.Top)
                {
                    tile.isSelected = true;
                    selectedTile = tile;
                    hasSelectedTile = true;
                }
                else tile.isSelected = false;
            }

            if (!hasSelectedTile)
                selectedTile = null;

            if (oldMouseState.LeftButton == ButtonState.Pressed && newMouseState.LeftButton == ButtonState.Released)
            {
                if (hasSelectedTile)
                {
                    renderMenu = true;
                    menuItems = GetMenu();
                }
            }

            //Keyboard input
            if (newKeyState.IsKeyDown(Keys.Up))
            {
                MoveScreen(0, 3);
            }
            if (newKeyState.IsKeyDown(Keys.Down))
            {
                MoveScreen(0, -3);
            }
            if (newKeyState.IsKeyDown(Keys.Left))
            {
                MoveScreen(3, 0);
            }
            if (newKeyState.IsKeyDown(Keys.Right))
            {
                MoveScreen(-3, 0);
            }

            //Drag movement input
            if (!renderMenu && newMouseState.Position.X <= Game1.WINDOW_WIDTH && newMouseState.Position.X >= 0 &&
                newMouseState.Position.Y <= Game1.WINDOW_HEIGHT && newMouseState.Position.Y >= 0 &&
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                MoveScreen((newMouseState.Position - oldMouseState.Position).ToVector2());
            }

            oldMouseState = newMouseState;
            oldKeyState = newKeyState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            foreach (Tile tile in map)
            {
                tile.Draw(spritebatch);
            }
        }

        private void MoveScreen(float x, float y)
        {
            foreach (Tile tile in map)
            {
                tile.position += new Vector2(x, y);
            }
        }

        private void MoveScreen(Vector2 vector)
        {
            MoveScreen(vector.X, vector.Y);
        }

        private Tile[,] ImportLevel(ContentManager content, string fileName)
        {
            fileName = "content/Levels/" + fileName + ".txt";

            if (File.Exists(fileName))
            {
                StreamReader inFile = File.OpenText(fileName);

                List<string> lines = new List<string>();

                while (!inFile.EndOfStream)
                {
                    lines.Add(inFile.ReadLine());
                }

                Tile[,] map = new Tile[lines.Count, lines[0].Length];

                for (int i = 0; i < lines.Count; i++)
                {
                    char[] characters = lines[i].ToCharArray();

                    for (int j = 0; j < characters.Length; j++)
                    {
                        Tile tile;

                        switch (characters[j])
                        {
                            case ('p'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Plain"), 0, 0, 0);
                                break;
                            default:
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Plain"), 0, 0, 0);
                                break;
                        }

                        map[i, j] = tile;
                    }
                }

                return map;

            }
            else throw new FileNotFoundException();
        }

        private List<PopupMenuBar> GetMenu()
        {
            List<PopupMenuBar> menu = new List<PopupMenuBar>();
            List<Tile> attackableTiles = new List<Tile>();
            int tileX = 0;
            int tileY = 0;

            bool hasChar = false;
            bool canAttack = false;

            //Loop through map to find selectedTile
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == selectedTile)
                    {
                        tileX = x;
                        tileY = y;
                    }
                }
            }

            if (selectedTile.charOnTile != null)
            {
                hasChar = true;
                Character ch = selectedTile.charOnTile;
                //Tile has character

                if (ch.equipedWeapon != null)
                {
                    for (int i = ch.equipedWeapon.minRange; i < ch.equipedWeapon.maxRange; i++)
                    {
                        //Check if tiles in range have characters
                    }
                }
            }

            return menu;
        }
    }
}
