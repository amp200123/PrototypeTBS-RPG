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

        public GameScreen(ContentManager content, EventHandler screenEvent, string fileName)
            : base(screenEvent)
        {
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
            MouseState mouseState = Mouse.GetState();
            KeyboardState newState = Keyboard.GetState();

            foreach (Tile tile in map)
            {
                if (mouseState.X < tile.boundingRectangle.Right && mouseState.X > tile.boundingRectangle.Left &&
                    mouseState.Y < tile.boundingRectangle.Bottom && mouseState.Y > tile.boundingRectangle.Top)
                    tile.isSelected = true;
                else tile.isSelected = false;
            }

            if (newState.IsKeyDown(Keys.Up))
            {
                MoveScreen(0, 3);
            }
            if (newState.IsKeyDown(Keys.Down))
            {
                MoveScreen(0, -3);
            }
            if (newState.IsKeyDown(Keys.Left))
            {
                MoveScreen(3, 0);
            }
            if (newState.IsKeyDown(Keys.Right))
            {
                MoveScreen(-3, 0);
            }
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
    }
}
