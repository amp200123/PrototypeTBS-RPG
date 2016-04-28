using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
        public List<Tile> startTiles { get; private set; }
        public int characterLimit { get; private set; }

        private List<Character> characters;

        private Random random;
        private ContentManager content;
        private TileBar tileBar;
        private List<PopupMenuBar> menuItems;

        private turnStatus turn = turnStatus.beforePlayer;
        private bool movingScreen = false;
        private bool renderMenu = false;
        private bool renderAttackMenu = false;
        private bool renderHealMenu = false;
        private bool renderMoveMenu = false;

        private List<Tile> attackableTiles;
        private List<Tile> healableTiles;
        private List<Character> deadCharacters;

        private MouseState oldMouseState = Mouse.GetState();
        private KeyboardState oldKeyState = Keyboard.GetState();

        public GameScreen(ContentManager content, EventHandler screenEvent, string fileName)
            : base(screenEvent)
        {
            this.content = content;

            random = new Random();
            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();

            menuItems = new List<PopupMenuBar>();
            characters = new List<Character>();
            deadCharacters = new List<Character>();

            ImportLevel(content, fileName);

            characterLimit = startTiles.Count;

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

            if (tileBar != null)
                tileBar.Draw(spritebatch);
        }

        public void AddCharacter(Character character, Tile tile)
        {
            tile.charOnTile = character;
            characters.Add(character);
        }

        public void AddCharacter(Character character, int x, int y)
        {
            AddCharacter(character, map[y, x]);
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

        private void ImportLevel(ContentManager content, string fileName)
        {
            fileName = "content/Levels/" + fileName + ".txt";

            if (File.Exists(fileName))
            {
                StreamReader inFile = File.OpenText(fileName);

                // Read map layout
                
                List<string> lines = new List<string>();

                while (true)
                {
                    string line = inFile.ReadLine();

                    if (line.StartsWith("#"))
                        continue;

                    if (!string.IsNullOrEmpty(line))
                        lines.Add(line);
                    else break;
                }

                map = new Tile[lines.Count, lines[0].Length];

                for (int i = 0; i < lines.Count; i++)
                {
                    char[] characters = lines[i].ToCharArray();

                    for (int j = 0; j < characters.Length; j++)
                    {
                        Tile tile;

                        switch (characters[j])
                        {
                            case ('p'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Plain"), "Plains", 0, 0, 1, true);
                                break;
                            case ('f'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Forest"), "Forest", 2, 0, 2, true);
                                break;
                            case ('m'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Mountain"), "Mountain", 5, 0, 0, false);
                                break;
                            case ('b'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Fort"), "Fort", 6, 20, 3, true);
                                break;
                            case ('s'):
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Marsh"), "Marsh", 0, 0, 2, true);
                                break;
                            default:
                                tile = new Tile(content, content.Load<Texture2D>("Tiles/Plain"), "Plains", 0, 0, 1, true);
                                break;
                        }

                        map[i, j] = tile;
                    }
                }

                //Read starting tile location

                lines = new List<string>();

                while (true)
                {
                    string line = inFile.ReadLine();

                    if (line.StartsWith("#"))
                        continue;

                    if (!string.IsNullOrEmpty(line))
                        lines.Add(line);
                    else break;
                }

                startTiles = new List<Tile>();
                foreach (string line in lines)
                {
                    string[] coords = line.Split(new string[] { "," }, StringSplitOptions.None);

                    startTiles.Add(map[Convert.ToInt32(coords[0]), Convert.ToInt32(coords[1])]);
                }

                //Read enemys and their placement

                lines = new List<string>();

                while (true)
                {
                    string line = inFile.ReadLine();

                    if (line != null && line.StartsWith("#"))
                        continue;

                    if (!string.IsNullOrEmpty(line))
                        lines.Add(line);
                    else break;
                }

                foreach (string line in lines)
                {
                    Specialization spec;
                    int level;
                    int y;
                    int x;
                    List<Item> inventory = new List<Item>();

                    string[] parts = line.Split(new string[] {","}, StringSplitOptions.None);

                    //Spec
                    switch (parts[0].ToLower()) // Spec
                    {
                        case "spearfighter":
                            spec = Game1.SpearFighter;
                            break;
                        case "warrior":
                            spec = Game1.Warrior;
                            break;
                        case "archer":
                            spec = Game1.Archer;
                            break;
                        case "swordsman":
                            spec = Game1.Swordsman;
                            break;
                        case "cavalier":
                            spec = Game1.Cavalier;
                            break;
                        case "mage":
                            spec = Game1.Mage;
                            break;
                        case "priest":
                            spec = Game1.Priest;
                            break;
                        case "shaman":
                            spec = Game1.Shaman;
                            break;
                        case "cleric":
                            spec = Game1.Cleric;
                            break;
                        default:
                            spec = Game1.Knight;
                            break;
                    }

                    level = Convert.ToInt32(parts[1]);
                    x = Convert.ToInt32(parts[2]);
                    y = Convert.ToInt32(parts[3]);

                    if (parts.Length > 4) 
                    {
                        for (int i = 4; i < parts.Length; i++) 
                        {
                            switch (parts[i].ToLower()) 
                            {
                                case "ironsword":
                                    inventory.Add(Game1.IronSword);
                                    break;
                                case "steelsword":
                                    inventory.Add(Game1.SteelSword);
                                    break;
                                case "ironlance":
                                    inventory.Add(Game1.IronLance);
                                    break;
                                case "steellance":
                                    inventory.Add(Game1.SteelLance);
                                    break;
                                case "ironaxe":
                                    inventory.Add(Game1.IronAxe);
                                    break;
                                case "steelaxe":
                                    inventory.Add(Game1.SteelAxe);
                                    break;
                                case "ironbow":
                                    inventory.Add(Game1.IronBow);
                                    break;
                                case "steelbow":
                                    inventory.Add(Game1.SteelBow);
                                    break;
                                case "firetome":
                                    inventory.Add(Game1.FireTome);
                                    break;
                                case "lightningtome":
                                    inventory.Add(Game1.LightningTome);
                                    break;
                                case "corrupttome":
                                    inventory.Add(Game1.CorruptTome);
                                    break;
                                case "healstaff":
                                    inventory.Add(Game1.HealStaff);
                                    break;
                                case "healtonic":
                                    inventory.Add(Game1.HealTonic);
                                    break;
                                case "elixir":
                                    inventory.Add(Game1.Elixir);
                                    break;
                            }
                        }
                    }


                    Character character = new DefaultEnemy(content, spec, level, inventory);

                    characters.Add(character);
                    map[y, x].charOnTile = character;
                }

            }
            else throw new FileNotFoundException();
        }

        private void GetMenu()
        {
            List<PopupMenuBar> menu = new List<PopupMenuBar>();
            int tileX = 0;
            int tileY = 0;

            bool hasChar = false;
            bool canAttack = false;
            bool canHeal = false;

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
                    if (eqWpn is Staff) 
                    {
                        healableTiles = new List<Tile>();

                        FindHealableTiles(selectedTile.charOnTile, selectedTile, eqWpn.maxRange, eqWpn.minRange, eqWpn.maxRange);

                        if (healableTiles.Count > 0)
                            canHeal = true;
                    }
                    else 
                    {
                        attackableTiles = new List<Tile>();

                        FindAttackableTile(selectedTile.charOnTile.alliance, selectedTile, eqWpn.maxRange, eqWpn.minRange, eqWpn.maxRange);

                        if (attackableTiles.Count > 0)
                            canAttack = true;
                    }
                }
            }

            if (canAttack)
                menu.Add(new PopupMenuBar(content, "Attack", new EventHandler(AttackMenuEvent)));

            if (canHeal)
                menu.Add(new PopupMenuBar(content, "Staff", new EventHandler(HealMenuEvent)));

            if (hasChar)
            {
                if (selectedTile.charOnTile.active)
                {
                    if (selectedTile.charOnTile.alliance == alliances.player && selectedTile.charOnTile.canMove)
                        menu.Add(new PopupMenuBar(content, "Move", new EventHandler(MoveMenuEvent)));
                    menu.Add(new PopupMenuBar(content, selectedTile.charOnTile.name, new EventHandler(ProfileMenuEvent)));
                    if (selectedTile.charOnTile.alliance == alliances.player)
                        menu.Add(new PopupMenuBar(content, "Wait", new EventHandler(WaitMenuEvent)));
                }
                else menu.Add(new PopupMenuBar(content, selectedTile.charOnTile.name, new EventHandler(ProfileMenuEvent)));
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

        /// <summary>
        /// Method to find and mark tiles that can be moved to
        /// </summary>
        /// <param name="currentTile">Current to find tiles from</param>
        /// <param name="movesLeft">Amount of movements left</param>
        private void FindMovableTiles(alliances alliance, Tile currentTile, int movesLeft)
        {
            //Make sure the character has enought movement to move here
            if (movesLeft < 0 || !currentTile.accessible)
                return;

            //Can't move to tile that is occupied
            if (currentTile.charOnTile != null)
            {
                //Enemies block a characters path
                if (currentTile.charOnTile.alliance != alliance)
                    return;
            }
            else currentTile.movable = true;

            //If there are no more moves left, no need to check any more tiles
            if (movesLeft == 0)
                return;

            //Find currentTile's index
            int currentY = 0;
            int currentX = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x].Equals(currentTile))
                    {
                        currentY = y;
                        currentX = x;
                    }
                }
            }

            //Check tiles in all 4 directions
            if (currentY > 0) //Up
            {
                Tile checkTile = map[currentY - 1, currentX];

                FindMovableTiles(alliance, checkTile, movesLeft - checkTile.movement);
            }
            if (currentX < map.GetLength(1) - 1) //Right 
            {
                Tile checkTile = map[currentY, currentX + 1];

                FindMovableTiles(alliance, checkTile, movesLeft - checkTile.movement);
            }
            if (currentY < map.GetLength(0) - 1) //Down
            {
                Tile checkTile = map[currentY + 1, currentX];

                FindMovableTiles(alliance, checkTile, movesLeft - checkTile.movement);
            }
            if (currentX > 0) //Left
            {
                Tile checkTile = map[currentY, currentX - 1];

                FindMovableTiles(alliance, checkTile, movesLeft - checkTile.movement);
            }
        }

        private void FindAttackableTile(alliances alliance, Tile currentTile, int rangeLeft, int minRange, int maxRange)
        {
            //Make sure that tile is still in range
            if (rangeLeft < 0)
                return;

            //Check if enemy exists and is in range
            if (currentTile.charOnTile != null && currentTile.charOnTile.alliance != alliance &&
                maxRange - rangeLeft <= maxRange && maxRange - rangeLeft >= minRange)
            {
                attackableTiles.Add(currentTile);
            }

            //If tile is on the edge of character's range, no need to check more tiles
            if (rangeLeft == 0)
                return;

            //Find currentTile's index
            int currentY = 0;
            int currentX = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x].Equals(currentTile))
                    {
                        currentY = y;
                        currentX = x;
                    }
                }
            }

            //Check tiles in all 4 directions
            if (currentY > 0) //Up
            {
                Tile checkTile = map[currentY - 1, currentX];

                FindAttackableTile(alliance, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentX < map.GetLength(1) - 1) //Right 
            {
                Tile checkTile = map[currentY, currentX + 1];

                FindAttackableTile(alliance, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentY < map.GetLength(0) - 1) //Down
            {
                Tile checkTile = map[currentY + 1, currentX];

                FindAttackableTile(alliance, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentX > 0) //Left
            {
                Tile checkTile = map[currentY, currentX - 1];

                FindAttackableTile(alliance, checkTile, rangeLeft - 1, minRange, maxRange);
            }
        }

        private void FindHealableTiles(Character healer, Tile currentTile, int rangeLeft, int minRange, int maxRange)
        {
            //Make sure that tile is still in range
            if (rangeLeft < 0)
                return;

            //Check if enemy exists and is in range
            if (currentTile.charOnTile != null && currentTile.charOnTile.alliance == healer.alliance && 
                currentTile.charOnTile != healer && currentTile.charOnTile.currentHp < currentTile.charOnTile.hp && 
                maxRange - rangeLeft <= maxRange && maxRange - rangeLeft >= minRange)
            {
                healableTiles.Add(currentTile);
            }

            //If tile is on the edge of character's range, no need to check more tiles
            if (rangeLeft == 0)
                return;

            //Find currentTile's index
            int currentY = 0;
            int currentX = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x].Equals(currentTile))
                    {
                        currentY = y;
                        currentX = x;
                    }
                }
            }

            //Check tiles in all 4 directions
            if (currentY > 0) //Up
            {
                Tile checkTile = map[currentY - 1, currentX];

                FindHealableTiles(healer, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentX < map.GetLength(1) - 1) //Right 
            {
                Tile checkTile = map[currentY, currentX + 1];

                FindHealableTiles(healer, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentY < map.GetLength(0) - 1) //Down
            {
                Tile checkTile = map[currentY + 1, currentX];

                FindHealableTiles(healer, checkTile, rangeLeft - 1, minRange, maxRange);
            }
            if (currentX > 0) //Left
            {
                Tile checkTile = map[currentY, currentX - 1];

                FindHealableTiles(healer, checkTile, rangeLeft - 1, minRange, maxRange);
            }
        }

        private void MoveMenuEvent(object sender, EventArgs e)
        {
            renderMenu = false;
            renderMoveMenu = true;

            FindMovableTiles(alliances.player, selectedTile, selectedTile.charOnTile.movement);
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

        private void HealMenuEvent(object sender, EventArgs e)
        {
            renderHealMenu = true;
            renderMenu = false;

            foreach (Tile tile in map)
            {
                foreach (Tile healTile in healableTiles)
                {
                    if (tile == healTile)
                        tile.healable = true;
                }
            }
        }

        private void ProfileMenuEvent(object sender, EventArgs e)
        {
            renderMenu = false;
            screenEvent.Invoke(this, new ProfileEventArgs(selectedTile.charOnTile));
        }

        private void WaitMenuEvent(object sender, EventArgs e)
        {
            selectedTile.charOnTile.active = false;
            renderMenu = false;
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

                if (ch.alliance == alliances.player)
                {
                    ch.currentHp += (int)Math.Round((ch.tile.health / 100f) * ch.hp);
                    if (ch.currentHp > ch.hp)
                        ch.currentHp = ch.hp;
                }
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
                tileBar = null;
                foreach (Tile tile in map)
                {
                    if (tile.boundingRectangle.Contains(newMouseState.Position))
                    {
                        tile.isSelected = true;
                        selectedTile = tile;
                        tileBar = new TileBar(content, selectedTile);
                    }
                    else tile.isSelected = false;
                }
            }

            //Menu selection stuff

            PopupMenuBar selectedMenu = null;
            foreach (PopupMenuBar menuItem in menuItems)
            {
                if (renderMenu && menuItem.boundingRectangle.Contains(newMouseState.Position))
                {
                    menuItem.isSelected = true;
                    selectedMenu = menuItem;
                }
                else menuItem.isSelected = false;
            }


            //Clicking input

            if (oldMouseState.LeftButton == ButtonState.Pressed && newMouseState.LeftButton == ButtonState.Released
                && !movingScreen)
            {
                if (selectedTile != null)
                {
                    if (!renderMenu && !renderAttackMenu && !renderMoveMenu && !renderHealMenu)
                    {
                        menuTile = selectedTile;
                        renderMenu = true;
                        GetMenu();
                    }
                    else if (renderAttackMenu && selectedTile.attackable)
                    {
                        Character attacker = menuTile.charOnTile;
                        int menuTileX = 0;
                        int menuTileY = 0;
                        Character defender = selectedTile.charOnTile;
                        int selectedTileX = 0;
                        int selectedTileY = 0;

                        //Find indexes of tiles
                        for (int y = 0; y < map.GetLength(0); y++)
                        {
                            for (int x = 0; x < map.GetLength(1); x++)
                            {
                                if (map[y, x].Equals(menuTile))
                                {
                                    menuTileY = y;
                                    menuTileX = x;
                                }

                                if (map[y, x].Equals(selectedTile))
                                {
                                    selectedTileY = y;
                                    selectedTileX = x;
                                }
                            }
                        }

                        int attackRange = Math.Abs(selectedTileX - menuTileX) + Math.Abs(selectedTileY - menuTileY);

                        if (attacker.Attack(defender, attackRange))
                            deadCharacters.Add(defender);
                        else if (defender.Attack(attacker, attackRange))
                            deadCharacters.Add(attacker);
                        else if (attacker.speed >= defender.speed + 5)
                        {
                            if (attacker.Attack(defender, attackRange))
                                deadCharacters.Add(defender);
                        }
                        else if (defender.speed >= attacker.speed + 5)
                        {
                            if (defender.Attack(attacker, attackRange))
                                deadCharacters.Add(attacker);
                        }

                        attacker.active = false;

                        //Exp awardment
                        if (!deadCharacters.Contains(attacker)) //Make sure attacker is alive
                        {
                            float expBase = (defender.level - attacker.level) + 1;
                            if (expBase < 1)
                                expBase = 1 / (1 + Math.Abs(expBase));
                            int expModifier;

                            //Check if they defeated the defender
                            if (deadCharacters.Contains(defender))
                                expModifier = 20;
                            else expModifier = 5;

                            if (attacker.GiveExp((int)Math.Ceiling(expBase * expModifier)))
                            {
                                screenEvent.Invoke(this, new LevelUpEventArgs(attacker));
                            }
                        }


                        renderAttackMenu = false;
                        foreach (Tile tile in attackableTiles)
                        {
                            tile.attackable = false;
                        }
                        attackableTiles = new List<Tile>();
                    }
                    else if (renderMoveMenu && selectedTile.movable)
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
                    else if (renderHealMenu && selectedTile.healable)
                    {
                        (menuTile.charOnTile.equipedWeapon as Staff).Heal(menuTile.charOnTile, selectedTile.charOnTile);
                        menuTile.charOnTile.active = false;

                        renderHealMenu = false;
                        foreach (Tile tile in healableTiles)
                        {
                            tile.healable = false;
                        }
                        healableTiles = new List<Tile>();
                    }
                }
                
                if (selectedMenu != null && renderMenu)
                {
                    selectedMenu.MenuEvent();
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
                {
                    ch.tile.charOnTile = null;
                    ch.tile = null;
                }
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
