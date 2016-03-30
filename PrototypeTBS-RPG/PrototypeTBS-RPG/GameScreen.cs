﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PrototypeTBS_RPG.Specializations;
using PrototypeTBS_RPG.Items;

namespace PrototypeTBS_RPG
{
    enum turnStatus
    {
        beforePlayer,
        player,
        beforeEnemy,
        enemy,
        none
    }

    class GameScreen : Screen
    {
        public Tile[,] map { get; private set; }
        public Tile selectedTile { get; private set; }
        public Tile menuTile { get; private set; }
        public string eventAction { get; private set; }

        private List<Character> characters;
        private Character characterA;
        private Character characterB;

        private ContentManager content;
        private List<PopupMenuBar> menuItems;

        private turnStatus turn = turnStatus.beforePlayer;
        private bool movingScreen = false;
        private bool renderMenu = false;
        private bool renderAttackMenu = false;
        private bool renderMoveMenu = false;

        private List<Tile> attackableTiles;
        private List<Character> deadCharacters;

        private MouseState oldMouseState;
        private KeyboardState oldKeyState;

        public GameScreen(ContentManager content, EventHandler screenEvent, string fileName)
            : base(screenEvent)
        {
            this.content = content;

            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();

            menuItems = new List<PopupMenuBar>();
            characters = new List<Character>();
            deadCharacters = new List<Character>();

            characterA = new Character(content, "Char A", new Knight(content), alliances.player);
            Sword swordA = new Sword(content.Load<Texture2D>("Weapons/IronSword"), "Iron Sword", 4, 0, 100);
            characterA.inventory.Add(swordA);
            characterA.Equip(swordA);
            characterB = new Character(content, "Char B", new Knight(content), alliances.enemy);

            characters.Add(characterA);
            characters.Add(characterB);

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
            map[1, 2].charOnTile = characterB;
        }

        public override void Update(GameTime gametime)
        {
            switch (turn)
            {
                case (turnStatus.beforePlayer):
                    BeforePlayerTurn();
                    break;

                case (turnStatus.player):
                    PlayerTurnUpdate(gametime);
                    break;

                case (turnStatus.beforeEnemy):
                    BeforeEnemyTurn();
                    break;

                case (turnStatus.enemy):
                    EnemyTurnUpdate(gametime);
                    break;

                default:
                    turn = turnStatus.beforePlayer;
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            foreach (Tile tile in map)
            {
                tile.Draw(spritebatch);
            }

            if (renderMenu)
            {
                foreach (PopupMenuBar menuBar in menuItems)
                {
                    menuBar.Draw(spritebatch);
                }
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
                            case ('f'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Forest"), 4, 0, 1);
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

        private void GetMenu()
        {
            List<PopupMenuBar> menu = new List<PopupMenuBar>();
            List<Tile> attackableTiles = new List<Tile>();
            int tileX = 0;
            int tileY = 0;

            bool hasChar = false;
            bool canAttack = false;

            //Loop through map to find selectedTile in map
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
                Weapon eqWpn = selectedTile.charOnTile.equipedWeapon;
                //Tile has character

                if (eqWpn != null && selectedTile.charOnTile.alliance == alliances.player && selectedTile.charOnTile.active)
                {
                    //Character has a weapon, is a player unit, and is active
                    for (int i = eqWpn.minRange; i <= eqWpn.maxRange; i++)
                    {
                        //Check if tiles in range have characters

                        if (tileY + i <= map.GetLength(0) - 1)
                            if (map[tileY + i, tileX].charOnTile != null)
                                if (map[tileY + i, tileX].charOnTile.alliance == alliances.enemy)
                                    attackableTiles.Add(map[tileY + i, tileX]);

                        if (tileY - i >= 0)
                            if (map[tileY - i, tileX].charOnTile != null)
                                if (map[tileY - i, tileX].charOnTile.alliance == alliances.enemy)
                                    attackableTiles.Add(map[tileY - i, tileX]);

                        if (tileX + i <= map.GetLength(1) - 1)
                            if (map[tileY, tileX + i].charOnTile != null)
                                if (map[tileY, tileX + i].charOnTile.alliance == alliances.enemy)
                                    attackableTiles.Add(map[tileY, tileX + i]);

                        if (tileX - i >= 0)
                            if (map[tileY, tileX - i].charOnTile != null)
                                if (map[tileY, tileX - i].charOnTile.alliance == alliances.enemy)
                                    attackableTiles.Add(map[tileY, tileX - i]);

                        //Check if diagonals need to be checked
                        if (i > 1)
                        {
                            //Check diagonals
                        }

                        if (attackableTiles.Count > 0)
                            canAttack = true;
                        
                        this.attackableTiles = attackableTiles;
                    }
                }
            }

            if (canAttack)
                menu.Add(new PopupMenuBar(content, "Attack", new EventHandler(AttackMenuEvent)));

            if (hasChar)
            {
                if (selectedTile.charOnTile.alliance == alliances.player && selectedTile.charOnTile.canMove)
                    menu.Add(new PopupMenuBar(content, "Move", new EventHandler(MoveMenuEvent)));
                menu.Add(new PopupMenuBar(content, selectedTile.charOnTile.name, new EventHandler(ProfileMenuEvent)));
            }

            menu.Add(new PopupMenuBar(content, "Options", new EventHandler(OptionsMenuEvent)));
            menu.Add(new PopupMenuBar(content, "End Turn", new EventHandler(EndTurnMenuEvent)));
            menu.Add(new PopupMenuBar(content, "Quit", new EventHandler(QuitMenuEvent)));

            for (int i = 0; i < menu.Count; i++)
            {
                menu[i].position.X = Game1.WINDOW_WIDTH - 80;
                menu[i].position.Y = 30 + 60 * i;
            }

            menuItems = menu;
        }

        private void MoveMenuEvent(object sender, EventArgs e)
        {
            renderMenu = false;
            renderMoveMenu = true;

            int selectedX = 0;
            int selectedY = 0;

            //TODO ; Overhall this into pathfinding

            //Find index of tile
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (map[y, x] == selectedTile)
                        {
                            selectedX = x;
                            selectedY = y;
                        }
                    }
                }

            foreach (Tile tile in map)
            {
                int tileX = 0;
                int tileY = 0;

                //Find index of tile
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (map[y, x].Equals(tile))
                        {
                            tileY = y;
                            tileX = x;
                        }
                    }
                }

                if (selectedTile.charOnTile.movement >= Math.Abs(tileX - selectedX) + Math.Abs(tileY - selectedY))
                    tile.movable = true;
            }
        }

        private void AttackMenuEvent(object sender, EventArgs e)
        {
            renderAttackMenu = true;
            renderMenu = false;

            foreach (Tile tile in map)
            {
                foreach (Tile atkTile in attackableTiles)
                {
                    if (tile == atkTile)
                        tile.attackable = true;
                }
            }
        }

        private void ProfileMenuEvent(object sender, EventArgs e)
        {
            renderMenu = false;
            eventAction = "profile";
            screenEvent.Invoke(this, new EventArgs());
        }

        private void OptionsMenuEvent(object sender, EventArgs e)
        {

        }

        private void EndTurnMenuEvent(object sender, EventArgs e)
        {
            renderMenu = false;
            turn = turnStatus.beforeEnemy;
        }

        private void QuitMenuEvent(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void BeforePlayerTurn()
        {
            foreach (Character ch in characters)
            {
                ch.active = true;
                ch.canMove = true;
            }

            turn = turnStatus.player;
        }

        private void PlayerTurnUpdate(GameTime gametime)
        {
            MouseState newMouseState = Mouse.GetState();
            KeyboardState newKeyState = Keyboard.GetState();

            //Tile selection stuff

            if (!renderMenu)
            {
                selectedTile = null;
                foreach (Tile tile in map)
                {
                    if (newMouseState.X <= tile.boundingRectangle.Right && newMouseState.X > tile.boundingRectangle.Left &&
                        newMouseState.Y <= tile.boundingRectangle.Bottom && newMouseState.Y > tile.boundingRectangle.Top)
                    {
                        tile.isSelected = true;
                        selectedTile = tile;
                    }
                    else tile.isSelected = false;
                }
            }

            //Menu selection stuff

            PopupMenuBar selectedMenu = null;
            foreach (PopupMenuBar menuItem in menuItems)
            {
                if (renderMenu &&
                    newMouseState.X <= menuItem.boundingRectangle.Right && newMouseState.X > menuItem.boundingRectangle.Left &&
                    newMouseState.Y <= menuItem.boundingRectangle.Bottom && newMouseState.Y > menuItem.boundingRectangle.Top)
                {
                    menuItem.isSelected = true;
                    selectedMenu = menuItem;
                }
                else menuItem.isSelected = false;
            }


            //Clicking input

            if (oldMouseState.LeftButton == ButtonState.Pressed && newMouseState.LeftButton == ButtonState.Released)
            {
                if (selectedTile != null && !renderMenu && !renderAttackMenu && !renderMoveMenu && !movingScreen)
                {
                    menuTile = selectedTile;
                    renderMenu = true;
                    GetMenu();
                }
                else if (selectedMenu != null && renderMenu)
                {
                    selectedMenu.MenuEvent();
                }
                else if (renderAttackMenu)
                {
                    Character attacker = menuTile.charOnTile;
                    Character defender = selectedTile.charOnTile;

                    if (attacker.Attack(defender))
                        deadCharacters.Add(defender);
                    else if (defender.Attack(attacker))
                        deadCharacters.Add(attacker);
                    else if (attacker.speed >= defender.speed + 5)
                    {
                        if (attacker.Attack(defender))
                            deadCharacters.Add(defender);
                    }
                    else if (defender.speed >= attacker.speed + 5)
                    {
                        if (defender.Attack(attacker))
                            deadCharacters.Add(attacker);
                    }

                    attacker.active = false;

                    renderAttackMenu = false; 
                    foreach (Tile tile in attackableTiles)
                    {
                        tile.attackable = false;
                    }
                    attackableTiles = new List<Tile>();
                }
                else if (renderMoveMenu)
                {
                    if (selectedTile.movable)
                    {
                        selectedTile.charOnTile = menuTile.charOnTile;
                        selectedTile.charOnTile.canMove = false; 

                        renderMoveMenu = false;

                        foreach (Tile tile in map)
                        {
                            if (tile.movable)
                                tile.movable = false;
                        }
                    }
                }
            }

            if (oldMouseState.RightButton == ButtonState.Pressed && newMouseState.RightButton == ButtonState.Released)
            {
                if (renderMenu)
                {
                    renderMenu = false;
                    menuTile = null;
                }
                else if (renderAttackMenu)
                {
                    renderAttackMenu = false;

                    foreach (Tile tile in attackableTiles)
                    {
                        tile.attackable = false;
                    }
                    attackableTiles = new List<Tile>();
                }
                else if (renderMoveMenu)
                {
                    renderMoveMenu = false;

                    foreach (Tile tile in map)
                    {
                        if (tile.movable)
                            tile.movable = false;
                    }
                }
            }

            //Keyboard input
            if (newKeyState.IsKeyDown(Keys.Up))
                MoveScreen(0, -3);
            if (newKeyState.IsKeyDown(Keys.Down))
                MoveScreen(0, 3);
            if (newKeyState.IsKeyDown(Keys.Left))
                MoveScreen(-3, 0);
            if (newKeyState.IsKeyDown(Keys.Right))
                MoveScreen(3, 0);

            //Drag movement input
            if (!renderMenu && newMouseState.Position.X <= Game1.WINDOW_WIDTH && newMouseState.Position.X >= 0 &&
                newMouseState.Position.Y <= Game1.WINDOW_HEIGHT && newMouseState.Position.Y >= 0 &&
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed &&
                newMouseState.Position != oldMouseState.Position)
            {
                movingScreen = true;
                MoveScreen((newMouseState.Position - oldMouseState.Position).ToVector2());
            }

            if (newMouseState.LeftButton == ButtonState.Released)
                movingScreen = false;

            foreach (Character ch in deadCharacters)
            {
                if (ch.tile != null)
                    ch.tile.charOnTile = null;
            }

            oldMouseState = newMouseState;
            oldKeyState = newKeyState;
        }

        private void BeforeEnemyTurn()
        {


            turn = turnStatus.enemy;
        }

        private void EnemyTurnUpdate(GameTime gametime)
        {


            turn = turnStatus.beforePlayer;
        }
    }
}
