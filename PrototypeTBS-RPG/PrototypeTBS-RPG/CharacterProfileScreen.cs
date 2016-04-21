using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PrototypeTBS_RPG
{
    class CharacterProfileScreen : Screen
    {
        private ContentManager content;
        private List<InventoryBar> inventory;
        private List<MiniMenuBar> popupMenu;
        private SpriteFont descriptionFont;
        private SpriteFont largeFont;
        private Character character;

        private MouseState oldState;
        private InventoryBar selectedBar;
        private MiniMenuBar selectedPopup;

        private bool renderPopupMenu = false;

        public CharacterProfileScreen(Character character, ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            oldState = Mouse.GetState();

            this.content = content;
            this.character = character;

            descriptionFont = content.Load<SpriteFont>("Fonts/ProfileDescriptionFont");
            largeFont = content.Load<SpriteFont>("Fonts/ProfileLargeFont");
            
            popupMenu = new List<MiniMenuBar>();

            inventory = new List<InventoryBar>();
            for (int i = 0; i < 5; i++)
            {
                Item item;
                if (i < character.inventory.Count)
                    item = character.inventory[i];
                else item = null;

                inventory.Add(new InventoryBar(content, character, item,
                    new Vector2(9 * Game1.WINDOW_WIDTH / 12, 220 + i * 35), i));
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();

            if ((newState.RightButton == ButtonState.Pressed && !renderPopupMenu) ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                screenEvent.Invoke(this, new EventArgs());

            if (!renderPopupMenu)
            {
                selectedBar = null;
                foreach (InventoryBar bar in inventory)
                {
                    if (newState.X <= bar.boundingRectangle.Right && newState.X > bar.boundingRectangle.Left &&
                         newState.Y <= bar.boundingRectangle.Bottom && newState.Y > bar.boundingRectangle.Top)
                    {
                        bar.isSelected = true;
                        selectedBar = bar;
                    }
                    else bar.isSelected = false;
                }
            }
            else
            {
                selectedPopup = null;
                foreach (MiniMenuBar bar in popupMenu)
                {
                    if (newState.X <= bar.boundingRectangle.Right && newState.X > bar.boundingRectangle.Left &&
                         newState.Y <= bar.boundingRectangle.Bottom && newState.Y > bar.boundingRectangle.Top)
                    {
                        bar.isSelected = true;
                        selectedPopup = bar;
                    }
                    else bar.isSelected = false;
                }
            }

            if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
            {
                if (selectedBar != null && selectedBar.item != null && !renderPopupMenu && character.alliance == alliances.player)
                {
                    renderPopupMenu = true;
                    GetMenu();
                }
                else if (renderPopupMenu && selectedPopup != null)
                {
                    selectedPopup.MenuEvent();
                }
            }

            if (oldState.RightButton == ButtonState.Pressed && newState.RightButton == ButtonState.Released)
            {
                if (renderPopupMenu)
                    renderPopupMenu = false;
            }

            oldState = newState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //TODO: add background or something to stylize 

            DrawText(spritebatch, largeFont, character.name, new Vector2(Game1.WINDOW_WIDTH / 2, 40));
            DrawText(spritebatch, descriptionFont, "Lvl " + character.level + " " + character.spec.name,
                new Vector2(Game1.WINDOW_WIDTH / 2, 80));
            DrawText(spritebatch, descriptionFont, character.exp + " / 100 xp", new Vector2(Game1.WINDOW_WIDTH / 2, 105));

            DrawText(spritebatch, largeFont, "Stats", new Vector2(3 * Game1.WINDOW_WIDTH / 12, 160));
            DrawText(spritebatch, descriptionFont, character.currentHp + " / " + character.hp + " Hp", 
                new Vector2(3 * Game1.WINDOW_WIDTH / 12, 200));
            DrawText(spritebatch, largeFont, "Inventory", new Vector2(9 * Game1.WINDOW_WIDTH / 12, 160));

            DrawText(spritebatch, descriptionFont, "Atk: " + character.strength, new Vector2(2 * Game1.WINDOW_WIDTH / 12, 250));
            DrawText(spritebatch, descriptionFont, "Mag: " + character.magic, new Vector2(2 * Game1.WINDOW_WIDTH / 12, 290));
            DrawText(spritebatch, descriptionFont, "Spd: " + character.speed, new Vector2(2 * Game1.WINDOW_WIDTH / 12, 330));
            DrawText(spritebatch, descriptionFont, "Skl: " + character.skill, new Vector2(2 * Game1.WINDOW_WIDTH / 12, 370));
            DrawText(spritebatch, descriptionFont, "Lck: " + character.luck, new Vector2(4 * Game1.WINDOW_WIDTH / 12, 250));
            DrawText(spritebatch, descriptionFont, "Def: " + character.defense, new Vector2(4 * Game1.WINDOW_WIDTH / 12, 290));
            DrawText(spritebatch, descriptionFont, "Res: " + character.resistance, new Vector2(4 * Game1.WINDOW_WIDTH / 12, 330));
            DrawText(spritebatch, descriptionFont, "Mov: " + character.movement, new Vector2(4 * Game1.WINDOW_WIDTH / 12, 370));

            foreach (InventoryBar bar in inventory)
            {
                bar.Draw(spritebatch);
            }

            if (renderPopupMenu)
            {
                foreach (MiniMenuBar bar in popupMenu)
                {
                    bar.Draw(spritebatch);
                }
            }
        }

        private void DrawText(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 pos)
        {
            spritebatch.DrawString(font, text,
                new Vector2(pos.X - font.MeasureString(text).X / 2, pos.Y - font.MeasureString(text).Y / 2),
                Color.Black);
        }

        private void GetMenu()
        {
            List<MiniMenuBar> menu = new List<MiniMenuBar>();

            if (selectedBar.item.usable)
                menu.Add(new MiniMenuBar(content, "Use", new EventHandler(UsePopupEvent)));

            if (selectedBar.item is Weapon)
            {
                if (character.equipedWeapon == selectedBar.item)
                {
                    menu.Add(new MiniMenuBar(content, "Unequip", new EventHandler(UnequipPopupEvent)));
                }
                else menu.Add(new MiniMenuBar(content, "Equip", new EventHandler(EquipPopupEvent)));
            }

            menu.Add(new MiniMenuBar(content, "Discard", new EventHandler(DiscardPopupEvent)));

            for (int i = 0; i < menu.Count; i++)
            {
                menu[i].position.X = (selectedBar.position.X - selectedBar.texture.Width / 2) 
                    + menu[i].texture.Width / 2 + i * menu[i].texture.Width;
                menu[i].position.Y = selectedBar.position.Y - 30;
            }

            popupMenu = menu;
        }

        private void UsePopupEvent(object sender, EventArgs e)
        {
            renderPopupMenu = false;
            selectedBar.item.Use(character);
            character.inventory.Remove(selectedBar.item);

            ReloadInv();
        }

        private void EquipPopupEvent(object sender, EventArgs e)
        {
            renderPopupMenu = false;
            character.Equip(selectedBar.item as Weapon);

            ReloadInv();
        }

        private void UnequipPopupEvent(object sender, EventArgs e)
        {
            renderPopupMenu = false;
            character.Unequip(selectedBar.item as Weapon);

            ReloadInv();
        }

        private void DiscardPopupEvent(object sender, EventArgs e)
        {
            renderPopupMenu = false;
            character.Discard(selectedBar.item);

            ReloadInv();
        }

        private void ReloadInv()
        {
            inventory = new List<InventoryBar>();
            for (int i = 0; i < 5; i++)
            {
                Item item;
                if (i < character.inventory.Count)
                    item = character.inventory[i];
                else item = null;

                inventory.Add(new InventoryBar(content, character, item,
                    new Vector2(9 * Game1.WINDOW_WIDTH / 12, 220 + i * 35), i));
            }
        }
    }
}
